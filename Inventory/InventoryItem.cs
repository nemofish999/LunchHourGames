using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.Inventory
{
    public class InventoryItem : GameEntity
    {
        public enum InventoryType
        {
            Weapon,    // Gun, Granade, Bow
            Ammo,      // Bullets, shells, arrows
            Part,      // Tire, sparkplug
            Supply,    // Food, Water, MedPack, Gas, Oil, Flashlight, Matches
            Tool,      // Hammer, Wrench
            Money      // Dollars, Gold, Diamonds
        }

        protected String name;
        protected String desc;
        protected InventoryType inventoryType;

        protected GameEntity gameEntityHoldingItem;

        public InventoryItem(LunchHourGames lhg, InventoryType inventoryType, String referenceName, String displayName)
            : base(lhg, GameEntity.EntityType.Inventory, referenceName, displayName)
        {
            this.inventoryType = inventoryType;
        }

        public InventoryType MyInventoryType
        {
            get { return this.inventoryType; }
        }

        public GameEntity GameEntityHoldingItem
        {
            get { return this.gameEntityHoldingItem; }
            set { this.gameEntityHoldingItem = value; }
        }

        public virtual InventoryItem copy()
        {
            return null;
        }
    }
}
