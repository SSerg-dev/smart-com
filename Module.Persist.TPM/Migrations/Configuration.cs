namespace Module.Persist.TPM.Migrations
{
    using Core.Security;
    using global::Persist;
    using global::Persist.Migrations;
    using global::Persist.Model;
    using Module.Persist.TPM.Utils;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : PersistConfiguration
    {
        protected override void Seed(DatabaseContext context)
        {
            //SystemSettings(context);
            //Notifiers(context);
            //AddHandlers(context);
            //AddInterfaceSettings(context, true, true);
            //context.SaveChanges();
            //ApplyAccessPoints(context);
            //base.Seed(context);
            //AddSecurityData(context);
        }

        protected override void AddSecurityData(DatabaseContext context)
        {
            base.AddSecurityData(context);

            var users = new User[] {
                    new User() {
                        Sid = "S-1-5-21-1833980595-3398159026-1082415255-1127",
                        Name = "smartcom\\mikhail.volovich"
                    },
                    new User() {
                        Sid = "S-1-5-21-1833980595-3398159026-1082415255-1122",
                        Name = "smartcom\\maxim.molokanov"
                    },
                    new User() {
                        Sid = "S-1-5-21-1833980595-3398159026-1082415255-1320",
                        Name = "smartcom\\denis.moskvitin"
                    },
                    new User() {
                        Sid = "S-1-5-21-1833980595-3398159026-1082415255-1303",
                        Name = "smartcom\\artem.morozov"
                    },
                    new User() {
                        Sid = "S-1-5-21-1833980595-3398159026-1082415255-1298",
                        Name = "smartcom\\alexander.streltsov"
                    },
                    new User() {
                        Sid = "S-1-5-21-3086326434-3727772798-3018203464-1001",
                        Name = "DESKTOP-QGLKDB7\\odmin"
                    },
                    new User() {
                        Sid = "S-1-5-21-1423607127-3261154063-2530728265-1002",
                        Name = "MMDESKTOP\\Admin"
                    },
                    new User() {
                        Sid = "S-1-5-21-2262509386-4168703749-3092966379-1001",
                        Name = "DESKTOP-EHG7H2F\\Anatoliy Soldatov"
                    },
                    new User() {
                        Sid = "S-1-5-21-1359428713-1080950318-513062694-1002",
                        Name = "AndreyFilyushki\\andrey.filyushkin"
                    },
                    new User() {
                        Sid = "S-1-5-21-3490268862-552405869-1615426310-1002",
                        Name = "desktop-o4tq49c\\denismoskvitin"
                    },
                };

            context.Users.AddRange(users);

            context.UserRoles.AddRange(users.Select(u => new UserRole()
            {
                Role = context.Roles.First(x => x.SystemName == "Administrator"),
                User = u,
                IsDefault = true
            }));

            var userRole = new Role()
            {
                SystemName = "User",
                DisplayName = "Пользователь",
                IsAllow = true
            };
            context.Roles.Add(userRole);
            context.SaveChanges();
            ApplyAccessPointRoles(context);
        }

        protected override void ApplyAccessPoints(DatabaseContext context)
        {
            string apFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql\\TPM_AccessPoints.sql");
            context.ExecuteSqlCommand(File.ReadAllText(apFile));
            context.SaveChanges();
            base.ApplyAccessPoints(context);
        }

        protected override void AddInterfaceSettings(DatabaseContext context, bool updateHandlers, bool updateInterfaces)
        {
            base.AddInterfaceSettings(context, updateHandlers, updateInterfaces);
            if (updateHandlers || updateInterfaces)
            {
                ConfigManager.AddInputBaselineInterfaceSettings(updateInterfaces, context);

                ConfigManager.AddOutputIncrementalInterfaceSettings(updateInterfaces, context);
            }
        }

        protected override void SystemSettings(DatabaseContext context)
        {
            base.SystemSettings(context);

        }

        protected override void Notifiers(DatabaseContext context)
        {
            base.Notifiers(context);
        }

        private static bool IsUserAccess(AccessPoint ap)
        {
            return IsUserReadAccessPoint(ap);
        }

        private static bool IsUserReadAccessPoint(AccessPoint ap)
        {
            return UserResources.Any(r => IsReadAccessPoint(ap, r));
        }

        private static bool IsReadAccessPoint(AccessPoint ap, string resource)
        {
            string deletedResource = "Deleted" + resource;
            string historicalResource = "Historical" + resource;
            bool isResource = ap.Resource.Equals(resource) ||
                ap.Resource.Equals(deletedResource) ||
                ap.Resource.Equals(historicalResource);
            bool isAction = ap.Action.StartsWith("Get") ||
                ap.Action.Equals("Export");
            return (isResource && isAction);
        }

        private static string[] UserResources = new string[] {
        };

        public static void ApplyAccessPointRoles(DatabaseContext context)
        {
            const string userRoleName = "User";
            IEnumerable<Role> allRoles = context.Roles.ToList();

            Role userRole = allRoles.First(x => x.SystemName.Equals(userRoleName));

            IEnumerable<AccessPoint> accessPoints = context.AccessPoints.ToList();

            List<AccessPointRole> accessPointRoles = new List<AccessPointRole>();

            IEnumerable<AccessPoint> businessPlanningPoints = accessPoints.Where(x => IsUserAccess(x));
            accessPointRoles.AddRange(businessPlanningPoints.Select(x => new AccessPointRole()
            {
                AccessPointId = x.Id,
                AccessPoint = x,
                RoleId = userRole.Id,
                Role = userRole
            }));
            context.AccessPointRoles.AddRange(accessPointRoles);
        }
    }
}
