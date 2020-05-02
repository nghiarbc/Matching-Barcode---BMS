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
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : Window
    {
        ClsConfig.ClsConfig config = new ClsConfig.ClsConfig();
        LogfileCreator.clsMachineLog MClog = new LogfileCreator.clsMachineLog();
        
        public Setup()
        {
            InitializeComponent();
        }

        private void btnthoat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.Close();
            }
        }

        private void btnsave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //xử lý lưu data vào text file
                List<string> _list = new List<string>();
                _list.Add("< Config >");
                _list.Add("MODEL_NAME = " + txtModel.Text);
                _list.Add("CP_PATTERN = " + txtCPPattern.Text);
                _list.Add("HP_PATTERN = " + txtHPPattern.Text); 
                _list.Add("BMS_PATTERN = " + txtBMSPattern.Text);
                _list.Add("USER = " + txtuser.Text);
                _list.Add("ID = " + txtid.Text);
                _list.Add("SYSTEM = " + cbsystem.Text);
                _list.Add("< End >");
                if (config.Config_Save(_list, Environment.CurrentDirectory +  @"\Data\Config\" , "Config.ini"))
                    MessageBox.Show("Thành công !", "Infor", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Thất bại, kiểm tra lại thông tin !", "Infor", MessageBoxButton.OK, MessageBoxImage.Information);

                MClog.Save_log("save_config:" + txtModel.Text + txtCPPattern.Text + txtHPPattern.Text + txtBMSPattern.Text + txtuser.Text + txtid.Text);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            data.Add("SYSTEM");
            data.Add("LOACAL");

            // ... Get the ComboBox reference.
            cbsystem.ItemsSource = data;
            List<string> _list = new List<string>();
            if (config.Config_Load(ref _list, Environment.CurrentDirectory + @"\Data\Config\", "Config.ini"))
            {
                foreach(string str in _list)
                {
                    string[] _str = str.Split('=');
                    switch (_str[0].Trim())
                    {
                        case "MODEL_NAME":
                            txtModel.Text = _str[1].Trim();
                            break;
                        case "CP_PATTERN":
                            txtCPPattern.Text = _str[1].Trim();
                            break;
                        case "HP_PATTERN":
                            txtHPPattern.Text = _str[1].Trim();
                            break;
                        case "BMS_PATTERN":
                            txtBMSPattern.Text = _str[1].Trim();
                            break;
                        case "USER":
                            txtuser.Text = _str[1].Trim();
                            break;
                        case "ID":
                            txtid.Text = _str[1].Trim();
                            break;
                        case "SYSTEM":
                            cbsystem.Text = _str[1].Trim();
                            break;
                    }
                }
            }
            else
                MessageBox.Show("Không load được file Config.ini ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void cbsystem_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
