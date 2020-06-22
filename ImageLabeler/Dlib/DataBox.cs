using System.Xml.Serialization;

namespace ImageLabeler
{
    public interface IDataBox : IDlibObject
    {
        int Height { get; set; }
        string Label { get; set; }
        int Left { get; set; }
        int Top { get; set; }
        int Width { get; set; }
    }

    [XmlType("box")]
    public sealed class DataBox : IDataBox
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
