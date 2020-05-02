using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matching_Barcode.Models
{
    class clsMatchingCP7S2P
    {
        LogfileCreator.clsMachineLog MCLog = new LogfileCreator.clsMachineLog();
        LogfileCreator.clsMatchingLog MatchingLog = new LogfileCreator.clsMatchingLog();
        OptionDefine.Compare_Pattern PatternCheck = new OptionDefine.Compare_Pattern();
        OptionDefine.clsCheckTrungInformation CheckTrungCode = new OptionDefine.clsCheckTrungInformation();
        string _tilte = "";
        public struct  CPMatching
        {
            public string MC_TIME;
            public string MODEL;
            public string CELL_BATCH;
            public string CELL_LOT;
            public string CP;
            public string CELL_ID1;
            public string CELL_ID2;
            public string CELL_ID3;
            public string CELL_ID4;
            public string CELL_ID5;
            public string CELL_ID6;
            public string CELL_ID7;
            public string CELL_ID8;
            public string CELL_ID9;
            public string CELL_ID10;
            public string CELL_ID11;
            public string CELL_ID12;
            public string CELL_ID13;
            public string CELL_ID14;
            public string MC_RESULT;
            public string USER;
            public string ID;
        }
        public clsMatchingCP7S2P()
        {
            CreatTitle();
        }
        private void CreatTitle()
        {
             _tilte = "";
            _tilte += "MC_TIME" + "\t";
            _tilte += "MODEL" + "\t";
            _tilte += "CELL_BATCH" + "\t";
            _tilte += "CELL_LOT" + "\t";
            _tilte += "CP" + "\t";
            _tilte += "CELL_ID1" + "\t";
            _tilte += "CELL_ID2" + "\t";
            _tilte += "CELL_ID3" + "\t";
            _tilte += "CELL_ID4" + "\t";
            _tilte += "CELL_ID5" + "\t";
            _tilte += "CELL_ID6" + "\t";
            _tilte += "CELL_ID7" + "\t";
            _tilte += "CELL_ID8" + "\t";
            _tilte += "CELL_ID9" + "\t";
            _tilte += "CELL_ID10" + "\t";
            _tilte += "CELL_ID11" + "\t";
            _tilte += "CELL_ID12" + "\t";
            _tilte += "CELL_ID13" + "\t";
            _tilte += "CELL_ID14" + "\t";
            _tilte += "MC_RESULT" + "\t";
            _tilte += "USER" + "\t";
            _tilte += "ID" + "\t";
            _tilte += "\r\n";
        }
        public bool CreatLogLocal(string _model)
        {
            try
            {
                return MatchingLog.CreatLog_Local(_model, _tilte);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public CPMatching _DATAMatchingReport = new CPMatching();        

        public bool Check_Config()
        {
            try
            {
                return true;
            }
            catch (Exception e)
            {
                MCLog.Save_log("Check_Config:" + e.ToString());
                return false;
            }
        }
        public bool Check_CP_Pattern(string _barcode, string _Pattern)
        {
            try
            {                
                return PatternCheck.ComparePatternBarcode(_barcode, _Pattern);
            }
            catch (Exception e)
            {
                MCLog.Save_log("Check_CP_Pattern:" + e.ToString());
                return false;
            }
        }
        public bool Check_Trung_Code(string _barcode, List<string> _List)
        {
            try
            {
                return CheckTrungCode.CheckDuplicateInforamation(_barcode, _List);
            }
            catch (Exception e)
            {
                MCLog.Save_log("Check_Trung_Code:" + e.ToString());
                return false;
            }
        }
        public bool Update_ListBarcodeOK(string _barcode)
        {
            try
            {
                return CheckTrungCode.SaveList(_barcode, "MATCHING_BARCODE_LIST_OK");
            }
            catch (Exception e)
            {
                MCLog.Save_log("Update_ListBarcodeOK:" + e.ToString());
                return false;
            }
        }
        public bool Load_ListBarcodeOK(ref List<string> _List)
        {
            try
            {
                return CheckTrungCode.LoadList("MATCHING_BARCODE_LIST_OK", ref _List);
            }
            catch (Exception e)
            {
                MCLog.Save_log("Load_ListBarcodeOK:" + e.ToString());
                return false;
            }
        }
        public bool Update_ListBarcodeNG(string _barcode)
        {
            try
            {
                return CheckTrungCode.SaveList(_barcode, "MATCHING_BARCODE_LIST_NG");
            }
            catch (Exception e)
            {
                MCLog.Save_log("Update_ListBarcodeNG:" + e.ToString());
                return false;
            }
        }
        public bool Load_ListBarcodeNG(ref List<string> _List)
        {
            try
            {
                return CheckTrungCode.LoadList("MATCHING_BARCODE_LIST_NG", ref _List);
            }
            catch (Exception e)
            {
                MCLog.Save_log("Load_ListBarcodeNG:" + e.ToString());
                return false;
            }
        }
        public bool Check_Cell_ID(string _barcode, string _Pattern)
        {
            try
            {
                return PatternCheck.ComparePatternBarcode(_barcode, _Pattern);
            }
            catch (Exception e)
            {
                MCLog.Save_log("Check_Cell_ID:" + e.ToString());
                return false;
            }
        }
        public bool Matching_Report(string _model)
        {
           string _data = Matching_Update() + "\r\n";
            try
            {
                MatchingLog.SaveLog_Local(_data, _model, _tilte);
                return true;
            }
            catch (Exception e)
            {
                MCLog.Save_log("Matching_Report:" + e.ToString());
                return false;
            }
        }
        public bool Matching_Reset()
        {
            try
            {
                _DATAMatchingReport.MC_TIME = "";
                _DATAMatchingReport.MODEL = "";
                _DATAMatchingReport.CELL_LOT = "";
                _DATAMatchingReport.CP = "";
                _DATAMatchingReport.CELL_ID1 = "";
                _DATAMatchingReport.CELL_ID2 = "";
                _DATAMatchingReport.CELL_ID3 = "";
                _DATAMatchingReport.CELL_ID4 = "";
                _DATAMatchingReport.CELL_ID5 = "";
                _DATAMatchingReport.CELL_ID6 = "";
                _DATAMatchingReport.CELL_ID7 = "";
                _DATAMatchingReport.CELL_ID8 = "";
                _DATAMatchingReport.CELL_ID9 = "";
                _DATAMatchingReport.CELL_ID10 = "";
                _DATAMatchingReport.CELL_ID11 = "";
                _DATAMatchingReport.CELL_ID12= "";
                _DATAMatchingReport.CELL_ID13 = "";
                _DATAMatchingReport.CELL_ID14 = "";
                _DATAMatchingReport.MC_RESULT = "";
                _DATAMatchingReport.USER = "";
                _DATAMatchingReport.ID = "";
                return true;
            }
            catch (Exception e)
            {
                MCLog.Save_log("Matching_Report:" + e.ToString());
                return false;
            }
        }
        private string Matching_Update()
        {
            string _str = "";
            try
            {
                _str += _DATAMatchingReport.MC_TIME +"\t";
                _str += _DATAMatchingReport.MODEL + "\t";
                _str += _DATAMatchingReport.CELL_BATCH + "\t";
                _str += _DATAMatchingReport.CELL_LOT + "\t";
                _str += _DATAMatchingReport.CP + "\t";
                _str += _DATAMatchingReport.CELL_ID1 + "\t";
                _str += _DATAMatchingReport.CELL_ID2 + "\t";
                _str += _DATAMatchingReport.CELL_ID3 + "\t";
                _str += _DATAMatchingReport.CELL_ID4 + "\t";
                _str += _DATAMatchingReport.CELL_ID5 + "\t";
                _str += _DATAMatchingReport.CELL_ID6 + "\t";
                _str += _DATAMatchingReport.CELL_ID7 + "\t";
                _str += _DATAMatchingReport.CELL_ID8 + "\t";
                _str += _DATAMatchingReport.CELL_ID9 + "\t";
                _str += _DATAMatchingReport.CELL_ID10 + "\t";
                _str += _DATAMatchingReport.CELL_ID11 + "\t";
                _str += _DATAMatchingReport.CELL_ID12 + "\t";
                _str += _DATAMatchingReport.CELL_ID13 + "\t";
                _str += _DATAMatchingReport.CELL_ID14 + "\t";
                _str += _DATAMatchingReport.MC_RESULT + "\t";
                _str += _DATAMatchingReport.USER + "\t";
                _str += _DATAMatchingReport.ID + "\t";
                return _str;
            }
            catch (Exception e)
            {
                MCLog.Save_log("Matching_Update:" + e.ToString());
                return "NAK";
            }
        }
    }
}
