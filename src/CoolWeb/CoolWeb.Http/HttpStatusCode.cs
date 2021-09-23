namespace CoolWeb.Http
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public enum HttpStatusCode
    {
        Ok = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        PartialContent = 206,
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        PermanentRedirect = 308,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        RequestTimeout = 408,
        InternalServerError = 500,
    }

    public static class HttpStatusCodeExtensions
    {
        public static string Message(this HttpStatusCode code)
        {
            var regex = new Regex(@"[A-Z]{1}[a-z]+");
            return string.Join(" ", regex.Matches(code.ToString()).Select(m => m.Value));
        }
    }
}
