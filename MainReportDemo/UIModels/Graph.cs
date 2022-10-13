using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts.Wpf;
using LiveCharts;
using System.Windows.Media;

namespace MainReportDemo.UIModels
{
    internal class Graph
    {
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; } = value => value.ToString() + "%";
        public List<string> Labels = new List<string> { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

        public void BuildGraph()
        {
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Поступило обращений",
                    Values = new ChartValues<double> { 0, 3, 1, 3, 5, 9, 10, 1, 5, 2, 5, 1 },
                    Fill = Brushes.Gray,
                },

                new LineSeries
                {
                    Title = "% обращений с нарушенным SLA",
                    Values = new ChartValues<double> { 1, 3, 2, 4, 8, 7, 15, 9, 4, 5, 2, 15 },
                    Fill = Brushes.Transparent,
                    StrokeThickness = 1,
                    LineSmoothness = 0,
                    Stroke = Brushes.DarkBlue
                }
            };
        }
    }
}
