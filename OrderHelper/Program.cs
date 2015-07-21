using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections;

namespace OrderHelper
{
    public static class Definition
    {
        public static readonly string DCP_PRODUCT_FILE = "ProductsList.dcp.json";
        public static readonly string AMR_PRODUCT_FILE = "ProductsList.amr.json";

        public static readonly string CUSTOMER_NGAOPHAYAO_FILE = "NgaoPhayao.customer.json";
        public static readonly string CUSTOMER_SOBPRABTHOEN_FILE = "SobprabThoen.customer.json";
        public static readonly string CUSTOMER_JAEHOM_FILE = "Jaehom.customer.json";
        public static readonly string CUSTOMER_WANGHNUA_FILE = "Wanghnua.customer.json";
        public static readonly string CUSTOMER_LOCAL_FILE = "Local.customer.json";
        public static readonly string CUSTOMER_BANFON_FILE = "BanFon.customer.json";
    }

    public static class Space
    {
        public enum SessionCommand { None, NewSession, EditSession };
        public enum RouteType { None, NgaoPhayao, SobprabThoen, Jaehom, Wanghnua, Local, BanFon };
        public enum OperativeType { Undefined ,Male, Female };
        public enum InfoType { DCP, AMR };
    
        public static string[] GetRouteOption() 
        {
            return new string[] { "งาว-พะเยาว์", "สบปราบ-เถิน", "แจ้ห่ม", "วังเหนือ", "ในเมือง", "บ้านฟ่อน" };
        }

        public static string[] GetRecentSession()
        {
            string[] files = System.IO.Directory.GetFiles(".", "*.session");
            
            if (files.Length == 0)
                return new string[]{};
            
            // Decode file structure
            return new string[] { };
        }

        public static List<CustomerInfo> GetCustomersByRoute(Space.RouteType route)
        {
            string fileName = "";
            switch (route)
            {
                case RouteType.NgaoPhayao:
                    fileName = "NgaoPhayao.customer.json";
                    break;
                case RouteType.Wanghnua:
                    fileName = "WangHnua.customer.json";
                    break;
                case RouteType.SobprabThoen:
                    fileName = "SobprabThoen.customer.json";
                    break;
                case RouteType.Jaehom:
                    fileName = "Jaehom.customer.json";
                    break;
                case RouteType.Local:
                    fileName = "Local.customer.json";
                    break;
                case RouteType.BanFon:
                    fileName = "BanFon.customer.json";
                    break;
            }

            StreamReader reader = new StreamReader(fileName);
            string jsonData = reader.ReadToEnd();
            reader.Close();
            
            // Extract data
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            dynamic data = jss.Deserialize<dynamic>(jsonData);
            
            // Create a list for return
            List<CustomerInfo> customersList = new List<CustomerInfo>();

            foreach (Dictionary<string, object> item in data)
            {
                // 0 is name
                // 1 is phone number
                // 2 is type
                // 3 is order
                string[] tmps = new string[4];
                int i = 0;
                foreach (string val in item.Values)
                {
                    tmps[i++] = val;
                }
                customersList.Add(new CustomerInfo(tmps[0], tmps[1], tmps[2], int.Parse(tmps[3])));
            }

            return customersList;
        }

        public static List<Product> GetProductList()
        {
            StreamReader reader = new StreamReader("ProductsList.dcp.json");
            string jsonData = reader.ReadToEnd();
            reader.Close();

            // Extract data
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            dynamic data = jss.Deserialize<dynamic>(jsonData);

            // Create a list for return
            List<Product> productsList = new List<Product>();

            foreach (Dictionary<string, object> item in data)
            {
                // 0 is product name
                // 1 is price
                // 2 is unit
                // 3 is location
                string[] tmps = new string[5];
                int i = 0;
                foreach (string val in item.Values)
                    tmps[i++] = val;

                Space.OperativeType operativeType = OperativeType.Female;
                if (tmps[4] == "M")
                    operativeType = OperativeType.Male;
                else if (tmps[4] == "F")
                    operativeType = OperativeType.Female;

                productsList.Add(new Product(tmps[0], double.Parse(tmps[1]), tmps[2], double.Parse(tmps[3]), operativeType));
            }

            return productsList;
        }

