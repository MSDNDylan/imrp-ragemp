using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace IMRP.Util
{
    public class Logging : Script
    {
        public static void Log(LogType logType, string logMessage)
        {
            try
            {
                string timeStamp = $"[{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}]";

                switch (logType)
                {
                    case LogType.ServerInfo:

                        IMRP.Database.Collections.ServerLog newSLog = new IMRP.Database.Collections.ServerLog();
                        newSLog.LogType = LogType.ServerInfo;
                        newSLog.Message = $"{timeStamp} {logMessage}";
                        newSLog.WasError = false;
                       // if (!Server.ServerInitalizing)  newSLog.AddNew();

                        NAPI.Util.ConsoleOutput($"{timeStamp} {logMessage}");

                        System.Threading.Tasks.Task.Run(() =>
                        {
                            NAPI.Task.Run(() =>
                            {
                                var task = Task.Run(async () => await Discord.Bot.SendIMRPLogAsync($"{timeStamp} {logMessage}"));
                                task.Wait();
                            });
                        });
                        break;

                    case LogType.ServerError:

                        IMRP.Database.Collections.ServerLog newSLog1 = new IMRP.Database.Collections.ServerLog();
                        newSLog1.LogType = LogType.ServerError;
                        newSLog1.Message = $"{timeStamp} {logMessage}";
                        newSLog1.WasError = true;
                        if (!Server.ServerInitalizing) newSLog1.AddNew();

                        NAPI.Util.ConsoleOutput($"{timeStamp} [SERVER ERROR] {logMessage}");
                        break;

                    case LogType.DatabaseInfo:

                        IMRP.Database.Collections.DatabaseLog newDBLog = new IMRP.Database.Collections.DatabaseLog();
                        newDBLog.LogType = LogType.DatabaseInfo;
                        newDBLog.Message = $"{timeStamp} {logMessage}";
                        newDBLog.WasError = false;
                        if (!Server.ServerInitalizing) newDBLog.AddNew();

                        NAPI.Util.ConsoleOutput($"{timeStamp} {logMessage}");
                        break;

                    case LogType.DatabaseError:

                        IMRP.Database.Collections.DatabaseLog newDBLog1 = new IMRP.Database.Collections.DatabaseLog();
                        newDBLog1.LogType = LogType.DatabaseError;
                        newDBLog1.Message = $"{timeStamp} {logMessage}";
                        newDBLog1.WasError = false;
                        if (!Server.ServerInitalizing) newDBLog1.AddNew();

                        NAPI.Util.ConsoleOutput($"{timeStamp} [DATABASE ERROR] {logMessage}");
                        break;
                    case LogType.CharacterInfo:

                        IMRP.Database.Collections.CharacterLog newCharLog = new IMRP.Database.Collections.CharacterLog();
                        newCharLog.LogType = LogType.CharacterInfo;
                        newCharLog.Message = $"{timeStamp} {logMessage}";
                        newCharLog.WasError = false;
                        if (!Server.ServerInitalizing) newCharLog.AddNew();

                        NAPI.Util.ConsoleOutput($"{timeStamp} {logMessage}");
                        break;
                    case LogType.Economy:
                        IMRP.Database.Collections.EconomyLog newEconLog = new IMRP.Database.Collections.EconomyLog();
                        newEconLog.LogType = LogType.Economy;
                        newEconLog.Message = $"{timeStamp} {logMessage}";
                        newEconLog.WasError = false;
                        if (!Server.ServerInitalizing) newEconLog.AddNew();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
        public enum LogType
        {
            ServerInfo,
            ServerError,
            DatabaseInfo,
            DatabaseError,
            CharacterInfo,
            Economy
        }
    }
}
