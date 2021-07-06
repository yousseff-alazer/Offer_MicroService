using log4net;
using System.Reflection;

namespace Offer.Helpers
{
    public class LogHelper
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogException(string msg, string stackTrace)
        {
            log.Error(string.Format("Exception Msg: {0}", msg));
            log.Error(string.Format("Exception StackTrace: {0}", stackTrace));
        }

        public static void LogInfo(string msg)
        {
            log.Info(msg);
        }
    }
}