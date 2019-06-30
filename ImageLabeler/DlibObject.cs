using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ImageLabeler
{
    public abstract class DlibObject
    {
        public static T Load<T>(string path) where T: DlibObject
        {
            XmlSerializer mSerializer = new XmlSerializer(typeof(T));

            using (System.IO.Stream fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return (T)mSerializer.Deserialize(fStream);
            }
        }

        public static void Save<T>(DataSet obj, string path) where T : DlibObject
        {
            XmlSerializer mSerializer = new XmlSerializer(typeof(T));

            using (System.IO.Stream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                mSerializer.Serialize(fStream, obj);
            }
        }
    }

    [XmlType("dataset")]
    public sealed class DataSet : DlibObject
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("comment")]
        public string Comment { get; set; }

        [XmlArray("images")]
        public List<DataImage> Images { get; set; }
    }

    [XmlType("image")]
    public sealed class DataImage : DlibObject
    {
        [XmlAttribute("file")]
        public string File { get; set; }

        [XmlElement("box")]
        public List<DataBox> Boxes { get; set; }

        public override string ToString()
        {
            return File;
        }
    }

    [XmlType("box")]
    public sealed class DataBox : DlibObject
    {
        [XmlAttribute("top")]
        public int Top { get; set; }

        [XmlAttribute("left")]
        public int Left { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        [XmlElement("label")]
        public string Label { get; set; }

        public override string ToString()
        {
            return string.Format("({0}, {1}) W:{2} H:{3}", Top, Left, Width, Height);
        }
    }
}
