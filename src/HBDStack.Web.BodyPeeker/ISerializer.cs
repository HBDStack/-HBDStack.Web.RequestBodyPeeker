namespace HBDStack.Web.BodyPeeker;

public interface ISerializer
{
    T Deserialize<T>(string value);
    string Serialize(object value);
}