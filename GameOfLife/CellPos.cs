using System;

namespace GameOfLife
{
    public struct CellPos : IEquatable<CellPos>, IComparable<CellPos>
    {
        public int Row, Column;

        public CellPos(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public override bool Equals(object obj) =>
            obj is CellPos && Equals((CellPos)obj);

        public bool Equals(CellPos other) =>
            Row == other.Row && Column == other.Column;

        public int CompareTo(CellPos other)
        {
            if (other.Row == Row)
                return 0;
            return Column.CompareTo(other.Column);
        }
        public override string ToString() =>
            $"Row = {Row}, Column = {Column }";
    }
}
