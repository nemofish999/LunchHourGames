using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Sprite;
using LunchHourGames.Combat;
using LunchHourGames.Combat.Attacks;
using LunchHourGames.Inventory;
using LunchHourGames.Inventory.Weapons;
using LunchHourGames.Hexagonal;
using LunchHourGames.Common;

namespace LunchHourGames.Players
{
    public abstract class Player : GameEntity
    {
        public enum Type
        {
            Unknown,
            Human,
            Zombie,
            Animal
        }

        public static Player.Type getTypeFromString(string typeAsString)
        {
            Player.Type type = Player.Type.Unknown;
            switch (typeAsString)
            {
                case "human":
                    type = Player.Type.Human;
                    break;

                case "zombie":
                    type = Player.Type.Zombie;
                    break;

                case "animal":
                    type = Player.Type.Animal;
                    break;
            }

            return type;
        }

        protected String templateName;  // The player template found in player.xml
        protected String name;    // Display name of the player that is given by the user during player setup
        protected int idNumber;  // Unique number to identify this player in any list in the system
        protected Type type;
        protected bool isComputerControlled;  // TRUE if the computer is making the decisions for this player; FALSE is human player
        protected bool isMainPlayer;  // TRUE if the player is the main character the user created at the beginning of the game

        protected TimeSpan eventStartTime;

        // Drawing the player
        protected PlayerSprite sprite;

        // Location of the player on the combat board
        protected CombatLocation combatLocation;

        protected DerivedAttributes attributes;

        protected int roll;

        // Animated States
        protected bool isAnimating;           // TRUE if the player going through an animation sequence
        protected AnimationCallback animationCallback;  // Object that has requested to be notified when animation has started and ended.
        protected AnimationType     animationType;      // The type of animation being performed

        protected List<Vector3> movePath;     // Path to move around on hexagonal (combat) board
        protected int totalMoveCount;         // Total number of moves to move the player (original size of movePath before we start moving)
        protected float walkingSpeed;         // Speed at which the player can move

        // Basic States
        protected bool isActive;     // TRUE if the player is actively in control of the game play 
        protected bool isAlive;      // TRUE if the player is alive in the game 
        protected bool isTurn;       // TRUE if the player's turn is available
        protected bool isClickable;  // TRUE if the player can be clicked on by the mouse
        protected bool canSelect;    // TRUE if the player can be selected to       
        protected bool isBlinking;   // TRUE means player is blinking which means the player has been hit or is going to be removed from the screen
   
        private InventoryStorage inventory; // List of items the player has in his inventory (i.e. carrying with him)
        private Weapon weapon;              // Current weapon the player has 

        protected bool isIdVisible;  // TRUE if the player's ID is visible
        protected bool isNameVisible; // TRUE if the player's name is visible

        public static Random random = new Random();

        protected StaticSprite playerHUDSprite;

        public Player(LunchHourGames lhg, String templateName, String displayName, Player.Type type, bool isComputerControlled, PlayerSprite sprite)
            : base(lhg, EntityType.Player, templateName, displayName)
        {
            this.templateName = templateName;
            this.type = type;
            this.isComputerControlled = isComputerControlled;
            this.isActive = true;
            this.sprite = sprite;
            this.roll = -1;  // indicate that a roll has not taken place
            this.walkingSpeed = 1.0f;

            this.idNumber = random.Next(10, 1000);
            this.isIdVisible = false;

            this.attributes = new DerivedAttributes();          
        }

        public void setCombatPosition(CombatLocation combatLocation, bool isAnimating )
        {
            this.combatLocation = combatLocation;
            this.isAnimating = isAnimating;
            updateCombatPosition();
        }

        public void updateCombatPosition()
        {
            Vector3 position = Vector3.Zero;
            if ( IsMoving && (movePath != null) && (movePath.Count() > 0))
            {
                position = movePath[0];
            }
            else
            {
                // Put the character in the middle of the cell
                position = MyBoard.putGameEntityOnCell(this, combatLocation.i, combatLocation.j);
            }

            combatLocation.position = position;
            sprite.updateLocation(combatLocation);
        }

        public void updateCombatPosition(Vector3 position)
        {
            Hex newHex = MyBoard.findHexAt(position);
            if (newHex != null)
            {
                Hex currentHex = getCurrentHex();
                if (currentHex != newHex)
                {
                    GameEntity gameEntity = newHex.MyGameEntity;
                    if (gameEntity != null)
                    {
                        // some other player is occupying this spot.  This isn't supposed to happen
                        // Only one player or obstacle on a hex.
                        return;
                    }

                    currentHex.MyGameEntity = null;
                    newHex.MyGameEntity = this;
                    combatLocation.i = newHex.I;
                    combatLocation.j = newHex.J;
                }

                combatLocation.position = position;
                combatLocation.direction = getDirection(movePath);
                sprite.updateLocation(combatLocation);
            }
        }

