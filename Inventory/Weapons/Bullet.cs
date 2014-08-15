using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Combat;
using LunchHourGames.Hexagonal;
using LunchHourGames.Players;
using LunchHourGames.Sprite;

namespace LunchHourGames.Inventory.Weapons
{
    public class Bullet : Ammo
    {
        public Bullet(LunchHourGames lhg, String referenceName, String displayName, Weapon weapon)
            : base(lhg, referenceName, displayName, weapon)
        {
        }

        public override InventoryItem copy()
        {
            Bullet bullet = new Bullet(lhg, referenceName, displayName, weapon);
            bullet.Initialize();
            return bullet;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.texture = Game.Content.Load<Texture2D>("Sprites/Weapons/bullet");
            this.sprite = new SimpleSprite3D(lhg, texture);
        }
    }
}
