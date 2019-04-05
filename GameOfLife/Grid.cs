using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    public struct Grid<T>: IEnumerable<CellPos> where T : struct
    {
        // first row at top, first col at left
        public readonly int Rows, Columns;

        private T[,] cells;

        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            cells = new T[rows, columns];
        }

        public T this[CellPos pos]
        {
            get => cells[pos.Row, pos.Column];
            set => cells[pos.Row, pos.Column] = value;
        }

        public IEnumerable<CellPos> HorizontalNeighbours(CellPos pos)
        {
            var left = new CellPos(pos.Row, pos.Column - 1);
            var right = new CellPos(pos.Row, pos.Column + 1);
            if (ContainsPos(left))
                yield return left;

            if (ContainsPos(right))
                yield return right;
        }
        public IEnumerable<CellPos> VerticalNeighbours(CellPos pos)
        {
            var up = new CellPos(pos.Row - 1, pos.Column);
            var down = new CellPos(pos.Row + 1, pos.Column);

            if (ContainsPos(up))
                yield return up;

            if (ContainsPos(down))
                yield return down;
        }
        public IEnumerable<CellPos> DiagonalNeighbours(CellPos pos)
        {
            var upLeft = new CellPos(pos.Row - 1, pos.Column - 1);
            var downLeft = new CellPos(pos.Row + 1, pos.Column - 1);
            var upRight = new CellPos(pos.Row - 1, pos.Column + 1);
            var downRight = new CellPos(pos.Row + 1, pos.Column + 1);

            if (ContainsPos(upLeft))
                yield return upLeft;
            if (ContainsPos(downLeft))
                yield return downLeft;
            if (ContainsPos(upRight))
                yield return upRight;
            if (ContainsPos(downRight))
                yield return downRight;
        }
        public IEnumerable<CellPos> Neighbours(CellPos pos) =>
            HorizontalNeighbours(pos)
            .Concat(VerticalNeighbours(pos))
            .Concat(DiagonalNeighbours(pos));

        public bool ContainsPos(CellPos pos)
            => (pos.Row >= 0 && pos.Column >= 0 && pos.Row < Rows && pos.Column < Columns);

        public IEnumerator<CellPos> GetEnumerator()
        {
            for (int row = 0; row < Rows; row++)
                for (int col = 0; col < Columns; col++)
                    yield return new CellPos(row, col);
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                    builder.Append(cells[row, col].ToString().Substring(0, 1));
                builder.Append('\n');
            }

            return builder.ToString();
        }

        public Grid<T> Clone()
        {
            var grid = new Grid<T>(Rows, Columns);
            grid.cells = (T[,]) grid.cells.Clone();
            return grid;
        }
    }
}
