using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Controls
{
    public class Tooltip : Microsoft.Xna.Framework.DrawableGameComponent
    {
       private LunchHourGames lhg;       

       public Tooltip(LunchHourGames lhg, string text)
           : base(lhg)
       {
       }

       public Tooltip(LunchHourGames lhg, string text, Texture2D background)
            :base(lhg)
        {
            this.lhg = lhg;
        }

    }
}
