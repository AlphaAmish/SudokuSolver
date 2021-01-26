using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SudokuSolver
{
    public class SudokuDriver
    {
        public Board Board { get; set; }

        private readonly IReadOnlyCollection<int> RequiredDigits = new List<int>()
        {
            1,2,3,4,5,6,7,8,9
        }.AsReadOnly();

        public void Solve()
        {
            while (!Board.AllCellsVerified)
            {
                if (Board.AllCellsFilled)
                {
                    //See if puzzle is solved correctly
                }
                else
                {
                    int? rowMissingOneDigit = RowMissingOneDigit();
                    if (rowMissingOneDigit != null && rowMissingOneDigit.HasValue)
                    {
                        SolveRow(rowMissingOneDigit.Value);
                    }
                    else
                    {
                        int? columnMissingOneDigit = ColumnMissingOneDigit();
                        if (columnMissingOneDigit != null && columnMissingOneDigit.HasValue)
                        {
                            SolveColumn(columnMissingOneDigit.Value);
                        }
                        else
                        {
                            int? subgridMissingOneDigit = SubgridMissingOneDigit();
                            if (subgridMissingOneDigit != null && columnMissingOneDigit.HasValue)
                            {
                                SolveSubgrid(subgridMissingOneDigit.Value);
                            }
                            else
                            {
                                TrySolvingAnyBoardPiece();
                            }
                        }
                    }
                }
            }
        }
        private int? RowMissingOneDigit()
        {
            int? rowMissingOneDigit = null;
            int missingDigits = 0;

            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    if (!Board.Cells.Single(c => c.XPosition == column && c.YPosition == row).Digit.HasValue)
                    {
                        missingDigits++;
                    }
                }
                if (missingDigits == 1)
                {
                    rowMissingOneDigit = row;
                    break;
                }
                else
                {
                    missingDigits = 0;
                }
            }

            return rowMissingOneDigit;
        }

        private int? ColumnMissingOneDigit()
        {
            int? columnMissingOneDigit = null;
            int missingDigits = 0;

            for (int column = 0; column < 9; column++)
            {
                for (int row = 0; row < 9; row++)
                {
                    if (!Board.Cells.Single(c => c.XPosition == column && c.YPosition == row).Digit.HasValue)
                    {
                        missingDigits++;
                    }
                }
                if (missingDigits == 1)
                {
                    columnMissingOneDigit = column;
                    break;
                }
                else
                {
                    missingDigits = 0;
                }
            }

            return columnMissingOneDigit;
        }

        private int? SubgridMissingOneDigit()
        {
            int? subgridMissingOneDigit = null;
            int missingDigits = 0;
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
                        if (!Board.Cells.Single(c => c.XPosition == column && c.YPosition == row).Digit.HasValue)
                        {
                            missingDigits++;
                        }
                    }
                }
                if (missingDigits == 1)
                {
                    subgridMissingOneDigit = subgrid;
                    break;
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
            return subgridMissingOneDigit;
        }

        private void SolveRow(int row)
        {
            List<Cell> rowCells = Board.Cells.FindAll(c => c.YPosition == row);
            List<int> rowDigits = (from cell in rowCells
                                   where cell.Digit.HasValue
                                   select cell.Digit.Value).ToList();
            List<int> missingDigits = RequiredDigits.Except(rowDigits).ToList();
            List<int> columnDigits = new List<int>();
            List<int> subgridDigits = new List<int>();

            foreach (Cell cell in rowCells)
            {
                if (!cell.Digit.HasValue)
                {
                    columnDigits = ColumnDigits(cell);
                    subgridDigits = SubgridDigits(cell);
                    foreach (int missingDigit in missingDigits)
                    {
                        if (!columnDigits.Contains(missingDigit) && 
                            !subgridDigits.Contains(missingDigit) &&
                            !cell.PossibleDigits.Contains(missingDigit))
                        {
                            cell.PossibleDigits.Add(missingDigit);
                        }
                    }
                }
            }

            bool solvedOne = false;
            do
            {
                solvedOne = VerifiyPossibleDigits(rowCells, missingDigits);
            } while (solvedOne); 
        }

        private void SolveColumn(int column)
        {
            List<Cell> columnCells = Board.Cells.FindAll(c => c.XPosition == column);
            List<int> columnDigits = (from cell in columnCells
                                      where cell.Digit.HasValue
                                      select cell.Digit.Value).ToList();
            List<int> missingDigits = RequiredDigits.Except(columnDigits).ToList();
            List<int> rowDigits = new List<int>();
            List<int> subgridDigits = new List<int>();

            foreach (Cell cell in columnCells)
            {
                if (!cell.Digit.HasValue)
                {
                    rowDigits = RowDigits(cell);
                    subgridDigits = SubgridDigits(cell);
                    foreach (int missingDigit in missingDigits)
                    {
                        if (!rowDigits.Contains(missingDigit) && 
                            !subgridDigits.Contains(missingDigit) &&
                            !cell.PossibleDigits.Contains(missingDigit))
                        {
                            cell.PossibleDigits.Add(missingDigit);
                        }
                    }
                }
            }

            bool solvedOne = false;
            do
            {
                solvedOne = VerifiyPossibleDigits(columnCells, missingDigits);
            } while (solvedOne);
        }

        private void SolveSubgrid(int subgrid)
        {
            List<Cell> subgridCells = Board.Cells.FindAll(c => c.Subgrid == subgrid);
            List<int> subgridDigits = (from cell in subgridCells
                                       where cell.Digit.HasValue
                                       select cell.Digit.Value).ToList();
            List<int> missingDigits = RequiredDigits.Except(subgridDigits).ToList();
            List<int> rowDigits = new List<int>();
            List<int> columnDigits = new List<int>();

            foreach (Cell cell in subgridCells)
            {
                if (!cell.Digit.HasValue)
                {
                    rowDigits = RowDigits(cell);
                    columnDigits = ColumnDigits(cell);
                    foreach (int missingDigit in missingDigits)
                    {
                        if (!rowDigits.Contains(missingDigit) && 
                            !columnDigits.Contains(missingDigit) &&
                            !cell.PossibleDigits.Contains(missingDigit))
                        {
                            cell.PossibleDigits.Add(missingDigit);
                        }
                    }
                }
            }

            bool solvedOne = false;
            do
            {
                solvedOne = VerifiyPossibleDigits(subgridCells, missingDigits);
            } while (solvedOne);
        }

        private List<int> SubgridDigits(Cell cell)
        {
            return (from c in Board.Cells
                    where c.Subgrid == cell.Subgrid
                    && c.Digit.HasValue
                    select c.Digit.Value).ToList();
        }
        private List<int> ColumnDigits(Cell cell)
        {
            return (from c in Board.Cells
                    where c.XPosition == cell.XPosition
                    && c.Digit.HasValue
                    select c.Digit.Value).ToList();
        }
        private List<int> RowDigits(Cell cell)
        {
            return (from c in Board.Cells
                    where c.YPosition == cell.YPosition
                    && c.Digit.HasValue
                    select c.Digit.Value).ToList();
        }

        private void ClearPossibleDigits(Cell cell, int digit)
        {
            List<Cell> rowCells = Board.Cells.FindAll(c => c.YPosition == cell.YPosition);
            List<Cell> columnCells = Board.Cells.FindAll(c => c.XPosition == cell.XPosition);
            List<Cell> subgridCells = Board.Cells.FindAll(c => c.Subgrid == cell.Subgrid);
            foreach (Cell c in rowCells)
            {
                c.PossibleDigits.Remove(digit);
            }
            foreach (Cell c in columnCells)
            {
                c.PossibleDigits.Remove(digit);
            }
            foreach (Cell c in subgridCells)
            {
                c.PossibleDigits.Remove(digit);
            }
            cell.PossibleDigits.Clear();
        }

        private bool VerifiyPossibleDigits(List<Cell> cellList, List<int> missingDigits)
        {
            //Find cells that only has one digit in its list of possible digits
            foreach (Cell cell in cellList)
            {
                if (cell.PossibleDigits.Count == 1)
                {
                    int digit = cell.PossibleDigits[0];
                    cell.Digit = digit;
                    cell.Verified = true;
                    ClearPossibleDigits(cell, digit);
                    missingDigits.Remove(digit);
                    return true;
                }
            }

            //Find a row or columns missing digit that is in only one cells list of possible digits
            foreach (int missingDigit in missingDigits)
            {
                List<Cell> cells = cellList.FindAll(c => c.PossibleDigits.Contains(missingDigit));
                if (cells != null && cells.Count == 1)
                {
                    cells[0].Digit = missingDigit;
                    cells[0].Verified = true;
                    ClearPossibleDigits(cells[0], missingDigit);
                    missingDigits.Remove(missingDigit);
                    return true;
                }
            }
            
            return false;
        }

        private void TrySolvingAnyBoardPiece()
        {
            for (int row = 0; row < 9; row++)
            {
                SolveRow(row);
            }

            for (int column = 0; column < 9; column++)
            {
                SolveColumn(column);
            }

            for (int subgrid = 0; subgrid < 9; subgrid++)
            {
                SolveSubgrid(subgrid);
            }
        }
    }
}