        public override void setGraphicsMatrices(Matrix view, Matrix projection, Matrix world)
        {
            this.sprite.setGraphicsMatrices(view, projection, world);
        }

        public override Matrix MyView
        {
            set
            {
                base.MyView = value;
                this.sprite.MyView = view;
            }
        }

        public StaticSprite MyHUDSprite
        {
            get { return this.playerHUDSprite; }
            set { this.playerHUDSprite = value; }
        }

        public PlayerSprite CurrentSprite
        {
            get { return this.sprite; }
            set { this.sprite = value; }
        }
       
        public CombatLocation Location
        {
            set { setCombatPosition(value, this.isAnimating); }
            get { return this.combatLocation;  }
        }

        public CombatBoard MyBoard
        {
            get { return this.combatLocation.board; }
        }

        public int ID
        {
            set { this.idNumber = value; }
            get { return this.idNumber;  }
        }

        public int Roll
        {
            set { this.roll = value; }
            get { return this.roll; }
        }

        public virtual void recalcAllStats(int level)
        {
        }

        public String MyTemplateName
        {
            get { return this.templateName; }
        }

        public bool IsMoving
        {
            get { return (animationType == AnimationType.Walking || animationType == AnimationType.Running); }                   
        }

        public bool IsMainPlayer
        {
            get { return this.isMainPlayer; }
            set { this.isMainPlayer = value; }
        }

        public TimeSpan EventStartTime
        {
            get { return this.eventStartTime; }
            set { this.eventStartTime = value; }
        }

        public bool IsComputerControlled
        {
            set { this.isComputerControlled = value; }
            get { return this.isComputerControlled; }
        }

        public bool IsAlive
        {
            get { return this.isAlive; }
            set { this.isAlive = value; }
        }

        public bool IsActive
        {
            get { return this.isActive; }
            set { this.isActive = value; }
        }

        public bool IsTurnAvailable
        {
            get { return this.isTurn; }
            set { this.isTurn = value; }
        }

        public void showID(bool show)
        {
            this.isIdVisible = show;
        }

        public String MyName
        {
            set { this.name = value; }
            get { return this.name;  }
        }

        public Type MyType
        {
            set { this.type = value; }
            get { return this.type; }
        }
 
        public DerivedAttributes MyAttributes
        {
            set { this.attributes = value; }
            get { return this.attributes; }
        }

        public InventoryStorage MyInventory
        {
            get { return this.inventory;  }
            set { this.inventory = value; }
        }

        public PlayerSprite MySprite
        {
            get { return this.sprite; }
        }

        public void subtractActionPointsBy(int num)
        {
            MyAttributes.actionPoints -= num;
        }

        public Rectangle getCurrentRectangle()
        {
            return this.combatLocation.screen.convertWorldToScreenRectangle(this.sprite.MyRectangle3D);
        }

        public void enableAnimation(bool isAnimating, AnimationKey animationKey)
        {
            this.isAnimating = isAnimating;
            this.sprite.IsAnimating = isAnimating;
            this.sprite.Direction = animationKey;
        }

        public float WalkingSpeed
        {
            get { return this.walkingSpeed; }
            set { this.walkingSpeed = value; }
        }

        public void showPlayerStanding()
        {
            animationType = AnimationType.Standing;
            sprite.changeSpriteSheet(PlayerSpriteSheet.Type.Standing, Location.direction);
        }

        public void showPlayerWalking()
        {
            animationType = AnimationType.Walking;
            sprite.changeSpriteSheet(PlayerSpriteSheet.Type.Walking, Location.direction);
        }

        public void showPlayerRunning()
        {
            animationType = AnimationType.Running;
            sprite.changeSpriteSheet(PlayerSpriteSheet.Type.Running, Location.direction);
        }

        public void showPlayerBeenHit()
        {
            animationType = AnimationType.BeenHit;
            sprite.changeSpriteSheet(PlayerSpriteSheet.Type.BeenHit, Location.direction);
        }

        public void showPlayerAttacking(AttackType attackType)
        {
            animationType = AnimationType.Attacking;
            sprite.changeSpriteSheet(PlayerSpriteSheet.Type.Attacking, Location.direction);
        }

        public void showPlayerDying()
        {
            animationType = AnimationType.Dying;
            sprite.changeSpriteSheet(PlayerSpriteSheet.Type.Dying, Location.direction);
        }

