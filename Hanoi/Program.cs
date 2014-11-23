using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hanoi
{
    class Program
    {
        static void Main(string[] args)
        {
            //Na dzien dzisiejszy brak UI. Postaram sie dostarczyc do wtorku.
            //Przykladowe zastosowanie programu:

            int pegAmount = 3;

            GameState initial = new GameState();
            initial.AddPegs(pegAmount);
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(3, "green"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "yellow"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(2, "green"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "yellow"));
            initial.Pegs.ElementAtOrDefault(0).PushDisc(new Disc(1, "green"));

            GameState desired = new GameState();
            desired.AddPegs(pegAmount);
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(2, "yellow"));
            desired.Pegs.ElementAtOrDefault(1).PushDisc(new Disc(1, "yellow"));
            desired.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(3, "green"));
            desired.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(2, "green"));
            desired.Pegs.ElementAtOrDefault(2).PushDisc(new Disc(1, "green"));

            string payoff = GameState.GetSequence(initial, desired);

            StringBuilder output = new StringBuilder();
            output.AppendLine(initial.Pegs.Count().ToString());
            output.AppendLine(pegAmount.ToString());
            output.Append(payoff);

            StreamWriter file = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\hanoi.txt");
            file.WriteLine(output.ToString());

            file.Close();
        }
    }
}
