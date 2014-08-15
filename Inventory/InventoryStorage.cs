using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using LunchHourGames.Inventory.Weapons;

namespace LunchHourGames.Inventory
{
    public class InventoryStorage
    {
        private List<InventoryItem> items = new List<InventoryItem>();

        public InventoryStorage()
        {
        }

        public void addItem(InventoryItem item)
        {
            items.Add(item);
        }

        public void removeItem(InventoryItem item)
        {
            items.Remove(item);
        }

        public InventoryStorage copy()
        {
            InventoryStorage storage = new InventoryStorage();
            foreach (InventoryItem item in items)
            {
                InventoryItem copyItem = item.copy();
                storage.addItem(copyItem);
            }

            return storage;
        }

        public Weapon getWeapon(String referenceName)
        {
            Weapon weapon = null;

            foreach (InventoryItem item in items)
            {
                if (item.MyReferenceName.Equals(referenceName) && item.MyInventoryType == InventoryItem.InventoryType.Weapon)
                    return (Weapon)item;
            }

            return weapon;
        }      
    }
}
