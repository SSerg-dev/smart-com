using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class BaseClientViewsController : ApiController
    {
        private readonly IAuthorizationManager authorizationManager;

        public BaseClientViewsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<ClientTree> GetConstraintedQuery(DatabaseContext Context, bool onlyBaseClient)
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            DateTime dt = DateTime.Now;
            IQueryable<ClientTree> query;
            if (onlyBaseClient)
            {
                query = Context.Set<ClientTree>().Where(x => DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0) && x.IsBaseClient == true);
            }
            else
            {
                query = Context.Set<ClientTree>().Where(x => DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0));
            }

            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);
            return query;
        }

        [ClaimsAuthorize]
        [Route("api/BaseClientViews")]
        [HttpGet]
        public IHttpActionResult GetBaseClientViews()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                List<int> clientTreesConstraint = GetConstraintedQuery(context, true).Select(g => g.ObjectId).ToList();
                DateTime dt = DateTime.Now;
                List<ClientTree> clientTrees = context.Set<ClientTree>().Where(x => DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)).ToList();
                ClientTree rootclient = clientTrees.FirstOrDefault(g => g.Type == "root");
                List<ClientTree> clientGroup = clientTrees.Where(g => g.parentId == rootclient.ObjectId && g.Type != "root").ToList();
                //Child root = new Root { Children = new List<Child>() };
                ChildExpand children = new ChildExpand { Id = 0, ObjectId = 0, Checked = false, Expanded = true, Text = "Clients", Children = new List<ChildAbstract>() };
                foreach (ClientTree client in clientGroup.OrderBy(g => g.ObjectId))
                {
                    List<ClientTree> clientlist1 = clientTrees.Where(g => g.parentId == client.ObjectId).ToList();
                    List<ClientTree> clientlist2 = clientTrees.Where(g => clientlist1.Select(h => h.ObjectId).Contains(g.parentId)).ToList();
                    List<ClientTree> clientlist3 = clientTrees.Where(g => clientlist2.Select(h => h.ObjectId).Contains(g.parentId)).ToList();
                    List<ClientTree> clientlist4 = clientTrees.Where(g => clientlist3.Select(h => h.ObjectId).Contains(g.parentId)).ToList();
                    List<ChildLeaf> children1 = new List<ChildLeaf>();
                    if (clientlist1.Any(g => g.IsBaseClient))
                    {
                        foreach (var client1 in clientlist1.Where(g => g.IsBaseClient))
                        {
                            if (clientTreesConstraint.Contains(client1.ObjectId))
                            {
                                children1.Add(new ChildLeaf
                                {
                                    Id = client1.Id,
                                    ObjectId = client1.ObjectId,
                                    Leaf = true,
                                    Checked = false,
                                    Text = client1.Name
                                });
                            }
                        }
                    }
                    if (clientlist2.Any(g => g.IsBaseClient))
                    {
                        foreach (var client2 in clientlist2.Where(g => g.IsBaseClient))
                        {
                            if (clientTreesConstraint.Contains(client2.ObjectId))
                            {
                                children1.Add(new ChildLeaf
                                {
                                    Id = client2.Id,
                                    ObjectId = client2.ObjectId,
                                    Leaf = true,
                                    Checked = false,
                                    Text = client2.Name
                                });
                            }
                        }
                    }
                    if (clientlist3.Any(g => g.IsBaseClient))
                    {
                        foreach (var client3 in clientlist3.Where(g => g.IsBaseClient))
                        {
                            if (clientTreesConstraint.Contains(client3.ObjectId))
                            {
                                children1.Add(new ChildLeaf
                                {
                                    Id = client3.Id,
                                    ObjectId = client3.ObjectId,
                                    Leaf = true,
                                    Checked = false,
                                    Text = client3.Name
                                });
                            }

                        }
                    }
                    if (clientlist4.Any(g => g.IsBaseClient))
                    {
                        foreach (var client4 in clientlist4.Where(g => g.IsBaseClient))
                        {
                            if (clientTreesConstraint.Contains(client4.ObjectId))
                            {
                                children1.Add(new ChildLeaf
                                {
                                    Id = client4.Id,
                                    ObjectId = client4.ObjectId,
                                    Leaf = true,
                                    Checked = false,
                                    Text = client4.Name
                                });
                            }
                        }
                    }
                    var isconstraints = clientTreesConstraint.Any(x => children1.Select(g => g.ObjectId).ToList().Contains(x));
                    if (clientlist1.Count > 0 && isconstraints)
                    {
                        ChildExpand child = new ChildExpand
                        {
                            Id = client.Id,
                            ObjectId = client.ObjectId,
                            Checked = false,
                            Text = client.Name,
                            Expanded = clientlist1.Count > 0,
                            Children = children1.OfType<ChildAbstract>().ToList()
                        };
                        children.Children.Add(child);
                    }
                    //else
                    //{
                    //    ChildLeaf child = new ChildLeaf
                    //    {
                    //        Id = client.Id,
                    //        ObjectId = client.ObjectId,
                    //        Checked = false,
                    //        Text = client.Name,
                    //        Leaf = true,
                    //        Children = children1.OfType<ChildAbstract>().ToList()
                    //    };
                    //    children.Children.Add(child);
                    //}

                }
                //root.Children.Add(childroot);
                var rrrr = new { success = true, children };
                var jsonser = JsonConvert.SerializeObject(rrrr, new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                var token = JToken.Parse(jsonser);
                return Content(HttpStatusCode.OK, token);
            }
        }
        public abstract class ChildAbstract
        {
            public int Id { get; set; }
            public int ObjectId { get; set; }
            public string Text { get; set; }
            public bool Checked { get; set; }
            public List<ChildAbstract> Children { get; set; }
        }
        public class ChildExpand : ChildAbstract
        {
            public bool Expanded { get; set; }
        }

        public class ChildLeaf : ChildAbstract
        {
            public bool Leaf { get; set; }
        }
    }
}
