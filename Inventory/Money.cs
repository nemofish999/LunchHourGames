using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.Inventory
{
    public class Money : InventoryItem
    {
        public Money(LunchHourGames lhg, String referenceName, String displayName)
            : base(lhg, InventoryType.Money, referenceName, displayName)
        {
        }
    }
}
