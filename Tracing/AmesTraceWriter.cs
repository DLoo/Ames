using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Tracing;

namespace Ames.Tracing {
    public class AmesTraceWriter : ITraceWriter {


        public void Trace(HttpRequestMessage request, string category,
                          TraceLevel level, Action<TraceRecord> traceAction) {
            TraceRecord traceRecord = new TraceRecord(request, category, level);
            traceAction(traceRecord);
            
            ShowTrace(traceRecord);
        }

        private void ShowTrace(TraceRecord traceRecord) {
            //Console.WriteLine(
            var printTraceRecord = String.Format(
                "{0} {1}: Category={2}, Level={3} {4} {5} {6} {7}",
                traceRecord.Request.Method.ToString(),
                traceRecord.Request.RequestUri.ToString(),
                traceRecord.Category,
                traceRecord.Level,
                traceRecord.Kind,
                traceRecord.Operator,
                traceRecord.Operation,
                traceRecord.Exception != null
                    ? traceRecord.Exception.GetBaseException().Message
                    : !string.IsNullOrEmpty(traceRecord.Message)
                        ? traceRecord.Message
                        : string.Empty
             );
              //  ));
            
            Console.WriteLine(printTraceRecord);

            string path = HttpContext.Current.Server.MapPath("~/Logs/");
            //Creates all directories and subdirectories in the specified path unless they already exist.
            Directory.CreateDirectory(path);
            File.AppendAllText(path + "MyTestLog.txt", printTraceRecord + "\r\n");
        }

    }
}
