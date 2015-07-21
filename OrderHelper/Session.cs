using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OrderHelper
{
    [Serializable]
    public class Session
    {
        private readonly string sessionDate;
        private Space.RouteType route;
        private List<CustomerOrder> customerOrder;
        private List<CustomerOrder> _customerOrder;

        public Session(string sessionDate, Space.RouteType route)
        {
            this.sessionDate = sessionDate;
            this.route = route;
            customerOrder = new List<CustomerOrder>();
            _customerOrder = null;
        }

        public void SaveCustomerOrder(CustomerInfo custInfo, List<OrderedItem> orderedList, List<OrderedItem> orderedListAmr, bool isNotOrder)
        {
            CustomerOrder res = null;
            if (customerOrder.Count > 0)
                res = customerOrder.Where(e => e.CustomerName == custInfo.CustomerName).SingleOrDefault();

            // Check weather customer order of certain client already exist

            if (res != null)
            {
                // Customer have existing record
                int idx = customerOrder.IndexOf(res);
                customerOrder[idx].SetOrderedList(orderedList, orderedListAmr, isNotOrder);
            }
            else
            {
                customerOrder.Add(new CustomerOrder(custInfo, orderedList, orderedListAmr, isNotOrder));
            }

        }

        public bool GetCustomerSkipNotification(CustomerInfo custInfo)
        {
            if (customerOrder.Count == 0)
                return false;

            var res = customerOrder.Where(e => e.CustomerName == custInfo.CustomerName).SingleOrDefault();

            if (res == null)
                return false;

            return res.IsSkip;
        }

        public List<OrderedItem> GetCustomerOrder(CustomerInfo custInfo)
        {
            if (customerOrder.Count == 0)
                return null;

            var res = customerOrder.Where(e => e.CustomerName == custInfo.CustomerName).SingleOrDefault();

            if (res == null)
                return null;

            return res.GetOrderedList();
        }

        public List<OrderedItem> GetCustomerOrderAmr(CustomerInfo custInfo)
        {
            if (customerOrder.Count == 0)
                return null;

            var res = customerOrder.Where(e => e.CustomerName == custInfo.CustomerName).SingleOrDefault();

            if (res == null)
                return null;

            return res.GetOrderedListAmr();
        }

        public int GetNumberOfCustomerWhoHaveOrdered()
        {
            int count = 0;
            foreach(CustomerOrder custOrder in customerOrder)
                if(custOrder.HasOrdered)
                    count++;
            
            return count;
        }

        public void GetItemListRound(out ItemsPool allDcpItems, out ItemsPool allAmrItems)
        {
            if (_customerOrder == null)
            {
                GetItemListAll(out allDcpItems, out allAmrItems);
                _customerOrder = (List<CustomerOrder>)DeepClone(customerOrder);
                return;
            }

            ItemsPool itemPool = new ItemsPool();
            ItemsPool itemPoolAmr = new ItemsPool();

            for (int i = 0; i < customerOrder.Count; i++)
            {
                var res = _customerOrder.Where(e => e.CustomerName == customerOrder[i].CustomerName).SingleOrDefault();
                
                if (customerOrder[i].IsSkip)
                    continue;

                ////// DCP
                List<OrderedItem> items = customerOrder[i].GetOrderedList();
                if (res == null)
                {
                    // Customer make an order
                    foreach (OrderedItem item in items)
                        itemPool.AddNewItem(item);
                }
                else
                {
                    // Check for addtional item
                    foreach (OrderedItem item in items)
                        if(!res.HasOrderedItem(item.Name, item.Note, item.Unit, item.Amount))
                            itemPool.AddNewItem(item);
                }


                ////// AMR
                items = customerOrder[i].GetOrderedListAmr();
                if (res == null)
                {
                    // Customer make an order
                    foreach (OrderedItem item in items)
                        itemPoolAmr.AddNewItem(item);
                }
                else
                {
                    // Check for addtional item
                    foreach (OrderedItem item in items)
                        if (!res.HasOrderedItemAmr(item.Name, item.Note, item.Unit, item.Amount))
                            itemPoolAmr.AddNewItem(item);
                }
            }

            ///// STAMP FOR CUSTOMER ORDER
            _customerOrder = (List<CustomerOrder>)DeepClone(customerOrder);

            allDcpItems = itemPool;
            allAmrItems = itemPoolAmr;
        }

        public void ClearCustomerCache()
        {
            _customerOrder = null;
        }

        public void GetItemListAll(out ItemsPool allDcpItems, out ItemsPool allAmrItems)
        {
            ItemsPool itemPool = new ItemsPool();
            ItemsPool itemPoolAmr = new ItemsPool();
            foreach (CustomerOrder cust in customerOrder)
            {
                if (cust.IsSkip)
                    continue;

                List<OrderedItem> items = cust.GetOrderedList();
                foreach (OrderedItem item in items)
                    itemPool.AddNewItem(item);

                items = cust.GetOrderedListAmr();
                foreach (OrderedItem item in items)
                    itemPoolAmr.AddNewItem(item);
            }

            allDcpItems = itemPool;
            allAmrItems = itemPoolAmr;
        }

        public bool CustomerHasSkip(string customerName)
        {
            var res = customerOrder.Where(e => e.CustomerName == customerName).SingleOrDefault();

            if (res == null)
                return false;
            
            if (res.IsSkip)
                return true;

            return false;
        }

        public bool CustomerHasOrdered(string customerName)
        {
            var res = customerOrder.Where(e => e.CustomerName == customerName).SingleOrDefault();

            if (res == null)
                return false;

            if (res.HasOrdered)
                return true;

            return false;
        }
        
        public List<CustomerInfo> GetCustomersList()
        {
            List<CustomerInfo> custList = new List<CustomerInfo>();

            foreach (CustomerOrder cust in customerOrder)
            {
                if (cust.IsSkip)
                    continue;

                if (cust.HasOrdered)
                    custList.Add(cust.GetCustomerInfo());
            }

            return custList.OrderBy(e => e.Order).ToList<CustomerInfo>();
        }

        private object DeepClone(object obj)
        {
            object objResult = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            return objResult;
        }

        public List<string> GetEveryOrderedItemName()
        {
            List<string> uniqueNameList = new List<string>();
            
            //// DCP
            foreach (CustomerOrder cust in customerOrder)
            {
                if (cust.IsSkip)
                    continue;

                List<OrderedItem> tmp = cust.GetOrderedList();
                foreach (OrderedItem item in tmp)
                {
                    string codeName = item.Name;
                    if (item.Note.Length > 0)
                        codeName += string.Format(" ({0})", item.Note);
                    codeName += string.Format(" : {0}", item.Unit);

                    if (!uniqueNameList.Contains(codeName))
                        uniqueNameList.Add(codeName);
                }
            }

            //// AMR
            foreach (CustomerOrder cust in customerOrder)
            {
                if (cust.IsSkip)
                    continue;

                List<OrderedItem> tmp = cust.GetOrderedListAmr();
                foreach (OrderedItem item in tmp)
                {
                    string codeName = item.Name;
                    if (item.Note.Length > 0)
                        codeName += string.Format(" ({0})", item.Note);
                    codeName += string.Format(" : {0}", item.Unit);

                    if (!uniqueNameList.Contains(codeName))
                        uniqueNameList.Add(codeName);
                }
            }

            return uniqueNameList;
        }

        public Dictionary<string, double> GetSpecificOrderedItem(string input)
        {
            Dictionary<string, double> orderedPair = new Dictionary<string, double>();

            string[] tmp = input.Split(new char[] { ':' });

            string unit = tmp[1].Trim();
            string name = tmp[0].Trim();
            string note = "";

            if (tmp[0].Contains("("))
            {
                // Contain note
                string[] tmp2 = tmp[0].Split(new char[] { '(', ')'});
                name = tmp2[0].Trim();  // name
                note = tmp2[1].Trim();  // note
            }

            //// DCP
            foreach (CustomerOrder cust in customerOrder)
            {
                if (cust.IsSkip)
                    continue;

                List<OrderedItem> tmp3 = cust.GetOrderedList();
                foreach (OrderedItem item in tmp3)
                    if (item.Name == name && item.Note == note && item.Unit == unit)
                        orderedPair.Add(cust.CustomerName, item.Amount);
            }

            //// AMR
            foreach (CustomerOrder cust in customerOrder)
            {
                if (cust.IsSkip)
                    continue;

                List<OrderedItem> tmp3 = cust.GetOrderedListAmr();
                foreach (OrderedItem item in tmp3)
                    if (item.Name == name && item.Note == note && item.Unit == unit)
                        orderedPair.Add(cust.CustomerName, item.Amount);
            }

            return orderedPair;
        }

        public void UpdateDeliveryOrder(List<CustomerInfo> newCustomerInfo)
        {
            for (int i = 0; i < customerOrder.Count; i++)
            {
                CustomerInfo tmp = newCustomerInfo.Where(e => e.CustomerName == customerOrder[i].CustomerName).Single();

                if (tmp.Order == customerOrder[i].CustomerDeliveryOrder)
                    continue;

                customerOrder[i].CustomerDeliveryOrder = tmp.Order;
            }
        }
    }
}
