using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace OrderHelper
{
    public partial class MenuForm : Form
    {
        private Space.SessionCommand sessionCommand = Space.SessionCommand.NewSession;
        private Space.RouteType newRouteData = Space.RouteType.None;
        private bool isFirstOption;
        private Dictionary<string, string> historyFile;
        private string selectedFile;

        public MenuForm()
        {
            InitializeComponent();
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            isFirstOption = true;
            updateToggle();
        }

        public Space.SessionCommand SessionCommand
        {
            get { return sessionCommand; }
        }

        public Space.RouteType RouteData
        {
            get { return newRouteData; }
        }

        private void updateToggle()
        {
            if (isFirstOption)
            {
                cbOption.Items.Clear();
                cbOption.Items.AddRange(Space.GetRouteOption());
                cbOption.Text = "กรุณาเลือกสาย";
                cbOption.Enabled = true;
            }
            else
            {
                cbOption.Items.Clear();
                cbOption.Items.AddRange(Space.GetRecentSession());
                cbOption.Text = "กรุณาเลือกรอบ";
                cbOption.Enabled = true;
                cbOption.Items.Clear();
                ViewRecentHistory();

                HistoryLimit(7);
                foreach(var name in historyFile)
                    cbOption.Items.Add(name.Key);
            }
        }

        private void btnNewSession_Click(object sender, EventArgs e)
        {
            sessionCommand = Space.SessionCommand.NewSession;
            isFirstOption = true;
            updateToggle();
        }

        private void btnEditSession_Click(object sender, EventArgs e)
        {
            sessionCommand = Space.SessionCommand.EditSession;
            isFirstOption = false;
            updateToggle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbOption.SelectedIndex == -1)
            {
                MessageBox.Show("กรุณาเลือกตัวเลือก ด้วยค่ะ");
                return;
            }

            if (isFirstOption)
            {
                switch (cbOption.SelectedIndex + 1)
                {
                    case (int)Space.RouteType.NgaoPhayao:
                        newRouteData = Space.RouteType.NgaoPhayao;
                        break;
                    case (int)Space.RouteType.SobprabThoen:
                        newRouteData = Space.RouteType.SobprabThoen;
                        break;
                    case (int)Space.RouteType.Jaehom:
                        newRouteData = Space.RouteType.Jaehom;
                        break;
                    case (int)Space.RouteType.Wanghnua:
                        newRouteData = Space.RouteType.Wanghnua;
                        break;
                    case (int)Space.RouteType.Local:
                        newRouteData = Space.RouteType.Local;
                        break;
                    case (int)Space.RouteType.BanFon:
                        newRouteData = Space.RouteType.BanFon;
                        break;
                    default:
                        newRouteData = Space.RouteType.None;
                        break;
                }

                if (isSessionAlreadyExist(newRouteData))
                {
                    MessageBox.Show("รอบบิลดังกล่าวได้ถูกเปิดไว้แล้ว\r\nกรุณากลับไปแก้ไขข้อมูลที่รอบบิลเก่า", "ไม่สามารถสร้างรอบบิลใหม่ได้");
                    return;
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                selectedFile = historyFile[cbOption.SelectedItem.ToString()];
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }

            this.Close();
        }

        private void HistoryLimit(double dayLimit)
        {
            DateTime limit = DateTime.Now.AddDays(dayLimit * -1);

            for (int i = 0; i < historyFile.Count; )
            {
                string[] tmpDate = historyFile.ElementAt(i).Value.Split(new char[] { '.', '_'});
                int[] date = new int[3];
                date[0] = int.Parse(tmpDate[2]);
                date[1] = int.Parse(tmpDate[3]);
                date[2] = int.Parse(tmpDate[4]);

                DateTime createdDate = new DateTime(date[2] - 543, date[1], date[0]);
                // DateTime createdDate = File.GetCreationTime(historyFile.ElementAt(i).Value);
                
                int compDate = DateTime.Compare(limit, createdDate);
                if (compDate > 0)
                {
                    historyFile.Remove(historyFile.ElementAt(i).Key);
                    continue;
                }
                i++;
            }
        }

        private void ViewRecentHistory()
        {
            historyFile = new Dictionary<string, string>();
            string[] fileNames = Directory.GetFiles(".");
            var res = fileNames.Where(e => e.EndsWith(".data"));
            foreach (string name in res)
            {
                string[] tmp1 = name.Remove(0,2).Split('.');

                string dispName = string.Format("{0} ({1})", RouteTranslate(tmp1[0]), tmp1[1].Replace("_", "/"));
                historyFile.Add(dispName,name);
            }
        }

        public string RestoreSessionFile
        {
            get { return selectedFile;  }
        }

        private bool isSessionAlreadyExist(Space.RouteType route)
        {
            string nDate = string.Format("{0}_{1}_{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year + 543);

            string serialName = string.Format("{0}.{1}.data", route.ToString(), nDate);

            if (File.Exists(serialName))
                return true;

            return false;
        }

        private string RouteTranslate(string input)
        {
            switch (input)
            {
                case "NgaoPhayao": return "งาว-พะเยาว์";
                case "SobprabThoen": return "สบปราบ-เถิน";
                case "Jaehom": return "แจ้ห่ม";
                case "Wanghnua": return "วังเหนือ";
                case "Local": return "ในเมือง";
                case "BanFon": return "บ้านฟ่อน";
                default: return "????";
            }
        }
    }
}