        public override void Update(GameTime gameTime)
        {
            switch (animationType)
            {
                case AnimationType.Standing:
                    break;

                case AnimationType.Walking:
                case AnimationType.Running:
                    updateMove();
                    break;

                case AnimationType.Attacking:
                    updateAttacking();
                    break;

                case AnimationType.BeenHit:
                    updateBeenHit();
                    break;

                case AnimationType.Dying:
                    updateDying();
                    break;

                default:
                    break;
            }

            this.sprite.MyProjection = this.combatLocation.screen.MyProjection;
            this.sprite.MyView = this.combatLocation.screen.MyView;

            this.sprite.Update(gameTime);
        }

        private void updateMove()
        {
            if (movePath == null)
            {
                enableAnimation(false, Location.direction);
                showPlayerStanding();
                if (animationCallback != null)
                    animationCallback.animationEnd(this, animationType);
            }
            else
            {
                enableAnimation(true, Location.direction);
                int pathCount = this.movePath.Count();
                if (pathCount == 0)
                    movePath = null;
                else
                {
                    // Let the callback know we are starting to move.
                    if (pathCount == totalMoveCount && animationCallback != null)
                        animationCallback.animationBegin(this, animationType);

                    Vector3 vector = this.movePath[0];
                    updateCombatPosition(vector);
                    movePath.RemoveAt(0);
                }
            }
        }

        private void updateAttacking()
        {
            if (sprite.CurrentFrame == 0)
                animationCallback.animationBegin(this, AnimationType.Attacking);

            if (this.sprite.CurrentFrame < this.sprite.FrameCount - 1)
                enableAnimation(true, Location.direction);
            else
            {
                if (isAnimating)
                    animationCallback.animationEnd(this, AnimationType.Attacking);

                enableAnimation(false, Location.direction);
            }
        }

        private void updateBeenHit()
        {
            if (sprite.CurrentFrame == 0)
                animationCallback.animationBegin(this, AnimationType.BeenHit);

            if (this.sprite.CurrentFrame < this.sprite.FrameCount - 1)
                 enableAnimation(true, Location.direction);
            else
            {
                if ( isAnimating )
                    animationCallback.animationEnd(this, AnimationType.BeenHit);

                enableAnimation(false, Location.direction);                
            }
        }

        private void updateDying()
        {
            if (sprite.CurrentFrame == 0)
                animationCallback.animationBegin(this, AnimationType.Dying);

            if (this.sprite.CurrentFrame < this.sprite.FrameCount - 1)
                enableAnimation(true, Location.direction);
            else
            {
                if ( isAnimating )
                    animationCallback.animationEnd(this, AnimationType.Dying);

                enableAnimation(false, Location.direction);                
            }

        }

        public Hex getCurrentHex()
        {
            return Location.board.MyHexBoard.getHex(Location.i, Location.j);
        }      

        public void removeFromCombatBoard()
        {
            Hex hex = getCurrentHex();
            hex.MyGameEntity = null;

            Location.board.removeGameEntity(this);
        }

        public void face(Hex hex)
        {
            Vector3 facingPosition = MyBoard.getCellCenter(hex);
            AnimationKey direction = getDirection(Location.position, facingPosition);
            face(direction);
        }

        public void face(AnimationKey direction)
        {
            Location.direction = direction;
            enableAnimation(false, direction);
            sprite.updateFrame();
        }

        public AnimationKey getDirection(List<Vector3> path)
        {
            AnimationKey direction = Location.direction;  // Our current direction
           
            if (path.Count() > 2)
            {
                Vector3 currentPosition = path[0];
                Vector3 nextPosition = path[1];
                direction = getDirection(currentPosition, nextPosition);
            }

            return direction;
        }

        public AnimationKey getDirection(List<Hex> path)
        {
            AnimationKey direction = Location.direction;  // Our current direction
            /*
            if (path != null && path.Count() > 2)
            {
                Hex startHex = path[0];
                Vector2 currentPosition = startHex.getCenterAsVector();
                Hex endHex = path[path.Count() - 1];
                Vector2 nextPosition = endHex.getCenterAsVector();
                direction = getDirection(currentPosition, nextPosition);
            }
            */

            return direction;
        }

        public AnimationKey getDirection(Point point)
        {
            AnimationKey direction = Location.direction;  // Our current direction
            /*
            Hex playerHex = Location.board.MyHexBoard.getHex(Location.i, Location.j);            
            Hex pointHex = Location.board.MyHexBoard.FindHexMouseClick(point);

            if (playerHex != null && pointHex != null)
           
                return getDirection(playerHex.getCenterAsVector(), pointHex.getCenterAsVector());
            }
             * */

            return direction;
        }

