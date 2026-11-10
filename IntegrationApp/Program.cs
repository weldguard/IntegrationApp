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

            string xmlFilePath = config["LogikalSample:XMLFilePath"];
            string OneCprogID = config["OneCConfig:ProgID"];
            string baseUsrPwd = config["OneCConfig:BaseUsrPwd"];
            string articleProduct = config["OneCConfig:ArtProduct"];

            XmlSerializer xmlSerialyzer = new XmlSerializer(typeof(LogikalData));
            using StreamReader xmlFile = new StreamReader(xmlFilePath, Encoding.UTF8);
            LogikalData logData = (LogikalData)xmlSerialyzer.Deserialize(xmlFile);

            Console.WriteLine(logData);

            Type typeOneC = Type.GetTypeFromProgID(OneCprogID);
            dynamic objOneC = Activator.CreateInstance(typeOneC);
            dynamic v8Db = objOneC.Connect(baseUsrPwd);

            dynamic nomenclatureProduct = v8Db.Справочники.Номенклатура.НайтиПоРеквизиту("Артикул", articleProduct);
            dynamic newDoc = v8Db.Документы.ЗаказНаПроизводство.СоздатьДокумент();
            newDoc.Комментарий = logData.xmlHeader.ToString();
            foreach (var xmlPos in logData.xmlPositions)
            {
                dynamic productRow = newDoc.Продукция.Добавить();
                productRow.Номенклатура = nomenclatureProduct;
                productRow.КлючСвязи = xmlPos.id;
                newDoc.Комментарий += $"Pos: {xmlPos.id}, Description: {xmlPos.description} \n";
                foreach (var xmlCopm in xmlPos.xmlComponents)
                {
                    dynamic newRow = newDoc.Запасы.Добавить();
                    dynamic nomenclature = v8Db.Справочники.Номенклатура.НайтиПоРеквизиту("Артикул", xmlCopm.xmlArticle);
                    newRow.Номенклатура = nomenclature;
                    newRow.Количество = xmlCopm.quantity;
                    newRow.КлючСВязи = xmlPos.id;
                }
            }
            newDoc.Дата = DateTime.Now;
            dynamic orgOneC = v8Db.Справочники.Организации.НайтиПоНаименованию("Ассоль ООО");
            newDoc.Организация = orgOneC;
            newDoc.Записать();
        }
    }
}