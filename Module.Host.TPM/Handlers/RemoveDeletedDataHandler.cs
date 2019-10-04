using ProcessingHost.Handlers;
using System;
using Looper.Core;
using Utility.LogWriter;
using System.Diagnostics;
using Persist;
using Module.Persist.TPM.Utils;

namespace Module.Host.TPM.Handlers
{
    /// <summary>
    /// Класс для удаления из БД ненужных данных
    /// </summary>
    class RemoveDeletedDataHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            ILogWriter handlerLogger = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                handlerLogger = new FileLogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Remove deleted data from DB started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

                using (DatabaseContext context = new DatabaseContext())
                {
                    // удаление записей удаленных от 6 недель назад из таблицы BaseLine
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            string sqlCommand = "DELETE FROM [dbo].[BaseLine] WHERE [Disabled] = 1 and [DeletedDate] IS NOT NULL and DATEADD(DAY, -(42), SYSDATETIME()) >= [DeletedDate]";
                            context.Database.ExecuteSqlCommand(sqlCommand);

                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();

                            if (handlerLogger != null)
                            {
                                handlerLogger.Write(true, e.ToString(), "Error");
                            }
                        }
                    }

                    // удаление записей созданных от 2 недель назад из таблицы ChangesIncident
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            string sqlCommand = "DELETE FROM [dbo].[ChangesIncident] WHERE [CreateDate] IS NOT NULL and DATEADD(DAY, -(14), SYSDATETIME()) >= [CreateDate]";
                            context.Database.ExecuteSqlCommand(sqlCommand);

                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();

                            if (handlerLogger != null)
                            {
                                handlerLogger.Write(true, e.ToString(), "Error");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);

                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, e.ToString(), "Error");
                }
            }
            finally
            {
                logger.Debug("Finish '{0}'", info.HandlerId);
                sw.Stop();

                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, String.Format("Remove deleted data from DB ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds), "Message");
                }
            }
        }
    }
}
