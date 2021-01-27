using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
    public class Board
    {
        public List<Cell> Cells { get; set; }
        public bool AllCellsFilled
        {
            get
            {
                foreach (Cell cell in Cells)
                {
                    if (!cell.Digit.HasValue)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public Board(bool? keepEmpty = null) 
        {
            if (keepEmpty.HasValue && keepEmpty.Value == true)
            {
                Cells = new List<Cell>();
                return;
            }
            else
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
}
