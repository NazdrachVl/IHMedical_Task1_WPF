using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHMedicalTask1WPF
{
    class CellPosition
    {
        public int X { get; set; }
        public int Y { get; set; }

        public CellPosition() : this(0, 0) { }

        public CellPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public CellPosition Copy()
        {
            return new CellPosition(X, Y);
        }
    }
}
