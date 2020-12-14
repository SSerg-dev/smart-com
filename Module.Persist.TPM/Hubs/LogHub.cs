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
using Utility.Azure;

namespace Module.Persist.TPM
{
    public class LogHub : Hub
    {
        /// <summary>
        /// Список подписчиков
        /// </summary>
        private static List<Subscriber> subscribers = new List<Subscriber>();

        /// <summary>
        /// Объект блокировки для синхронизации потоков
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// Хранение логов из Azure (для исключения лишних запросов в Azure)
        /// </summary>

        private static AzureLogData azureLog = new AzureLogData();

        /// <summary>
        /// Токен для отмены задачи
        /// </summary>
        private static CancellationTokenSource tokenCancelSource;

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        /// <summary>
        /// Подписаться на обновления
        /// </summary>
        /// <param name="promoId">ID промо</param>
        /// <param name="blocked">Статус открытого промо на клиенте</param>
        public void SubscribeStatus(Guid promoId, bool blocked)
        {
            DatabaseContext context = new DatabaseContext();
            bool needCreateTaskForCheckUpdates;

            // синхронизируем потоки (да вручную, специально)
            lock (locker)
            {
                // если это будет единственный подписчик, то необходимо создать задачу проверки обновлений
                needCreateTaskForCheckUpdates = subscribers.Count == 0;

                Subscriber subscriber = subscribers.FirstOrDefault(n => n.ConnectionID == Context.ConnectionId);

                if (subscriber == null)
                {
                    subscriber = new Subscriber(Context.ConnectionId, promoId, Context.User.Identity.Name, blocked, null);
                    subscribers.Add(subscriber);
                }
                else
                {
                    subscriber.NeedUpdateStatus = true;
                    subscriber.Blocked = blocked;
                }
            }

            if (needCreateTaskForCheckUpdates)
            {
                tokenCancelSource = new CancellationTokenSource();
                Task.Factory.StartNew(CheckUpdatesLogs);
            }

            context.Dispose();
        }

        /// <summary>
        /// Подписаться на получение содержимого лога
        /// </summary>
        public void SubscribeLog()
        {
            DatabaseContext context = new DatabaseContext();
            
            lock (locker)
            {
                // Без подписки на статус не может быть подписки на лог
                Subscriber subscriber = subscribers.FirstOrDefault(n => n.ConnectionID == Context.ConnectionId);
                if (subscriber != null)
                {
                    string contentLog;
                    string statusHandler;
                    DateTime? lastWriteDate;
                    BlockedPromo calculatingInfo = context.Set<BlockedPromo>().Where(n => n.PromoId == subscriber.PromoID).OrderByDescending(n => n.CreateDate).FirstOrDefault();

                    subscriber.NeedLog = true;

                    if (calculatingInfo != null)
                    {
                        GetContentOfLog(calculatingInfo.HandlerId, out contentLog, out lastWriteDate, out statusHandler);

                        if (contentLog != null)
                            Clients.Caller.addInfoInLog(contentLog);
                    }
                }
            }

            context.Dispose();
        }

        /// <summary>
        /// Отписаться от получения содержимого лога
        /// </summary>
        public void UnsubscribeLog()
        {
            lock (locker)
            {
                Subscriber subscriber = subscribers.FirstOrDefault(n => n.ConnectionID == Context.ConnectionId);
                if (subscriber != null)
                {
                    subscriber.NeedLog = false;
                    subscriber.LastModification = null;
                }
            }
        }

        /// <summary>
        /// Приостановить подписку на изменение блокировки промо
        /// </summary>
        public void UnsubscribeStatus()
        {
            lock (locker)
            {
                Subscriber subscriber = subscribers.FirstOrDefault(n => n.ConnectionID == Context.ConnectionId);
                if (subscriber != null)
                {
                    subscriber.NeedUpdateStatus = false;
                }
            }
        }

        /// <summary>
        /// Отключиться от хаба
        /// </summary>
        /// <returns></returns>
        public void DisconnectFromHub()
        {
            RemoveFromSubscribers();
        }

        /// <summary>
        /// Удалить подписчика при отключении от хаба
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            RemoveFromSubscribers();

            return base.OnDisconnected(stopCalled);
        }

