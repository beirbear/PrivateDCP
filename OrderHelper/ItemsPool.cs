using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderHelper
{
    public class ItemsPool
    {
        private bool isSetLocation;
        private List<ItemIdentity> identityList;

        public ItemsPool()
        {
            identityList = new List<ItemIdentity>();
        }

        public void AddNewItem(OrderedItem orderedItem)
        {
            string nameIdentity = GetNameIdentity(orderedItem);

            // Destructure ordered item
            var res = identityList.Where( e => e.Identity == nameIdentity).FirstOrDefault();

            if (res == null)
            {
                ItemIdentity tmp = new ItemIdentity(
                    new Product(
                        orderedItem.Name, 
                        orderedItem.Price, 
                        orderedItem.Unit, 
                        0,
                        orderedItem.Note), 
                    orderedItem.Amount);

                // Create a new identity
                identityList.Add(tmp);
            }
            else
            {
                // Append identity
                res.AddNewAmount(orderedItem.Amount);
            }
        }

        public string GetNameIdentity(OrderedItem item)
        {
            return item.Name + item.Note + item.Unit;
        }

        public int TotalItems
        {
            get { return identityList.Count;  }
        }

        public void UpdateStoredLocationAndOperative(ref List<Product> productList)
        {
            for (int i = 0; i < TotalItems; i++)
            {
                var res = productList.Where(e => e.Name == identityList[i].Name &&
                                        e.Unit == identityList[i].Unit).FirstOrDefault();

                if (res != null)
                {
                    identityList[i].StoredLocation = res.Location;
                    identityList[i].Operative = res.Operative;
                }
                else
                    throw new Exception("Unfound data");
            }

            isSetLocation = true;
        }

        public bool IsSetLocation
        {
            get { return isSetLocation; }
        }

        public List<ItemIdentity> GetItemsByUnitLocationAndOperative(string unit, double storedLoc, Space.OperativeType operative)
        {
            return identityList.Where(e =>      e.Unit == unit && 
                                                e.StoredLocation == storedLoc &&
                                                e.Operative == operative).OrderBy(e => e.Name).ToList<ItemIdentity>();
        }

        public List<ItemIdentity> GetItemsByUnitException(List<string> units, double storedLoc, Space.OperativeType operative)
        {
            return identityList.Where(e =>      e.StoredLocation == storedLoc &&
                                                e.Operative == operative &&
                                                !units.Contains(e.Unit)).OrderBy(e => e.Unit).ThenBy(e => e.Name).ToList<ItemIdentity>();
        }

        public List<ItemIdentity> GetItemsByUndefinedUnit(List<string> unitRef)
        {
            return (from ItemIdentity t in identityList
                    where !unitRef.Contains(t.Unit)
                    select t).OrderBy(e => e.Unit).ThenBy(e => e.Name).ToList<ItemIdentity>();
        }

        public List<ItemIdentity> GetItemsOrderByUnitAndName()
        {
            return identityList.OrderBy(e => e.Unit).ThenBy(e => e.Name).ToList<ItemIdentity>();
        }
    }
}
