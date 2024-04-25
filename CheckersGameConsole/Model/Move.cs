using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGameConsole.Model
{
    public class Move
    {
        public int FromRow { get; set; }
        public int ToRow { get; set; }
        public int FromColumn { get; set; }
        public int ToColumn { get; set;}

        public Move(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            FromRow = fromRow;
            FromRow = fromRow;
            ToRow = toRow;
            FromColumn = fromColumn;
            ToColumn = toColumn;
        }

        public override string ToString()
        {
            return $"{FromRow} {FromColumn} {ToRow} {ToColumn}";
        }

        public override bool Equals(object? obj)
        {
            if(obj == null || !(obj is Move))
                return false;
            Move move = (Move)obj;
            return move.FromRow == FromRow && move.FromColumn == FromColumn && move.ToRow == ToRow && move.ToColumn == ToColumn;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ToRow, ToColumn, FromRow, FromColumn).GetHashCode();    
        }
    }
}
