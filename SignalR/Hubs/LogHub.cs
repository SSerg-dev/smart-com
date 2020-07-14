using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Module.Persist.TPM.Model.TPM;
using SignalR.Models;
using System.Data.Entity;
using System.Data;
using System.Web;
using System.Collections.Concurrent;
using System.IO;
using Microsoft.AspNet.SignalR.Hubs;
using SignalR.Database;
using System.Data.SqlClient;

namespace SignalR.Hubs
{
    public sealed class TimerStatusHandlerLogHub
    {
        public static Timer Timer { get; }

        static TimerStatusHandlerLogHub()
        {
            Timer = new Timer(async (object state) =>
            {
                try
                {
                    using (var databaseContext = new DatabaseContext())
                    {
                        Console.WriteLine("[HANDLER STATUS TIMER]: Check all groups.");
                        foreach (var activeHandler in DataLogHub.Data.Keys)
                        {
                            var sql = $"SELECT Status FROM LoopHandler WHERE Id = '{activeHandler.HandlerId}' AND (Status IS NULL OR Status = 'INPROGRESS')";
                            var response = databaseContext.Database.SqlQuery<string>(sql);

                            if (await response.CountAsync() == 0)
                            {
                                activeHandler.Timer.Change(Timeout.Infinite, Timeout.Infinite);
                                DataLogHub.Data.TryRemove(activeHandler);
                                Console.WriteLine($"[TIMER]: Stop timer for group {activeHandler.HandlerId}");
                            }
                        }
                    }

                    if (DataLogHub.Data.Count == 0)
                    {
                        Console.WriteLine("[HANDLER STATUS TIMER]: Stop");
                        Timer.Change(Timeout.Infinite, Timeout.Infinite);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            });
        }
    }

    public static class DataLogHub
    {
        public static ConcurrentDictionary<HandlerLogData, ConcurrentDictionary<UserLogData, byte>> Data { get; }  = 
            new ConcurrentDictionary<HandlerLogData, ConcurrentDictionary<UserLogData, byte>>();

        public static void TryAddOrCreateCouple(
            ConcurrentDictionary<HandlerLogData, ConcurrentDictionary<UserLogData, byte>> data, HandlerLogData handlerLogData, UserLogData userLogData)
        {
            try
            {
                if (data.ContainsKey(handlerLogData))
                {
                    if (!data[handlerLogData].Any(x => x.Key.Equals(userLogData)))
                    {
                        data[handlerLogData].TryAdd(userLogData, 0);
                    }
                }
                else
                {
                    data.TryAdd(handlerLogData, new ConcurrentDictionary<UserLogData, byte> { [userLogData] = 0 });
                    handlerLogData.Timer.Change(0, 500);
                }

                Console.WriteLine("[HANDLER STATUS TIMER]: Try start");
                TimerStatusHandlerLogHub.Timer.Change(0, 1000);
                Console.WriteLine($"[DATA]: Add {userLogData.ConnectionId} to the dictionary.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public static void RemoveUserLogData(
            ConcurrentDictionary<HandlerLogData, ConcurrentDictionary<UserLogData, byte>> data, HandlerLogData handlerLogData, UserLogData userLogData)
        {
            try
            {
                if (data.ContainsKey(handlerLogData))
                {
                    data[handlerLogData].TryRemove(userLogData);
                    if (data[handlerLogData].Count() == 0)
                    {
                        var handlerLogDataForRemove = Data.First(x => x.Key.Equals(handlerLogData)).Key;
                        handlerLogDataForRemove.Timer.Change(Timeout.Infinite, Timeout.Infinite);
                        Console.WriteLine($"[TIMER]: Stop for group {handlerLogDataForRemove.HandlerId}");
                        data.TryRemove(handlerLogDataForRemove);
                    }

                    Console.WriteLine($"[DATA]: Remove {userLogData.ConnectionId} from the dictionary.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }

    public static class FileLogHub
    {
        public static string GetPathToLogText(string group) =>
            $@"C:\Windows\Temp\TPM\HandlerLogs\{group}.txt";

        public static string GetLogText(string path)
        {
            try
            {
                return File.Exists(path) ? File.ReadAllText(path) : "The log file is not exist.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }

    public static class Log
    {
        public static Dictionary<string, string> MessageType { get; } = new Dictionary<string, string>()
        {
            ["Message"] = "INFO",
            ["Error"] = "ERROR",
            ["Warning"] = "WARNING"
        };

        public static string GenerateLogLine(string tag, string message)
        {
            return $"[{tag}]: {message}";
        }

        /*
        public static List<Dictionary<string, string>> ParseLog(string lines)
        {
            var list = new List<Dictionary<string, string>>();

            foreach (var line in lines)
            {
            }

            return list;
        }
        */
    }

    public class LogHub : Hub
    {
        public void BindMeWithHandler(string handlerId)
        {
            try
            {
                var log = FileLogHub.GetLogText(FileLogHub.GetPathToLogText(handlerId));
                var timerForUpdateGroupLog = CreateTimerForUpdateGroupLog(handlerId, log);
                var handlerLogData = new HandlerLogData(handlerId, log, timerForUpdateGroupLog);
                var userLogData = new UserLogData(Context.ConnectionId, handlerId);

                DataLogHub.TryAddOrCreateCouple(DataLogHub.Data, handlerLogData, userLogData);
                AddToGroup(handlerId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public Task AddToGroup(string group)
        {
            try
            {
                return Groups.Add(Context.ConnectionId, group);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public Task RemoveFromGroup(string group)
        {
            try
            {
                return Groups.Remove(Context.ConnectionId, group);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public string GetGroupNameByConenctionId(string connectionId)
        {
            try
            {
                var currentConnectionId = Guid.Parse(connectionId);
                foreach (var userLogDataHashSet in DataLogHub.Data.Values)
                {
                    var currentUserLogData = userLogDataHashSet.Select(x => x.Key).FirstOrDefault(x => x.ConnectionId == currentConnectionId);
                    if (currentUserLogData != null)
                    {
                        return currentUserLogData.Group;
                    }
                }

                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public override Task OnConnected()
        {
            Console.WriteLine($"[CONNECT]: {Context.ConnectionId}");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine($"[DISCONNECT]: {Context.ConnectionId}");
            var handlerId = GetGroupNameByConenctionId(Context.ConnectionId);

            if (!String.IsNullOrEmpty(handlerId))
            {
                var handlerLogData = new HandlerLogData(handlerId);
                var userLogData = new UserLogData(Context.ConnectionId);

                DataLogHub.RemoveUserLogData(DataLogHub.Data, handlerLogData, userLogData);
                RemoveFromGroup(Context.ConnectionId);
            }

            return base.OnDisconnected(stopCalled);
        }

        public Timer CreateTimerForUpdateGroupLog(string group, string log)
        {
            Console.WriteLine($"[TIMER]: Start timer for group {group}");
            return new Timer((object state) =>
            {
                try
                {
                    Console.WriteLine($"[TIMER]: The group {group} contains {DataLogHub.Data[new HandlerLogData(group)].Count} persons.");

                    if (DataLogHub.Data.ContainsKey(new HandlerLogData(group)))
                    {
                        foreach (var client in DataLogHub.Data[new HandlerLogData(group)].Keys)
                        {
                            Console.WriteLine($" - {client.ConnectionId}");
                        }
                    }

                    Clients.Group(group).sendLog(log);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            });
        }
    }
}
