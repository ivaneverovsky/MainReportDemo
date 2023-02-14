using MainReportDemo.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainReportDemo.UI
{
    /// <summary>
    /// Логика взаимодействия для ContractInfoWindow.xaml
    /// </summary>
    public partial class ContractInfoWindow : Window
    {
        public ContractInfoWindow()
        {
            InitializeComponent();

            CloseButton.Focus();
        }

        private void CloseTable(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
