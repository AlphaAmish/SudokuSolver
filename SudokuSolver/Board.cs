using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
    public class Board
    {
        public List<Cell> Cells { get; set; }

        public Board() 
        {
            Cells = new List<Cell>();
            int startRow = 0;
            int endRow = 3;
            int startColumn = 0;
            int endColumn = 3;

            for (int subgrid = 0; subgrid < 9; subgrid++)
            {
                for (int row = startRow; row < endRow; row++)
                {
                    for (int column = startColumn; column < endColumn; column++)
                    {
                        Cells.Add(new Cell()
                        {
                            XPosition = column,
                            YPosition = row,
                            Subgrid = subgrid,
                            PossibleDigits = new List<int>(),
                            Verified = false
                        });
                    }
                }
                if (subgrid == 0 || 
                    subgrid == 1 || 
                    subgrid == 3 || 
                    subgrid == 4 || 
                    subgrid == 6 || 
                    subgrid == 7
                    )
                {
                    startColumn += 3;
                    endColumn += 3;
                }
                else if (subgrid == 2 || subgrid == 5)
                {
                    startColumn = 0;
                    endColumn = 3;
                    startRow += 3;
                    endRow += 3;
                }            
            }
        }
    }
}
