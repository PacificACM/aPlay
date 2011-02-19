using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xcom.aPlay.Util
{
    static class Log
    {
        public static void LogDebugMessage(string msg)
        {
            Console.Error.WriteLine(msg);
        }
        //TODO implement this...
        internal static void LogMsg(string p)
        {
           // throw new NotImplementedException();
        }
    }
}
