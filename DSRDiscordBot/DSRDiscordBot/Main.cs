using RGiesecke.DllExport;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DSRDiscordBot
{
    public class Main
    {
        public static BotManager bot;

        #if WIN64
                [DllExport("RVExtensionArgs", CallingConvention = CallingConvention.Winapi)]
        #else
                [DllExport("_RVExtensionArgs@20", CallingConvention = CallingConvention.Winapi)]
        #endif
        public static int RvExtensionArgs(StringBuilder output, int outputSize, [MarshalAs(UnmanagedType.LPStr)] string function, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 4)] string[] args, int argCount) {
            if(function == "start") {

                RequestManager.initialize();
                bot = new BotManager(RequestManager.FormatArgs(args,0), RequestManager.FormatArgs(args, 1));
                return 100;
            }
            if(function == "request") {
                if(bot.requests.Count > 0) {
                    int type = bot.requests.ElementAt(0);
                    bot.requests.RemoveAt(0);
                    return type;
                } else {
                    return -1;
                }
            }
            if(function == "response") {
                string respType = RequestManager.FormatArgs(args, 0);

                int typeNum = int.Parse(respType);

                RequestManager.ProcessResponse(bot, typeNum, args);
            }

            return 100;
        }
    }
}
