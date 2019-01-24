using Fogy.Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fogy.Web.Api.Wrappers
{
    public class WrappingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (IsSwagger(request))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var response = await base.SendAsync(request, cancellationToken);
            WrapResultIfNeeded(request, response);

            return response;
        }

        private void WrapResultIfNeeded(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return;

            object resultContent = null;

            var code = (int)response.StatusCode;

            if (!response.TryGetContentValue(out resultContent) || resultContent == null)
            {
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Content = new ObjectContent<AjaxResponse>(new AjaxResponse(), new JsonMediaTypeFormatter());

                return;
            }

            if (resultContent is AjaxResponse)
                return;

            // TODO SwaggerDocument
            response.Content = new ObjectContent<AjaxResponse>(new AjaxResponse(resultContent), new JsonMediaTypeFormatter());
        }

        private bool IsSwagger(HttpRequestMessage request)
        {
            return request.RequestUri.PathAndQuery.StartsWith("/swagger");
        }
    }
}
