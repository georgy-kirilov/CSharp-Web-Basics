namespace CoolWeb.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using static HttpConstants;

    public class HttpRequest
    {
        public static readonly Dictionary<string, Dictionary<string, string>> Sessions = new();

        public HttpRequest(string rawRequest)
        {
            this.Headers = new();
            this.Cookies = new();
           
            var lines = rawRequest.Split(NewLine, StringSplitOptions.None);
            var headerLineArguments = lines[0].Split(' ');

            this.Method = (HttpMethod) Enum.Parse(typeof(HttpMethod), headerLineArguments[0], ignoreCase: true);
            this.Path = headerLineArguments[1];

            var bodyBuilder = new StringBuilder();
            bool isInsideHeaders = true;

            for (int lineIndex = 1; lineIndex < lines.Length; lineIndex++)
            {
                string line = lines[lineIndex];

                if (string.IsNullOrWhiteSpace(line))
                {
                    isInsideHeaders = false;
                    continue;
                }

                if (isInsideHeaders)
                {
                    this.ParseHeader(line);
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }
            }

            if (this.Headers.ContainsKey(RequestCookieHeaderName))
            {
                var cookies = this.Headers[RequestCookieHeaderName].Split(HeaderValueAttributesSeparator);

                foreach (string cookie in cookies)
                {
                    var cookieArguments = cookie.Split("=");

                    string cookieName = cookieArguments[0];
                    string cookieValue = cookieArguments[1];

                    if (this.Cookies.ContainsKey(cookieName))
                    {
                        this.Cookies[cookieName] = cookieValue;
                    }
                    else
                    {
                        this.Cookies.Add(cookieName, cookieValue);
                    }

                    if (cookieName == SessionCookieName)
                    {
                        this.SessionCookie = cookieValue;
                    }
                }
            }
        }

        private void ParseHeader(string header)
        {
            var headerArguments = header.Split(HeaderKeyValueSeparator);

            string headerName = headerArguments[0], headerValue = headerArguments[1];

            if (this.Headers.ContainsKey(headerName))
            {
                this.Headers[headerName] = headerValue;
            }
            else
            {
                this.Headers.Add(headerName, headerValue);
            }
        }
        public string Path { get; set; }

        public HttpMethod Method { get; set; }

        public string SessionCookie { get; }

        public Dictionary<string, string> Headers { get; }

        public Dictionary<string, string> Cookies { get; }
    }
}
