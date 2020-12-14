using Core.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Utils
{
    public class PerformanceLogger
    {
        private Stopwatch sw;
        private bool isToLog;

        public PerformanceLogger()
        {
            isToLog = AppSettingsManager.GetSetting<bool>("UsePerformanceLogger", false);
        }

        public void Start()
        {
            if (isToLog)
            {
                sw = new Stopwatch();
                sw.Start();
            }
        }

        public void Stop(string additionalInfo = " ")
        {
            if (isToLog)
            {
                sw.Stop();

                StackTrace stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(1).GetMethod();
                var methodName = method.Name;
                var className = method.DeclaringType.Name;

                var time = sw.Elapsed.ToString();
                var timeInMs = sw.ElapsedMilliseconds.ToString();
                string message = className + "," + methodName + ",T" + time + "," + timeInMs + "," + additionalInfo;
                var logger = LogManager.GetLogger("PerformanceLog");
                logger.Trace(message);
            }
        }
    }
}
