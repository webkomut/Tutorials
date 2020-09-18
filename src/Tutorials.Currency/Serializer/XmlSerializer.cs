using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Tutorials.Currency.Serializer
{
    public class XmlSerializer : IXmlSerializer
    {
        public T Deserializer<T>(string value)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            var dataReader = new StringReader(value);
            return (T) serializer.Deserialize(dataReader);
        }

        public string Serializer(object value)
        {
            var nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add("", "");
            var writer = new StringWriter();
            var xmlSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            };
            using var xmlWriter = XmlWriter.Create(writer, xmlSettings);
            var serializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
            serializer.Serialize(xmlWriter, value, nameSpace);
            return writer.ToString();
        }
    }
}