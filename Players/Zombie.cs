using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.AI;
using LunchHourGames.Combat;
using LunchHourGames.Combat.Actions;

using LunchHourGames.Hexagonal;
using LunchHourGames.Inventory;
using LunchHourGames.Inventory.Weapons;
using LunchHourGames.Sprite;
using LunchHourGames.Combat.Attacks;

namespace LunchHourGames.Players
{
    public class Zombie : Player
    {
        private float pathingNoiseQuotient;
        private float visualDetectionQuotient;
        private float auditoryDetectionQuotient;
        private float infectionQuotient;

        private ZombieAI zombieAI;    // Handles zombie artifical inteligence: decisions, movement, attack formations, etc...
        private AttackAI attackAI;    // Handles attack artifical inteligence: angles of attack, damage radius, and effectiveness of attack

        private ZombieAttack zombieAttack;

        public enum AcquisitionMode
        {
            Idle,
            Wandering,
            Attacking,
            Eating,
            Dying,
            Terminated
        }

        public Zombie(LunchHourGames lhg, String templateName, String name, PlayerSprite sprite)
            : base(lhg, templateName, name, Type.Zombie, true, sprite)
        {
        }

        public float PNQ
        {
            get { return this.pathingNoiseQuotient; }
            set { this.pathingNoiseQuotient = value; }
        }

        public float VDQ
        {
            get { return this.visualDetectionQuotient; }
            set { this.visualDetectionQuotient = value; }
        }

        public float ADQ
        {
            get { return this.auditoryDetectionQuotient; }
            set { this.auditoryDetectionQuotient = value; }        
        }

        public float IQ
        {
            get { return this.infectionQuotient; }
            set { this.infectionQuotient = value; }
        }

        public ZombieAI MyAI
        {
            get { return this.zombieAI; }
        }

        public override void recalcAllStats(int level)
        {
            MyAttributes.calculate(level, roll);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        // Combat Action Handling

        public CombatMenu.CombatMenuItem handleCombatChooseAction()
        {
            if (isNearHumanToAttack())
               return CombatMenu.CombatMenuItem.Attack;
            else
               return CombatMenu.CombatMenuItem.Move;
        }

        public int handleCombatChooseMove(AnimationCallback callback)
        {
            // locate the nearest human
            List<Human> humans = MyBoard.getHumans();
            if (humans.Count() > 0)
            {
                Human human = humans[0];
                HexBoard hexBoard = MyBoard.MyHexBoard;

                return MyBoard.movePlayer(this, human.Location, MyAttributes.actionPoints, callback);
            }

            return 0;
        }


        public override bool canMainPlayerWalkTo()
        {
            return false;
        }

        public override bool canMainPlayerAttack(Weapon weapon)
        {
            return true;
        }

        public bool isNearHumanToAttack()
        {
            Human human = getHumanToAttack();
            if (human != null)
                return true;

            return false;
        }

        public Human getHumanToAttack()
        {
            // Get the hex that the zombie is facing.
            Hex facingHex = MyBoard.getHexNextTo(Location.getHex(), Location.direction);
            if (facingHex != null)
            {
                GameEntity gameEntity = facingHex.MyGameEntity;
                if (gameEntity != null && gameEntity.MyEntityType == EntityType.Player)
                {
                    Player player = (Player)gameEntity;
                    if (player.MyType == Type.Human)
                        return (Human)player;
                }

                GameEntity inactiveEntity = facingHex.InactiveEntity;
                if (inactiveEntity != null && inactiveEntity.MyEntityType == EntityType.Player)
                {
                    Player player = (Player)inactiveEntity;
                    if (player.MyType == Type.Human)
                        return (Human)player;
                }
            }

            return null;
        }

        public ZombieAttack getCombatAttack(CombatSystem combatSystem)
        {
            if (zombieAttack == null)
                zombieAttack = new ZombieAttack(lhg, combatSystem);

            return zombieAttack;
        }
    }
}
