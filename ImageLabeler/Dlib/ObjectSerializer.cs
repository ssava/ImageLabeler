using System.IO;
using System.Xml.Serialization;

namespace ImageLabeler
{
    public interface IDlibObject
    {

    }

    public abstract class ObjectSerializer
    {
        public static T Load<T>(string path) where T: IDlibObject
        {
            XmlSerializer mSerializer = new XmlSerializer(typeof(T));

            using (System.IO.Stream fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return (T)mSerializer.Deserialize(fStream);
            }
        }

        public static void Save<T>(DataSet obj, string path) where T : IDlibObject
        {
            XmlSerializer mSerializer = new XmlSerializer(typeof(T));

            using (System.IO.Stream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                mSerializer.Serialize(fStream, obj);
            }
        }
    }
}
