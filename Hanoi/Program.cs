using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hanoi
{
    class Program
    {
        static void Main1(string[] args)
        {
            Console.WriteLine("Universal Hanoi Problem Solver");
            Console.WriteLine("Bartosz Jedrasik");
            Console.WriteLine("v. 1.0\n");

            BuildHanoiProblem();
        }

        static void Main2(string[] args)
        {
            GameState initial = new GameState();
            initial.AddPegs(3);
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(3, "yellow"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(3, "green"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "yellow"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "green"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "yellow"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "green"));

            GameState desired = new GameState();
            desired.AddPegs(3);
            desired.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(3, "yellow"));
            desired.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "yellow"));
            desired.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "yellow"));
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(3, "green"));
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(2, "green"));
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(1, "green"));

            VisualizeSolution(initial, desired);
        }

        static void Main3(string[] args)
        {
            GameState initial = new GameState();
            initial.AddPegs(5);
            initial.Pegs.ElementAtOrDefault(3).PushDisc(new Disc(12, "blue"));
            initial.Pegs.ElementAtOrDefault(4).PushDisc(new Disc(11, "red"));
            initial.Pegs.ElementAtOrDefault(4).PushDisc(new Disc(9, "red"));
            initial.Pegs.ElementAtOrDefault(3).PushDisc(new Disc(6, "blue"));
            initial.Pegs.ElementAtOrDefault(3).PushDisc(new Disc(5, "blue"));
            initial.Pegs.ElementAtOrDefault(4).PushDisc(new Disc(4, "red"));
            initial.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(2, "yellow"));
            initial.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(1, "yellow"));

            GameState desired = new GameState();
            desired.AddPegs(5);
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(12, "blue"));
            desired.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(11, "red"));
            desired.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(9, "red"));
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(6, "blue"));
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(5, "blue"));
            desired.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(4, "red"));
            desired.Pegs.ElementAtOrDefault(3).PushDisc(new Disc(2, "yellow"));
            desired.Pegs.ElementAtOrDefault(3).PushDisc(new Disc(1, "yellow"));

            VisualizeSolution(initial, desired);
        }

        private static void VisualizeSolution(GameState initial, GameState desired)
        {
            GameState visualization = new GameState(initial, true);

            Console.Clear();
            Console.WriteLine("Total moves: 0\n\n");
            ConsolePresenter.PresentState(visualization);
            Console.WriteLine("\nGoal:\n");
            ConsolePresenter.PresentState(desired);
            Console.WriteLine("\nReady?");
            Console.ReadKey(true);

            Console.WriteLine("Calculating...");
            string solution = GameState.GetSequence(visualization, desired);
            int pegNoDigits = (int)Math.Floor(Math.Log10(visualization.Pegs.Count) + 1);
            int a = pegNoDigits - 1, b = a + 2, moveCounter = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Total moves: " + moveCounter + "\n\n");
                ConsolePresenter.PresentState(visualization);
                Console.WriteLine("\nGoal:\n");
                ConsolePresenter.PresentState(desired);
                try 
                {
                    ++moveCounter;
                    visualization.Pegs.ElementAt(int.Parse(solution[a].ToString()) - 1).MoveDisc(visualization.Pegs.ElementAt(int.Parse(solution[b].ToString()) - 1));
                    solution = solution.Substring(5);
                }
                catch
                {
                    break;
                }
                Thread.Sleep(300);
            }

            Console.WriteLine("\nDone! Press any key to continue.");
            Console.ReadKey(true);
        }

        private static void BuildHanoiProblem()
        {
            GameState initial = new GameState();
            GameState desired = new GameState();
            int pegCount;
            string input;
            do
            {
                Console.WriteLine("How many pegs does the problem have?");
            }
            while (!int.TryParse((input = Console.ReadLine()), out pegCount));
            initial.AddPegs(pegCount);
            desired.AddPegs(pegCount);
            input = "";
            bool finished = false;

            while (!finished)
            {
                int action;
                do
                {
                Console.Clear();
                Console.WriteLine("1. Initial state: ");
                ConsolePresenter.PresentState(initial);
                Console.WriteLine("\n2. Desired state: ");
                ConsolePresenter.PresentState(desired);
                Console.WriteLine("\n3. Visualize solution");
                Console.WriteLine("\n4. Save solution to file");
                Console.WriteLine("\n5. Quit");
                Console.WriteLine("\nWhich state to modify?");
                }
                while (!int.TryParse((input = Console.ReadLine()), out action) && action >= 1 && action <= 2);

                switch (action)
                { 
                    case 1:
                        initial = ManipulateState(initial);
                        break;
                    case 2:
                        desired = ManipulateState(desired);
                        break;
                    case 3:
                        if (initial.SameParametersAs(desired))
                        {
                            VisualizeSolution(initial, desired);
                        }
                        else
                        {
                            Console.WriteLine("\nImpossible, given states have conflicting parameters!");
                        }
                        break;
                    case 4:
                        if (initial.SameParametersAs(desired))
                        {
                            SaveToFile(initial, desired);
                        }
                        else
                        {
                            Console.WriteLine("\nImpossible, given states have conflicting parameters!");
                        }
                        break;
                    case 5:
                        return;
                }
            }
        }

        private static void SaveToFile(GameState initial, GameState desired)
        {
            string solution = GameState.GetSequence(initial, desired);
            StringBuilder output = new StringBuilder();
            output.AppendLine(initial.GetAllDiscs().Count.ToString());
            output.AppendLine(initial.Pegs.Count.ToString());
            output.Append(solution);

            StreamWriter file = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\hanoi.txt");
            file.WriteLine(output.ToString());
            file.Close();

            Console.WriteLine("\nDone saving to your desktop as hanoi.txt!");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
        }

        private static GameState ManipulateState(GameState state)
        {
            string input;
            int pegToModify;
            do
            {
                Console.WriteLine("\nWhich peg to modify?");
            }
            while (!int.TryParse((input = Console.ReadLine()), out pegToModify) && pegToModify < state.Pegs.Count);
            int operation;
            do
            {
                Console.WriteLine("\n1. Add peg");
                Console.WriteLine("2. Remove peg");
            }
            while (!int.TryParse((input = Console.ReadLine()), out operation) && operation >= 1 && operation <= 2);
            if (operation == 1)
            {
                int discSize;
                do
                {
                    Console.WriteLine("How big should the disc be?");
                }
                while (!int.TryParse(input = Console.ReadLine(), out discSize));
                Console.WriteLine("Designate the color of the disc (won't affect how it is displayed)");
                string discColor = Console.ReadLine();
                try
                {
                    state.Pegs.ElementAt(pegToModify).PushDisc(new Disc(discSize, discColor));
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
            if (operation == 2)
            {
                state.Pegs.ElementAt(pegToModify).Stack.Pop();
            }
            return state;
        }
    }
}
