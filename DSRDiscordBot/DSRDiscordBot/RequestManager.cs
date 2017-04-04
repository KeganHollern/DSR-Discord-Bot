using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSRDiscordBot {
    public struct DiscordRequest {
        public string msg;
        public bool adminOnly;
    }
    public class RequestManager {
        public static List<DiscordRequest> RequestTypes = new List<DiscordRequest>();
        public static void initialize() {
            addRequest("!performance", false);
            addRequest("!statistics", false);
            addRequest("!shutdown", true);
            addRequest("!players", false);
            addRequest("!performance", false); // doesnt get added because the command already exists
        }

        public static void addRequest(string rMsg, bool rAdminOnly) {
            for(int i = 0; i < RequestTypes.Count;i++) {
                if(RequestTypes.ElementAt(i).msg == rMsg) {
                    return;
                }
            }
            RequestTypes.Add(new DiscordRequest() { msg = rMsg, adminOnly = rAdminOnly });
        }

        public static string FormatArgs(string[] args, int index) {
            try {
                return args[index].Trim('"');
            } catch {
                return "";
            }
        }

        public static void ProcessResponse(BotManager bot, int type, string[] args) {
            switch (type) {
                case 0: { // !performance
                        bot.Respond("FPS: " + FormatArgs(args, 1) + ", SQF THREADS: " + FormatArgs(args, 2));
                        break;
                    }
                case 1: { // !statistics
                        bot.Respond("UPTIME: " + FormatArgs(args, 1) + " min, PLAYERS: " + FormatArgs(args, 2) + ",  ZOMBIES: " + FormatArgs(args, 3));
                        break;
                    }
                case 2: { // !shutdown
                        bot.Respond("The server is shutting down!");
                        break;
                    }
                case 3: { // !players
                        bot.Respond("Players: \n    " + FormatArgs(args, 1).Replace("`", "\n    "));
                        break;
                    }
                default: {
                        bot.Respond("Unknown Message Received! Please Update Your Bot!");
                        break;
                    }
            }
        }

        public static void ProcessRequest(BotManager bot, string msg, bool isOwner) {
            string helpMsg = "Commands: \n    ";
            for(int i = 0; i < RequestTypes.Count; i++) {
                DiscordRequest request = RequestTypes.ElementAt(i);
                if(request.msg == msg) {
                    if(request.adminOnly && !isOwner) {
                        bot.Respond("Sorry! Only the Discord Owner can do this command!");
                        return;
                    } else {
                        bot.requests.Add(i);
                        return;
                    }
                } else if(msg == "!help") {
                    if (request.adminOnly && !isOwner) {
                        continue;
                    }
                    helpMsg += request.msg + " \n    ";
                }
            }
            if (msg == "!help") {
                bot.Respond(helpMsg);
            } else {
                bot.Respond("That is not a command! Type !help to view a list of all available commands!");
            }

        }
    }
}
