using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.Screen
{
    public interface GamePlayEvent
    {
        void GoalReached(GameScreen gameScreen);

    }
}
