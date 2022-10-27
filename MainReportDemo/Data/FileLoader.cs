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
            using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(); //get data from file
                    for (int i = 1; i < result.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            fileData.Add(result.Tables[0].Rows[i].ItemArray[1], result.Tables[0].Rows[i].ItemArray[40]);
                        }
                        catch (Exception ex)
                        {
                            int err_string = i + 1;
                            MessageBox.Show(ex.Message + "\nСтрока " + err_string + " будет пропущена.", "Ошибка");
                            continue;
                        }
                    }
                }
            }
            MessageBox.Show("Файл загружен!", "Готово");
            return fileData;
        }
    }
}
