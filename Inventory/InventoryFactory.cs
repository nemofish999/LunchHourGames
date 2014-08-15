using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using LunchHourGames.Screen;
using LunchHourGames.Inventory.Weapons;

namespace LunchHourGames.Inventory
{
    public class InventoryFactory
    {
        private LunchHourGames lhg;
        private LoadingProgress loadingProgress;

        public InventoryFactory(LunchHourGames lhg, LoadingProgress loadingProgress)
        {
            this.lhg = lhg;
            this.loadingProgress = loadingProgress;
        }

        public InventorySystem createInventorySystem()
        {
            return null;
        }

        public InventoryStorage loadInventoryStorage(XmlNode inventoryNode)
        {
            InventoryStorage storage = new InventoryStorage();

            Gun pistol = new Gun(lhg, "pistol", ".22 Pistol", 1, false, 12, 1, 2);
            pistol.Initialize();
            storage.addItem(pistol);

            Bullet bullet = new Bullet(lhg, "bullet", "Bullet", pistol);
            bullet.Initialize();
            bullet.Count = 6;
            storage.addItem(bullet);

            pistol.MyAmmo = bullet;
            return storage;
        }
    }
}
