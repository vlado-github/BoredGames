using BoredGames.Server.Domain.Games.Base;
using Newtonsoft.Json;

namespace BoredGames.Server.Domain.Games.Entities;

public class GameFormSchema : IFormSchema
{
    public GameFormSchema()
    {
        Elements = new List<FormElement>();
    }
    
    public IList<FormElement> Elements { get; set; }
    
    public string ToJson()
    {
        return JsonConvert.SerializeObject(Elements,Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }
}

public class FormElement
{
    public FormElement(string id)
    {
        Id = id;
    }
    
    [JsonProperty("$el")]
    public string Element { get; set; }
    
    [JsonProperty("$cmp")]
    public string Component { get; set; }
    
    [JsonProperty("$formkit")]
    public string Type { get; set; }
    
    [JsonProperty("attrs")]
    public object Attributes { get; set; }
    
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("value")]
    public string Value { get; set; }
    
    [JsonProperty("min")]
    public string Min { get; set; }
    
    [JsonProperty("max")]
    public string Max { get; set; }
    
    [JsonProperty("label")]
    public string Label { get; set; }
    
    [JsonProperty("help")]
    public string Help { get; set; }
    
    [JsonProperty("children")]
    public string Children { get; set; }
    
    [JsonProperty("step")]
    public string Step { get; set; }
    
    [JsonProperty("validation")]
    public string Validation { get; set; }

    [JsonProperty("props")] 
    public Properties? Properties { get; set; }
}

public class Properties
{
    public Properties()
    {
        Values = new Dictionary<string, string>();
    }
    
    public IDictionary<string, string> Values { get; set; }
}