using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MainReportDemo.Data
{
    internal class FileLoader
    {
        public async Task LoadFile(OpenFileDialog ofd)
        {
            List<string> fileData = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(ofd.FileName))
                {
                    while (true)
                    {
                        
                    }
                }
                MessageBox.Show("done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}
