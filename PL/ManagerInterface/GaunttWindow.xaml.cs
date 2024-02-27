using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using PL.Engineer;
using PL.EngineerInterface;
using PL.ManagerInterface;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GaunttWindow : Window
    {
        private DataTable DataTable { get; set; }
        public DataView DataView { get; set; }

        public GaunttWindow()
        {
            InitializeComponent();
        }
        private void AddRow()
        {
            DataRow dr = DataTable.NewRow();
            dr["Test"] = "Wert1";
            DataTable.Rows.Add(dr);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            List<Equipment> eq = new List<Equipment>
            {
                new Equipment(){Name="IC Chip",Operaion=1,StartDate=new DateTime(2017, 10, 5, 0, 0, 0),EndDate= new DateTime(2017, 10, 27, 0, 0, 0)},
                new Equipment(){Name="IC Chip",Operaion=2,StartDate=new DateTime(2017, 10, 28, 0, 0, 0),EndDate= new DateTime(2017, 10, 29, 0, 0, 0)},
                new Equipment(){Name="IC Chip",Operaion=3,StartDate=new DateTime(2017, 10, 15, 0, 0, 0),EndDate= new DateTime(2017, 10, 31, 0, 0, 0)},
                new Equipment(){Name="PC",Operaion=4,StartDate=new DateTime(2017, 10, 2, 0, 0, 0),EndDate= new DateTime(2017, 10, 24, 0, 0, 0)},
                new Equipment(){Name="PC",Operaion=3,StartDate=new DateTime(2017, 10, 25, 0, 0, 0),EndDate= new DateTime(2017, 10, 28, 0, 0, 0)},
                new Equipment(){Name="Computer",Operaion=2,StartDate=new DateTime(2017, 10, 23, 0, 0, 0),EndDate= new DateTime(2017, 10, 24, 0, 0, 0)},
                new Equipment(){Name="Lap",Operaion=4,StartDate=new DateTime(2017, 10, 20, 0, 0, 0),EndDate= new DateTime(2017, 10, 21, 0, 0, 0)},
            };

            DataTable = new DataTable();

            DateTime dtTodayNoon = DateTime.Now;
            DateTime dtYestMidnight =DateTime.Now-new TimeSpan(days:0,hours:12,0,0);
            double diffResult = (dtYestMidnight - dtTodayNoon).TotalDays;
            DataTable.Columns.Add("Date and Equipment");
            for (int i = 0; i <= diffResult; i++)
            {
                DataTable.Columns.Add(dtTodayNoon.AddDays(i).ToString("MMM dd"));
            }

            DataRow dr = DataTable.NewRow();
            for (int i = 0; i < eq.Count; i++)
            {
                for (int j = 0; j < DataTable.Columns.Count; j++)
                {
                    if (DataTable.Columns[j].ToString() == "Date and Equipment")
                    {
                        if (dr[j].ToString() != eq[i].Name.ToString())
                        {
                            if (dr[j].ToString() != "")
                            {
                                DataTable.Rows.Add(dr);
                                dr = DataTable.NewRow();
                            }
                            dr[j] = eq[i].Name.ToString();
                        }

                    }
                    else if (Convert.ToDateTime(DataTable.Columns[j].ToString(), new CultureInfo("en-US")) >= eq[i].StartDate && Convert.ToDateTime(DataTable.Columns[j].ToString(), new CultureInfo("en-US")) <= eq[i].EndDate)
                    {
                        dr[j] = eq[i].Operaion.ToString();
                    }
                }

            }
            DataTable.Rows.Add(dr);
            //FooBar1.ItemsSource = DataTable.DefaultView;
        }
    }


    public class ForeGroundConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (str == null) return Brushes.Black;

            int intValue;
            if (!int.TryParse(str, out intValue)) return Brushes.Black;

            if (intValue == 1) return Brushes.Red;
            else if (intValue == 2) return Brushes.Yellow;
            else if (intValue == 3) return Brushes.Blue;
            else return Brushes.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ValueColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (str == null) return Brushes.Black;

            int intValue;
            if (!int.TryParse(str, out intValue)) return null;

            if (intValue == 1) return Brushes.Red;
            else if (intValue == 2) return Brushes.Yellow;
            else if (intValue == 3) return Brushes.Blue;
            else return Brushes.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Equipment
    {
        public string Name { get; set; }
        public int Operaion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
