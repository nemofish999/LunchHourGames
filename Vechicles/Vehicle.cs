using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.Inventory;

namespace LunchHourGames.GamePlay
{
    public class Vehicle : GameEntity
    {
        public enum Type
        {
            Van,
            Car
        }

        private Type type;
        private List<InventoryItem> inventory = new List<InventoryItem>();

        public Vehicle(LunchHourGames lhg, String referenceName, String displayName)
            :base(lhg, EntityType.Vehicle, referenceName, displayName)
        {
        }
    }
}
