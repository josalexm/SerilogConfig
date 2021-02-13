using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SECF.Web.Models.Serilog
{
    public class SerilogEmailParamsDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string ApiKeySendGrid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Destinations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LogEventLevel LogEventLevel { get; set; }
    }
}
