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
            string xmlPath = config["LogikalSample:XMLFile"];
            string progID = config["1cPath:1cVersion"];
            string connectionString = config["ConnectionString"];
            XmlSerializer xmlSerialyzer = new XmlSerializer(typeof(LogikalData));
            using (StreamReader xmlFile = new StreamReader(xmlPath, Encoding.UTF8))
            {
                LogikalData logData = (LogikalData)xmlSerialyzer.Deserialize(xmlFile);
            }
            Type typeOneC = Type.GetTypeFromProgID(progID);
            dynamic objOneC = Activator.CreateInstance(typeOneC);
            dynamic v8Db = objOneC.Connect(connectionString);

            /* TODO:        
            схема: 
            ссылки на номенклатуру
            создание документа
            из класса внутри c# в документ 1с
            заполнение документа
            внедряем маппинг
            внедряем проверку необходимых
            если не находим проблем, то сохраняем бэкап
            если проблема, то анализируем и логируем изменения
            не забыть, что юзер будет technical в 1с с ограниченными правами
            */



        }
    }
}