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
        public List<object> LoadColumns(OpenFileDialog ofd)
        {
            List<object> columns = new List<object>();

            using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(); //get data from file

                    for (int i = 0; i < result.Tables[0].Columns.Count; i++)
                    {
                        try
                        {
                            columns.Add(result.Tables[0].Rows[0].ItemArray[i]);
                        }
                        catch (Exception ex)
                        {
                            int err_column = i + 1;
                            MessageBox.Show(ex.Message + "\nСтолбец " + err_column + " будет пропущен.", "Ошибка");

                            continue;
                        }
                    }
                }
            }

            return columns;
        }
        public Dictionary<object, object> LoadFile(OpenFileDialog ofd, int startIndex, int endIndex)
        {
            if (startIndex > endIndex)
            {
                int a = endIndex;
                endIndex = startIndex;
                startIndex = a;
            }
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
                            fileData.Add(result.Tables[0].Rows[i].ItemArray[startIndex], result.Tables[0].Rows[i].ItemArray[endIndex]);
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

            MessageBox.Show("Данные загружены.", "Готово");

            return fileData;
        }
    }
}
