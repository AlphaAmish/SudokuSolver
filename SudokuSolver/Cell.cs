using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
    public class Cell
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int Subgrid { get; set; }
        public int? Digit { get; set; }
        public List<int> PossibleDigits { get; set; }
        public bool Verified { get; set; }
    }
}
