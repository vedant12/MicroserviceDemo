using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json.Serialization;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        public IHttpClientFactory _factory { get; }
        public BaseService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<object> SendAsync(RequestDto<T> model)
        {
            HttpClient client = _factory.CreateClient("MangoAPI");
            
            HttpRequestMessage message = new();

            message.Headers.Add("Accept", "application/json");

            message.RequestUri = new Uri(model.Url);

            if(model.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(model.Data), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage responseMessage;

            switch (model.ApiType)
            {
                case ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;  
            }

            responseMessage = await client.SendAsync(message);

            switch (responseMessage.StatusCode) 
            {
                case System.Net.HttpStatusCode.NotFound:
                    return new ResponseDto<T>() { IsSuccess = false, Message = "Not Found" };
                    break;
                case System.Net.HttpStatusCode.Forbidden:
                    return new ResponseDto<T>() { IsSuccess = false, Message = "Forbidden" };
                    break;
                case System.Net.HttpStatusCode.Unauthorized:
                    return new ResponseDto<T>() { IsSuccess = false, Message = "Unauthorized" };
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    return new ResponseDto<T>() { IsSuccess = false, Message = "Internal Server Error" };
                    break;
                default:
                    var apiContent = await responseMessage.Content.ReadAsStringAsync();
                    try
                    {
                        var jObject = JObject.Parse(apiContent);
                        var resultToken = jObject["result"];

                        if (resultToken != null && resultToken.Type == JTokenType.Array)
                        {
                            return jObject.ToObject<ResponseDto<List<T>>>();
                        }
                        else
                        {
                            return jObject.ToObject<ResponseDto<T>>();
                        }
                    }
                    catch (JsonException)
                    {
                        return new ResponseDto<T> { IsSuccess = false, Message = "Invalid response format" };
                    }
                    break;
            }

        }
    }
}
