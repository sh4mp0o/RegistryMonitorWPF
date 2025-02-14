using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using RegistryMonitorWPF.Models;
using RegistryMonitorWPF.Services;
using RegistryMonitorWPF.Helpers;

namespace RegistryMonitorWPF.ViewModels
{
    /// <summary>
    /// ViewModel для главного окна приложения. Управляет загрузкой данных, обработкой XML и взаимодействием с UI.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly XmlProcessor _xmlProcessor = new XmlProcessor();
        public ObservableCollection<RegistryRecord> Records { get; set; } = new ObservableCollection<RegistryRecord>();

        private string _filePath;
        private string _url;
        private bool _isOnline;
        private string _totalRecords;
        private DateTime? _selectedDate;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Путь к локальному XML-файлу.
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        /// <summary>
        /// URL-адрес XML-файла.
        /// </summary>
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                OnPropertyChanged(nameof(Url));
            }
        }

        /// <summary>
        /// Определяет, используется ли онлайн-режим.
        /// </summary>
        public bool IsOnline
        {
            get => _isOnline;
            set
            {
                _isOnline = value;
                OnPropertyChanged(nameof(IsOnline));
                OnPropertyChanged(nameof(IsOffline));
            }
        }

        /// <summary>
        /// Определяет, используется ли оффлайн-режим.
        /// </summary>
        public bool IsOffline => !IsOnline;

        /// <summary>
        /// Дата, с которой нужно анализировать данные.
        /// </summary>
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        /// <summary>
        /// Отображает общее количество записей в реестре.
        /// </summary>
        public string TotalRecords
        {
            get => _totalRecords;
            set
            {
                _totalRecords = value;
                OnPropertyChanged(nameof(TotalRecords));
            }
        }
        public ICommand BrowseFileCommand { get; }
        public ICommand ShowRecordDetailsCommand { get; }
        public ICommand SearchChangesCommand { get; }

        /// <summary>
        /// Конструктор ViewModel. Инициализирует команды.
        /// </summary>
        public MainViewModel()
        {
            BrowseFileCommand = new RelayCommand(BrowseFile);
            SearchChangesCommand = new RelayCommand(async () => await LoadDataAsync(), CanSearch);
            ShowRecordDetailsCommand = new RelayCommand<RegistryRecord>(ShowRecordDetails);
        }



        /// <summary>
        /// Определяет, можно ли выполнить команду поиска (нужно выбрать дату).
        /// </summary>
        /// <returns>Возвращает true, если дата выбрана.</returns>
        private bool CanSearch()
        {
            return SelectedDate != null;
        }

        /// <summary>
        /// Отображает развернутую информацию о записи в реестре.
        /// </summary>
        private void ShowRecordDetails(RegistryRecord record)
        {
            if (record == null) return;

            MessageBox.Show($"Детали записи:\n\n"
                + $"Дата регистрации заявления: {record.ApplicationDate?.ToShortDateString()}\n"
                + $"Дата гос.регистрации: {record.RegistrationDate?.ToShortDateString()}\n"
                + $"Дата внесения в реестр: {record.InclusionDate?.ToShortDateString()}\n"
                + $"Дата исключения из реестра: {record.ExclusionDate?.ToShortDateString()}\n"
                + $"Название ПО: {record.Name}\n"
                + $"ID: {record.Id}\n"
                + $"Тип события: {record.Type}",
                "Подробности записи", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Загружает данные из XML-файла или URL.
        /// </summary>
        public async Task LoadDataAsync()
        {
            if (SelectedDate == null)
            {
                MessageBox.Show("Выберите корректную дату.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string xmlData;
            try
            {
                if (IsOnline)
                {
                    xmlData = await DataFetcher.DownloadXmlFromUrlAsync(Url);
                }
                else
                {
                    if (!File.Exists(FilePath))
                    {
                        MessageBox.Show("Файл не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    xmlData = await DataFetcher.ReadFileAsync(FilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Application.Current.Dispatcher.Invoke(() => Records.Clear());

            var parsedRecords = await _xmlProcessor.ProcessXmlDataAsync(xmlData, SelectedDate.Value);

            if (parsedRecords.Count > 0)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var record in parsedRecords)
                    {
                        Records.Add(record);
                    }
                });
            }
            else
            {
                MessageBox.Show("Нет записей для указанной даты.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            TotalRecords = $"Всего записей: {Records.Count}";
        }

        /// <summary>
        /// Открывает диалоговое окно для выбора файла.
        /// </summary>
        public void BrowseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML файлы (*.xml)|*.xml|Все файлы (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Вызывает обновление состояния команд (например, при изменении даты).
        /// </summary>
        /// <param name="propertyName">Имя свойства, которое изменилось.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested(); // Для обновления команд
        }
    }
}
