using System.Collections.Generic;
using System.Xml.Serialization;

namespace ImageLabeler
{
    public interface IDataImage : IDlibObject
    {
        List<DataBox> Boxes { get; set; }
        string File { get; set; }
    }

    [XmlType("image")]
    public sealed class DataImage : IDataImage
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
}
