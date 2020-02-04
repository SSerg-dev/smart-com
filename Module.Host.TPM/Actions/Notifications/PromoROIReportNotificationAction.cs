using Core.Dependency;
using Core.Settings;
using Module.Frontend.TPM.Controllers;
using Module.Persist.TPM.Utils;

using NLog;
using NLog.Layouts;
using NLog.Targets;

using Persist;
using Persist.Model;
using Persist.Model.Settings;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Module.Host.TPM.Actions.Notifications
{
    public class PromoROIReportNotificationAction : BaseNotificationAction
    {
        private readonly string _notificationName = "PROMO_ROI_REPORT_NOTIFICATION";
        private readonly ISettingsManager _settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));

        public override void Execute()
        {
            var stopWatch = Stopwatch.StartNew();
            try
            {
                using (var databaseContext = new DatabaseContext())
                {
                    var currentMarsWeekName = (databaseContext.Database.SqlQuery<string>(
                        $"SELECT TOP(1) MarsWeekName FROM Dates WHERE OriginalDate = '{DateTime.Now}'")).FirstOrDefault();

                    if (currentMarsWeekName ==  _settingsManager.GetSetting<string>("PROMO_ROI_REPORT_MARS_WEEK_NAME", "W4"))
                    {
                        var currentNotification = databaseContext.Set<MailNotificationSetting>().FirstOrDefault(x => x.Name == _notificationName);
                        if (currentNotification == null)
                        {
                            logger.Warn($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find {nameof(MailNotificationSetting)} with name {_notificationName}.");
                            Warnings.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find {nameof(MailNotificationSetting)} with name {_notificationName}.");
                            return;
                        }

                        var users = databaseContext.Set<User>().Where(x => !x.Disabled).ToList();
                        var recipients = NotificationsHelper.GetRecipientsByNotifyName(_notificationName, databaseContext);

                        // Получаем инфу из грида получателей.
                        var recipientUserNames = recipients.Where(x => x.Type.ToLower() == "user").Select(x => x.Value);
                        var recipientRoleSystemNames = recipients.Where(x => x.Type.ToLower() == "role").Select(x => x.Value);
                        var recipientEmails = recipients.Where(x => x.Type.ToLower() == "email").Select(x => x.Value).ToList();

                        // Получаем пользователей из инфы из грида получателей.
                        var recipientUsersByUserName = users.Where(x => recipientUserNames.Any(y => y == x.Name));
                        var recipientRoles = databaseContext.Set<Role>().ToList().Where(x => !x.Disabled && recipientRoleSystemNames.Any(y => y == x.SystemName));
                        var recipientUsersByRoles = databaseContext.Set<UserRole>().ToList().Where(x => recipientRoles.Any(y => y.Id == x.RoleId)).Where(x => !x.User.Disabled).Select(x => x.User);
                        var recipientUsersByEmails = users.Where(x => recipientEmails.Any(y => y == x.Email));

                        // Формируем список всех получателей.
                        var allRecipientUsers = recipientUsersByUserName.Union(recipientUsersByRoles).Union(recipientUsersByEmails).Distinct();

                        // Производим валидацию e-mail адресов пользователей-получателей.
                        var recipientUsersWithInvalidEmail = new List<User>();
                        foreach (var recipientUser in allRecipientUsers.ToList())
                        {
                            var emailIsValid = ValidateEmail(recipientUser.Email);
                            if (!emailIsValid)
                            {
                                recipientUsersWithInvalidEmail.Add(recipientUser);

                                logger.Warn($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Email not found or invalid for user: {recipientUser.Name}.");
                                Warnings.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Email not found or invalid for user: {recipientUser.Name}.");
                            }
                        }
                        allRecipientUsers = allRecipientUsers.Except(recipientUsersWithInvalidEmail);

                        var emailsWithoutUser = recipientEmails.Where(x => !users.Any(y => y.Email == x)).Distinct().Except(new List<string> { currentNotification.To });
                        foreach (var emailWithoutUser in emailsWithoutUser)
                        {
                            logger.Warn($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): There is no User with such email: {emailWithoutUser}.");
                            Warnings.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): There is no User with such email: {emailWithoutUser}.");
                        }

                        // TEST ONLY 
                        //LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(@"C:\Smartcom\Projects\TPM\Development\Support_Admin\Source\ProcessingService.TPM\NLog.config");
                        // TEST ONLY 

                        var mailLoggingRule = LogManager.Configuration?.LoggingRules?.FirstOrDefault(x => x.LoggerNamePattern == "MailNotification");
                        if (mailLoggingRule == null)
                        {
                            Errors.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Mail logging rule wasn't found.");
                            logger.Error($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Mail logging rule wasn't found.");
                            return;
                        }

                        var mailTarget = mailLoggingRule.Targets.FirstOrDefault() as MailTarget;
                        if (mailTarget == null)
                        {
                            Errors.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Mail logging target wasn't found.");
                            logger.Error($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Mail logging target wasn't found.");
                            return;
                        }

                        if (!recipients.Any())
                        {
                            logger.Warn($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find recipients.");
                            Warnings.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find recipients.");
                            return;
                        }

                        var to = currentNotification.To;
                        if (string.IsNullOrEmpty(to))
                        {
                            logger.Error($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find to email.");
                            Errors.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find to email.");
                            return;
                        }
                        else if (!ValidateEmail(to))
                        {
                            logger.Error($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): To e-mail is invalid.");
                            Errors.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): To e-mail is invalid.");
                            return;
                        }
                        else
                        {
                            Results.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): To: {to}", new Object());
                        }

                        var recipientCopy = currentNotification.CC;
                        if (string.IsNullOrEmpty(recipientCopy))
                        {
                            logger.Warn($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find recipient copy.");
                            //Warnings.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find recipient copy.");
                            //return;
                        }
                        else if (!ValidateEmail(recipientCopy))
                        {
                            logger.Error($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): CC e-mail is invalid.");
                            Errors.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): CC e-mail is invalid.");
                            return;
                        }
                        else
                        {
                            Results.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): CC: {recipientCopy}", new Object());
                        }

                        var recipientShadowCopy = currentNotification.BCC;
                        if (string.IsNullOrEmpty(recipientShadowCopy))
                        {
                            logger.Warn($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find recipient shadow copy.");
                            //Warnings.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find recipient shadow copy.");
                            //return;
                        }
                        else if (!ValidateEmail(recipientShadowCopy))
                        {
                            logger.Error($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): BCC e-mail is invalid.");
                            Errors.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): BCC e-mail is invalid.");
                            return;
                        }
                        else
                        {
                            Results.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): BCC: {recipientShadowCopy}", new Object());
                        }

                        var subject = currentNotification.Subject;
                        if (string.IsNullOrEmpty(subject))
                        {
                            logger.Warn($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find subject.");
                            Warnings.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find subject.");
                            return;
                        }
                        else
                        {
                            string runtimeName = AppSettingsManager.GetSetting("RUNTIME_ENVIROMENT", "");
                            if (!String.IsNullOrEmpty(runtimeName))
                            { 
                                subject = $"{runtimeName} {subject}";
                            }
                            Results.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Subject: {subject}", new Object());
                        }

                        var body = currentNotification.Body;
                        if (string.IsNullOrEmpty(body) || (body.Length > 0 && !Char.IsLetterOrDigit(body[0])))
                        {
                            body = String.Empty;
                            logger.Warn($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Can't find body.");
                        }
                        else
                        {
                            Results.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Body: {body}", new Object());
                        }

                        var marsDayFullNameWithoutYear = _settingsManager.GetSetting<string>("PROMO_ROI_REPORT_MARS_DAY_FULL_NAME_WITHOUT_YEAR", "P5 W1 D1");
                        var originalDateForOnlyCurrentYear = (databaseContext.Database.SqlQuery<DateTime>(
                            $"SELECT TOP(1) OriginalDate FROM Dates WHERE MarsYear = {DateTimeOffset.Now.Year} AND MarsDayFullName = '{DateTimeOffset.Now.Year + " " + marsDayFullNameWithoutYear}' ORDER BY OriginalDate")).FirstOrDefault();

                        var needSendOnlyCurrentYear = DateTime.Now >= originalDateForOnlyCurrentYear;

                        // Формируем итоговый список адресов получателей.
                        var allRecipientEmails = allRecipientUsers.Select(x => x.Email).ToList();
                        allRecipientEmails.Add(to);
                        allRecipientEmails = allRecipientEmails.Distinct().ToList();

                        foreach (var recipientEmail in allRecipientEmails)
                        {
                            var currentUser = users.FirstOrDefault(x => x.Email == recipientEmail);

                            Role defaultRole = null;
                            if (currentUser != null)
                            {
                                var defaultUserRoleForCurrentUser = databaseContext.Set<UserRole>().FirstOrDefault(x => x.UserId == currentUser.Id && x.IsDefault);
                                defaultRole = databaseContext.Set<Role>().FirstOrDefault(x => !x.Disabled && x.Id == defaultUserRoleForCurrentUser.RoleId);
                            }

                            if (!needSendOnlyCurrentYear)
                            {
                                SendEmailPreviousYearPromo(databaseContext, currentUser, mailTarget, recipientEmail, recipientCopy, recipientShadowCopy, subject, body, defaultRole);
                            }
                            SendEmailCurrentYearPromo(databaseContext, currentUser, mailTarget, recipientEmail, recipientCopy, recipientShadowCopy, subject, body, defaultRole);
                        }

                        if (allRecipientEmails.Any())
                        {
                            logger.Info($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Notification sent to: {string.Join(", ", allRecipientEmails)}.");
                            Results.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Notification sent to: {string.Join(", ", allRecipientEmails)}", new Object());
                        }
                        else
                        {
                            logger.Info($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): There are no appropriate recipinets for notification: {_notificationName}.");
                            Warnings.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): There are no appropriate recipinets for notification: {_notificationName}.");
                        }
                    }
                    else
                    {
                        logger.Warn($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Current mars week name is {currentMarsWeekName}");
                        Warnings.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Current mars week name is {currentMarsWeekName}");
                    }
                }

                logger.Info($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Succeeded.");
            }
            catch (Exception exception)
            {
                logger.Error(exception, $"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): Failed.");
                Errors.Add($"({nameof(PromoROIReportNotificationAction.Execute)} method. Timing: {stopWatch.Elapsed}): failed. Exception: {exception.ToString()}");
            }
            finally
            {
                stopWatch.Stop();
                logger.Info($"({nameof(PromoROIReportNotificationAction.Execute)} method. Duration: {stopWatch.Elapsed}): Completed.");
            }
        }

        private void SendEmailPreviousYearPromo(DatabaseContext databaseContext, User user, MailTarget mailTarget, string recipientEmail, string recipientCopy, string recipientShadowCopy, string subject, string body, Role defaultRole)
        {
            var stopWatch = Stopwatch.StartNew();
            try
            {
                var year = DateTimeOffset.Now.Year - 1;
                subject += " " + year;
                var promoROIReportFileName = PromoROIReportsController.ExportXLSXYearStatic(databaseContext, user, year, defaultRole);
                SendEmail(mailTarget, recipientEmail, recipientCopy, recipientShadowCopy, subject, body, promoROIReportFileName, year);
                logger.Info($"({nameof(PromoROIReportNotificationAction.SendEmailPreviousYearPromo)} method. Timing: {stopWatch.Elapsed}): Succeeded.");
            }
            catch (Exception exception)
            {
                logger.Error(exception, $"({nameof(PromoROIReportNotificationAction.SendEmailPreviousYearPromo)} method. Timing: {stopWatch.Elapsed}): Failed.");
                throw exception;
            }
            finally
            {
                stopWatch.Stop();
                logger.Info($"({nameof(PromoROIReportNotificationAction.SendEmailPreviousYearPromo)} method. Duration: {stopWatch.Elapsed}): Completed.");
            }
        }

        private void SendEmailCurrentYearPromo(DatabaseContext databaseContext, User user, MailTarget mailTarget, string recipientEmail, string recipientCopy, string recipientShadowCopy, string subject, string body, Role defaultRole)
        {
            var stopWatch = Stopwatch.StartNew();
            try
            {
                var year = DateTimeOffset.Now.Year;
                subject += " " + year;
                var promoROIReportFileName = PromoROIReportsController.ExportXLSXYearStatic(databaseContext, user, year, defaultRole);
                SendEmail(mailTarget, recipientEmail, recipientCopy, recipientShadowCopy, subject, body, promoROIReportFileName, year);
                logger.Info($"({nameof(PromoROIReportNotificationAction.SendEmailCurrentYearPromo)} method. Timing: {stopWatch.Elapsed}): Succeeded.");
            }
            catch (Exception exception)
            {
                logger.Error(exception, $"({nameof(PromoROIReportNotificationAction.SendEmailCurrentYearPromo)} method. Timing: {stopWatch.Elapsed}): Failed.");
                throw exception;
            }
            finally
            {
                stopWatch.Stop();
                logger.Info($"({nameof(PromoROIReportNotificationAction.SendEmailCurrentYearPromo)} method. Duration: {stopWatch.Elapsed}): Completed.");
            }
        }

        public void SendEmail(MailTarget mailTarget, string recipient, string recipientCopy, string recipientShadowCopy, string subject, string body, string attachmentFileName, int year)
        {
            var stopWatch = Stopwatch.StartNew();
            try
            {
                var smtpClient = new SmtpClient();
                var mailMessage = new MailMessage();
                var from = ((SimpleLayout)mailTarget.From).OriginalText; 
                var fromMailAddress = new MailAddress(from);
                var host = ((SimpleLayout)mailTarget.SmtpServer).OriginalText;

                smtpClient.Host = host;
                smtpClient.Port = mailTarget.SmtpPort;
                smtpClient.UseDefaultCredentials = true;

                var user = ((SimpleLayout)mailTarget.SmtpUserName).OriginalText;
                var password = ((SimpleLayout)mailTarget.SmtpPassword).OriginalText;

                if (!String.IsNullOrEmpty(user) && !String.IsNullOrEmpty(password)) 
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(user, password);
                }

                smtpClient.EnableSsl = mailTarget.EnableSsl;

                mailMessage.From = fromMailAddress;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = false;
                mailMessage.Body = body;

                if (!string.IsNullOrEmpty(recipientCopy))
                {
                    mailMessage.CC.Add(recipientCopy);
                }

                if (!string.IsNullOrEmpty(recipientShadowCopy))
                {
                    mailMessage.Bcc.Add(recipientShadowCopy);
                }

                mailMessage.Attachments.Add(CreateAttachmet(attachmentFileName, year));
                mailMessage.To.Add(recipient);
            
                smtpClient.Send(mailMessage);
                logger.Info($"({nameof(PromoROIReportNotificationAction.SendEmail)} method. Timing: {stopWatch.Elapsed}): Succeeded.");
            }
            catch (Exception exception)
            {
                logger.Error(exception, $"({nameof(PromoROIReportNotificationAction.SendEmail)} method. Timing: {stopWatch.Elapsed}): Failed.");
                throw exception;
            }
            finally
            {
                stopWatch.Stop();
                logger.Info($"({nameof(PromoROIReportNotificationAction.SendEmail)} method. Duration: {stopWatch.Elapsed}): Completed.");
            }
        }

        
        private Attachment CreateAttachmet(string filePath, int year)
        {
            var stopWatch = Stopwatch.StartNew();
            try
            {
                string exportedFilesDirectory = AppSettingsManager.GetSetting<string>("EXPORT_DIRECTORY", @"C:\Windows\Temp\TPM\ExportFiles");
                filePath = Path.Combine(exportedFilesDirectory, filePath);

                var newFilePath = filePath.Insert(filePath.LastIndexOf("."), "_" + year);
                File.Move(filePath, newFilePath);
                filePath = newFilePath;

                logger.Info($"({nameof(PromoROIReportNotificationAction.CreateAttachmet)} method. Timing: {stopWatch.Elapsed}): File path: {filePath}.", new Object());
                Results.Add($"({nameof(PromoROIReportNotificationAction.CreateAttachmet)} method. Timing: {stopWatch.Elapsed}): File path: {filePath}.", new Object());

                var attachment = new Attachment(filePath, MediaTypeNames.Application.Octet);
                var disposition = attachment.ContentDisposition;

                disposition.CreationDate = File.GetCreationTime(filePath);
                disposition.ModificationDate = File.GetLastWriteTime(filePath);
                disposition.ReadDate = File.GetLastAccessTime(filePath);
                disposition.FileName = Path.GetFileName(filePath);
                disposition.Size = new FileInfo(filePath).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;

                logger.Info($"({nameof(PromoROIReportNotificationAction.CreateAttachmet)} method. Timing: {stopWatch.Elapsed}): Succeeded.");
                return attachment;
            }
            catch (Exception exception)
            {
                logger.Error(exception, $"({nameof(PromoROIReportNotificationAction.CreateAttachmet)} method. Timing: {stopWatch.Elapsed}): Failed.");
                throw exception;
            }
            finally
            {
                logger.Info($"({nameof(PromoROIReportNotificationAction.CreateAttachmet)} method. Duration: {stopWatch.Elapsed}): Completed.");
            }
        }

        private bool ValidateEmail(string email)
        {
            var stopWatch = Stopwatch.StartNew();
            try
            {
                var mailAddress = new MailAddress(email);
                logger.Info($"({nameof(PromoROIReportNotificationAction.ValidateEmail)} method. Timing: {stopWatch.Elapsed}): Succeeded.");
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, $"({nameof(PromoROIReportNotificationAction.ValidateEmail)} method. Timing: {stopWatch.Elapsed}): Failed.");
                return false;
            }
            finally
            {
                logger.Info($"({nameof(PromoROIReportNotificationAction.ValidateEmail)} method. Duration: {stopWatch.Elapsed}): Completed.");
            }
        }
    }
}
