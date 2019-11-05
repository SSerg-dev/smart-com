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
	/// Класс для получения ограничений
	/// </summary>
	public static class ConstraintsHelper
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
		public static List<Guid> GetUserIdsByRecipients(List<Recipient> recipients, DatabaseContext context, out IList<string> errors, string[] roles = null)
		{
			errors = new List<string>();
			List<Constraint> constraints = new List<Constraint>();
			List<Guid> users = new List<Guid>();
			List<Tuple<string, string>> recipientsWithType = new List<Tuple<string, string>>();

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
						userIds = context.Users
							.Where(x => x.Email == valueAndType.Item1 && !x.Disabled)
							.Select(y => y.Id);
						if (userIds.Count() > 1)
						{
							//errors.Add(String.Format("{0} users found with same email: {1}", userIds.Count(), valueAndType.Item1));
							//continue;

							if (roles == null || roles?.GetLength(0) == 0)
							{
								users.Add(userIds.FirstOrDefault());
							}
							else
							{
								string[] roleIds = context.Roles
								.Where(x => roles.Contains(x.SystemName) && !x.Disabled)
								.Select(y => y.Id.ToString()).ToArray();

								foreach (Guid userId in userIds)
								{
									Guid userIdWithDefRole = context.UserRoles
									.Where(x => x.IsDefault && roleIds.Contains(x.RoleId.ToString()) && x.UserId == userId)
									.Select(y => y.UserId).FirstOrDefault();

									if (!userIdWithDefRole.Equals(Guid.Empty))
									{
										users.Add(userIdWithDefRole);
										break;
									}
								}

								if (users.Count() == 0)
								{
									errors.Add(String.Format("Users with email {0} has inappropriate default role.", valueAndType.Item1));
								}
							}
						}
						else if (Guid.Empty == userIds.FirstOrDefault())
						{
							errors.Add(String.Format("There is no User with such email: {0}", valueAndType.Item1));
						}
						else
						{
							if (roles == null || roles?.GetLength(0) == 0)
							{
								users.Add(userIds.FirstOrDefault());
							}
							else
							{
								string[] roleIds = context.Roles
								.Where(x => roles.Contains(x.SystemName) && !x.Disabled)
								.Select(y => y.Id.ToString()).ToArray();

								Guid userId = context.UserRoles
									.Where(x => x.IsDefault && roleIds.Contains(x.RoleId.ToString()) && x.User.Email == valueAndType.Item1)
									.Select(y => y.UserId).FirstOrDefault();

								if (!userId.Equals(Guid.Empty))
								{
									users.Add(userId);
								}
								else
								{
									errors.Add(String.Format("Users with email {0} has inappropriate default role.", valueAndType.Item1));
								}
							}
						}

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
		public static List<Recipient> GetRecipientsByNotifyName(string notificationName, DatabaseContext context)
		{
			Guid mailNotificationSettingsId = context.MailNotificationSettings
				.Where(y => y.Name == notificationName && !y.Disabled)
				.Select(x => x.Id).FirstOrDefault();

			// Находим гарантированного получателя
			string toMail = context.MailNotificationSettings
				.Where(y => y.Id == mailNotificationSettingsId)
				.Select(x => x.To).FirstOrDefault();

			// Находим дополнительных получателей
			List<Recipient> recipients = context.Recipients
				.Where(x => x.MailNotificationSettingId == mailNotificationSettingsId).ToList();

			if (toMail != null)
			{
				recipients.Add(new Recipient() { Value = toMail, Type = "Email" });
			}

			return recipients;
		}
	}
}
