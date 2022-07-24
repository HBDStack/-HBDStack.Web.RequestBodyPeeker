using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Request.Body.Peeker.Test
{
    public class ResponsePeekWithEncodingTests
    {
        private HttpContext _httpContext;

        private static HttpContext MakeFakeContext(object body)
        {
            var context = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            var response = new Mock<HttpResponse>();

            var mem = new MemoryStream();
            mem.WriteAsync(Encoding.UTF8.GetBytes(body.ToString()!));
            mem.Seek(0, 0);

            request.Setup(z => z.Body).Returns(mem);
            request.Setup(z => z.ContentLength).Returns(body.ToString()!.Length);
            context.Setup(c => c.Request).Returns(request.Object);
            
            context.Setup(c => c.Response).Returns(response.Object);
            response.Setup(z => z.Body).Returns(mem);
            response.Setup(z => z.ContentLength).Returns(body.ToString()!.Length);
            
            return context.Object;
        }

        [Test]
        public void Peek_TestToIfTheStreamIsReadableAgain()
        {
            const string body = "{ali:alp}";
            _httpContext = MakeFakeContext(body);
            var peekBody = _httpContext.Response.PeekBody(new UTF8Encoding());

            Assert.AreEqual(body, peekBody);

            Debug.Assert(_httpContext.Response.ContentLength != null, "_httpContext.Response.ContentLength != null");
            var contentLen = (int) _httpContext.Response.ContentLength;
            var buffer = new byte[contentLen];
            _httpContext.Response.Body.Read(buffer, 0, contentLen);

            var result = Encoding.UTF8.GetString(buffer);
            Assert.AreEqual(body, result);
        }


        [Test]
        public async Task PeekAsync_TestToIfTheStreamIsReadableAgain()
        {
            const string body = "{ali:alp}";
            _httpContext = MakeFakeContext(body);
            var peekBody = await _httpContext.Response.PeekBodyAsync(new UTF8Encoding());

            Assert.AreEqual(body, peekBody);

            Debug.Assert(_httpContext.Response.ContentLength != null, "_httpContext.Response.ContentLength != null");
            var contentLen = (int) _httpContext.Response.ContentLength;
            var buffer = new byte[contentLen];
            var readAsync = await _httpContext.Response.Body.ReadAsync(buffer, 0, contentLen);

            Assert.AreEqual(body, Encoding.UTF8.GetString(buffer));
        }

        [Test]
        public void PeekT_TestToIfTheStreamIsReadableAgain()
        {
            var myClass = new DummyClass()
            {
                Name = "ali",
                SurName = "alp"
            };

            _httpContext = MakeFakeContext(JsonConvert.SerializeObject(myClass));
            var peekBody = _httpContext.Response.PeekBody<DummyClass>(new UTF8Encoding());

            Assert.AreEqual(0, myClass.CompareTo(peekBody));

            Debug.Assert(_httpContext.Response.ContentLength != null, "_httpContext.Response.ContentLength != null");
            var contentLen = (int) _httpContext.Response.ContentLength;
            var buffer = new byte[contentLen];
            var read = _httpContext.Response.Body.Read(buffer, 0, contentLen);

            var result = JsonConvert.DeserializeObject<DummyClass>(Encoding.UTF8.GetString(buffer));
            Assert.AreEqual(0, myClass.CompareTo(result));
        }


        [Test]
        public async Task PeekTAsync_TestToIfTheStreamIsReadableAgain()
        {
            var myClass = new DummyClass()
            {
                Name = "ali",
                SurName = "alp"
            };

            _httpContext = MakeFakeContext(JsonConvert.SerializeObject(myClass));
            var peekBody = await _httpContext.Response.PeekBodyAsync<DummyClass>(new UTF8Encoding());

            Assert.AreEqual(0, myClass.CompareTo(peekBody));

            Debug.Assert(_httpContext.Response.ContentLength != null, "_httpContext.Response.ContentLength != null");
            var contentLen = (int) _httpContext.Response.ContentLength;
            var buffer = new byte[contentLen];
            var readAsync = await _httpContext.Response.Body.ReadAsync(buffer, 0, contentLen);

            var result = JsonConvert.DeserializeObject<DummyClass>(Encoding.UTF8.GetString(buffer));
            Assert.AreEqual(0, myClass.CompareTo(result));
        }
    }
}