using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderHelper
{
    [Serializable]
    class CustomerOrder
    {
        private CustomerInfo customer;
        private List<OrderedItem> items;
        private List<OrderedItem> itemsAmr;
        private bool isNotOrder;

        public CustomerOrder(CustomerInfo customer, List<OrderedItem> orderedItem, List<OrderedItem> orderedItemAmr, bool isNotOrder)
        {
            this.customer = customer;
            items = orderedItem;
            itemsAmr = orderedItemAmr;
            this.isNotOrder = isNotOrder;
        }

        public void SetOrderedList(List<OrderedItem> orderedItem, List<OrderedItem> orderedItemAmr, bool isNotOrder)
        {
            items = orderedItem;
            itemsAmr = orderedItemAmr;
            this.isNotOrder = isNotOrder;
        }

        public List<OrderedItem> GetOrderedList()
        {
            return items;
        }

        public List<OrderedItem> GetOrderedListAmr()
        {
            return itemsAmr;
        }

        public string CustomerName
        {
            get { return customer.CustomerName;  }
        }

        public int CustomerDeliveryOrder
        {
            get { return customer.Order; }
            set { customer.SetNewOrder(value); }
        }

        public bool IsSkip
        {
            get { return isNotOrder;  }
        }

        public bool HasOrderedItem(string productName, string productNote, string productUnit, double amount)
        {
            if (isNotOrder)
                return false;

            var res = items.Where(e => e.Name == productName &&
                                        e.Unit == productUnit &&
                                        e.Note == productNote &&
                                        e.Amount == amount).SingleOrDefault();
            if (res == null)
                return false;

            return true;
        }

        public bool HasOrderedItemAmr(string productName, string productNote, string productUnit, double amount)
        {
            if (isNotOrder)
                return false;

            var res = itemsAmr.Where(e => e.Name == productName &&
                                        e.Unit == productUnit &&
                                        e.Note == productNote &&
                                        e.Amount == amount).SingleOrDefault();
            if (res == null)
                return false;

            return true;
        }

        public bool HasOrdered
        {
            get 
            {
                if (IsSkip) return true;
                return (items.Count > 0 ? true : (itemsAmr.Count > 0 ? true : false)); 
            }
        }

        public CustomerInfo GetCustomerInfo()
        {
            return customer;
        }
    }
}
