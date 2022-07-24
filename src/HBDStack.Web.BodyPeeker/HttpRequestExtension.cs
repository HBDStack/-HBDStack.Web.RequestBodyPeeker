using System;
using System.Text;
using System.Threading.Tasks;
using HBDStack.Web.BodyPeeker;
using static System.String;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Http;

public static class HttpRequestExtension
{
    /// <summary>
    /// Peek at the Http request stream without consuming it
    /// </summary>
    /// <param name="request">Http Request object</param>
    /// <param name="encoding">user's desired encoding</param>
    /// <returns>string representation of the request body</returns>
    public static string PeekBody(this HttpRequest request, Encoding encoding = null)
    {
        try
        {
            encoding ??= new UTF8Encoding();
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            if (buffer.Length == 0) return Empty;

            var _ = request.Body.Read(buffer, 0, buffer.Length);
            return encoding.GetString(buffer);
        }
        finally
        {
            request.Body.Position = 0;
        }
    }

    /// <summary>
    /// Asynchronous Peek at the Http request stream without consuming it
    /// </summary>
    /// <param name="request">Http Request object</param>
    /// <param name="encoding">user's desired encoding</param>
    /// <returns>string representation of the request body</returns>
    public static async Task<string> PeekBodyAsync(this HttpRequest request, Encoding encoding = null)
    {
        try
        {
            encoding ??= new UTF8Encoding();
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            if (buffer.Length == 0) return Empty;

            var _ = await request.Body.ReadAsync(buffer, 0, buffer.Length);
            return encoding.GetString(buffer);
        }
        finally
        {
            request.Body.Position = 0;
        }
    }

    /// <summary>
    /// Peek at the Http request stream without consuming it
    /// </summary>
    /// <param name="request">Http Request object</param>
    /// <param name="encoding">user's desired encoding</param>
    /// <param name="serializer"><see cref="ISerializer"/></param>
    /// <returns>T type which provided at invocation</returns>
    public static T PeekBody<T>(this HttpRequest request, Encoding encoding = null, ISerializer serializer = null)
        where T : class
    {
        serializer ??= new DefaultSerializer();
        var bodyAsText = request.PeekBody(encoding);
        return serializer.Deserialize<T>(bodyAsText);
    }

    /// <summary>
    /// Peek asynchronously at the Http request stream without consuming it
    /// </summary>
    /// <param name="request">Http Request object</param>
    /// <param name="encoding">user's desired encoding</param>
    /// <param name="serializer"><see cref="ISerializer"/></param>
    /// <returns>T type which provided at invocation</returns>
    public static async Task<T> PeekBodyAsync<T>(this HttpRequest request, Encoding encoding = null, ISerializer serializer = null) where T : class
    {
        serializer ??= new DefaultSerializer();
        var bodyAsText = await request.PeekBodyAsync(encoding);
        return serializer.Deserialize<T>(bodyAsText);
    }
}