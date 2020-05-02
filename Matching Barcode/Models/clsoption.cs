using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Matching_Barcode.Models
{
    class clsoption
    {
        public void Updateqty(ref Label OK, ref Label NG,ref Label NGRATE,bool result)
        {

            try
            {
                    switch (result)
                    {
                        case true:
                            OK.Content = (int.Parse(OK.Content.ToString()) + 1).ToString();
                            break;
                        case false:
                            NG.Content = (int.Parse(NG.Content.ToString()) + 1).ToString();
                            break;
                    }
                    NGRATE.Content = Math.Round(double.Parse(NG.Content.ToString()) * 100 / double.Parse(OK.Content.ToString()), 2).ToString() + "%";
                    saveqty(OK, NG, NGRATE);
               
            }
            catch (Exception)
            {
            }
        }
        private void saveqty(Label lbok, Label lbNG, Label NGrate )
        {
            FileStream FS = new FileStream(Environment.CurrentDirectory  + @"\Data\Config\Count.ini", FileMode.Create);
            StreamWriter SW = new StreamWriter(FS);

            SW.WriteLine("<Quantity>");
            SW.WriteLine("FCT_OK=" + lbok.Content);
            SW.WriteLine("FCT_NG=" + lbNG.Content);
            SW.WriteLine("NGRate=" + NGrate.Content);

            SW.Close();
            FS.Close();
        }
        public void Loadqty(Label lbok, Label lbNG, Label NGrate)
        {
            string[] data = null;
            string str;
            try
            {
                FileStream FS = new FileStream(Environment.CurrentDirectory + @"\Data\Config\Count.ini", FileMode.Open);
                StreamReader SR = new StreamReader(FS);
                while (SR.EndOfStream == false)
                {
                    str = SR.ReadLine();
                    data = str.Split('=');

                    switch (data[0])
                    {
                        case "FCT_OK":
                            lbok.Content = data[1];
                            break;
                        case "FCT_NG":
                            lbNG.Content = data[1];
                            break;
                        case "NGRate":
                            NGrate.Content = data[1];
                            break;
                    }
                }
                SR.Close();
                FS.Close();
            }
            catch (Exception e)
            {
                lbok.Content = "0";
                lbNG.Content = "0";
                NGrate.Content = "0";
            }
        }
    }
}
