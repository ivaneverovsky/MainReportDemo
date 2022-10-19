using ExcelDataReader;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace MainReportDemo.Data
{
    internal class FileLoader
    {
        public Dictionary<object, object> LoadFile(OpenFileDialog ofd)
        {
            Dictionary<object, object> fileData = new Dictionary<object, object>();

            try
            {
                using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet(); //get data from file

                        for (int i = 1; i < result.Tables[0].Rows.Count; i++)
                            fileData.Add(result.Tables[0].Rows[i].ItemArray[1], result.Tables[0].Rows[i].ItemArray[40]);
                    }
                }

                MessageBox.Show("Файл загружен.", "Готово");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }

            return fileData;
        }
    }
}
