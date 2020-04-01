using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ETicket.Utils
{
    public static class XmlHelper
    {
        public static XmlNode SelectSingleNode(string xmlText, string xPath)
        {
            var document = new XmlDocument();
            document.LoadXml(xmlText);

            return document.DocumentElement.SelectSingleNode(xPath);
        }

        public static T Deserialize<T>(string value)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var stream = new StringReader(value))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}