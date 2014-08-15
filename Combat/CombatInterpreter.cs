using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.Console;
using LunchHourGames.Players;
using LunchHourGames.Sprite;
using LunchHourGames.Inventory;
using LunchHourGames.Combat.Attacks;

namespace LunchHourGames.Combat
{
    public class CombatInterpreter : ConsoleInterpreter
    {
        private CombatSystem combatSystem;

        public CombatInterpreter(CombatSystem combatSystem)
        {
            this.combatSystem = combatSystem;
        }
        
        public void parse(LHGConsole console, String[] arguments)
        {
            switch (arguments[0])
            {
            case "add":
                handleAdd( console, arguments );
                break;

            case "animate":
                handleAnimate( console, arguments );
                break;

            case "camera":
                handleCamera(console, arguments);
                break;

            case "change":
                handleChange( console, arguments );
                break;

            case "clear":
                handleClear( console, arguments );
                break;

            case "console":
                handleConsole(console, arguments);
                break;

            case "do":
                handleDo( console, arguments );
                break;

            case "face":
                handleFace( console, arguments );
                break;

            case "goto":
                handleGoto(console, arguments);
                break;

            case "gun":
                handleGun(console, arguments);
                break;

            case "help":
                handleHelp(console, arguments);
                break;

            case "hide":
                handleHide( console, arguments );
                break;

            case "kill":
                handleKill( console, arguments );
                break;
            
            case "load":
                handleLoad( console, arguments );
                break;

            case "move":
                handleMove( console, arguments );
                break;

            case "music":
                handleMusic( console, arguments );
                break;

            case "redo":
                handleRedo( console, arguments );
                break;

            case "remove":
                handleRemove( console, arguments );
                break;

            case "roll":
                handleRoll( console, arguments );
                break;

            case "save":
                handleSave( console, arguments );
                break;

            case "select":
                handleSelect( console, arguments );
                break;

            case "set":
                handleSet( console, arguments );
                break;

            case "show":
                handleShow( console, arguments );
                break;

            case "undo":
                handleUndo( console, arguments );
                break;

            case "quit":
                handleQuit( console, arguments );
                break;

            case "version":
                handleVersion( console, arguments );
                break;
            }
        }

        private void handleAdd(LHGConsole console, String[] arguments)
        {
        }
        
        private void handleAnimate(LHGConsole console, String[] arguments)
        {
            // animate player <player number> <stop | walk | hit | die>
            if (arguments[1].Equals("player"))
            {
                Player player = getPlayer(arguments[2]);
                if (player == null)
                    console.WriteLine("Error: Unable to find player '" + arguments[2] + "'!");
                else
                {
                    bool shouldAnimate = true;
                    if (arguments.Length == 4)
                    {
                        switch (arguments[3])
                        {
                            case "stop":
                                shouldAnimate = false;
                                player.showPlayerStanding();
                                break;

                            case "walk":
                                player.showPlayerWalking();
                                break;

                            case "hit":
                                player.showPlayerBeenHit();
                                break;

                            case "die":
                                player.showPlayerDying();
                                break;

                            case "attack":
                                player.showPlayerAttacking(AttackType.Grab);
                                break;

                            case "run":
                                player.showPlayerRunning();
                                break;
                        }
                    }
                  
                    player.enableAnimation(shouldAnimate, player.Location.direction);
                }
            }
        }

        private void handleCamera(LHGConsole console, String[] arguments)
        {
            switch (arguments[1])
            {
                case "test":
                    if (arguments[2].Equals("on"))
                        combatSystem.MyScreen.doCameraTest(true);
                    else
                        combatSystem.MyScreen.doCameraTest(false);
                    return;

                case "reset":
                    combatSystem.MyScreen.resetCamera();
                    return;
            }

            console.WriteLine("Error:  Invalid number of arguments for command!");
        }

