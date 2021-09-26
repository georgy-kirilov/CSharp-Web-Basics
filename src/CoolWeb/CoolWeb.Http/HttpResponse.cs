namespace CoolWeb.Http
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    public class HttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode = HttpStatusCode.Ok, string contentType = ContentTypes.Text)
        {
            this.Headers = new();
            this.Content = Array.Empty<byte>();

            this.StatusCode = statusCode;
            this.ContentType = contentType;

            this.Server = HttpConstants.Server;
            this.Version = HttpConstants.Version;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string ContentType { get; set; }

        public string Version { get; set; }

        public string Server { get; set; }

        public byte[] Content { get; set; }

        public HeaderCollection Headers { get; }

        private int BodyBytesCount => this.Content?.Length ?? 0;

        public byte[] ToByteArray()
        {
            var headersBytes = Encoding.UTF8.GetBytes(this.ToString());
            var bytes = new byte[headersBytes.Length + this.BodyBytesCount];

            Array.Copy(headersBytes, bytes, headersBytes.Length);
            Array.Copy(this.Content, sourceIndex: 0, bytes, headersBytes.Length, this.BodyBytesCount);

            return bytes;
        }

        public override string ToString()
        {
            var builder = new HttpStringBuilder();

            KeyValuePair<string, object>[] mandatoryHeaders = new KeyValuePair<string, object>[]
            {
                new("Server", this.Server),
                new("Content-Type", this.ContentType),
                new("Content-Length", this.BodyBytesCount),
                new("Date", DateTime.UtcNow.ToString("R")),
                new("Connection", "Keep-Alive"),
            };

            foreach (var header in mandatoryHeaders)
            {
                this.Headers.Add(header.Key, header.Value);
            }

            int statusCodeNumber = (int) this.StatusCode;

            builder.AppendLine($"{this.Version} {statusCodeNumber} {this.StatusCode.Message()}");

            foreach (var header in mandatoryHeaders)
            {
                builder.AppendLine($"{header.Key}: {header.Value}");
            }

            builder.AppendLine();

            return builder.ToString();
        }
    }
}
