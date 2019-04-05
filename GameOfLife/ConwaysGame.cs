using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public struct ConwaysGame
    {
        public Grid<CellState> LastGen;
        public Grid<CellState> CurrentGen;

        public ConwaysGame(Grid<CellState> seed) =>
            CurrentGen = LastGen = seed;

        public void DoGeneration()
        {
            LastGen = CurrentGen;
            CurrentGen = new Grid<CellState>(LastGen.Rows, LastGen.Columns);
            var me = this;
            
            foreach (var cellPos in LastGen)
            {
                var numLivingNeighbours = LastGen.Neighbours(cellPos)
                    .Where(pos => me.LastGen[pos] == CellState.Alive)
                    .Count();

                if (LastGen[cellPos] == CellState.Alive)
                {
                    var underpopulated = numLivingNeighbours < 2;
                    var overpopulated = numLivingNeighbours > 3;

                    if (underpopulated || overpopulated)
                    {
                        CurrentGen[cellPos] = CellState.Dead;
                    }
                    else
                    {
                        CurrentGen[cellPos] = CellState.Alive;
                    }
                }
                else if (numLivingNeighbours == 3)
                {
                    CurrentGen[cellPos] = CellState.Alive;
                }
                else
                {
                    CurrentGen[cellPos] = CellState.Dead;
                }
            }
        }
    }
}
