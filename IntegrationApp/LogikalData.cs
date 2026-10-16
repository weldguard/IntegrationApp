using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegrationApp
{
    [XmlRoot("LogikalERPExport")]
    public class LogikalData
    {
        [XmlElement]
        public XMLHeader xmlHeader;
        [XmlArray("Positions")]
        [XmlArrayItem("Position")]
        public List<XMLPosition> xmlPositions;
    }

    public class XMLHeader
    {
        [XmlElement("ProjectNumber")]
        public string projectNumber;
        [XmlElement("ProjectName")]
        public string name;
        [XmlElement("CustomerCode")]
        public string customerCode;
    }

    public class XMLPosition
    {
        [XmlAttribute("Id")]
        public int id;
        [XmlArray("Components")]
        [XmlArrayItem("Component")]
        public List<XMLComponent> xmlComponents;
    }

    public class XMLComponent
    {
        [XmlElement("Article")]
        public string xmlArticle;
        [XmlElement("Quanity")]
        public double Quantity;
    }


}
