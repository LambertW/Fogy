using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Web.Models
{
    public class AjaxResponse
    {
        public string TargetUrl { get; set; }

        public bool Success { get; set; }

        public ErrorInfo Error { get; set; }

        public bool UnAuthorizedRequest { get; set; }

        public object Result { get; set; }

        public AjaxResponse()
        {
            Success = true;
        }

        public AjaxResponse(bool success)
        {
            Success = success;
        }

        public AjaxResponse(object result)
            :this(true)
        {
            Result = result;
        }

        public AjaxResponse(ErrorInfo error, bool unAuthorizedRequest = false)
        {
            Error = error;
            UnAuthorizedRequest = unAuthorizedRequest;
            Success = false;
        }
    }
}
