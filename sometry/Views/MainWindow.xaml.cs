using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using RegistryMonitorWPF.Models;
using RegistryMonitorWPF.ViewModels;

namespace RegistryMonitorWPF.Views
{
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
            this.Background = new SolidColorBrush(Colors.White);
        }
        /// <summary>
        /// Обрабатывает кнопку "Обзор".
        /// </summary>
        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.BrowseFile();
        }

        /// <summary>
        /// Обрабатывает двойной щелчок по записи в DataGrid.
        /// </summary>
        private void ResultsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ResultsDataGrid.SelectedItem is RegistryRecord selectedRecord)
            {
                if (DataContext is MainViewModel viewModel)
                {
                    viewModel.ShowRecordDetailsCommand.Execute(selectedRecord);
                }
            }
        }
    }
}
