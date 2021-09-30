namespace CoolWeb.Http
{
    using System.Collections;
    using System.Collections.Generic;

    public class HeaderCollection : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly List<KeyValuePair<string, object>> headers = new();

        public void Add(string key, object value)
        {
            this.headers.Add(new(key, value));
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (var header in this.headers)
            {
                yield return header;
            }
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
