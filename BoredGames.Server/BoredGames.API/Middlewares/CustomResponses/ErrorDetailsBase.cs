using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BoredGames.API.Middlewares.CustomResponses
{
    public class ErrorDetailsBase
    {
        public ErrorDetailsBase(string type, int status, string title)
        {
            Type = type;
            Status = status;
            Title = title;
        }

        public string Type { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(this, serializerSettings);
        }
    }
}
