using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOPCWrapperLib
{
    public class MyOPCServer
    {
        private OPCServer server;
        private string[] itemIDs;
        private System.Timers.Timer watchDog = new System.Timers.Timer(2000);
        private EServerState serverState; //проверить работу OPCServerState, возможно не нужен свой enum
        public string serverName { get; private set; }
        public string serverHost { get; private set; }
        public MyOPCGroup Group { get; set; }
        public EServerState ServerState
        {
            get => serverState;
            private set
            {
                if (serverState != value)
                {
                    serverState = value;
                    ServerStateChanged?.Invoke(serverState);
                }
            }
        }
        public event Action<EServerState> ServerStateChanged;

        public MyOPCServer(string ServerName, string ServerHost)
        {
            server = new OPCServer();
            serverName = ServerName;
            serverHost = ServerHost;
            //watchDog.Elapsed += WatchDog_Elapsed;
        }

        private void WatchDog_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                ServerState = (EServerState)(int)server.ServerState;
            }
            catch (Exception)
            {
                ServerState = EServerState.OPCDisconnected;
                Connect();
            }
        }

        private void Connect()
        {
            try
            {
                server.Connect(serverName, serverHost);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Connect(params string[] ItemIDs)
        {
            itemIDs = ItemIDs;
            try
            {
                server.Connect(serverName, serverHost);
                Group = new MyOPCGroup(server.OPCGroups.Add("Stand13_Group"), ItemIDs);
                Group.IsActive = true;
                Group.IsSubscribed = true;
                Group.UpdateRate = 500;
                //Group.AddMyOPCTags(itemIDs);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Disconnect()
        {
            Group.IsSubscribed = false;
            Group.IsActive = false;
            server.OPCGroups.RemoveAll();
            server.Disconnect();
        }

        public enum EServerState : int
        {
            OPCRuning = 1,
            OPCFailed,
            OPCNoconfig,
            OPCSuspended,
            OPCTest,
            OPCDisconnected
        }
    }
}
