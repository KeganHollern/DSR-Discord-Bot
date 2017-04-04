using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DSRDiscordBot {

   


    public class BotManager {
        DiscordClient client;
        Channel chatChannel;
        string discordKey;
        string discordServer;
        bool alreadyConnected;
        

        public List<int> requests;

        public BotManager(string key,string server) {
            this.discordKey = key;
            this.discordServer = server;
            this.requests = new List<int>();
            this.alreadyConnected = false;

            this.client = new DiscordClient();
            this.client.MessageReceived += Client_MessageReceived;
            this.client.ServerAvailable += Client_ServerAvailable;

            this.client.Connect(this.discordKey, TokenType.Bot);
        }

        public void Shutdown() {
            Respond("The server is now Offline!");
            this.client.MessageReceived -= Client_MessageReceived;
            this.client.ServerAvailable -= Client_ServerAvailable;
            this.chatChannel = null;
            this.client.Disconnect();
        }

        public void Respond(string msg) {
            if(chatChannel != null) {
                chatChannel.SendMessage(msg);
            }
        }

        // events
        private void Client_ServerAvailable(object sender, ServerEventArgs e) {
            if(e.Server.Name != this.discordServer) {
                return;
            }
            if (!alreadyConnected) {
                chatChannel = e.Server.DefaultChannel;
                chatChannel.SendMessage("The server is now Online!");
                alreadyConnected = true;
            }
        }
        private void Client_MessageReceived(object sender, MessageEventArgs e) {
            if(e.Channel == chatChannel) {
                if (e.Message.Text.StartsWith("!")) {

                    RequestManager.ProcessRequest(this, e.Message.Text.ToLower(), (e.Message.User == e.Server.Owner));
                }
            }
        }
    }
}
