using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Web.Models
{
    public class ValicationErrorInfo
    {
        public string Message { get; set; }

        public string[] Members { get; set; }

        public ValicationErrorInfo() { }

        public ValicationErrorInfo(string message)
        {
            Message = message;
        }

        public ValicationErrorInfo(string message, string[] members)
            : this(message)
        {
            Members = members;
        }
    }
}
