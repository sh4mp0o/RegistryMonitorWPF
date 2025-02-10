using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using RegistryMonitorWPF.Models;

namespace RegistryMonitorWPF.Services
{
    /// <summary>
    /// Класс для обработки XML-данных, содержащих записи реестра.
    /// </summary>
    public class XmlProcessor
    {
        /// <summary>
        /// Асинхронно обрабатывает XML-данные и извлекает записи, соответствующие указанной дате.
        /// </summary>
        /// <param name="xmlContent">Строка XML-содержимого.</param>
        /// <param name="analysisDate">Дата, с которой необходимо фильтровать записи.</param>
        /// <returns>Список записей реестра, соответствующих критериям.</returns>
        public async Task<List<RegistryRecord>> ProcessXmlDataAsync(string xmlContent, DateTime analysisDate)
        {
            return await Task.Run(() =>
            {
                var records = new List<RegistryRecord>();

                XDocument doc = XDocument.Parse(xmlContent);
                foreach (var item in doc.Descendants("item"))
                {
                    var record = new RegistryRecord
                    {
                        Date = DateTime.TryParse(item.Element("inclusionDate")?.Value, out DateTime incDate) ? incDate :
                               DateTime.TryParse(item.Element("exclusionDate")?.Value, out DateTime excDate) ? excDate :
                               DateTime.TryParse(item.Element("editHistory")?.Element("date")?.Value, out DateTime editDate) ? editDate : (DateTime?)null,
                        Name = item.Element("name")?.Value ?? "Неизвестно",
                        Id = item.Element("registrationNumber")?.Value ?? "Нет ID",
                        Type = GetEventType(item)
                    };

                    if (record.Date.HasValue && record.Date.Value >= analysisDate)
                    {
                        records.Add(record);
                    }
                }

                return records;
            });
        }

        /// <summary>
        /// Определяет тип события для записи реестра (например, включение, исключение, изменение).
        /// </summary>
        /// <param name="item">Элемент XML, содержащий данные записи.</param>
        /// <returns>Тип события в виде строки.</returns>
        private string GetEventType(XElement item)
        {
            if (!string.IsNullOrEmpty(item.Element("exclusionDate")?.Value)) return "Исключение из реестра";
            if (!string.IsNullOrEmpty(item.Element("inclusionDate")?.Value)) return "Включение в реестр";
            if (!string.IsNullOrEmpty(item.Element("editHistory")?.Element("date")?.Value)) return "Изменение записи";
            return "Прочее";
        }
    }
}
