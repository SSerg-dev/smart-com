using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Settings;
using Microsoft.AspNet.SignalR;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System.Threading;
using Newtonsoft.Json;
using Persist.Model;

namespace Module.Persist.TPM
{
    public class TasksLogHub : Hub
    {
        /// <summary>
        /// Объект блокировки для синхронизации потоков, обрабатывающих лог
        /// </summary>
        private static object lockerLog = new object();

        /// <summary>
        /// Объект блокировки для синхронизации потоков, обрабатывающих задачи
        /// </summary>
        private static object lockerHandler = new object();

        /// <summary>
        /// Токен для отмены задачи обновления лога
        /// </summary>
        private static CancellationTokenSource tokenCancelSourceLog;

        /// <summary>
        /// Токен для отмены задачи обновления грида задач
        /// </summary>

        private static CancellationTokenSource tokenCancelSourceHandler;

        /// <summary>
        /// Подисчики на обновления статусов задач
        /// </summary>

        private static List<SubcriberHandler> subscribersHandler = new List<SubcriberHandler>();

        /// <summary>
        /// Подисчики на обновления лога
        /// </summary>

        private static List<SubcriberLog> subscribersLog = new List<SubcriberLog>();

        /// <summary>
        /// Подписаться на обновления лога
        /// </summary>
        /// <param name="handlerID">ID обработчика</param>
        public void SubscribeLog(Guid handlerID)
        {
            DatabaseContext context = new DatabaseContext();
            bool needCreateTaskForCheckUpdates;

            // синхронизируем потоки (да вручную, специально)
            lock (lockerLog)
            {
                // если это будет единственный подписчик, то необходимо создать задачу проверки обновлений
                needCreateTaskForCheckUpdates = subscribersLog.Count == 0;
                SubcriberLog subscriber = subscribersLog.FirstOrDefault(n => n.ConnectionID == Context.ConnectionId);

                if (subscriber == null)
                {
                    LoopHandler calculatingInfo = context.Set<LoopHandler>().Find(handlerID);
                    if (calculatingInfo != null)
                    {
                        string contentLog;
                        string statusHandler;
                        DateTime? dateModification;

                        GetContentOfLog(handlerID, out contentLog, out dateModification, out statusHandler);
                           
                        subscriber = new SubcriberLog(Context.ConnectionId, Context.User.Identity.Name, null, statusHandler, handlerID);
                        subscribersLog.Add(subscriber);

                        if (contentLog != null)
                        {
                            Clients.Caller.addInfoInLog(contentLog);
                        }
                    }
                }
            }

            if (needCreateTaskForCheckUpdates)
            {
                tokenCancelSourceLog = new CancellationTokenSource();
                Task.Factory.StartNew(CheckUpdatesLogs);
            }

            context.Dispose();
        }

        /// <summary>
        /// Подписаться на обновления статусов задач
        /// </summary>
        public void SubscribeHandler()
        {
            DatabaseContext context = new DatabaseContext();
            bool needCreateTaskForCheckUpdates;

            // синхронизируем потоки (да вручную, специально)
            lock (lockerHandler)
            {
                // если это будет единственный подписчик, то необходимо создать задачу проверки обновлений
                needCreateTaskForCheckUpdates = subscribersHandler.Count == 0;

                SubcriberHandler subscriber = subscribersHandler.FirstOrDefault(n => n.ConnectionID == Context.ConnectionId);

                if (subscriber == null)
                {
                    subscriber = new SubcriberHandler(Context.ConnectionId, Context.User.Identity.Name, context);
                    subscribersHandler.Add(subscriber);
                }
            }

            if (needCreateTaskForCheckUpdates)
            {
                tokenCancelSourceHandler = new CancellationTokenSource();
                Task.Factory.StartNew(CheckUpdatesHandlers);
            }

            context.Dispose();
        }

        /// <summary>
        /// Отписаться от получения содержимого лога
        /// </summary>
        public void UnsubscribeLog(Guid handlerId)
        {
            RemoveFromSubscribersLog(handlerId);
        }

        /// <summary>
        /// Отписаться от получения статусов задач
        /// </summary>
        public void UnsubscribeHandler()
        {
            RemoveFromSubscribersHandler();
        }

        /// <summary>
        /// Удалить подписчика при отключении от хаба
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            RemoveFromSubscribersLog();
            RemoveFromSubscribersHandler();

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Удалить подписчика лога
        /// </summary>
        /// <param name="handlerId">ID обработчика</param>
        private void RemoveFromSubscribersLog(Guid? handlerId = null)
        {
            lock (lockerLog)
            {
                SubcriberLog subscriber = subscribersLog.FirstOrDefault(n => n.ConnectionID == Context.ConnectionId && (!handlerId.HasValue || n.HandlerID == handlerId));
                if (subscriber != null)
                    subscribersLog.Remove(subscriber);

                // если подписчиков больше нет, то прекратить мониторинг
                if (subscribersLog.Count == 0 && tokenCancelSourceLog != null)
                    tokenCancelSourceLog.Cancel();
            }
        }

