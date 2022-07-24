using System;
using System.Text;
using System.Threading.Tasks;
using HBDStack.Web.BodyPeeker;
using static System.String;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Http;

public static class HttpResponseExtension
{
    /// <summary>
    /// Peek at the Http request stream without consuming it
    /// </summary>
    /// <param name="response">Http Request object</param>
    /// <param name="encoding">user's desired encoding</param>
    /// <returns>string representation of the request body</returns>
    public static string PeekBody(this HttpResponse response, Encoding encoding = null)
    {
        encoding ??= new UTF8Encoding();
        //response.EnableBuffering();
        var buffer = new byte[Convert.ToInt32(response.ContentLength)];
        if (buffer.Length == 0) return Empty;

        var _ = response.Body.Read(buffer, 0, buffer.Length);
        return encoding.GetString(buffer);
    }

    /// <summary>
    /// Asynchronous Peek at the Http request stream without consuming it
    /// </summary>
    /// <param name="response">Http Request object</param>
    /// <param name="encoding">user's desired encoding</param>
    /// <returns>string representation of the request body</returns>
    public static async Task<string> PeekBodyAsync(this HttpResponse response, Encoding encoding = null)
    {
        encoding ??= new UTF8Encoding();
        //response.EnableBuffering();
        var buffer = new byte[Convert.ToInt32(response.ContentLength)];
        if (buffer.Length == 0) return Empty;

        var _ = await response.Body.ReadAsync(buffer, 0, buffer.Length);
        return encoding.GetString(buffer);
    }

    /// <summary>
    /// Peek at the Http request stream without consuming it
    /// </summary>
    /// <param name="response">Http Request object</param>
    /// <param name="encoding">user's desired encoding</param>
    /// <param name="serializer"><see cref="ISerializer"/></param>
    /// <returns>T type which provided at invocation</returns>
    public static T PeekBody<T>(this HttpResponse response, Encoding encoding = null, ISerializer serializer = null)
        where T : class
    {
        serializer ??= new DefaultSerializer();
        var bodyAsText = response.PeekBody(encoding);
        return serializer.Deserialize<T>(bodyAsText);
    }

    /// <summary>
    /// Peek asynchronously at the Http response stream without consuming it
    /// </summary>
    /// <param name="response">Http Request object</param>
    /// <param name="encoding">user's desired encoding</param>
    /// <param name="serializer"><see cref="ISerializer"/></param>
    /// <returns>T type which provided at invocation</returns>
    public static async Task<T> PeekBodyAsync<T>(this HttpResponse response, Encoding encoding = null, ISerializer serializer = null) where T : class
    {
        serializer ??= new DefaultSerializer();
        var bodyAsText = await response.PeekBodyAsync(encoding);
        return serializer.Deserialize<T>(bodyAsText);
    }
}