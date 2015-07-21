using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderHelper
{
    [Serializable]
    class SerialObject
    {
        private readonly Space.RouteType route;
        private readonly List<CustomerInfo> customersList;
        private readonly List<Product> productsList;
        private readonly List<Product> productsListAmr;
        private readonly Session session;
        private readonly int defaultTab;
        private readonly string date;

        public SerialObject(Space.RouteType route, 
            List<CustomerInfo> customersList,
            List<Product> productsList,
            List<Product> productsListAmr,
            Session session,
            int defaultTab,
            string date)
        {
            this.route = route;
            this.customersList = customersList;
            this.productsList = productsList;
            this.productsListAmr = productsListAmr;
            this.session = session;
            this.defaultTab = defaultTab;
            this.date = date;
        }

        public Space.RouteType Route
        {
            get { return route; }
        }

        public List<CustomerInfo> CustomerList
        {
            get { return customersList;  }
        }

        public List<Product> ProductList
        {
            get { return productsList; }
        }

        public List<Product> ProductListAmr
        {
            get { return productsListAmr; }
        }

        public Session Session
        {
            get { return session;  }
        }

        public int DefaultTab
        {
            get { return defaultTab; }
        }

        public string Date
        {
            get { return date; }
        }
    }
}
