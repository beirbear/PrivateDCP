using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderHelper
{
    [Serializable]
    public class OrderedItem
    {
        private string itemName;
        private string itemUnit;
        private double amount;
        private double price;
        private double multiplier;
        private string itemNote;

        public OrderedItem(string itemName, string itemUnit, string itemNote, double amount, double multiplier, double price)
        {
            this.itemName = itemName;
            this.itemUnit = itemUnit;
            this.itemNote = itemNote;
            this.amount = amount;
            this.price = price;
            this.multiplier = multiplier;
        }

        public string Name
        {
            get { return itemName;  }
        }

        public string Unit
        {
            get { return itemUnit;  }
        }

        public string Note
        {
            get { return itemNote;  }
        }

        public double Amount
        {
            get { return amount; }
        }

        public double Price
        {
            get { return price; }
        }

        public double Multiplier
        {
            get { return multiplier; }
            set { this.multiplier = value;  }
        }
    }
}
