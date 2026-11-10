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
        [XmlElement("Header")]
        public XMLHeader xmlHeader;
        [XmlArray("Positions")]
        [XmlArrayItem("Position")]
        public List<XMLPosition> xmlPositions;

        public override string ToString()
        {
            var xmlPosCol = xmlPositions.Select(x => x.ToString());
            var xmlPosStr = string.Join(',', xmlPosCol);
            return $"Header:\n{xmlHeader}\nPositions:\n{xmlPosStr}\n";
        }
    }

    public class XMLHeader
    {
        [XmlElement("ProjectNumber")]
        public string projectNumber;
        [XmlElement("ProjectName")]
        public string projectName;
        [XmlElement("CustomerCode")]
        public string customerCode;

        public override string ToString()
        {
            return $"Project Number: {projectNumber}\nProject Name: {projectName}\nCustomer Code: {customerCode}\n";
        }
    }

    public class XMLPosition
    {
        [XmlAttribute("Id")]
        public int id;
        [XmlElement("Description")]
        public string description;
        [XmlArray("Components")]
        [XmlArrayItem("Component")]
        public List<XMLComponent> xmlComponents;

        public override string ToString()
        {
            var xmlCompCollect = xmlComponents.Select(x => x.ToString());
            var strComps = string.Join('\n',xmlCompCollect);
            return $"ID = {id}, Description = {description}\nComponents:\n{strComps}";
        }
    }

    public class XMLComponent
    {
        [XmlElement("Article")]
        public string xmlArticle;
        [XmlElement("Quantity")]
        public double quantity;

        public override string ToString()
        {
            return $"Article = {xmlArticle}, Quanity = {quantity}";
        }
    }


}
