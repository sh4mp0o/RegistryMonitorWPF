using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using RegistryMonitorWPF.Models;
using RegistryMonitorWPF.Helpers;

namespace RegistryMonitorWPF.Services
{
    public static class DataFetcher
    {
        /// <summary>
        /// Асинхронно загружает XML-данные с указанного URL.
        /// </summary>
        /// <param name="url">URL-адрес XML-файла</param>
        /// <returns>Содержимое XML-файла в виде строки</returns>
        public static async Task<string> DownloadXmlFromUrlAsync(string url)
        {
            try
            {
                WebClient client = new WebClient();
                return await client.DownloadStringTaskAsync(new Uri(url));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }
        }

        /// <summary>
        /// Асинхронно читает содержимое локального XML-файла.
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Содержимое XML-файла в виде строки</returns>
        public static async Task<string> ReadFileAsync(string filePath)
        {
            try
            {
                return await Task.Run(() => File.ReadAllText(filePath));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }
        }
    }
}
