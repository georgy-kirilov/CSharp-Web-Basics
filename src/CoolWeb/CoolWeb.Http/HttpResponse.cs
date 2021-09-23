namespace CoolWeb.Http
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    public class HttpResponse
    {
        public HttpResponse(string content, string contentType = ContentTypes.Text) 
            : this(Encoding.UTF8.GetBytes(content), contentType)
        {

        }

        public HttpResponse(byte[] content, string contentType = ContentTypes.Text) 
            : this(HttpStatusCode.Ok, content, contentType, HttpConstants.Version, HttpConstants.Server)
        {
        }

        public HttpResponse(HttpStatusCode statusCode, byte[] content, string contentType, string version, string server)
        {
            this.Headers = new();
            this.StatusCode = statusCode;
            this.Content = content;
            this.ContentType = contentType;
            this.Version = version;
            this.Server = server;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string ContentType { get; set; }

        public string Version { get; set; }

        public string Server { get; set; }

        public byte[] Content { get; set; }

        public HeaderCollection Headers { get; }

        public byte[] ToByteArray()
        {
            var headersBytes = Encoding.UTF8.GetBytes(this.ToString());
            var bytes = new byte[headersBytes.Length + this.Content.Length];
            Array.Copy(headersBytes, bytes, headersBytes.Length);
            Array.Copy(this.Content, sourceIndex: 0, bytes, headersBytes.Length, this.Content.Length);
            return bytes;
        }

        public override string ToString()
        {
            var builder = new HttpStringBuilder();

            var mandatoryHeaders = new KeyValuePair<string, object>[]
            {
                new("Server", this.Server),
                new("Content-Type", this.ContentType),
                new("Content-Length", this.Content.Length),
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
