using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderHelper
{
    [Serializable]
    public class Product
    {
        private readonly string productName;
        private readonly double price;
        private readonly string unit;
        private double storedLocation;
        private Space.OperativeType operative;
        private string note;

        public Product(string name, double price, string unit, double storedLoc, Space.OperativeType operative)
        {
            this.productName = name.Trim();
            this.price = price;
            this.unit = unit.Trim();
            this.storedLocation = storedLoc;
            this.operative = operative;
        }

        public Product(string name, double price, string unit, double storedLoc, string note)
        {
            this.productName = name.Trim();
            this.price = price;
            this.unit = unit.Trim();
            this.storedLocation = storedLoc;
            this.note = note.Trim();
        }

        public Product(string name, double price, string unit, double storedLoc)
        {
            this.productName = name.Trim();
            this.price = price;
            this.unit = unit.Trim();
            this.storedLocation = storedLoc;
        }

        public string Name
        {
            get { return productName; }
        }

        public double Price
        {
            get { return price; }
        }

        public string Note
        {
            get { return note; }
        }

        public string Unit
        {
            get { return unit;  }
        }

        public double Location
        {
            get { return storedLocation;  }
        }

        public void SetLocation(double loc)
        {
            storedLocation = loc;
        }

        public Space.OperativeType Operative
        {
            get { return operative;  }
            set { operative = value; }
        }

        public string OperativeString
        {
            get {
                if (operative == Space.OperativeType.Male)
                    return "M";
                else if (operative == Space.OperativeType.Female)
                    return "F";
                else
                    throw new Exception("Shouln't reach this part og code");
            }
        }
    }
}