        /// <summary>
        /// Удалить подписчика по статусам задач
        /// </summary>
        private void RemoveFromSubscribersHandler()
        {
            lock (lockerHandler)
            {
                SubcriberHandler subscriber = subscribersHandler.FirstOrDefault(n => n.ConnectionID == Context.ConnectionId);
                if (subscriber != null)
                    subscribersHandler.Remove(subscriber);

                // если подписчиков больше нет, то прекратить мониторинг
                if (subscribersLog.Count == 0 && tokenCancelSourceHandler != null)
                    tokenCancelSourceHandler.Cancel();
            }
        }

        /// <summary>
        /// Задача проверки обновлений логов
        /// </summary>
        private void CheckUpdatesLogs()
        {
            // завершить задачу, если она была отменена
            while (!tokenCancelSourceLog.Token.IsCancellationRequested)
            {
                try
                {
                    lock (lockerLog)
                    {
                        // группируем по Promo ID
                        var subscribersGrouped = subscribersLog.GroupBy(n => n.HandlerID);

                        // каждую группу рассматриваем отдельно (добавим немного параллельности)
                        Parallel.ForEach(subscribersGrouped, (subscribersGroup) =>
                        {
                            DatabaseContext context = new DatabaseContext();
                            LoopHandler calculatingInfo = context.Set<LoopHandler>().Find(subscribersGroup.Key);

                            if (calculatingInfo != null)
                            {
                                string contentLog;
                                string statusHandler;
                                DateTime? lastWriteDate;

                                GetContentOfLog(calculatingInfo.Id, out contentLog, out lastWriteDate, out statusHandler);

                                Parallel.ForEach(subscribersGroup, (subscriber) =>
                                {
                                    // если дата последнего чтения устарела, то шлем его клиенту  

                                    if (contentLog != null && (subscriber.StatusHandler != statusHandler || !subscriber.LastModification.HasValue || DateTime.Compare(subscriber.LastModification.Value, lastWriteDate.Value) < 0))
                                    {
                                        subscriber.LastModification = lastWriteDate;
                                        subscriber.StatusHandler = statusHandler;
                                        Clients.Client(subscriber.ConnectionID).addInfoInLog(contentLog);
                                    }
                                });
                            }

                            context.Dispose();
                        });
                    }
                }
                catch { }

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Прочитать содержимое лога и сформировать ответ
        /// </summary>
        /// <param name="handlerID">ID обработчика</param>
        /// <param name="contentLog">Выходной параметр: содержание лога</param>
        /// <param name="lastWriteDate">Выходной параметр: дата последней записи в лог</param>
        /// <param name="statusHandler">Выходной параметр: статус задачи</param>
        private void GetContentOfLog(Guid handlerID, out string contentLog, out DateTime? lastWriteDate, out string statusHandler)
        {
            contentLog = null;
            statusHandler = "";

            string contentOfFileLog;
            GetContentOfFileLog(handlerID, out lastWriteDate, out contentOfFileLog);

            DatabaseContext context = new DatabaseContext();
            LoopHandler handler = context.Set<LoopHandler>().FirstOrDefault(x => x.Id == handlerID);

            if (handler != null)
            {
                statusHandler = handler.Status;
                contentLog = JsonConvert.SerializeObject(new
                {
                    success = true,
                    data = contentOfFileLog != null ? contentOfFileLog : "",
                    description = handler.Description,
                    status = statusHandler,
                });
            }

            context.Dispose();
        }

        /// <summary>
        /// Прочитать содержимое файла лога
        /// </summary>
        /// <param name="handlerID">ID обработчика</param>
        /// <param name="lastWriteDate">Выходной параметр: дата последней записи в лог</param>
        /// <param name="contentOfLog">Выходной параметр: содержание лога</param>
        private void GetContentOfFileLog(Guid handlerID, out DateTime? lastWriteDate, out string contentOfLog)
        {
            string logDir = AppSettingsManager.GetSetting("HANDLER_LOG_DIRECTORY", "HandlerLogs");
            string logFileName = String.Format("{0}.txt", handlerID);
            string filePath = System.IO.Path.Combine(logDir, logFileName);

            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        byte[] buffer = new byte[fs.Length];

                        lastWriteDate = File.GetLastWriteTime(filePath);
                        fs.Read(buffer, 0, buffer.Length);
                        contentOfLog = System.Text.Encoding.UTF8.GetString(buffer);
                    }
                }
                else
                {
                    lastWriteDate = null;
                    contentOfLog = null;
                }
            }
            catch (Exception e)
            {
                lastWriteDate = null;
                contentOfLog = null;
            }
        }

