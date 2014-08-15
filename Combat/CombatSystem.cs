/*
 * **Combat Specification Document**

# Introduction
This document defines the logical flow of combat scenes for Trail of the Dead.  It defines the combat game mechanics and provides a cursory idea of the interface elements.  Specifics of the interface elements will be defined in a separate document.

# Controls
## Mouse and Keyboard
If the user is using a mouse and a keyboard as his primary input, a cursor will be displayed, and the user will make selections with left-clicks.  The user can go cancel selections with a right-click.  User’s can continue dialog with either a left-click, or the enter button.

## Gamepad
If the user is using a gamepad as his primary input, the user will be able to use the directional input to cycle through selections on the hex cell, as well as on the action menu.   Users can continue the dialog and make selections with the A button.  The user can cancel selections with the B button.

# Scene Introduction
Before displaying the combat scene, all characters, enemies, and entities will be loaded into the environment.  Once everything is ready to be displayed, the scene will fade in and the view will be zoomed out to display as much of the scene as possible.  If the scene is large enough, the camera can pass across the landscape as the scene title is displayed. The scene title will fade in and slowly fade out.

[Storyboard]

Optionally, at this time a dialog event can occur.  This dialog event can have dynamic camera work that will focus on specific characters.  The primary purpose of this dialog will be to provide context for the scene’s objectives.

Next, the scene’s objectives will fade in, and fade out, showing the user what must be done.  There will always be at least a primary objective, and optionally, additional secondary objectives.  After objectives are displayed, play can begin.

# Combat
Once play begins, it will continue until one of these conditions are met:

*Main Character Dies (HP <= 0)
*All Primary Objectives Met
*Characters Escape (For Certain Combat Scenes)

It's important for the engine to be able to detect when a user is stuck and provide additional information to complete the task.  For example, if the character has killed all enemies on the screen, but must complete a specific objective, e.g., "Save woman in car," the interface should visually call the objective to the attention of the user.

[Storyboard]

To begin play, the Engine will determine Initiative for each player (characters and enemies)

Init = 1d20 + Agility

## Actions
At the beginning of a character’s turn, his hex cell will be highlighted, and the camera will zoom in, and center on him.  A menu with the following actions will appear:

* Move
* Attack
* Defend
* Item
* <Contextual Action> - only appears when character is within range to trigger an event. e.g. "Pull Switch", "Honk Car Horn".
* End Turn

These actions all use a specific number of Action Points.  A character’s turn ends when he is out of Action Points, or when he selects his turn to be over.  We will go more in depth into each move.  After an action is completed, that specific amount of AP is deducted from the characters AP pool.  If a player has AP remaining, the Action Bar will again be displayed, and his turn will continue.  If this depletes his AP, this character’s turn is over.

### Move
After a user selects Move from a characters action menu, all hexes that he can move to, based on remaining AP, and hex availability, will be highlighted.  A character cannot move to or pass through any hex cell that contains an enemy, another player, or an obstacle.  When the user left-clicks a selectable cell, the character will move to that cell.  This completes the Move action.

Move Action Range = 1 AP per Cell

[Storyboard]

### Attack
After a user selects Attack from the action menu, a list of available attacks will be displayed by the character.  These can be attacks that use equipped weapons, or special abilities.  Once a specific attack is selected, all hexes that he can attack, based on weapon range will be highlighted.  A character cannot attack a member of his own party.  When the user mouses over a cell, the cells affected by the attack will be highlighted.  See “Attack Types” below.  

#### Attack Types
Each attack move can be categorized as one of the following types:

1. Linear Point-to-Point (LP)
This can be a basic attack like "Shoot" or "Stab".  Only one cell is affected.  The attack trajectory follows a straight line, i.e., it is not something that can be thrown over a wall.

Examples: Handgun, Baseball Bat

[Image]

2. Linear Spread (LS)
This attack functions similarly to LP, in that it has a linear trajectory.  The difference is that after the primary point of attack, adjacent cells are affected in a spread pattern.  

Example: Shotgun

[Image]

3. Linear Point-to-Point with Splash (LP+S)
This attack functions the same as an LP attack, but has splash damage radiating from the focal point of attack.

Example: Rocket Launcher

[Image]

4. Parabolic Point-to-Point (PP)
This attack has a parabolic trajectory to a specific point, so it is reserved for items that can be thrown at enemies. 

Example: Bow and Arrow

[Image]

5. Parabolic Point-to-Point with Splash (PP+S)
This attack functions the same as a PP attack, but has splash damage radiating from the focal point of attack.

Example: Grenade

[Image]

### Defend
The Defend Action will increase the defensive resistance of a character for a turn. Once a character completes an Attack, Move, or Item action, he cannot defend.  Upon selecting Defend, that characters turn is over.  Additionally, the character's chance to be infected decreases dramatically.

### Item
When the user selects the Item action, the usable inventory screen will appear, and the user will be able to select an Item to use.  Basically, these Items are finite resources that can be used to attack enemies or heal allies.  

Examples: Grenades, Molotov Cocktails, First Aid Kits, Twinkies

### Contextual Actions
Contextual Actions only appear for a player if a conditional is met.  For example, if a player is near a parked car, he can "Honk Car Horn", or if the primary player is on an Escape Zone, he can select "Escape".  (See Escape Zone section under Special Rules).  Contextual Actions may use a variable amount of AP.  To define a new Contextual Action, the prerequisites are the condition for it's availability, the amount of AP it consumes, whether or not it ends the turn, and the event it generates.

### End Turn
Selecting "End Turn" means the player has completed his actions, and is ready to allow the next player or enemy to do their turn.

# Enemy Behavior
To create increasingly challenging and interesting combat, it's important to define specific attributes that will determine the behavior of enemies.

## Pathing Intelligence
Each enemy will be assigned a specific Pathing Noise Quotient (PNQ) to measurably distort the effectiveness of the pathing logic for that enemy.  Essentially, the lower the better, so an enemy with a PNQ of .00 will have perfect pathing, while an enemy with a PNQ of .99 will almost never move in a meaningful way.

## Target Acquisition
To define the specifics as to how an enemy determines what entities are accurately prioritized into targets, we must define the logic of how an enemy processes this information.  Actions in a combat turn can produce events.  These events can be detected by enemies, and will be used to determine if the event is worth investigating or attacking.  Each enemy has two modes of targeting: Investigation and Attack.  If an enemy is in Investigation mode, he will continue to investigate the source of an event, until he discovers it is not an enemy, or if it is an enemy, it is upgraded to Attack mode.

### Events
As stated before, actions in combat produce events.  An event can be either auditory or visual, and an action often produces both.  These events will have a range of influence.  For example, a player out of visual range of a group of zombies could honk a car horn. The horn of the car will have a range of influence. In addition to events, the players themselves act as visual data for the enemy to process.  The absolute simplest way for an enemy to acquire a target, is to see the target.

### Visual Acquisition
Each enemy will be assigned a specific visual detection range (VDR) that will determine how many spaces away that enemy can see.  Once an entity is within the VDR of a specific enemy, that enemy can see the event or character.

### Auditory Acquisition
Each enemy will be assigned a specific auditory detection range (ADR) that will determine how many spaces away it can hear things.  Once the area of influence of an auditory event intersects with the area within the ADR of a specific enemy, that event or character has been heard.

Note: An enemies' VDR and ADR can be whatever the designer chooses, but it's a general rule of thumb that enemies can hear farther than they can see.  An enemy with an ADR of 0 is deaf, and an enemy with a VDR of 0 is blind.

### Reacquisition
Enemies will continue to investigate or attack targets until one of the following conditions are met:

1. Target being investigated is not worthy of attack.
2. Target being attacked has died, and, where applicable, has been eaten. (See Character Death)
3. A closer target worthy of attack is seen.

In the first two cases, the enemy will no longer have a target, and will continue as he did before he had a target.  In the latter case, he has a new target and will attack that target.

# Special Rules
Just like the English language, there are always exceptions.  This section is for special cases that don't necessarily fit under any of the other sections.

## Escape Zones
Some combat levels can be escaped.  Basically, these are optional levels, where the party has the ability to get back into their vehicle, and move on to the next town.  Escape can be the right thing to do, if the fight is not going your way, you don't care to rescue anyone (negative morolometer hit), or if you just change your mind.  At the beginning of these optional levels, before play starts, you will have a chance to skip the level.  But once play starts, your characters will have to reach the escape zone to skip it.  The escape zone is three hex cells on either the left or right side of the combat field (whichever side is the logical location of the characters' vehicle).  Once the primary character is on an escape cell, he can select the contextual action "Escape", but when he does, only the characters in the Escape Zone will escape.  This means it is possible to sacrifice characters for the greater good of the party. 

## Respawn Cells
Some combat levels will contain cells that allow more enemies to appear.  These cells must be defined with the following information: The rate of respawn, which enemies will respawn, and total number of potentially spawned enemies.  From a design perspective, these cells should be somewhere logical, like the dangerous side of the map.

## Character Death
If the primary characters HP reaches 0, the game is over, but play can continue if a secondary character's HP reaches 0.  At that point that character falls to the ground.  If the enemies that killed him eat human flesh (as zombies oft do), they will eat him for 2 full turns.  After 2 turns, they are no longer targetting him.  If those enemies that killed him are infectious, after an additional turn that character is now an enemy of the same type.

# In Conclusion
Please be aware this document is subject to change, but all engine and design changes should be noted within this document, upon majority approval by the team.



 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


using LunchHourGames.AI;
using LunchHourGames.GamePlay;
using LunchHourGames.Drawing;
using LunchHourGames.Screen;
using LunchHourGames.Hexagonal;
using LunchHourGames.Sprite;
using LunchHourGames.Players;
using LunchHourGames.Console;
using LunchHourGames.Inventory;
using LunchHourGames.Obstacles;

namespace LunchHourGames.Combat
{
    public class CombatSystem
    {
        private LunchHourGames lhg;                   // The main XNA game object for our game.  Holds all game objects and resources.

        private CombatTurnBased turnBased;            // Handles the turn-based logic of the game.  This includes rolling dice and handling player actions    
        private CombatScreen screen;                  // Handles drawing all items that are part of the combat screen: stage, board, game entities, menu, and HUD
        private CombatBoard board;                    // Handles the hexagonal combat board: hex grid, player/obstacle sprites and positions
        private CombatInterpreter interpreter;        // Handles console commands that are used in combat mode
        private int level;                            // Level number for the current combat board
        private bool isOptional;                      // TRUE if this combat scene is optional; FALSE means it is required

        private Song combatTheme;                     // The main combat theme

        public enum Result                            // Possible result of the combat play for the main player
        {            
            Success,                                  // Main player was successful and completed all missions and goals
            Fail,                                     // Main player failed, game is now over
            Skipped,                                  // Main player didn't want to play this optional combat scene
            ContinueLater,                            // Main player wants to exit and save current state
            Exit                                      // Main player wants to exit and not save current state
        }

        public CombatSystem(LunchHourGames lhg)
        {
            this.lhg = lhg;
            this.turnBased = new CombatTurnBased(lhg, this);
            this.interpreter = new CombatInterpreter(this);
        }

        public CombatInterpreter Interpreter
        {
            get { return this.interpreter; }
        }

        public void startLevel(Party party, int level)
        {
            this.level = level;
             
            // TO DO:  Need to do this in XML
            // Set the main player location and add them to the board
            Player mainPlayer = party.getMainPlayer();
            CombatLocation location = new CombatLocation(this.board, 9, 9, AnimationKey.NorthEast);
            mainPlayer.Location = location;
            board.addGameEntity(mainPlayer);

            Player soldier = party.getHumanByReferenceName("soldier");
            location = new CombatLocation(this.board, 9, 2, AnimationKey.NorthEast);
            soldier.Location = location;
            board.addGameEntity(soldier);

            this.turnBased.startLevel(level);
            this.screen.startLevel(level);

            try
            {
                combatTheme = lhg.Content.Load<Song>("Audio/combat theme_1");
                MediaPlayer.Play(combatTheme);
                MediaPlayer.IsRepeating = true;
            }
            catch
            {
            }
        }

        public List<Player> MyPlayers
        {
            get { return this.board.MyGameEntities.MyPlayers; }
        }

        public List<Player> ActivePlayers
        {
            get { return this.turnBased.ActivePlayers; }
        }

        public GameEntities MyGameEntities
        {
            get { return this.board.MyGameEntities; }
        }
        
        public int Level
        {
            set { this.level = value; }
            get { return this.level; }
        }       

        public CombatScreen MyScreen
        {
            get { return this.screen; }
            set
            {
                this.screen = value;
                this.screen.MyCombatSystem = this;
            }
        }

        public CombatBoard MyBoard
        {
            get { return this.board; }
            set { this.board = value; }
        }

        public CombatTurnBased MyTurnBased
        {
            get { return this.turnBased; }
        }

        public void turnMusic(bool allow)
        {

        }

        public void Update(GameTime gameTime)
        {
            this.turnBased.Update(gameTime);
        }

        public void endCombat(Result result)
        {
            // Tell the game flow we are finished playing this combat scene
        }
    }
}
