using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using MvcDemo.Services;

namespace MvcDemo.Controllers
{
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

            var contentId = _contentManager.SaveImage(file.FileName, null, file.ContentType, file.InputStream);
            
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
        public HttpResponseMessage FetchImage(string id) {
            var contentInfo = _contentManager.LoadContentInfo(id);
            if (contentInfo == null) {
                HttpContext.Current.Response.StatusCode = 404;
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            var content = _contentManager.LoadContent(id);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new ByteArrayContent(content)
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentInfo.ContentType);
            return httpResponseMessage;
        }
    }
}
