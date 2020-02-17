using Core.Security;
using NLog;
using Persist;
using Persist.Model;
using System;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading;

namespace Frontend.TPM
{
    public class TestAdSecurityManager : ISecurityManager
    {
        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();

        public IQueryable<User> GetAllUsers()
        {
            return new DatabaseContext().Users;
        }

        public User GetUserByKey(Guid key)
        {
            return new DatabaseContext().Users.Find(key);
        }

        public User CreateUser(User user)
        {
            var pc = new PrincipalContext(
                ContextType.Domain,
                ConfigurationManager.AppSettings[Consts.DOMAIN_NAME],
                ConfigurationManager.AppSettings[Consts.AD_CONNECTION_USERNAME],
                ConfigurationManager.AppSettings[Consts.AD_CONNECTION_PASSWORD]
            );
            UserPrincipal _up = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, user.Name);

            UserPrincipal currUP = UserPrincipal.Current;
            var currPC = currUP.Context;
            
            UserPrincipal up = UserPrincipal.FindByIdentity(currPC, IdentityType.SamAccountName, user.Name);

            if (up == null)
            {
                throw new Exception("В Active Directory отсутствует пользователь с таким логином.");
            }
            else
            {
                user.Sid = up.Sid.ToString();
                if (String.IsNullOrEmpty(user.Email) && up.EmailAddress != null)
                {
                    user.Email = up.EmailAddress;
                }
            }

            var db = new DatabaseContext();

            if (db.Users.FirstOrDefault(u => u.Sid == user.Sid && !u.Disabled) != null)
            {
                throw new Exception("Пользователь с таким идентификатором уже существует.");
            }

            db.Users.Add(user);
            db.SaveChanges();

            return user;
        }

        public bool DeleteUser(Guid key)
        {
            logger.Debug("Starting user delete in AdSecurityManager.");
            var db = new DatabaseContext();
            var user = db.Users.Find(key);
            if (user == null)
            {
                logger.Debug("User delete error: user not found.");
                throw new Exception("Пользователь не найден.");
            }

            logger.Debug("User delete strting delete constrains.");
            var constraints = db.Constraints.Where(x => x.UserRole.UserId == user.Id);
            foreach (var item in constraints)
            {
                db.Constraints.Remove(item);
            }
            logger.Debug("User delete strting delete UserRoles.");
            var userRoles = db.UserRoles.Where(x => x.UserId == user.Id);
            foreach (var item in userRoles)
            {
                db.UserRoles.Remove(item);
            }
            logger.Debug("User delete strting delete GridSettings.");
            var gridSettings = db.GridSettings.Where(x => x.UserRole.UserId == user.Id);
            foreach (var item in gridSettings)
            {
                db.GridSettings.Remove(item);
            }

            user.Disabled = true;
            user.DeletedDate = DateTime.Now;
            logger.Debug("User delete ended successfully.");
            db.SaveChanges();

            return true;
        }

        public User UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(User user, string password)
        {
            throw new NotImplementedException();
        }
    }
}