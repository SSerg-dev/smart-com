using System;
using System.Collections;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Frontend.TPM.FunctionalHelpers.ClientTreeFunction
{
    public static class ClientTreeHelper
    {
        public static List<ClientResult> GetChildrenBaseClient(ClientTree root, List<ClientTree> clients)
        {
            List<ClientTree> children = clients.Where(x => x.parentId == root.ObjectId).ToList();
            List<ClientResult> clientArray = new List<ClientResult>();
            foreach (var client1 in children)
            {
                if (client1.IsBaseClient)
                {
                    clientArray.Add(new ClientResult { Id = client1.ObjectId, Name = client1.Name });
                }
                else
                {
                    List<ClientTree> clientTrees2 = clients.Where(x => x.parentId == client1.ObjectId).ToList();
                    foreach (var client2 in clientTrees2)
                    {
                        if (client2.IsBaseClient)
                        {
                            clientArray.Add(new ClientResult { Id = client2.ObjectId, Name = client2.Name });
                        }
                        else
                        {
                            List<ClientTree> clientTrees3 = clients.Where(x => x.parentId == client2.ObjectId).ToList();
                            foreach (var client3 in clientTrees3)
                            {
                                if (client3.IsBaseClient)
                                {
                                    clientArray.Add(new ClientResult { Id = client3.ObjectId, Name = client3.Name });
                                }
                                else
                                {
                                    List<ClientTree> clientTrees4 = clients.Where(x => x.parentId == client3.ObjectId).ToList();
                                    foreach (var client4 in clientTrees4)
                                    {
                                        if (client4.IsBaseClient)
                                        {
                                            clientArray.Add(new ClientResult { Id = client4.ObjectId, Name = client4.Name });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return clientArray;
        }
    }
    public class ClientResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
