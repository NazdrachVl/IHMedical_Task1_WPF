using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHMedicalTask1WPF
{
    class Cell
    {
        public bool IsQueen { get; set; }

        public CellType Type { get; set; }

        public Cell(CellType type, bool isQueen)
        {
            Type = type;
            IsQueen = isQueen;
        }

        public Cell(CellType type) : this(type, false) { }

        public Cell() : this(CellType.Empty, false) { }

        public Cell Copy()
        {
            return new Cell(Type, IsQueen);
        }
    }

    public enum CellType
    {
        Empty,
        White,
        Black
    }
}