        public static List<Product> GetProductListAmr()
        {
            StreamReader reader = new StreamReader("ProductsList.amr.json");
            string jsonData = reader.ReadToEnd();
            reader.Close();

            // Extract data
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            dynamic data = jss.Deserialize<dynamic>(jsonData);

            // Create a list for return
            List<Product> productsList = new List<Product>();

            foreach (Dictionary<string, object> item in data)
            {
                // 0 is product name
                // 1 is price
                // 2 is unit
                // 3 is location
                string[] tmps = new string[5];
                int i = 0;
                foreach (string val in item.Values)
                    tmps[i++] = val;

                Space.OperativeType operativeType = OperativeType.Female;
                if (tmps[4] == "M")
                    operativeType = OperativeType.Male;
                else if (tmps[4] == "F")
                    operativeType = OperativeType.Female;

                productsList.Add(new Product(tmps[0], double.Parse(tmps[1]), tmps[2], double.Parse(tmps[3]), operativeType));
            }

            return productsList;
        }

        public static Boolean IsNumeric (System.Object Expression)
        {
            if(Expression == null || Expression is DateTime)
                return false;

            if(Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;
   
            try 
            {
                if(Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                    return true;
            }
            catch {} // just dismiss errors but return false
            return false;
        }

        public static List<string> GetUnitIndex()
        {
            List<string> unitIndex = new List<string>();
            unitIndex.Add("ถัง");
            unitIndex.Add("ปีบ");
            unitIndex.Add("กระสอบ");
            unitIndex.Add("ลูก");
            unitIndex.Add("กล่อง");
            unitIndex.Add("ลัง");
            unitIndex.Add("ถุง");
            unitIndex.Add("ห่อ");
            unitIndex.Add("กิโล");
            unitIndex.Add("ซอง");
            unitIndex.Add("ชุด");

            return unitIndex;
        }

        public static List<double> GetLocationIndex()
        {
            List<double> locIndex = new List<double>();
            locIndex.Add(1);
            locIndex.Add(2);
            locIndex.Add(3);
            locIndex.Add(3.1);
            locIndex.Add(4);

            return locIndex;
        }

        public static int UnitPivot
        {
            get { return 7; }
        }

        public static double[] GetStoredLocation()
        {
            return new double[] { 1, 2, 3, 3.1 };
        }

        public static string[] GetOperativeType()
        {
            return new string[] { "ชาย", "หญิง" };
        }

        public static string[] GetStoredLocationType()
        {
            return new string[] { "ร้าน ห้องซ้าย", "ร้าน ห้องขวา" , "ทางเข้า ข้างอาม่า", "ห้องเย็น" };
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new MenuForm());

            MenuForm menuForm = new MenuForm();

            if (menuForm.ShowDialog() == DialogResult.Cancel)
                return;

            if (menuForm.SessionCommand == Space.SessionCommand.None)
                return;
            else if (menuForm.SessionCommand == Space.SessionCommand.NewSession)
            {
                // Create a new session
                OrderMakerForm orderMakerForm = new OrderMakerForm(menuForm.RouteData, Space.SessionCommand.NewSession, "");
                menuForm.Dispose();
                orderMakerForm.ShowDialog();

            }
            else if (menuForm.SessionCommand == Space.SessionCommand.EditSession)
            {
                // Resume session
                OrderMakerForm orderMakerForm = new OrderMakerForm(menuForm.RouteData, Space.SessionCommand.EditSession, menuForm.RestoreSessionFile);
                menuForm.Dispose();
                orderMakerForm.ShowDialog();
            }

            Environment.Exit(0);
        }
    }
}