        private void handleChange(LHGConsole console, String[] arguments)
        {
            // Change stat <player_number> <stat name> <value>
            if (arguments.Length != 5)
                console.WriteLine("Error:  Invalid number of arguments for command!");
            else
            {
                switch (arguments[1])
                {
                    case "stat":
                        {
                            Player player = getPlayer(arguments[2]);
                            if (player == null)
                                console.WriteLine("Error: Unable to find player '" + arguments[2] + "'!");
                            else
                            {
                                String statToChange = arguments[3];
                                try
                                {
                                    int value = Convert.ToInt16(arguments[4]);
                                    if ( player.MyType == Player.Type.Human )
                                        ((Human)player).changeStat(statToChange, value, combatSystem.Level, player.Roll);
                                }
                                catch (Exception ex)
                                {
                                    console.WriteLine("Error: Invalid value given!");
                                }
                            }
                        }
                        break;
                }
            }
        }
        
        private void handleClear(LHGConsole console, String[] arguments)
        {
            console.clearBuffer();
        }

        private void handleConsole(LHGConsole console, String[] arguments)
        {
            if ( arguments.Length != 2 )
                console.WriteLine("Error: Invaild number of arguments for this command");
            else
            {
                switch ( arguments[1] )
                {
                    case "dark":
                        console.ConsoleDark = true;
                        break;
                    case "light":
                        console.ConsoleDark = false;
                        break;
                    default:
                        console.WriteLine("Error: Unknown property for this command");
                        break;
                }
            }
        }

        private void handleDo(LHGConsole console, String[] arguments)
        {
        }

        private void handleFace(LHGConsole console, String[] arguments)
        {
            // face player <player number> <n|ne|e|se|s|sw|w|nw>
            if (arguments.Length != 4)
                console.WriteLine("Error: Invalid number of arugments for command!");
            else if (arguments[1].Equals("player"))
            {
                Player player = getPlayer(arguments[2]);
                if (player == null)
                    console.WriteLine("Error: Unable to find player '" + arguments[2] + "'!");
                else
                {
                    AnimationKey animationKey;
                    switch (arguments[3])
                    {
                        case "n":
                            animationKey = AnimationKey.North;
                            break;

                        case "ne":
                            animationKey = AnimationKey.NorthEast;
                            break;

                        case "e":
                            animationKey = AnimationKey.East;
                            break;

                        case "se":
                            animationKey = AnimationKey.SouthEast;
                            break;

                        case "s":
                            animationKey = AnimationKey.South;
                            break;

                        case "sw":
                            animationKey = AnimationKey.SouthWest;
                            break;

                        case "w":
                            animationKey = AnimationKey.West;
                            break;

                        case "nw":
                            animationKey = AnimationKey.NorthWest;
                            break;

                        default:
                            console.WriteLine("Error: Invalid direction for animate. Pick n|ne|e|se|s|sw|w|nw");
                            return;
                    }
                            
                    player.face(animationKey);
                }
            }
        }

        private void handleGoto(LHGConsole console, String[] arguments)
        {
            if (arguments.Length != 2)
                console.WriteLine("Error: Invalid number of arugments for command!");
            else if (arguments[1].Equals("design"))
            {
                //this.combatSystem.lhg.gotoDesign();
            }
        }

        private void handleGun(LHGConsole console, String[] arguments)
        {
            /*
            if (arguments.Length != 2)
                console.WriteLine("Error: Invalid number of arugments for command!");
            else if (arguments[1].Equals("reload"))
            {
                List<Player> humans = combatSystem.getPlayer(Player.Type.Human);
                if (humans.Count() > 0)
                {
                    Player mainPlayer = humans[0];
                    Gun gun = (Gun) mainPlayer.MyWeapon;
                    gun.reload();
                }
            }
             */
        }

