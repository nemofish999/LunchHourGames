using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using LunchHourGames.Combat;
using LunchHourGames.Players;

namespace LunchHourGames
{
    public class GameFactory
    {
        private LunchHourGames lhg;
        XmlDocument gameDocument = new XmlDocument();

        private CombatFactory combatFactory;

        public GameFactory(LunchHourGames lhg)
        {
            this.lhg = lhg;
            this.gameDocument = new XmlDocument();
            string xmlContent = "Content/Xml/game.xml";
            
            gameDocument.Load(xmlContent);

            //combatFactory = new CombatFactory(lhg);
        }

        public CombatSystem loadCombat(string levelReferenceName, string combatReferenceName)
        {
            return null;
            //return combatFactory.loadCombat(gameDocument, levelReferenceName, combatReferenceName);
        }
    }
}
