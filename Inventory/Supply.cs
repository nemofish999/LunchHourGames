using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.Inventory
{
    public class Supply : InventoryItem
    {
        public Supply(LunchHourGames lhg, String referenceName, String displayName)
            : base(lhg, InventoryType.Supply, referenceName, displayName)
        {
        }
    }
}