        private void handleHelp(LHGConsole console, String[] arguments)
        {
            if (arguments.Length == 1)
            {
                console.WriteLine("Commands Available:");
                console.WriteLine("add, animate, change, clear, console, do, face, help, hide, kill, load, move");
                console.WriteLine("music, redo, remove, roll, save, select, set, show, undo, quit, version");
                console.WriteLine("");
                console.WriteLine("Type: help <command> to see specific help for that command");
            }
            else
            {
                switch (arguments[1])
                {
                    case "add":
                        
                        break;

                    case "animate":
                        console.WriteLine("animates a character's sprite");
                        console.WriteLine("animate <player number>       starts a character's cell animation");
                        console.WriteLine("animate <player number> stop  stops a character's cell animation");
                        break;

                    case "change":
                        console.WriteLine("Changes a game variable");
                        console.WriteLine("");
                        break;

                    case "clear":
                        console.WriteLine("Clears a player or game object from the board");
                        break;

                    case "console":
                        console.Write("Adjusts a property on the console");
                        console.WriteLine("");
                        console.WriteLine("console dark   tints the console darker");
                        console.WriteLine("console light   tints the console lighter");
                        break;

                    case "do":
                        console.WriteLine("Makes a player do a particular action");
                        break;

                    case "face":
                        console.WriteLine("Faces a character or object in a given position on the board");
                        console.WriteLine("");
                        console.WriteLine("face <player number> <direction: n|ne|e|se|s|sw|w|nw>"); 
                        break;

                    case "help":
                        console.WriteLine("Shows help for a given command");
                        console.WriteLine("");
                        console.WriteLine("help            shows all commands available");
                        console.WriteLine("help <command>  shows help for the given command");
                        break;

                    case "hide":
                        console.WriteLine("Hides a game property");
                        console.WriteLine("");
                        console.WriteLine("hide player numbers  hides the all players unique ID");
                        console.WriteLine("hide grid numbers    hides the grid numbers");                        
                        break;

                    case "kill":
                        console.WriteLine("Forces a player to die immediatelly");
                        console.WriteLine("");
                        console.WriteLine("kill <player number>  kills the character and plays the dying animation");
                        break;

                    case "load":
                        console.WriteLine("loads an active combat board game into memory"); 
                        break;

                    case "move":
                        console.WriteLine("Moves a player to a given board location");
                        console.WriteLine("");
                        console.WriteLine("move <player number> <board location>");                        
                        break;

                    case "music":
                        console.WriteLine("Turns on or off the current music");
                        console.WriteLine("");
                        console.WriteLine("music <on | off>");
                        break;

                    case "redo":
                        console.WriteLine("Redoes a move that was previously undone by the undo command");
                        break;

                    case "roll":
                        console.WriteLine("Roll a die for a given player");
                        console.WriteLine("");
                        console.WriteLine("roll <dice number> <player number>");
                        break;

                    case "remove":
                        console.WriteLine("Removes a character or object from the board");
                        break;

                    case "save":
                        console.WriteLine("Saves the current state of the combat board for later testing");
                        break;

                    case "select":
                        console.WriteLine("Selects a character's turn immediately");
                        break;

                    case "set":
                        console.WriteLine("Sets a game variable to a give variable");
                        break;

                    case "show":
                        console.WriteLine("Shows a value in the game system");
                        console.WriteLine("");
                        console.WriteLine("show grid numbers               Shows the x,y coordinates of the hex grid");
                        console.WriteLine("show player numbers             Shows the player's number for use in the console");
                        console.WriteLine("show stats  <player number>     Shows the S.U.R.V.I.V.A.L stats for the given player");
                        console.WriteLine("show skills <player number>     Shows the skills of the given player");
                        console.WriteLine("show derived <player number>    Shows the derived attributes for the given player");
                        break;

                    case "undo":
                        console.WriteLine("Undoes the last move on the board");
                        break;

                    case "quit":
                        console.WriteLine("Quits the game");
                        break;

                    case "version":
                        console.WriteLine("displays various version information");
                        break;
                }
            }
        }

        private void handleHide(LHGConsole console, String[] arguments)
        {
            // hide grid numbers               Hides the x,y coordinates of the hex grid
            // hide player numbers             Hides the player's number for use in the console
          
            if (arguments.Length == 1)
                console.WriteLine("Error: No arguments given for command");
            if (arguments.Length != 3)
                console.WriteLine("Error: Invalid number of arguments for command");
            else
            {
                switch (arguments[1])
                {
                    case "grid":
                        if (arguments[2].Equals("numbers"))
                        {
                            this.combatSystem.MyBoard.showGridNumbers(false);   
                        }
                        break;

                    case "player":
                        if (arguments[2].Equals("numbers"))
                        {
                            List<Player> players = this.combatSystem.MyPlayers;
                            foreach (Player player in players)
                                player.showID(false);
                        }
                        break;

                    default:
                        console.WriteLine("Error: Unknown argument used for command");
                        break;
                }
            }
        }

