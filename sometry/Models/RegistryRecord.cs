using System;
using System.ComponentModel;

namespace RegistryMonitorWPF.Models
{
    /// <summary>
    /// Представляет запись из реестра.
    /// </summary>
    public class RegistryRecord : INotifyPropertyChanged
    {
        private DateTime? date;
        private string name;
        private string id;
        private string type;

        public DateTime? Date { get => date; set { date = value; OnPropertyChanged(nameof(Date)); } }
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }
        public string Id { get => id; set { id = value; OnPropertyChanged(nameof(Id)); } }
        public string Type { get => type; set { type = value; OnPropertyChanged(nameof(Type)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
