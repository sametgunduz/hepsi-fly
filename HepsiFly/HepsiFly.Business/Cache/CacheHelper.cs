using System.Text;
using Newtonsoft.Json;

namespace HepsiFly.Business.Cache;

public class CacheHelper
{
    protected byte[] Serialize(object item)
    {
        var jsonString = JsonConvert.SerializeObject(item);
        return Encoding.UTF8.GetBytes(jsonString);
    }
    protected T Deserialize<T>(string[] serializedObject)
    {
        if (serializedObject == null)
            return default(T);
        return JsonConvert.DeserializeObject<T>(serializedObject[0]);
    }
}