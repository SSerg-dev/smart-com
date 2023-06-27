using Core.Security;
using Core.Security.Models;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Frontend.TPM.Util
{
    public class ReAuthorizationManager : IAuthorizationManager
    {
        private readonly RoleInfo currentRole;
        private readonly UserInfo currentUser;

        public ReAuthorizationManager(DatabaseContext context, Guid userId, Guid roleId)
        {
            Role role = context.Set<Role>().FirstOrDefault(g => g.Id == roleId);
            User user = context.Set<User>().FirstOrDefault(g => g.Id == userId);
            currentRole = new RoleInfo(this)
            {
                DisplayName = role.DisplayName,
                IsDefault = true,
                SystemName = role.SystemName
            };
            currentUser = new UserInfo(this)
            {
                Login = user.Name
            };
        }

        public IEnumerable<AccessPointInfo> GetAccessPointsForRole(RoleInfo roleInfo)
        {
            return null;
        }

        public RoleInfo GetCurrentRole()
        {
            return currentRole;
        }

        public string GetCurrentRoleName()
        {
            string role = currentRole.SystemName;
            return role;
        }

        public UserInfo GetCurrentUser()
        {
            return currentUser;
        }

        public IEnumerable<RoleInfo> GetRolesForUser()
        {
            return new[] { currentRole };
        }

        public void SetCurrentRole(RoleInfo roleInfo)
        {
        }

        public bool ValidateUser(string login, string password)
        {
            return true;
        }
    }
}
