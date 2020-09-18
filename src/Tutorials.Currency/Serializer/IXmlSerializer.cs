using System.Collections.Generic;
using System.Text;

namespace Tutorials.Currency.Serializer
{
    public interface IXmlSerializer
    {
        T Deserializer<T>(string value);
        string Serializer(object value);
    }
}
