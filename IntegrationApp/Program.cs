using System.Text;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;

namespace IntegrationApp
{
    internal class Program
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

        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            XmlSerializer xmlSerialyzer = new XmlSerializer(typeof(LogikalData));
            string xmlPath = config["LogikalSample:XMLFile"];
            using (StreamReader xmlFile = new StreamReader(xmlPath, Encoding.UTF8))
            {
                LogikalData logData = (LogikalData)xmlSerialyzer.Deserialize(xmlFile);
            }
            string progID = config["1cPath:1cVersion"];
            Type type1c = Type.GetTypeFromProgID(progID);
            dynamic obj1c = Activator.CreateInstance(type1c);
        }
    }
}