        public AnimationKey getDirection( Vector3 currentPosition, Vector3 nextPosition)
        {
            AnimationKey direction = Location.direction;  // Our current direction
            // Compass
            if ( nextPosition.X == currentPosition.X )
            {
                // We're not going East or West
                if (nextPosition.Z < currentPosition.Z)
                {
                    // We're going North
                    direction = AnimationKey.North;
                }

                if (nextPosition.Z > currentPosition.Z)
                {
                    // We're going South
                    direction = AnimationKey.South;
                }
            }

            if (nextPosition.X > currentPosition.X)
            {
                // We're going East
                if (nextPosition.Z < currentPosition.Z)
                {
                    // We're going NorthEast
                    direction = AnimationKey.NorthEast;
                }

                if (nextPosition.Z > currentPosition.Z)
                {
                    // We're going SouthEast
                    direction = AnimationKey.SouthEast;
                }
            }

            if (nextPosition.X < currentPosition.X)
            {
                // We're going West
                if (nextPosition.Z < currentPosition.Z)
                {
                    // We're going NorthWest
                    direction = AnimationKey.NorthWest;
                }

                if (nextPosition.Z > currentPosition.Z)
                {
                    // We're going SouthWest
                    direction = AnimationKey.SouthWest;
                }
            }

            if (nextPosition.Z == currentPosition.Z)
            {
                if (nextPosition.X < currentPosition.X)
                {
                    // we're going West
                    direction = AnimationKey.West;
                }

                if (nextPosition.X > currentPosition.X)
                {
                    // we're going East
                    direction = AnimationKey.East;
                }
            }

            return direction;
        }

        public override void Draw(GameTime gameTime)
        {
            if (isIdVisible)
            {
                Vector2 location = getVectorAbovePlayer();
                lhg.MySpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                lhg.MySpriteBatch.DrawString(lhg.SmallFont, this.idNumber.ToString(), location, Color.White);
                lhg.MySpriteBatch.End();
            }

            this.sprite.Draw(gameTime);

            /*
            if (isNameVisible)
            {
                SpriteBatch spriteBatch = characterSprite.mySpriteBatch;
                Vector2 location = new Vector2(characterSprite.Position.X, characterSprite.Position.Y - 10);
                spriteBatch.DrawString(font, this.name, location, Color.White);
            }*/
        }

        public Vector2 getVectorAbovePlayer()
        {
            Vector3 position = sprite.MyRectangle3D.topLeft;
            position.Y += 12;

            return this.combatLocation.screen.convertWorldToScreenVector2(position);
        }

        public override void walk(List<Vector3> path, AnimationCallback callback)
        {
            animationCallback = callback;
            movePath = path;
            totalMoveCount = path.Count;
            showPlayerWalking();
        }

        public virtual void attack(AttackType attackType, AnimationCallback callback)
        {
            animationCallback = callback;
            showPlayerAttacking(attackType);
        }

        public override void hit(Ammo ammo, AnimationCallback callback)
        {
            animationCallback = callback;

            // TO DO:  Change animation based on the ammo that hit you.  If it was a flamethrower then the player would be on fire.
            MyAttributes.hitPoints -= ammo.HitPoints;
            showPlayerBeenHit();
        }

        public override void die(AnimationCallback callback)
        {
            animationCallback = callback;
            animationType = AnimationType.Dying;
            showPlayerDying();

            this.isActive = false;
            this.isAlive = false;

            // Move the player to an inactive state on the hex board
            Hex currentHex = getCurrentHex();
            currentHex.MyGameEntity = null;
            currentHex.InactiveEntity = this;
        }

        public static int comparePlayersByInitiative(Player a, Player b)
        {
            if (a == null)
            {
                if (b == null)
                {
                    // If a is null and b is null, they're equal. 
                    return 0;
                }
                else
                {
                    // If a is null and b is not null, b is greater. 
                    return -1;
                }
            }
            else
            {
                // If a is not null...
                //
                if (b == null)
                // ...and b is null, a is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and b is not null, compare the initiatives of the two players
                    int aInitiative = a.MyAttributes.initiative;
                    int bInitiative = b.MyAttributes.initiative;

                    if (aInitiative >= bInitiative )
                    {
                        return 1;  // a has the greatest initiative
                    }
                    else
                    {
                        return -1;  // b has the greatest initiative
                    }
                }
            }
        }

        public Weapon MyWeapon
        {
            get { return this.weapon;  }
            set { this.weapon = value; }
        }

        public override Rectangle getExtents()
        {
            return this.getCurrentRectangle();
        }

        public PlayerMemento saveToMemento()
        {
            return null;
        }

        public void restoreFromMemento(PlayerMemento memento)
        {
        }
    }
}
