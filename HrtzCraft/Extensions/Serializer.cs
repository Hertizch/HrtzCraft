using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace HrtzCraft.Extensions
{
    public class Serializer
    {
        public static void SerializeToXml<T>(T t, string inFilename)
        {
            var serializer = new XmlSerializer(t.GetType());
            using (TextWriter textWriter = new StreamWriter(inFilename))
            {
                serializer.Serialize(textWriter, t);
            }
        }

        public static T DeserializeFromXml<T>(string inFilename)
        {
            var deserializer = new XmlSerializer(typeof(T));

            T retVal;

            using (TextReader textReader = new StreamReader(inFilename))
                retVal = (T) deserializer.Deserialize(textReader);

            return retVal;
        }
    }
}