        /// <summary>
        /// Задача проверки обновлений статусов обработчиков
        /// </summary>
        private void CheckUpdatesHandlers()
        {
            // завершить задачу, если она была отменена
            while (!tokenCancelSourceHandler.Token.IsCancellationRequested)
            {
                try
                {
                    lock (lockerHandler)
                    {
                        // каждого подписчика рассматриваем отдельно (добавим немного параллельности)
                        Parallel.ForEach(subscribersHandler, (subscriber) =>
                        {
                            DatabaseContext context = new DatabaseContext();

                            // если статус хотя бы одной задачи изменился, сигнализируем об этом
                            if (subscriber.TaskStatusesChanged(context))
                            {                                
                                Clients.Client(subscriber.ConnectionID).notifyUpdateHandlers();
                            }

                            context.Dispose();
                        });
                    }
                }
                catch { }

                Thread.Sleep(1000);
            }
        }
    }

    public class SubcriberLog
    {
        /// <summary>
        /// ID соединения
        /// </summary>
        public string ConnectionID { get; private set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Дата последнего обновления файла лога, отправленного клиенту
        /// </summary>
        public DateTime? LastModification { get; set; }

        /// <summary>
        /// Статус обработчика
        /// </summary>
        public string StatusHandler { get; set; }

        /// <summary>
        /// Id Handler'а для которого нужен лог (null, если лог не нужен)
        /// </summary>
        public Guid? HandlerID { get; set; }

        ///// <summary>
        ///// Определяет необходимо ли получать все обновления или только по своим задачам 
        ///// </summary>
        //public bool NeedAllUpdates { get; set; }

        public SubcriberLog(string connectionID, string userName, DateTime? lastModification, string statusHandler, Guid? handlerID)
        {
            ConnectionID = connectionID;
            UserName = userName;
            LastModification = lastModification;
            StatusHandler = statusHandler;
            HandlerID = handlerID;
            //NeedAllUpdates = needAllUpdates;
        }
    }

    public class SubcriberHandler
    {
        /// <summary>
        /// ID соединения
        /// </summary>
        public string ConnectionID { get; private set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Сгрупированные по статусу задачи
        /// </summary>
        // это свойство дает возможность понять необходимо ли обновлять грид
        public Dictionary<string, int> TasksGroupedByStatus { get; private set; }

        public SubcriberHandler(string connectionID, string userName, DatabaseContext context)
        {
            ConnectionID = connectionID;
            UserName = userName;

            var a = context.Set<LoopHandler>().Where(n => n.User.Name.ToLower() == userName.ToLower()).ToArray();

            TasksGroupedByStatus = context.Set<LoopHandler>().Where(n => n.User.Name.ToLower() == userName.ToLower() && n.Status != null).GroupBy(n => n.Status)
                                        .OrderBy(n => n.Key).ToDictionary(n => n.Key, g => g.Count());

            // если Status равен null, то обработчик создан, но не запущен, считаем их отдельно
            int countNotStartedHandlers = context.Set<LoopHandler>().Where(n => n.User.Name.ToLower() == userName.ToLower() && n.Status == null).Count();
            TasksGroupedByStatus.Add("NotStarted", countNotStartedHandlers);

        }

        /// <summary>
        /// Проверить изменились ли статусы задач
        /// </summary>
        /// <param name="tasksGroupedByStatus">Словарь проверяемых задач</param>
        /// <returns></returns>
        public bool TaskStatusesChanged(DatabaseContext context)
        {
            bool changed = false;
            Dictionary<string, int> tasksGroupedByStatus = context.Set<LoopHandler>().Where(n => n.User.Name.ToLower() == UserName.ToLower() && n.Status != null).GroupBy(n => n.Status)
                                                                .OrderBy(n => n.Key).ToDictionary(n => n.Key, g => g.Count());

            // если Status равен null, то обработчик создан, но не запущен, считаем их отдельно
            int countNotStartedHandlers = context.Set<LoopHandler>().Where(n => n.User.Name.ToLower() == UserName.ToLower() && n.Status == null).Count();
            tasksGroupedByStatus.Add("NotStarted", countNotStartedHandlers);

            foreach (var group in tasksGroupedByStatus)
            {
                int countTasksInGroup;
                TasksGroupedByStatus.TryGetValue(group.Key, out countTasksInGroup);

                if (countTasksInGroup != group.Value)
                {
                    changed = true;
                    TasksGroupedByStatus = tasksGroupedByStatus;
                    break;
                }
            }

            return changed;
        }
    }
}
