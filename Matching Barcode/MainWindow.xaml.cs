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
using Microsoft.Win32;

namespace Matching_Barcode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClsConfig.ClsConfig config = new ClsConfig.ClsConfig();
        LogfileCreator.clsMachineLog MClog = new LogfileCreator.clsMachineLog();
        Models.clsMatchingCPBMS3BC Matching = new Models.clsMatchingCPBMS3BC();
        MediaLib.Alarm AlarmMedia = new MediaLib.Alarm();
        DatabaseConnection.BPIMES BPIMES = new DatabaseConnection.BPIMES();
        Models.clsoption option = new Models.clsoption();

        string Barcode_ID = string.Empty;
        string SYSTEM_USING = "LOCAL";
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
            CheckConfig();
            option.Loadqty(lbOK, lbNG, lbRate);
            Matching.Load_ListBarcodeOK(ref OKBarcodeList);
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
                        case "CP_PATTERN":
                            CPPattern= txtcppattern.Text = _str[1].Trim();
                            break;
                        case "HP_PATTERN":
                            HPPattern = txthpattern.Text = _str[1].Trim();
                            break;
                        case "BMS_PATTERN":
                            BMSPattern = txtbmspattern.Text = _str[1].Trim();
                            break;
                        case "USER":
                            txtusername.Text = _str[1].Trim();
                            break;
                        case "ID":
                            txtuserid.Text = _str[1].Trim();
                            break;
                        case "SYSTEM":
                            lbsystem.Content = SYSTEM_USING = _str[1].Trim();
                            if (SYSTEM_USING == "SYSTEM")
                            {
                                lbsystem.Background = Brushes.Green;
                            }
                            else
                            {
                                lbsystem.Background = Brushes.Red;
                            }
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
                if(txtmodel.Text !="" && txtcppattern.Text != "" && txthpattern.Text != "" && txtbmspattern.Text != "" && txtusername.Text != "" && txtuserid.Text != "" )
                {

                    return true;
                }
                else
                {
                    //MessageBox.Show("Thiếu thông tin config \n Chọn cài đặt và nhập thông tin", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MClog.Save_log("CheckConfig: FAIL");
                    return false;
                }    
            }
            catch (Exception e)
            {
                MClog.Save_log("CheckConfig: FAIL:" + e.ToString());
                return false;
            }
        }
        string version = "";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initial_Config();
            MClog.Save_log("Window_Loaded");
            //version = "Ver: 2020.22.04.02"; //sửa chp BMS Matching

            //version = "2020.04.24.02";//Thêm MC log config
            version = "2020.04.29.8";//Thêm rework
            lbversion.Content = version;

            BPIMES.Serveradd = "10.138.4.132";
            BPIMES.Dbsource = "BPIMES";
            BPIMES.User = "BPIAdmin";
            BPIMES.Pass = "BPI@123456";

            //BPIMES.Serveradd = "172.16.16.117";
            //BPIMES.Serveradd = "NGHIARBC";
            //BPIMES.Dbsource = "PMES";
            //BPIMES.User = "NGHIAMES";
            //BPIMES.Pass = "dinhnghia2106";


            if (BPIMES.checkconnection())
            {
                lbconnection.Background = Brushes.Green;
            }
            else
            {
                lbconnection.Background = Brushes.Red;
            }
        }

        #region Define_Maching
        private int STEP = 1;
        private string CPPattern;
        private string HPPattern;
        private string BMSPattern;
        private string MODEL;
        private List<string> CurrentBarcodeList = new List<string>();
        private List<string> OKBarcodeList = new List<string>();
        private List<string> NGBarcodeList = new List<string>();
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
            if(Matching.Check_Trung_Code(_barcode, OKBarcodeList) == false && chbrework.IsChecked  == false)
            {
                HienthiKetqua_Item(txtbarcode, false, _barcode, 7,0);
                return;
            }
            else
            {
                txtbarcode.Background = Brushes.DarkSeaGreen;

            }    
            switch(STEP)
            {
                case 0:
                   
                    break;
                case 1: //CP CP Pattern
                    Matching.Matching_Reset();
                    HienthiKetqua_Final(Result.READY, STEP, _barcode); 
                    Matching._DATAMatchingReport.MC_TIME = DateTime.Now.ToString("yyyyMMddHHmmss");
                    Matching._DATAMatchingReport.MODEL = txtmodel.Text;
                    Matching._DATAMatchingReport.USER = txtusername.Text;
                    Matching._DATAMatchingReport.ID = txtuserid.Text;
                    if (CheckConfig())
                    {
                        Matching._DATAMatchingReport.BMS_PATTERN = _barcode;
                        if (Matching.Check_Pattern(_barcode, BMSPattern))
                        {
                            if (Matching.Check_Trung_Code(_barcode, CurrentBarcodeList))
                            {
                                HienthiKetqua_Item(txtCPCode, true, _barcode, 0, STEP);
                                STEP = 2;
                                break;
                            }
                            else
                            {
                                HienthiKetqua_Item(txtCPCode, false, _barcode, 4, STEP);
                            }
                        }
                        else
                        {
                            //errorcode++;
                            HienthiKetqua_Item(txtCPCode, false, _barcode, 1, STEP);
                        }
                    }
                    else
                    {
                       // errorcode++;
                        HienthiKetqua_Final(Result.FAIL, 10, _barcode);
                    }                   
                    break;
                case 2:
                   Matching._DATAMatchingReport.CP_PATTERN = _barcode;
                    //check process
                    int errorcode = 0;
                    string result = BPIMES.GetBeforeProcessResult(_barcode, "CP_FCT", ref errorcode);
                    if (result == "PASS") //pass
                    {


                        if (Matching.Check_Pattern(_barcode, CPPattern))
                        {
                            if (Matching.Check_Trung_Code(_barcode, CurrentBarcodeList))
                            {
                                HienthiKetqua_Item(txtCellID1, true, _barcode, 0, STEP);
                                STEP = 3;
                                break;
                            }
                            else
                            {
                                HienthiKetqua_Item(txtCellID1, false, _barcode, 3, STEP);
                            }
                        }
                        else
                        {
                            //errorcode++;
                            HienthiKetqua_Item(txtCellID1, false, _barcode, 2, STEP);
                        }
                    }
                    else if(result=="NULL")//FAIL or Null
                    {
                        HienthiKetqua_Final(Result.FAIL, 58, _barcode);
                    }
                    else if (result == "FAIL")//FAIL or Null
                    {

                        HienthiKetqua_Final(Result.FAIL, 59, _barcode);
                    }
                    else //error
                    {
                        HienthiKetqua_Final(Result.FAIL, 9000, _barcode);
                    }
                    break;
                case 3:
                    Barcode_ID = Matching._DATAMatchingReport.HP_PATTERN = _barcode;
                    if (Matching.Check_Pattern(_barcode, HPPattern))
                    {
                        if (Matching.Check_Trung_Code(_barcode, CurrentBarcodeList))
                        {
                            HienthiKetqua_Item(txtCellID2, true, _barcode, 0, STEP);

                            if (UploadFCTServer(Barcode_ID, "PASS"))
                            {

                                HienthiKetqua_Final(Result.PASS, STEP, _barcode);
                                option.Updateqty(ref lbOK, ref lbNG, ref lbRate, true);
                                STEP = 1;
                            }
                            else
                            {
                                HienthiKetqua_Final(Result.FAIL, 11, _barcode);
                            }

                            chbrework.IsChecked = false;
                            break;
                        }
                        else
                        {
                            HienthiKetqua_Item(txtCellID2, false, _barcode, 3, STEP);
                        }
                    }
                    else
                    {
                        //errorcode++;
                        HienthiKetqua_Item(txtCellID2, false, _barcode, 2, STEP);
                    }
                    break;    
            }
        }
        string[,] Barcode_List = new string[4, 2];
        private void HienthiKetqua_Item(TextBox _textbox, bool Flag, string _barcode, int _alarmcode, int _ItemSeq)
        {
            _textbox.Text = _barcode;
            
            Barcode_List[_ItemSeq, 0] = _barcode;
            if (Flag)
            {
                _textbox.Background = Brushes.DarkSeaGreen;
                HienthiKetqua_Final(Result.MATCHING, 0, _barcode);

                Barcode_List[_ItemSeq, 1] = "PASS";
                CurrentBarcodeList.Add(_barcode); //PASS thì thêm vào list hiện tại để check trùng code
            }
            else
            {
                _textbox.Background = Brushes.Red;
                HienthiKetqua_Final(Result.FAIL, _alarmcode, _barcode);

                Barcode_List[_ItemSeq, 1] = "FAIL";
            }
        }
        enum Result
        {
            PASS, FAIL, MATCHING, READY, RESET
        }
        Views.Alarm Alarm;
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
                    for(int i =0; i < CurrentBarcodeList.Count; i++)
                    {
                        Matching.Update_ListBarcodeOK(CurrentBarcodeList[i]);
                        OKBarcodeList.Add(CurrentBarcodeList[i]);
                    }
                    AlarmMedia.Alarm_PASS();
                    break;
                case Result.FAIL:
                    lbketqua.Content = "FAIL";
                    Matching._DATAMatchingReport.MC_RESULT = "FAIL";
                    lbketqua.Background = Brushes.Red;
                    lbketqua.Foreground = Brushes.YellowGreen;
                    string reasonfail = string.Empty;
                    string huongdanxuly = string.Empty;
                    switch (alarmcode)
                    {
                        case 0:
                            reasonfail = "";
                            huongdanxuly = "";
                            break;
                        case 1:
                            reasonfail = "NG CP Pattern";
                            huongdanxuly = "Kiểm tra lại thông tin CP Barcode hoặc setup Pattern";
                            UploadFCTServer(Barcode_ID, "FAIL");
                            break;
                        case 2:
                            reasonfail = "NG Cell ID";
                            huongdanxuly = "Kiểm tra lại thông tin Cell ID hoặc setup Batch \nNếu dúng thì sử dụng Cell khác và tiếp tục matching";
                            UploadFCTServer(Barcode_ID, "FAIL");
                            break;
                        case 3:
                            reasonfail = "Trùng Cell ID";
                            huongdanxuly = "Sử dụng Cell khác và tiếp tục matching";
                            break;
                        case 4:
                            reasonfail = "Trùng CP Barcode";
                            huongdanxuly = "Sử dụng CP Barcode khác và tiếp tục matching";
                            break;
                        case 5:
                            reasonfail = "CP đã được matching";
                            huongdanxuly = "Sử dụng CP Barcode khác và tiếp tục matching";
                            break;
                        case 6:
                            reasonfail = "Cell ID đã được matching";
                            huongdanxuly = "Sử dụng Cell khác và tiếp tục matching";
                            break;
                        case 7:
                            reasonfail = "Trùng code " + _barcode;
                            huongdanxuly = "Sử dụng Cell/CP khác và tiếp tục matching";
                            break;
                        case 10:
                            reasonfail = "Thiếu thông tin config";
                            huongdanxuly = "Kiểm tra lại thông tin config \nVào mục setup nhập đầy đủ các thông tin";
                            break;
                        case 58:
                            reasonfail = "Barcode chưa input";
                            huongdanxuly = "Kiểm tra lại công đoạn trước";
                            break;
                        case 59:
                            reasonfail = "Barcode NG công đoạn trước";
                            huongdanxuly = "Kiểm tra lại công đoạn trước";
                            break;
                        case 9000:
                            reasonfail = "Lỗi hệ thống";
                            huongdanxuly = "Liên hệ người phụ trách chương trình và hệ thống";
                            break;
                    }
                    option.Updateqty(ref lbOK, ref lbNG, ref lbRate, false);
                    MClog.Save_log("Matching FAIL: " + _barcode + ":Reasion:" + reasonfail );
                    Matching.Matching_Report(txtmodel.Text);
                    AlarmMedia.Alarm_FAIL();
                    Alarm = new Views.Alarm(alarmcode.ToString(), reasonfail, huongdanxuly);
                    Alarm.ShowDialog();
                    Alarm.Close();
                    break;
                case Result.MATCHING:
                    lbketqua.Content = "MATCHING...";
                    lbketqua.Background = Brushes.Yellow;
                    lbketqua.Foreground = Brushes.Black;
                    AlarmMedia.Alarm_START();
                    prgr1.Value = 100 * STEP / 15;
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

        int IDKeySave = 0;
        private bool UploadFCTServer(string _barcode, string _Result)
        {
            if (SYSTEM_USING != "SYSTEM") return true;
            int err = 0;
            try
            {

                //int iii = BPIMES.GetItemID_Max("ID", "TB_FCT_Log");
                if (BPIMES.FCT_Upload_Result("VLBP", "BMS_MC", MODEL, _barcode, _Result, txtusername.Text, txtuserid.Text, ref IDKeySave))
                {
                    for (int i = 1; i < Barcode_List.GetLength(0); i++)
                    {
                        if (!BPIMES.FCT_Upload_Items(IDKeySave, _barcode, "Barcode " + i.ToString(), "Null", "Null", Barcode_List[i, 0], Barcode_List[i, 1]))
                        {
                            err++;
                        }
                    }
                }
                else
                {
                    err++;
                }
                if (err == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                MClog.Save_log("UploadFCTServer:" + e.ToString());
                MessageBox.Show(e.ToString());
                return false;
            }
        }
        private void XoaData()
        {
            CurrentBarcodeList.Clear();

            txtCPCode.Text = "";
            txtCellID1.Text = "";
            txtCellID2.Text = "";
            txtCPCode.Background = Brushes.White;
            txtCellID1.Background = Brushes.White;
            txtCellID2.Background = Brushes.White;
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

        private void btnlogfile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.CurrentDirectory + @"\Log";
                openFileDialog.ShowDialog();
            }
        }

    }
}
