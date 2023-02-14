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
    /// Логика взаимодействия для CDSWindow.xaml
    /// </summary>
    public partial class CDSWindow : Window
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public bool Bull { get; set; }
        public CDSWindow()
        {
            InitializeComponent();
            Bull = false;
            Choose.Focus();
        }

        private void ChooseColumns(object sender, RoutedEventArgs e)
        {
            var selectionStart = startCombo.SelectedIndex;
            var selectionEnd = endCombo.SelectedIndex;

            if (selectionStart == -1)
            {
                MessageBox.Show("Не выбран столбец с номерами заявок.", "Внимание");
                return;
            }
            else if (selectionEnd == -1)
            {
                MessageBox.Show("Не выбран столбец с оценками пользователей.", "Внимание");
                return;
            }
            else if (selectionStart == selectionEnd)
            {
                MessageBox.Show("Столбцы совпадают, необходимо выбрать разные.", "Внимание");
                return;
            }

            StartIndex = selectionStart; 
            EndIndex = selectionEnd;
            Bull = true;

            Close();
        }
    }
}
