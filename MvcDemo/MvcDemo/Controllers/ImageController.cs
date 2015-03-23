using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MvcDemo.Services;

namespace MvcDemo.Controllers {
    public class ImageController : ApiController
    {
        readonly IContentManager _contentManager;

        public ImageController(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        [HttpGet]
        [HttpPost]
        public HttpResponseMessage UploadFile() {
            var file = HttpContext.Current.Request.Files[0];

            var contentId = _contentManager.SaveImage(file.FileName, file.ContentType, file.InputStream);
            
            HttpContext.Current.Response.ContentType = "application/json";
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new { name = file.FileName, id = contentId, };

            HttpContext.Current.Response.Write(serializer.Serialize(result));
            HttpContext.Current.Response.StatusCode = 200;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage ListFiles() {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(serializer.Serialize(_contentManager.ListImages()));
            HttpContext.Current.Response.StatusCode = 200;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage FetchImage(string imageId) {
            var content = _contentManager.LoadContent(imageId);
            if (content == null) {
                HttpContext.Current.Response.StatusCode = 404;
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            HttpContext.Current.Response.StatusCode = 200;

            var message = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new ByteArrayContent(content.Bytes)
            };
            message.Content.Headers.ContentType = new MediaTypeHeaderValue(content.ContentType);

            return message;
        }

        [HttpPost, Route("api/upload")]
        public async Task<IHttpActionResult> Upload() {
            if (!Request.Content.IsMimeMultipartContent())
                throw new Exception(); // divided by zero

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents) {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();
                //Do whatever you want with filename and its binaray data.
            }

            return Ok();
        }
    }
}
