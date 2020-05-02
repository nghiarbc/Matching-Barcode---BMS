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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignColors;
using MaterialDesignThemes;
namespace Matching_Barcode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClsConfig.ClsConfig config = new ClsConfig.ClsConfig();
        LogfileCreator.clsMachineLog MClog = new LogfileCreator.clsMachineLog();
        Models.clsMatchingCP7S2P Matching = new Models.clsMatchingCP7S2P();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btncaidat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed )
            {
                Matching_Barcode.Views.Setup setup = new Views.Setup();
                setup.ShowDialog();
                Load_Config();
            }
        }

        private void Initial_Config()
        {
            Load_Config();
            Matching.CreatLogLocal(MODEL);
        }
        private void Load_Config()
        {
            List<string> _list = new List<string>();
            if (config.Config_Load(ref _list, Environment.CurrentDirectory + @"\Data\Config\", "Config.ini"))
            {
                foreach (string str in _list)
                {
                    string[] _str = str.Split('=');
                    switch (_str[0].Trim())
                    {
                        case "MODEL_NAME":
                            MODEL = txtmodel.Text = _str[1].Trim();
                            break;
                        case "CELL_BATCH":
                            CELLBATCH= txtcellbatch.Text = _str[1].Trim();
                            break;
                        case "CELL_LOT":
                            txtcelllot.Text = _str[1].Trim();
                            break;
                        case "CP_PATTERN":
                            CPPattern = txtCPPattern.Text = _str[1].Trim();
                            break;
                        case "USER":
                            txtusername.Text = _str[1].Trim();
                            break;
                        case "ID":
                            txtuserid.Text = _str[1].Trim();
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Không load được file Config.ini ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CheckConfig()
        {
            try
            {
                if(txtmodel.Text !="" && txtcellbatch.Text != "" && txtcelllot.Text != "" && txtCPPattern.Text != "" && txtusername.Text != "" && txtuserid.Text != "" )
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initial_Config();
            MClog.Save_log("Window_Loaded");
        }

        #region Define_Maching
        private int STEP = 1;
        private string CPPattern;
        private string CELLBATCH;
        private string MODEL;
        #endregion
        private void txtbarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter )
            {
                Matching_Start(txtbarcode.Text);
                txtbarcode.Text = "";
            }
        }
        private void Matching_Start(string _barcode)
        {
            if (_barcode.Length <= 0) return;
            switch(STEP)
            {
                case 0:
                    //if(Matching.Check_Config())
                    //{
                    //    STEP = 1;
                    //    break;
                    //}
                    //else
                    //{
                    //    errorcode++;
                    //    HienthiKetqua_Final(Result.FAIL, STEP);
                    //}
                    break;
                case 1: //CP CP Pattern
                    Matching.Matching_Reset();
                    HienthiKetqua_Final(Result.READY, STEP, _barcode);
                    if (Matching.Check_Config())
                    {
                        Matching._DATAMatchingReport.MC_TIME = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Matching._DATAMatchingReport.MODEL = txtmodel.Text;
                        Matching._DATAMatchingReport.CELL_BATCH = txtcellbatch.Text;
                        Matching._DATAMatchingReport.CELL_LOT = txtcelllot.Text;
                        Matching._DATAMatchingReport.USER = txtusername.Text;
                        Matching._DATAMatchingReport.ID = txtuserid.Text;
                        Matching._DATAMatchingReport.CP = _barcode;
                        if (Matching.Check_CP_Pattern(_barcode, CPPattern))
                        {
                            HienthiKetqua_Item(txtCPCode, true, _barcode, STEP);
                            STEP = 2;
                            break;
                        }
                        else
                        {
                            //errorcode++;
                            HienthiKetqua_Item(txtCPCode, false, _barcode, STEP);
                        }
                    }
                    else
                    {
                       // errorcode++;
                        HienthiKetqua_Final(Result.FAIL, 0, _barcode);
                    }                   
                    break;
                case 2:
                    Matching._DATAMatchingReport.CELL_ID1 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID1, true, _barcode, STEP);
                        STEP = 3;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID1, false, _barcode, STEP);
                    }
                    break;
                case 3:
                    Matching._DATAMatchingReport.CELL_ID2 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID2, true, _barcode, STEP);
                        STEP = 4;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID2, false, _barcode, STEP);
                    }
                    break;
                case 4:
                    Matching._DATAMatchingReport.CELL_ID3 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID3, true, _barcode, STEP);
                        STEP = 5;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID3, false, _barcode, STEP);
                    }
                    break;
                case 5:
                    Matching._DATAMatchingReport.CELL_ID4 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID4, true, _barcode, STEP);
                        STEP = 6;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID4, false, _barcode, STEP);
                    }
                    break;
                case 6:
                    Matching._DATAMatchingReport.CELL_ID5 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID5, true, _barcode, STEP);
                        STEP = 7;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID5, false, _barcode, STEP);
                    }
                    break;
                case 7:
                    Matching._DATAMatchingReport.CELL_ID6 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID6, true, _barcode, STEP);
                        STEP = 8;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID6, false, _barcode, STEP);
                    }
                    break;
                case 8:
                    Matching._DATAMatchingReport.CELL_ID7 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID7, true, _barcode, STEP);
                        STEP = 9;
                        break;
                    }
                    else
                    {
                        // errorcode++;
                        HienthiKetqua_Item(txtCellID7, false, _barcode, STEP);
                    }
                    break;
                case 9:
                    Matching._DATAMatchingReport.CELL_ID8 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID8, true, _barcode, STEP);
                        STEP = 10;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID8, false, _barcode, STEP);
                    }
                    break;
                case 10:
                    Matching._DATAMatchingReport.CELL_ID9 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID9, true, _barcode, STEP);
                        STEP =11;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID9, false, _barcode, STEP);
                    }
                    break;
                case 11:
                    Matching._DATAMatchingReport.CELL_ID10 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID10, true, _barcode, STEP);
                        STEP = 12;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID10, false, _barcode, STEP);
                    }
                    break;
                case 12:
                    Matching._DATAMatchingReport.CELL_ID11 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID11, true, _barcode, STEP);
                        STEP = 13;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID11, false, _barcode, STEP);
                    }
                    break;
                case 13:
                    Matching._DATAMatchingReport.CELL_ID12 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID12, true, _barcode, STEP);
                        STEP = 14;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID12, false, _barcode, STEP);
                    }
                    break;
                case 14:
                    Matching._DATAMatchingReport.CELL_ID13 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID13, true, _barcode, STEP);
                        STEP = 15;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID13, false, _barcode, STEP);
                    }
                    break;
                case 15:
                    Matching._DATAMatchingReport.CELL_ID14 = _barcode;
                    if (Matching.Check_Cell_ID(_barcode, CELLBATCH))
                    {
                        HienthiKetqua_Item(txtCellID14, true, _barcode, STEP);
                        HienthiKetqua_Final(Result.PASS, STEP, _barcode);
                        STEP = 1;
                        break;
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID14, false, _barcode, STEP);
                    }
                    break;
            }
        }
        private void HienthiKetqua_Item(TextBox _textbox, bool Flag, string _barcode, int _STEP)
        {
            _textbox.Text = _barcode;
            if (Flag)
            {
                _textbox.Background = Brushes.DarkSeaGreen;
                HienthiKetqua_Final(Result.MATCHING, 0, _barcode);
            }
            else
            {
                _textbox.Background = Brushes.Red;
                HienthiKetqua_Final(Result.FAIL, STEP, _barcode);
            }
        }
        enum Result
        {
            PASS, FAIL, MATCHING, READY, RESET
        }
        private void HienthiKetqua_Final(Result  type, int alarmcode, string _barcode)
        {
            switch(type)
            {
                case Result.PASS:
                    lbketqua.Content = "PASS";
                    Matching._DATAMatchingReport.MC_RESULT = "PASS";
                    lbketqua.Background = Brushes.Green;
                    lbketqua.Foreground = Brushes.White;
                    Matching.Matching_Report(txtmodel.Text);
                    break;
                case Result.FAIL:
                    lbketqua.Content = "FAIL";
                    Matching._DATAMatchingReport.MC_RESULT = "FAIL";
                    lbketqua.Background = Brushes.Red;
                    lbketqua.Foreground = Brushes.YellowGreen;
                    switch (alarmcode)
                    {
                        case 0:

                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                    }
                    MClog.Save_log("Matching FAIL: " + _barcode);
                    Matching.Matching_Report(txtmodel.Text);
                    break;
                case Result.MATCHING:
                    lbketqua.Content = "MATCHING...";
                    lbketqua.Background = Brushes.Yellow;
                    lbketqua.Foreground = Brushes.Black;
                    break;
                case Result.READY:
                    STEP = 1;
                    XoaData();
                    lbketqua.Content = "SẮN SÀNG";
                    lbketqua.Background = Brushes.Gray;
                    lbketqua.Foreground = Brushes.Black;
                    Matching.Matching_Reset();
                    break;
                case Result.RESET:
                    STEP = 1;
                    XoaData();
                    lbketqua.Content = "RESET";
                    lbketqua.Background = Brushes.Gray;
                    lbketqua.Foreground = Brushes.Black;
                    MClog.Save_log("Matching RESET");
                    Matching.Matching_Reset();
                    break;
            }            
        }
        private void XoaData()
        {
            txtCPCode.Text = "";
            txtCellID1.Text = "";
            txtCellID2.Text = "";
            txtCellID3.Text = "";
            txtCellID4.Text = "";
            txtCellID5.Text = "";
            txtCellID6.Text = "";
            txtCellID7.Text = "";
            txtCellID8.Text = "";
            txtCellID9.Text = "";
            txtCellID10.Text = "";
            txtCellID11.Text = "";
            txtCellID12.Text = "";
            txtCellID13.Text = "";
            txtCellID14.Text = "";

            txtCPCode.Background = Brushes.White;
            txtCellID1.Background = Brushes.White;
            txtCellID2.Background = Brushes.White;
            txtCellID3.Background = Brushes.White;
            txtCellID4.Background = Brushes.White;
            txtCellID5.Background = Brushes.White;
            txtCellID6.Background = Brushes.White;
            txtCellID7.Background = Brushes.White;
            txtCellID8.Background = Brushes.White;
            txtCellID9.Background = Brushes.White;
            txtCellID10.Background = Brushes.White;
            txtCellID11.Background = Brushes.White;
            txtCellID12.Background = Brushes.White;
            txtCellID13.Background = Brushes.White;
            txtCellID14.Background = Brushes.White;
        }
        private void btnclear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Matching.Matching_Reset();
                HienthiKetqua_Final(Result.RESET, 0, "");
                MClog.Save_log("btnclear_MouseDown: Clear data");
            }

        }
    }
}
