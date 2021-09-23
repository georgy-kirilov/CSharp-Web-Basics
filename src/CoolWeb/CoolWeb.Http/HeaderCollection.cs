namespace CoolWeb.Http
{
    using System.Collections.Generic;

    public class HeaderCollection
    {
        private readonly List<KeyValuePair<string, object>> headers = new();

        public void Add(string key, object value)
        {
            this.headers.Add(new(key, value));
        }

        public override string ToString()
        {
            var builder = new HttpStringBuilder();

            foreach (var header in headers)
            {
                builder.AppendLine($"{header.Key}: {header.Value}");
            }

            return builder.ToString();
        }
    }
}
