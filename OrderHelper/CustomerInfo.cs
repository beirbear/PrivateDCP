using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderHelper
{
    [Serializable]
    public class CustomerInfo
    {
        private string customerName;
        private string phoneNumber;
        private string paymentType;
        private int order;

        public CustomerInfo(string customerName, string phoneNumber, string paymentType, int order)
        {
            this.customerName = customerName;
            this.phoneNumber = phoneNumber;
            this.paymentType = paymentType;
            this.order = order;
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber;  }
        }

        public int Order
        {
            get { return order; }
        }

        public void SetNewOrder(int newOrder)
        {
            order = newOrder;
        }

        public string PaymentType
        {
            get {
                if (paymentType.ToUpper() == "CASH")
                    return "เงินสด";
                return "เก่าไปใหม่มา";
            }
        }
    }
}
