using System.Text;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;

namespace IntegrationApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            string xmlPath = config["LogikalSample:XMLFilePath"];
            string progID = config["OneCConfig:progID"];
            string baseUsrPwd = config["OneCConfig:BaseUsrPwd"];
            XmlSerializer xmlSerialyzer = new XmlSerializer(typeof(LogikalData));
            using StreamReader xmlFile = new StreamReader(xmlPath, Encoding.UTF8);
            LogikalData logData = (LogikalData)xmlSerialyzer.Deserialize(xmlFile);
            Console.WriteLine(logData);
            Type typeOneC = Type.GetTypeFromProgID(progID);
            dynamic objOneC = Activator.CreateInstance(typeOneC);
            dynamic v8Db = objOneC.Connect(baseUsrPwd);

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
            покрытие тестами логических функций парсинга
            */
        }
    }
}