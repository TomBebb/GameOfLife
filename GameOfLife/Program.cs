using System;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{

    class Program
    {
        static void Main(string[] args)
        {
            var rand = new Random();
            var seed = new Grid<CellState>(10, 10);
            // setup blinker
            var centre = new CellPos(seed.Rows / 2, seed.Columns / 2);
            foreach(var pos in seed)
            {
                if (pos.Row == centre.Row && Math.Abs(pos.Column - centre.Column) <= 1)
                {
                    seed[pos] = CellState.Alive;
                }
                else
                {
                    seed[pos] = CellState.Dead;
                }
            }

            var conway = new ConwaysGame(seed);
            while(true)
            {
                Console.WriteLine(conway.CurrentGen);
                conway.DoGeneration();
                Console.WriteLine(conway.CurrentGen);
                Console.ReadLine();
            }
        }
    }
}
