using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using LunchHourGames.Hexagonal;

namespace LunchHourGames.Common
{
    class LHGCommon
    {
        public static int getValueInRange(double calculatedValue, int min, int max)
        {
            if (calculatedValue < min)
                return min;
            if (calculatedValue > max )
                return max;

            return (int)calculatedValue;
        }

        public static void printToConsole(string message)
        {
            System.Console.WriteLine(message);
        }

        public static void printToConsole( string name, Vector3 vector)
        {
            System.Console.WriteLine("{0}={1},{2},{3}", name, vector.X, vector.Y, vector.Z);
        }

        public static string formatString(string name, Vector3 vector)
        {
            return String.Format("{0}: {1:0.00}, {2:0.00}, {3:0.00}", name, vector.X, vector.Y, vector.Z);
        }

        public static void printToConsole(string name, Point point)
        {
            System.Console.WriteLine("{0}={1},{2}", name, point.X, point.Y);
        }

        public static void printToConsole(string name, MouseState mouse)
        {
            System.Console.WriteLine("{0}={1},{2} Wheel={3}", name, mouse.X, mouse.Y, mouse.ScrollWheelValue);
        }

        public static void printToConsole(string name, Hex hex)
        {
            System.Console.WriteLine("{0}={1},{2}", name, hex.I, hex.J);
        }

        public static void printToConsole(string name, int value)
        {
            System.Console.WriteLine("{0}={1}", name, value);
        }

        public static void printToConsole(string name, float value)
        {
            System.Console.WriteLine("{0}={1}", name, value);
        }

        public static string formatString(string name, float value)
        {
            return String.Format("{0}: {1:0.00}", name, value);
        }
    }
   
}
