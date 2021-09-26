namespace CoolWeb.Http
{
    using System;

    public class HttpRequest
    {
        public HttpRequest(string rawRequest)
        {
            Console.WriteLine(rawRequest);
            this.Headers = new();
            var lines = rawRequest.Split(HttpConstants.NewLine);
            var arguments = lines[0].Split(" ");

            this.Method = (HttpMethod) Enum.Parse(typeof(HttpMethod), arguments[0], ignoreCase: true);
            this.Path = arguments[1];
            this.Version = arguments[2];

            for (int row = 1; row < lines.Length; row++)
            {
                string line = lines[row];

                if (line == HttpConstants.NewLine)
                {
                    break;
                }

                var headerArguments = line.Split(": ");
                Console.WriteLine(headerArguments.Length);
                string key = headerArguments[0], value = headerArguments[1];

                switch (key?.ToLower())
                {
                    case "content-type":
                        this.ContentType = value;
                        break;

                    case "content-length":
                        this.ContentLength = int.Parse(value);
                        break;

                    default:
                        this.Headers.Add(key, value);
                        break;
                }
            }
        }

        public string Path { get; set; }

        public string Version { get; }

        public HttpMethod Method { get; set; }

        public string ContentType { get; set; }

        public int ContentLength { get; set; }

        public HeaderCollection Headers { get; }
    }
}