        private void handleKill(LHGConsole console, String[] arguments)
        {
        }

        private void handleLoad(LHGConsole console, String[] arguments)
        {
        }

        private void handleMove(LHGConsole console, String[] arguments)
        {
            // move player 1 2,3
            if (arguments.Length < 4)
                console.WriteLine("Error: Invalid number of arguments for move command!");
            else
            {
                if (arguments[1].Equals("player"))
                {
                    Player player = getPlayer(arguments[2]);
                    if ( player == null )
                        console.WriteLine("Error: Unable to find player '" + arguments[2] + "'!");

                    CombatLocation endLocation = getLocation(player, arguments[3]);
                    //this.combatSystem.movePlayer( player, endLocation );
                }
            }
        }

        private Player getPlayer(String idAsString)
        {
            Player player = null;

            short value;
            try
            {
                value = Convert.ToInt16(idAsString);
            }
            catch (Exception ex)
            {
                return null;
            }

            return combatSystem.MyGameEntities.getPlayer(value);
        }

        private CombatLocation getLocation(Player player, String coordsAsString)
        {
            int i = 0;
            int j = 0;

            try
            {
                String[] coords = coordsAsString.Split(',');
                if (coords.Length != 2)
                    return null;

                i = Convert.ToInt16(coords[0]);
                j = Convert.ToInt16(coords[1]);
            }
            catch (Exception ex)
            {
                return null;
            }
       
            CombatLocation endLocation = new CombatLocation(player.Location.board, i, j, player.Location.direction);
            return endLocation;
        }

        private void handleMusic(LHGConsole console, String[] arguments)
        {
            // music on|off
            if (arguments.Length != 2)
                console.WriteLine("Error: Invalid number of arguments for this command");
            else
            {
                switch (arguments[1])
                {
                    case "on":
                        combatSystem.turnMusic(true);
                        break;
                    case "off":
                        combatSystem.turnMusic(false);
                        break;
                    default:
                        console.WriteLine("Error: Invalid argument for this command!");
                        break;
                }
            }
        }

        private void handleRedo(LHGConsole console, String[] arguments)
        {
        }

        private void handleRemove(LHGConsole console, String[] arguments)
        {
        }

        private void handleRoll(LHGConsole console, String[] arguments)
        {
            // roll dice <player number>
            if (arguments.Length != 3)
                console.WriteLine("Error: Invalid number of arguments for this command");
            else
            {
                Player player = getPlayer(arguments[2]);
                if (player == null)
                    console.WriteLine("Error: Unable to find player '" + arguments[2] + "'!");
                else
                {
                    try
                    {
                        int dice = Convert.ToInt16(arguments[1]);
                        //combatSystem.rollDice(player, dice);                        
                    }
                    catch (Exception ex)
                    {
                        console.WriteLine("Error:  Invalid die number used");
                    }       
                }
            }
        }

        private void handleSave(LHGConsole console, String[] arguments)
        {
        }

        private void handleSelect(LHGConsole console, String[] arguments)
        {
        }

        private void handleSet(LHGConsole console, String[] arguments)
        {
        }

