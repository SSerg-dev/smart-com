using Persist;
using Persist.Model;
using Persist.Model.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Utils
{
	/// <summary>
	/// Класс для получения почты, userId и ограничений получателей,
	/// </summary>
	public static class NotificationsHelper
	{
		/// <summary>
		/// Получение списка ограничений по UserId
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public static List<Constraint> GetConstraitnsByUserId(Guid userId, DatabaseContext context)
		{
			
			Guid defaultUserRole = context.UserRoles
				.Where(x => x.UserId == userId && x.IsDefault)
				.Select(y => y.Id).FirstOrDefault();
			List<Constraint> constraints = context.Constraints
				.Where(x => defaultUserRole == x.UserRoleId).ToList();

			return constraints;
		}

		/// <summary>
		/// Поиск UserId получателей (Recipients)
		/// </summary>
		/// <param name="recipients"></param>
		/// <param name="context"></param
		/// <param name="errors"></param>
		/// <param name="roles">Список необходимых ролей</param>
		/// <returns></returns>
		public static List<Guid> GetUserIdsByRecipients(string notificationName, List<Recipient> recipients, DatabaseContext context, out IList<string> errors, out IList<string> guaranteedEmails, string[] roles = null)
		{
			errors = new List<string>();
			guaranteedEmails = new List<string>();
			List<Constraint> constraints = new List<Constraint>();
			List<Guid> users = new List<Guid>();
			List<Tuple<string, string>> recipientsWithType = new List<Tuple<string, string>>();
			Guid mailNotificationSettingsId = context.MailNotificationSettings
								.Where(y => y.Name == notificationName && !y.Disabled)
								.Select(x => x.Id).FirstOrDefault();

			foreach (Recipient recipient in recipients)
			{
				recipientsWithType.Add(new Tuple<string, string>(recipient.Value, recipient.Type));
			}

			foreach (Tuple<string, string> valueAndType in recipientsWithType)
			{
				IQueryable<Guid> userIds = null;

				// Поиск пользователя по разным типам получателя (Email, User, Role)
				switch (valueAndType.Item2)
				{
					case "Email":
						guaranteedEmails.Add(valueAndType.Item1);
						break;
					case "User":
						userIds = context.Users.Where(x => x.Name == valueAndType.Item1 && !x.Disabled).Select(y => y.Id);

						if (userIds.Count() > 1)
						{
							errors.Add(String.Format("{0} users found with same name (login): {1}", userIds.Count(), valueAndType.Item1));
							continue;
						}
						else if (!userIds.Any())
						{
							errors.Add(String.Format("There is no User with such name (login): {0}", valueAndType.Item1));
							continue;
						}

						if (roles != null || roles?.GetLength(0) > 0)
						{
							string[] roleIds = context.Roles
								.Where(x => roles.Contains(x.SystemName) && !x.Disabled)
								.Select(y => y.Id.ToString()).ToArray();

							if (!context.UserRoles.Any(x => x.IsDefault && roleIds.Contains(x.RoleId.ToString()) && x.User.Name == valueAndType.Item1))
							{
								errors.Add(String.Format("The user with the name {0} has an inappropriate default role", valueAndType.Item1));
								continue;
							}
						}
						users.Add(userIds.FirstOrDefault());

						break;
					case "Role":
						Guid roleId = context.Roles.Where(x => x.SystemName == valueAndType.Item1).Select(y => y.Id).FirstOrDefault();

						if (roleId.Equals(Guid.Empty))
						{
							errors.Add(String.Format("The role with such SystemName not found: {0}", valueAndType.Item1));
							continue;
						}

						if (roles != null && roles.GetLength(0) > 0)
						{
							if (!roles.Contains(valueAndType.Item1))
							{
								errors.Add(String.Format("The {0} role is not in the appropriate roles ({1})", valueAndType.Item1, string.Join(", ", roles)));
								continue;
							}

							users.AddRange(context.UserRoles
								.Where(x => x.RoleId == roleId && x.IsDefault && roles.Contains(x.Role.SystemName))
								.Select(y => y.UserId));
						}
						else
						{
							users.AddRange(context.UserRoles
								.Where(x => x.RoleId == roleId && x.IsDefault)
								.Select(y => y.UserId));
						}
						break;
				}
			}

			// Оставляем только уникальные Guid 
			users = users.Union(users).ToList();
			return users;
		}

		/// <summary>
		/// Найти получаетей по имени оповещения
		/// </summary>
		/// <param name="notificationName"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public static List<Recipient> GetRecipientsByNotifyName(string notificationName, DatabaseContext context, bool getUserFromSettings = true)
		{
			Guid mailNotificationSettingsId = context.MailNotificationSettings
				.Where(y => y.Name == notificationName && !y.Disabled)
				.Select(x => x.Id).FirstOrDefault();

			// Находим гарантированного получателя
			string toMail = String.Empty;
			if (getUserFromSettings)
			{
				toMail = context.MailNotificationSettings
				.Where(y => y.Id == mailNotificationSettingsId)
				.Select(x => x.To).FirstOrDefault();

			}

			// Находим дополнительных получателей
			List<Recipient> recipients = context.Recipients
				.Where(x => x.MailNotificationSettingId == mailNotificationSettingsId).ToList();

			if (!String.IsNullOrEmpty(toMail))
			{
				recipients.Add(new Recipient() { Value = toMail, Type = "Email" });
			}

			return recipients;
		}

		/// <summary>
		/// Получить все email нотификации по её названию из Recipients
		/// </summary>
		/// <param name="withLimits"></param>
		/// <returns></returns>
		public static List<string> GetUsersEmail(List<Guid> users, DatabaseContext context)
		{
			List<string> usersEmail = context.Users.Where(x => !x.Disabled && users.Contains(x.Id)).Select(x => x.Email).ToList();

			return usersEmail;
		}

		/// <summary>
		/// Получить все id юзеров с определённой ролью
		/// </summary>
		/// <param name="roleName"></param>
		/// <param name="context"></param>
		/// <param name="onlyDefault">По умолчанию ищутся пользователи с delault ролью</param>
		/// <returns></returns>
		public static List<Guid> GetUsersIdsWithRole(string roleName, DatabaseContext context, bool onlyDefault = true)
		{
			List<Guid> userIds = new List<Guid>();
			if (onlyDefault)
			{
				userIds = context.UserRoles
					.Where(x => x.Role.SystemName == roleName && !x.User.Disabled && !x.Role.Disabled && x.IsDefault)
					.Select(x => x.UserId).ToList();
			}
			else
			{
				userIds = context.UserRoles
					.Where(x => x.Role.SystemName == roleName && !x.User.Disabled && !x.Role.Disabled)
					.Select(x => x.UserId).ToList();
			}

			return userIds;
		}
	}
}
