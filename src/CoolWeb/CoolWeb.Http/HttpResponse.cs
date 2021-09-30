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
            this.Body = Array.Empty<byte>();

            this.StatusCode = statusCode;
            this.ContentType = contentType;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string ContentType { get; set; }

        public byte[] Body { get; set; }

        public HeaderCollection Headers { get; }

        private int BodyBytesCount => this.Body?.Length ?? 0;

        public byte[] ToByteArray()
        {
            var headersBytes = Encoding.UTF8.GetBytes(this.ToString());
            var responseBytes = new byte[headersBytes.Length + this.BodyBytesCount];

            Array.Copy(headersBytes, responseBytes, headersBytes.Length);
            Array.Copy(this.Body, sourceIndex: 0, responseBytes, headersBytes.Length, this.BodyBytesCount);

            return responseBytes;
        }

        public override string ToString()
        {
            var builder = new HttpStringBuilder();

            KeyValuePair<string, object>[] mandatoryHeaders = new KeyValuePair<string, object>[]
            {
                new("Server", HttpConstants.Server),
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

            builder.AppendLine($"{HttpConstants.Version} {statusCodeNumber} {this.StatusCode.Message()}");

            foreach (var header in this.Headers)
            {
                builder.AppendLine($"{header.Key}: {header.Value}");
            }

            builder.AppendLine();

            return builder.ToString();
        }
    }
}