        private void handleShow(LHGConsole console, String[] arguments)
        {
            // show grid numbers               Shows the x,y coordinates of the hex grid
            // show player numbers             Shows the player's number for use in the console
            // show stats  <player number>     Shows the S.U.R.V.I.V.A.L stats for the given player
            // show skills <player number>     Shows the skills of the given player
            // show derived <player number>    Shows the derived attributes for the given player
            // show all <player number> Shows all the properites of a player           

            if (arguments.Length == 1)
                console.WriteLine("Error: No arguments given for command");
            if (arguments.Length != 3)
                console.WriteLine("Error: Invalid number of arguments for command");
            else
            {
                switch (arguments[1])
                {
                    case "grid":
                        if (arguments[2].Equals("numbers"))
                        {
                            this.combatSystem.MyBoard.showGridNumbers(true);                        
                        }
                        break;

                    case "player":
                        {
                            if (arguments[2].Equals("numbers"))
                            {
                                List<Player> players = this.combatSystem.MyPlayers;
                                foreach (Player player in players)
                                    player.showID(true);
                            }
                        }
                        break;

                    case "stats":
                        {
                            Player player = getPlayer(arguments[2]);
                            if (player == null)
                                console.WriteLine("Error: Unable to find player '" + arguments[2] + "'!");
                            else if ( player.MyType == Player.Type.Human )
                                showStats(console, ((Human)player).MyStats);
                            else
                                console.WriteLine("Error: Only human players have S.U.R.V.I.V.A.L stats!");
                        }
                        break;

                    case "skills":
                        {
                            Player player = getPlayer(arguments[2]);
                            if (player == null)
                                console.WriteLine("Error: Unable to find player '" + arguments[2] + "'!");
                            else if ( player.MyType == Player.Type.Human )
                                showSkills(console, ((Human)player).MySkills);
                            else
                                console.WriteLine("Error: Player '" + arguments[2] + "' doesn't have skills!");                     
                        }
                        break;

                    case "derived":
                        {
                            Player player = getPlayer(arguments[2]);
                            if (player == null)
                                console.WriteLine("Error: Unable to find player '" + arguments[2] + "'!");
                            else
                                showDerived(console, player.MyAttributes);
                        }
                        break;

                    case "all":
                        {
                            Player player = getPlayer(arguments[2]);
                            if (player == null)
                                console.WriteLine("Error: Unable to find player '" + arguments[2] + "'!");
                            else
                                showProperties(console, player);
                        }
                        break;

                    default:
                        console.WriteLine("Error: Unknown argument used for command");
                        break;
                }
            }
        }

        private void showStats(LHGConsole console, PrimaryStatistics stats)
        {
            string statsAsString = string.Format("s={0} u={1} r={2} vit={3} i={4} vis={5} a={6} l={7}",
                stats.strength, stats.utilization, stats.resourcefulness, stats.vitality,
                stats.intelligence, stats.vision, stats.agility, stats.luck);

            console.WriteLine(statsAsString);
        }

        private void showSkills(LHGConsole console, Skills skills)
        {
            string skillsAsString = 
                string.Format("melee={0} guns={1} explosives={2} vrepair={3} aid={4} wmods={5} scavenge={6} speech={7}",
                skills.meleeWeapons, skills.guns, skills.explosives, skills.vehicularRepair,
                 skills.firstAid, skills.weaponMods, skills.scavenge, skills.speech);

            console.WriteLine(skillsAsString);
        }

        private void showDerived(LHGConsole console, DerivedAttributes derived)
        {
            string derivedAsString =
                string.Format("ap={0} crit={1} hp={2} res={3} acc={4} init={5}",
                derived.actionPoints, derived.criticalChance, derived.hitPoints, derived.resistance,
                derived.accuracy, derived.initiative);

            console.WriteLine(derivedAsString);
        }

        private void showProperties(LHGConsole console, Player player)
        {
            string propertiesAsString = string.Format("name={0} roll={1} hex=({2},{3})", player.MyName, player.Roll,
                player.Location.i, player.Location.j);
            console.WriteLine(propertiesAsString);

            if ( player.MyType == Player.Type.Human )
                showStats(console, ((Human)player).MyStats);

            showDerived(console, player.MyAttributes);
            if ( player.MyType == Player.Type.Human )
                showSkills(console, ((Human)player).MySkills);
        }

        private void handleUndo(LHGConsole console, String[] arguments)
        {
        }
        
        private void handleQuit(LHGConsole console, String[] arguments)
        {
        }

        private void handleVersion(LHGConsole console, String[] arguments)
        {
        }

        public void showWelcome(LHGConsole console, String message)
        {
            console.defaultInterpreter.showWelcome(console, "You are in combat mode.");
        }
    }
}