        private void CheckUpdatesLogs()
        {         
            // завершить задачу, если она была отменена
            while (!tokenCancelSource.Token.IsCancellationRequested)
            {
                try
                {
                    lock (locker)
                    {
                        // группируем по Promo ID
                        var subscribersGrouped = subscribers.GroupBy(n => n.PromoID);

                        // каждую группу рассматриваем отдельно (добавим немного параллельности)
                        Parallel.ForEach(subscribersGrouped, (subscribersGroup) =>
                        {
                            DatabaseContext context = new DatabaseContext();
                            BlockedPromo calculatingInfo = context.Set<BlockedPromo>().Where(n => n.PromoId == subscribersGroup.Key).OrderByDescending(n => n.CreateDate).FirstOrDefault();

                            if (calculatingInfo != null)
                            {
                                string contentLog;
                                string statusHandler;
                                DateTime? lastWriteDate;

                                GetContentOfLog(calculatingInfo.HandlerId, out contentLog, out lastWriteDate, out statusHandler);

                                Parallel.ForEach(subscribersGroup, (subscriber) =>
                                {

                                    if (subscriber.NeedUpdateStatus && subscriber.Blocked == calculatingInfo.Disabled)
                                    {
                                        subscriber.NeedUpdateStatus = false; // пока клиент не подтвердит, сообщения не шлем
                                        Clients.Client(subscriber.ConnectionID).changeStatusPromo(!calculatingInfo.Disabled);
                                    }

                                    // если нужен лог и дата последнего чтения устарела, то шлем его клиенту  
                                    if (subscriber.NeedLog)
                                    {
                                        if (contentLog != null && (subscriber.StatusHandler != statusHandler || !subscriber.LastModification.HasValue || DateTime.Compare(subscriber.LastModification.Value, lastWriteDate.Value) < 0))
                                        {
                                            subscriber.LastModification = lastWriteDate;
                                            subscriber.StatusHandler = statusHandler;
                                            Clients.Client(subscriber.ConnectionID).addInfoInLog(contentLog);
                                        }
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
            string contentOfFileLog;

            DatabaseContext context = new DatabaseContext();
            LoopHandler handler = context.Set<LoopHandler>().FirstOrDefault(x => x.Id == handlerID);

            statusHandler = handler.Status;

            GetContentOfFileLog(handlerID, statusHandler, out lastWriteDate, out contentOfFileLog);            

            if (contentOfFileLog != null)
            {
                contentLog = JsonConvert.SerializeObject(new
                {
                    success = true,
                    data = contentOfFileLog,
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
        private void GetContentOfFileLog(Guid handlerID, string handlerStatus, out DateTime? lastWriteDate, out string contentOfLog)
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
                    lastWriteDate = SetAzureLogData(logFileName, handlerStatus);
                    if (lastWriteDate == null)
                    {
                        azureLog.Data = "";
                        azureLog.LogFileName = logFileName;
                    }
                    contentOfLog = azureLog.Data;
                }
            }
            catch (Exception e)
            {
                lastWriteDate = null;
                contentOfLog = null;
            }
        }

        /// <summary>
        /// Устанавливает данные из Azure
        /// </summary>
        /// <param name="logFileName">Имя файла логов</param>
        private DateTime? SetAzureLogData(string logFileName, string handlerStatus)
        {
            if (azureLog.LogFileName != logFileName || azureLog.LastHandlerStatus != handlerStatus)
            {
                var result = AzureBlobHelper.ReadTextFromBlob("HandlerLogs", logFileName);
                if (string.IsNullOrEmpty(result))
                {
                    return null;
                }
                azureLog = new AzureLogData()
                {
                    Data = result,
                    LogFileName = logFileName
                };
            }
            return DateTime.Today;
        }
        private void RemoveFromSubscribers()
        {
            lock (locker)
            {
                Subscriber subscriber = subscribers.FirstOrDefault(n => n.ConnectionID == Context.ConnectionId);
                if (subscriber != null)
                    subscribers.Remove(subscriber);

                // если подписчиков больше нет, то прекратить мониторинг
                if (subscribers.Count == 0 && tokenCancelSource != null)
                    tokenCancelSource.Cancel();
            }
        }
        /// <summary>
        /// Структура данных логов из Azure
        /// </summary>
        private struct AzureLogData
        {
            // Строка с логами
            public string Data { get; set; }

            // Наименование файла лога
            public string LogFileName { get; set; }

            // Наименование файла лога
            public string LastHandlerStatus { get; set; }
        }
    }

    public class Subscriber
    {
        /// <summary>
        /// ID соединения
        /// </summary>
        public string ConnectionID { get; private set; }

        /// <summary>
        /// ID промо
        /// </summary>
        public Guid PromoID { get; private set; }

        /// <summary>
        /// Имя пользователя (на всякий случай)
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
        /// True, если необходимо отправлять лог клиенту
        /// </summary>
        public bool NeedLog { get; set; }

        /// <summary>
        /// Статус промо у клиента
        /// </summary>
        public bool Blocked { get; set; }

        /// <summary>
        /// Нужно ли слать клиенту сообщение о смене блокировки промо
        /// (Иногда переход в режим просмотра и красного хедера идет дольше, чем расчет промо :( )
        /// </summary>
        public bool NeedUpdateStatus { get; set; }

        public Subscriber(string connectionID, Guid promoID, string userName, bool blocked, DateTime? lastModification)
        {
            ConnectionID = connectionID;
            PromoID = promoID;
            UserName = userName;
            Blocked = blocked;
            LastModification = lastModification;
            StatusHandler = "";
            NeedUpdateStatus = true;
        }
    }

    public static class Log
    {
        public static Dictionary<string, string> MessageType { get; } = new Dictionary<string, string>()
        {
            ["Message"] = "INFO",
            ["Error"] = "ERROR",
            ["Warning"] = "WARNING"
        };

        public static string GenerateLogLine(string tag, string message)
        {
            return $"[{tag}]: {message}";
        }
    }
}
