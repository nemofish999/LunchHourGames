using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.Sprite
{
    public interface AnimationCallback
    {
        void animationBegin(GameEntity gameEntity, AnimationType type);
        void animationEnd(GameEntity gameEntity, AnimationType type);            
    }
}
