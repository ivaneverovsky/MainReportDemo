using System.Windows;

namespace MainReportDemo.UI
{
    public partial class ResultsWindow : Window
    {
        public ResultsWindow()
        {
            InitializeComponent();
            foreach (var item in ((MainWindow)Application.Current.MainWindow).reportListView.Items)
                reportListViewFull.Items.Add(item);

            CloseButton.Focus();
        }

        private void CloseTable(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
