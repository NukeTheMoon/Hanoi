using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanoi
{
    public static class ConsolePresenter
    {
        public static string IntToStringFast(int value, char[] baseChars)
        {
            // 32 is the worst cast buffer size for base 2 and int.MaxValue
            int i = 32;
            char[] buffer = new char[i];
            int targetBase = baseChars.Length;

            do
            {
                buffer[--i] = baseChars[value % targetBase];
                value = value / targetBase;
            }
            while (value > 0);

            char[] result = new char[32 - i];
            Array.Copy(buffer, i, result, 0, 32 - i);

            return new string(result);
        }

        public static string ToBase62(int n)
        {
            if (n >= 0 && n <= 61)
            {
                return IntToStringFast(n,
                    new char[] { '0','1','2','3','4','5','6','7','8','9',
                    'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                    'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'});
            }
            throw new Exception("This function does not return double digit numbers! Pass a value between 0 and 61");
        }

        public static ConsoleColor ToConsoleColor(string color)
        {
            switch (color)
            {
                case "cyan":
                    return ConsoleColor.Cyan;
                case "red":
                    return ConsoleColor.Red;
                case "green":
                    return ConsoleColor.Green;
                case "blue":
                    return ConsoleColor.Blue;
                case "magenta":
                    return ConsoleColor.Magenta;
                case "white":
                    return ConsoleColor.White;
                case "yellow":
                    return ConsoleColor.Yellow;
                case "darkblue":
                    return ConsoleColor.DarkBlue;
                case "darkcyan":
                    return ConsoleColor.DarkCyan;
                case "darkgray":
                    return ConsoleColor.DarkGray;
                case "darkgreen":
                    return ConsoleColor.DarkGreen;
                case "darkmagenta":
                    return ConsoleColor.DarkMagenta;
                case "darkred":
                    return ConsoleColor.DarkRed;
                case "darkyellow":
                    return ConsoleColor.DarkYellow;
                default:
                    return ConsoleColor.Gray;
            }
            throw new Exception("Windows console can only display a limited amount of colors. Pass a value between 0 and 14");
        }
        private static List<string> GetColors(GameState state)
        {
            List<Disc> allDiscs = state.GetAllDiscs();
            List<string> foundColors = new List<string>();
            for (var i=0; i<allDiscs.Count ; ++i)
            {
                if (!foundColors.Contains(allDiscs.ElementAt(i).Color))
                {
                    foundColors.Add(allDiscs.ElementAt(i).Color);
                }
            }
            return foundColors;
        }

        public static void PresentState(GameState state)
        {
            ConsoleColor original = Console.ForegroundColor;
            for (var i=0; i<state.Pegs.Count; ++i)
            {
                Console.Write(i + ": ");
                PresentPeg(state.Pegs.ElementAt(i), original);
            }
        }


        private static void PresentPeg(Peg peg, ConsoleColor original)
        {
            for (var i = peg.DiscCount - 1; i >= 0; --i)
            {
                PresentDisc(peg.GetDiscList().ElementAt(i));
            }
            Console.ForegroundColor = original;
            Console.Write('\n');
        }

        private static void PresentDisc(Disc disc)
        {
            Console.ForegroundColor = ToConsoleColor(disc.Color);
            Console.Write(ToBase62(disc.Size));
        }
    }
}
