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
            LogWriter handlerLogger = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Remove deleted data from DB started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                using (DatabaseContext context = new DatabaseContext())
                {
                    // удаление записей удаленных от 6 недель назад из таблицы BaseLine
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            string sqlCommand = "DELETE FROM [DefaultSchemaSetting].[BaseLine] WHERE [Disabled] = 1 and [DeletedDate] IS NOT NULL and DATEADD(DAY, -(43), SYSDATETIME()) >= [DeletedDate]";
                            context.ExecuteSqlCommand(sqlCommand);

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
                            string sqlCommand = "DELETE FROM [DefaultSchemaSetting].[ChangesIncident] WHERE [CreateDate] IS NOT NULL and DATEADD(DAY, -(14), SYSDATETIME()) >= [CreateDate]";
                            context.ExecuteSqlCommand(sqlCommand);

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

                    // удаление временных записей из таблицы PromoProductsCorrection, измененных от двух дней назад
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            string sqlCommand = "DELETE FROM [DefaultSchemaSetting].[PromoProductsCorrection] WHERE [TempId] IS NOT NULL and DATEADD(DAY, -(2), SYSDATETIME()) >= [ChangeDate]";
                            context.ExecuteSqlCommand(sqlCommand);

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
                    handlerLogger.Write(true, String.Format("Remove deleted data from DB ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
