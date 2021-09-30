namespace CoolWeb.Http
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    public class ResponseCookie
    {
        public ResponseCookie(string name, string value)
        {
            this.Name = name;
            this.Value = value;
            this.Path = "/";
            this.IsSecure = true;
            this.IsHttpOnly = true;
            this.ExpiresOn = null;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsHttpOnly { get; set; }

        public bool IsSecure { get; set; }

        public string Path { get; set; }

        public string Domain { get; set; }

        public int MaxAge { get; set; }

        public DateTime? ExpiresOn { get; set; }

        public CookieSameSiteOptions SameSiteOptions { get; set; }

        public KeyValuePair<string, string> CreateHeader()
        {
            var cookieAttributes = new Dictionary<string, object>
            {
                { this.Name, this.Value },
                { "Expires", this.ExpiresOn },
                { "Max-Age", this.MaxAge },
                { "Path", this.Path },
                { "Domain", this.Domain },
            };

            var cookieAttributesBuilder = new StringBuilder();

            foreach (var cookieAttribute in cookieAttributes)
            {
                if (cookieAttribute.Value != null)
                {
                    cookieAttributesBuilder.Append($"{cookieAttribute.Key}={cookieAttribute.Value}; ");
                }
            }

            return new (HttpConstants.ResponseCookieHeaderName, cookieAttributesBuilder.ToString());
        }
    }
}
