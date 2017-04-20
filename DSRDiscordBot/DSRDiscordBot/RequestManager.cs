using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSRDiscordBot {
    public struct DiscordRequest {
        public List<string> msg;
        public bool adminOnly;
    }
    public class RequestManager {
        public static List<DiscordRequest> RequestTypes = new List<DiscordRequest>();
        public static void initialize() {
            addRequest(new List<string>() { "!performance", "!perf" }, false);
            addRequest(new List<string>() { "!statistics", "!stats" }, false);
            addRequest(new List<string>() { "!shutdown", "!restart" }, true);
            addRequest(new List<string>() { "!players", "!plrs" }, false);
            addRequest(new List<string>() { "!allinfo", "!ainfo" }, false);
            addRequest(new List<string>() { "!time", "!gametime" }, false);
        }

        public static void addRequest(List<string> rMsg, bool rAdminOnly) {
            for(int i = 0; i < RequestTypes.Count;i++) {
                if(RequestTypes.ElementAt(i).msg.Equals(rMsg)) {
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
                        bot.Respond("FPS: " + FormatArgs(args, 1) + ", SQF THREADS: " + FormatArgs(args, 2) + ", CPS: " + FormatArgs(args, 3));
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
                case 4: { // !allinfo
                        bot.Respond("FPS: " + FormatArgs(args, 1) + ", SQF THREADS: " + FormatArgs(args, 2) + ", CPS: " + FormatArgs(args, 3) + ", UPTIME: " + FormatArgs(args, 1) + " min, PLAYERS: " + FormatArgs(args, 2) + ",  ZOMBIES: " + FormatArgs(args, 3));
                        break;
                    }
                case 5: { //time
                        bot.Respond("TIME: " + FormatArgs(args, 1) + " :clock:");
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
                if(request.msg.Contains(msg)) {
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
                    foreach(string mEntry in request.msg) {
                        helpMsg += mEntry + " or ";
                    }
                    helpMsg += " \n    ";
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
