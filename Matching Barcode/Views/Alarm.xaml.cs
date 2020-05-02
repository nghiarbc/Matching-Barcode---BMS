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

namespace Matching_Barcode.Views
{
    /// <summary>
    /// Interaction logic for Alarm.xaml
    /// </summary>
    public partial class Alarm : Window
    {
        public string alarmcode = "";
        public string alarmnoidung = "";
        public string alarmxuly = "";
        public Alarm(string _alarmcode, string _alarmnoidung, string _alarmxuly)
        {
            InitializeComponent();
            alarmcode = _alarmcode;
            alarmnoidung = _alarmnoidung;
            alarmxuly = _alarmxuly;
        }

        private void btnClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbAlarmCode.Content = alarmcode;
            lbAlarmNoidung.Content = alarmnoidung;
            lbAlarmXuly.Content = alarmxuly;
            
        }
    }
}
