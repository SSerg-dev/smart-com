using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Util;

namespace Frontend.TPM.Attributes
{
    class AntiXssFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string rawRequest = "";
            
            if (!actionContext.Request.Content.IsMimeMultipartContent())
            {
                using (MemoryStream streamMemory = new MemoryStream())
                {
                    actionContext.Request.Content.CopyToAsync(streamMemory);

                    using (StreamReader stream = new StreamReader(streamMemory))
                    {
                        stream.BaseStream.Position = 0;
                        rawRequest = stream.ReadToEnd();
                    }
                }
            }

            int failureIndex1;
            int failureIndex2;

            //bool isValid1 = RequestValidator.Current.InvokeIsValidRequestString(HttpContext.Current, actionContext.Request.RequestUri.ToString(), RequestValidationSource.RawUrl, "RawUrl", out failureIndex1);
            //bool isValid2 = RequestValidator.Current.InvokeIsValidRequestString(HttpContext.Current, rawRequest, RequestValidationSource.Form, "Form", out failureIndex2);

            bool isValid1 = IsValidRequestString(HttpContext.Current, actionContext.Request.RequestUri.ToString(), RequestValidationSource.RawUrl, "RawUrl", out failureIndex1);
            bool isValid2 = IsValidRequestString(HttpContext.Current, rawRequest, RequestValidationSource.Form, "Form", out failureIndex2);

            if (!isValid1 || !isValid2)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request");
            }
        }

        private bool IsValidRequestString(HttpContext context, string value, RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex)
        {
            if (requestValidationSource == RequestValidationSource.Headers)
            {
                validationFailureIndex = 0;
                return true;
            }
            return !IsDangerousString(value, out validationFailureIndex);
        }

        internal static bool IsDangerousString(string s, out int matchIndex)
        {
            matchIndex = 0;
            if (s.Contains(".xlsx"))
            {
                return false;
            }
            else
            {
                int startIndex = 0;
                while (true)
                {
                    int num2 = s.IndexOfAny(startingChars, startIndex);
                    if (num2 < 0)
                    {
                        return false;
                    }
                    if (num2 == (s.Length - 1))
                    {
                        return false;
                    }
                    matchIndex = num2;
                    char ch = s[num2];
                    if (ch != '&')
                    {
                        if ((ch == '<') && ((IsAtoZ(s[num2 + 1]) || (s[num2 + 1] == '!')) || ((s[num2 + 1] == '/') || (s[num2 + 1] == '?'))))
                        {
                            return true;
                        }
                    }
                    else if (s[num2 + 1] == '#')
                    {
                        return true;
                    }
                    startIndex = num2 + 1;
                }
            }
        }

        private static char[] startingChars = new char[] { '<', '&' };
        private static bool IsAtoZ(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
            {
                if (actionExecutedContext.Response.StatusCode == HttpStatusCode.NotFound)
                //if (!actionExecutedContext.Response.IsSuccessStatusCode)
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(actionExecutedContext.Response.StatusCode, "Incorrect request");
                }
            }
            else
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request");
            }
        }
    }
}
