using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matching_Barcode.Models
{
    class clsMatchingCPBMS3BC
    {
        LogfileCreator.clsMachineLog MCLog = new LogfileCreator.clsMachineLog();
        LogfileCreator.clsMatchingLog MatchingLog = new LogfileCreator.clsMatchingLog();
        OptionDefine.Compare_Pattern PatternCheck = new OptionDefine.Compare_Pattern();
        OptionDefine.clsCheckTrungInformation CheckTrungCode = new OptionDefine.clsCheckTrungInformation();
        string _tilte = "";
        public struct CPMatching
        {
            public string MC_TIME;
            public string MODEL;
            public string BMS_PATTERN;
            public string CP_PATTERN;
            public string HP_PATTERN;
            public string MC_RESULT;
            public string USER;
            public string ID;
        }
        public clsMatchingCPBMS3BC()
        {
            CreatTitle();
        }
        private void CreatTitle()
        {
            _tilte = "";
            _tilte += "MC_TIME" + "\t";
            _tilte += "MODEL" + "\t";
            _tilte += "BMS_PATTERN" + "\t";
            _tilte += "CP_PATTERN" + "\t";
            _tilte += "HP_PATTERN" + "\t";
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
        public bool Check_Pattern(string _barcode, string _Pattern)
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
                _DATAMatchingReport.BMS_PATTERN = "";
                _DATAMatchingReport.CP_PATTERN = "";
                _DATAMatchingReport.HP_PATTERN = "";
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
                _str += _DATAMatchingReport.MC_TIME + "\t";
                _str += _DATAMatchingReport.MODEL + "\t";
                _str += _DATAMatchingReport.BMS_PATTERN + "\t";
                _str += _DATAMatchingReport.CP_PATTERN + "\t";
                _str += _DATAMatchingReport.HP_PATTERN + "\t";
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
