using System.Collections.Generic;
using System.Xml.Serialization;

namespace ImageLabeler
{
    public interface IDataSet : IDlibObject
    {
        string Comment { get; set; }
        List<DataImage> Images { get; set; }
        string Name { get; set; }
        string Path { get; set; }
    }

    [XmlType("dataset")]
    public sealed class DataSet : IDataSet
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("comment")]
        public string Comment { get; set; }

        [XmlArray("images")]
        public List<DataImage> Images { get; set; }

        [XmlIgnore]
        public string Path { get; set; }

        public DataSet()
        {
            Images = new List<DataImage>();
        }
    }
}
