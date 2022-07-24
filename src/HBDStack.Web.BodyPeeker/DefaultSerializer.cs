namespace HBDStack.Web.BodyPeeker;

public class DefaultSerializer : ISerializer
{
    public T Deserialize<T>(string value) => System.Text.Json.JsonSerializer.Deserialize<T>(value);
    public string Serialize(object value) => System.Text.Json.JsonSerializer.Serialize(value);
}