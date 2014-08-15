using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.Inventory
{
    class Item
    {
        enum Type
        {
            Weapon,
            Tool,
            Resource,
            Obstacle,
            Money
        }

        private String name;
        private Type type;

        private bool isOnPlayer;  // TRUE if the item resides on the player (Gun, flashlight)

        //private CharacterSprite sprite;  // Sprite for this item


    }
}
