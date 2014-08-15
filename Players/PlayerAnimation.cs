///////////////////////////////////////////////////////////////////////////////
// PlayerAnimation.cs
//
// Wrapper to hold one animation sequence for a player
//
// Copyright (C) Lunch Hour Games. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames;
using LunchHourGames.Sprite;

namespace LunchHourGames.PlayerComponents
{
    public class PlayerAnimation
    {
        public enum Type
        {
            Unknown,
            Idle,
            Standing,
            Walking,
            Running,
            Attacking,
            BeenHit,
            Shooting,
            Dying,
            Exploding,
            OnFire,
            Disappearing,
            UsingItem,
            FallingDown
        }

        public enum Mode
        {
            Unknown,
            Combat,
            Travel,
            Scavenge,
            Story
        }

        private LunchHourGames lhg;
        private Type type;
        private Mode mode;
        private AnimatedSprite sprite;    

        public PlayerAnimation(LunchHourGames lhg, Type type, Mode mode, AnimatedSprite sprite)
        {
            this.lhg = lhg;
            this.type = type;
            this.mode = mode;
            this.sprite = sprite;
        }

        public static PlayerAnimation.Mode getModeFromString(string modeAsString)
        {
            PlayerAnimation.Mode mode = PlayerAnimation.Mode.Unknown;

            switch (modeAsString)
            {
                case "combat":
                    mode = PlayerAnimation.Mode.Combat;
                    break;

                case "travel":
                    mode = PlayerAnimation.Mode.Travel;
                    break;

                case "scavenge":
                    mode = PlayerAnimation.Mode.Scavenge;
                    break;

                case "story":
                    mode = PlayerAnimation.Mode.Story;
                    break;
            }

            return mode;
        }

        public static PlayerAnimation.Type getTypeFromString( string typeAsString )
        {
            PlayerAnimation.Type type = PlayerAnimation.Type.Unknown;

            switch ( typeAsString )
            {
                case "idle":
                   type = PlayerAnimation.Type.Idle;
                   break;
                
                case "standing":
                    type = PlayerAnimation.Type.Standing;
                    break;

                case "walking":
                    type = PlayerAnimation.Type.Walking;
                    break;

                case "running":
                    type = PlayerAnimation.Type.Running;
                    break;

                case "attacking":
                    type = PlayerAnimation.Type.Attacking;
                    break;

                case "beenhit":
                    type = PlayerAnimation.Type.BeenHit;
                    break;

                case "shooting":
                    type = PlayerAnimation.Type.Shooting;
                    break;

                case "dying":
                    type = PlayerAnimation.Type.Dying;
                    break;

                case "Exploding":
                    type = PlayerAnimation.Type.Exploding;
                    break;

                case "onfire":
                    type = PlayerAnimation.Type.OnFire;
                    break;

                case "disappearing":
                    type = PlayerAnimation.Type.Disappearing;
                    break;

                case "usingitem":
                    type = PlayerAnimation.Type.UsingItem;
                    break;

                case "fallingdown":
                    type = PlayerAnimation.Type.FallingDown;
                    break;

                default:
                    type = PlayerAnimation.Type.Unknown;
                    break;
            }

            return type;
        }

        public AnimatedSprite MyCharacterSprite
        {
            get { return this.sprite; }
        }

        public PlayerAnimation.Type MyType
        {
            get { return this.type; }
        }
    }                
}
