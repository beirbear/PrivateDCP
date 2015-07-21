using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderHelper
{
    public class ItemIdentity
    {
        private Product product;
        private List<double> amount;

        public ItemIdentity(Product product, double amount)
        {
            this.product = product;
            this.amount = new List<double>();
            this.amount.Add(amount);
        }

        public void AddNewAmount(double amount)
        {
            this.amount.Add(amount);
        }

        public string Identity
        {
            get { return product.Name + product.Note + product.Unit; }
        }

        public string Name
        {
            get { return product.Name;  }
        }

        public string Unit
        {
            get { return product.Unit;  }
        }

        public string Note
        {
            get { return product.Note; }
        }

        public double StoredLocation
        {
            get { return product.Location; }
            set { product.SetLocation(value); }
        }

        public Space.OperativeType Operative
        {
            get { return product.Operative; }
            set { product.Operative = value; }
        }

        public double GetTotalAmount()
        {
            double counter = 0;

            for(int i = 0; i < amount.Count; i++)
                counter += amount[i];

            return counter;
        }

        public string GetItemOrderInline()
        {
            string message = "";

            for (int i = 0; i < amount.Count; i++)
                message += "\t" + amount[i].ToString();

            return message;
        }
    }
}
