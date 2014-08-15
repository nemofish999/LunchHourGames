using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames
{
    // This class is responsible for all the sounds (music and sound fx) in the game.  It controls the global volume settings
    // and can handle the various timings that are needed for the effect.
    public class SoundSystem
    {
        //private Song introSong;
        //private Song combatSong;

        public SoundSystem(LunchHourGames lhg)
        {
        }

        /*
        private void LoadSounds()
        {
            //introSong = Content.Load<Song>("startmusic");
            introSong = Content.Load<Song>("Audio/LHGTrack1");
            combatSong = Content.Load<Song>("Audio/LHGTrack2");
        }

        public void turnMusicOff()
        {
            MediaPlayer.Stop();
        }

        public void turnMusicOn()
        {
            MediaPlayer.Play(this.introSong);
            MediaPlayer.IsRepeating = true;
        }
        */

    }
}
