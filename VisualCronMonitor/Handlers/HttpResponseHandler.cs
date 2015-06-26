//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Web.Http;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;

//namespace VisualCronMonitor.Handlers
//{
//    public static class HttpResponseHandler
//    {
//        public static HttpResponseException CreateException(HttpStatusCode code, object content, string reason)
//        {
//            var resp = new HttpResponseMessage(code)
//            {
//                Content = new StringContent(content.ToString()),
//                ReasonPhrase = reason
//            };
//            return new HttpResponseException(resp);
//        }

//        public static HttpResponseMessage CreateMessage<T>(T obj, string mediaType = null, HttpStatusCode code = HttpStatusCode.OK)
//        {
//            var settings = new JsonSerializerSettings
//            {
//                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
//                //Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter { CamelCaseText = true } }
//            };

//            var jsonObject = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);

//            var result = new HttpResponseMessage(code)
//            {
//                Content = new StringContent(jsonObject)
//            };

//            if(!string.IsNullOrWhiteSpace(mediaType))
//                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

//            return result;
//        }
//    }
//}
