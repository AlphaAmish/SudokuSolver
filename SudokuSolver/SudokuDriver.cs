using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SudokuSolver
{
    public class SudokuDriver
    {
        public Board Board { get; set; }
        private Board BoardBeforeGuessing { get; set; }

        private readonly IReadOnlyCollection<int> RequiredDigits = new List<int>()
        {
            1,2,3,4,5,6,7,8,9
        }.AsReadOnly();

        public void Solve()
        {
            while (!Board.AllCellsFilled)
            {
                int? rowMissingOneDigit = PieceMissingDigit(BoardPiece.Row, 1);
                if (rowMissingOneDigit != null && rowMissingOneDigit.HasValue)
                {
                    SolvePiece(BoardPiece.Row, rowMissingOneDigit.Value);
                }
                else
                {
                    int? columnMissingOneDigit = PieceMissingDigit(BoardPiece.Column, 1);
                    if (columnMissingOneDigit != null && columnMissingOneDigit.HasValue)
                    {
                        SolvePiece(BoardPiece.Column, columnMissingOneDigit.Value);
                    }
                    else
                    {
                        int? subgridMissingOneDigit = PieceMissingDigit(BoardPiece.Subgrid, 1);
                        if (subgridMissingOneDigit != null && columnMissingOneDigit.HasValue)
                        {
                            SolvePiece(BoardPiece.Subgrid, subgridMissingOneDigit.Value);
                        }
                        else
                        {
                            bool solvedOne = TrySolvingAnyBoardPiece();
                            if (!solvedOne)
                            {
                                CopyBoard();

                                if (TrySolvingBoardWithAGuess())
                                {
                                    return;
                                }
                                else
                                {
                                    //Guess failed
                                    Board = BoardBeforeGuessing;
                                }
                            }
                        }
                    }
                }
            }
        }

        private int? PieceMissingDigit(BoardPiece boardPiece, int numberOfMissingDigits)
        {
            int? pieceMissingDigits = null;
            int missingDigits = 0;

            switch (boardPiece)
            {
                case BoardPiece.Row:
                case BoardPiece.Column:
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (boardPiece.Equals(BoardPiece.Row))
                            {
                                if (!Board.Cells.Single(c => c.XPosition == j && c.YPosition == i).Digit.HasValue)
                                {
                                    missingDigits++;
                                }
                            }
                            else if (boardPiece.Equals(BoardPiece.Column))
                            {
                                if (!Board.Cells.Single(c => c.XPosition == i && c.YPosition == j).Digit.HasValue)
                                {
                                    missingDigits++;
                                }
                            }
                        }
                        if (missingDigits == numberOfMissingDigits)
                        {
                            pieceMissingDigits = i;
                            break;
                        }
                        else
                        {
                            missingDigits = 0;
                        }
                    }
                    break;
                case BoardPiece.Subgrid:
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
                        if (missingDigits == numberOfMissingDigits)
                        {
                            pieceMissingDigits = subgrid;
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
                    break;
            }

            return pieceMissingDigits;
        }      

        private bool SolvePiece(BoardPiece boardPiece, int pieceIndex)
        {
            bool solvedAtLeastOne = false;
            List<Cell> cells = new List<Cell>();
             
            
            switch (boardPiece) 
            {
                case BoardPiece.Row:
                    cells = Board.Cells.FindAll(c => c.YPosition == pieceIndex);
                    break;
                case BoardPiece.Column:
                    cells = Board.Cells.FindAll(c => c.XPosition == pieceIndex);
                    break;
                case BoardPiece.Subgrid:
                    cells = Board.Cells.FindAll(c => c.Subgrid == pieceIndex);
                    break;
            }
            List<int> cellDigits = (from cell in cells
                                    where cell.Digit.HasValue
                                    select cell.Digit.Value).ToList();
            List<int> missingDigits = RequiredDigits.Except(cellDigits).ToList();
            List<int> otherPiece1Digits = new List<int>();
            List<int> otherPiece2Digits = new List<int>();

            foreach (Cell cell in cells)
            {
                if (!cell.Digit.HasValue)
                {
                    switch (boardPiece)
                    {
                        case BoardPiece.Row:
                            otherPiece1Digits = ColumnDigits(cell);
                            otherPiece2Digits = SubgridDigits(cell);
                            break;
                        case BoardPiece.Column:
                            otherPiece1Digits = RowDigits(cell);
                            otherPiece2Digits = SubgridDigits(cell);
                            break;
                        case BoardPiece.Subgrid:
                            otherPiece1Digits = RowDigits(cell);
                            otherPiece2Digits = ColumnDigits(cell);
                            break;
                    }
                    
                    foreach (int missingDigit in missingDigits)
                    {
                        if (!otherPiece1Digits.Contains(missingDigit) &&
                            !otherPiece2Digits.Contains(missingDigit) &&
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
                if (FillPossibleDigits(cells, missingDigits))
                {
                    solvedOne = true;
                    solvedAtLeastOne = true;
                }
                else
                {
                    solvedOne = false;
                }
            }
            while (solvedOne);

            return solvedAtLeastOne;
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

        private bool FillPossibleDigits(List<Cell> cellList, List<int> missingDigits)
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

        private bool TrySolvingAnyBoardPiece()
        {
            bool solvedAtLeastOne = false;
            for (int row = 0; row < 9; row++)
            {
                if (SolvePiece(BoardPiece.Row, row))
                {
                    solvedAtLeastOne = true;
                }
            }

            for (int column = 0; column < 9; column++)
            {
                if (SolvePiece(BoardPiece.Column ,column))
                {
                    solvedAtLeastOne = true;
                }
            }

            for (int subgrid = 0; subgrid < 9; subgrid++)
            {
                if (SolvePiece(BoardPiece.Subgrid, subgrid))
                {
                    solvedAtLeastOne = true;
                }
            }

            return solvedAtLeastOne;
        }

        private bool TrySolvingBoardWithAGuess()
        {
            bool solved = false;

            int? rowMissingTwoDigits = PieceMissingDigit(BoardPiece.Row, 2);
            if (rowMissingTwoDigits != null && rowMissingTwoDigits.HasValue)
            {            
                List<Cell> cellsMissingDigits = Board.Cells.FindAll(c => c.YPosition == rowMissingTwoDigits.Value && !c.Digit.HasValue);
                solved = SetGuessAndSolve(BoardPiece.Row, cellsMissingDigits, rowMissingTwoDigits.Value);
            }
            else
            {
                int? columnMissingTwoDigits = PieceMissingDigit(BoardPiece.Column, 2);
                if (columnMissingTwoDigits != null && columnMissingTwoDigits.HasValue)
                {
                    List<Cell> cellsMissingDigits = Board.Cells.FindAll(c => c.XPosition == columnMissingTwoDigits.Value && !c.Digit.HasValue);
                    solved = SetGuessAndSolve(BoardPiece.Column, cellsMissingDigits, columnMissingTwoDigits.Value);
                }
                else
                {
                    int? subgridMissingTwoDigits = PieceMissingDigit(BoardPiece.Subgrid, 2);
                    if (subgridMissingTwoDigits != null && subgridMissingTwoDigits.HasValue)
                    {
                        List<Cell> cellsMissingDigits = Board.Cells.FindAll(c => c.Subgrid == subgridMissingTwoDigits.Value && !c.Digit.HasValue);
                        SetGuessAndSolve(BoardPiece.Subgrid, cellsMissingDigits, subgridMissingTwoDigits.Value);
                    }
                }
            }
            return solved;
        }

        private bool SetGuessAndSolve(BoardPiece boardPiece, List<Cell> cellsMissingDigits, int pieceIndex)
        {
            bool solved = false;
            int firstGuess = 0;
            firstGuess = cellsMissingDigits[0].PossibleDigits[0];
            cellsMissingDigits[0].Digit = firstGuess;
            ClearPossibleDigits(cellsMissingDigits[0], firstGuess);
            bool solvedOne = false;
            do
            {
                TrySolvingAnyBoardPiece();
            }
            while (solvedOne);

            if (!Board.AllCellsFilled)
            {
                CopyBoardBeforeGuessing();
                int secondGuess = 0;
                switch (boardPiece)
                {
                    case BoardPiece.Row:
                        cellsMissingDigits = Board.Cells.FindAll(c => c.YPosition == pieceIndex && !c.Digit.HasValue);
                        break;
                    case BoardPiece.Column:
                        cellsMissingDigits = Board.Cells.FindAll(c => c.XPosition == pieceIndex && !c.Digit.HasValue);
                        break;
                    case BoardPiece.Subgrid:
                        cellsMissingDigits = Board.Cells.FindAll(c => c.Subgrid == pieceIndex && !c.Digit.HasValue);
                        break;
                }
                
                secondGuess = cellsMissingDigits[0].PossibleDigits[1];
                cellsMissingDigits[0].Digit = secondGuess;
                ClearPossibleDigits(cellsMissingDigits[0], secondGuess);
                solvedOne = false;
                do
                {
                    TrySolvingAnyBoardPiece();
                }
                while (solvedOne);
                if (Board.AllCellsFilled)
                {
                    solved = true;
                }
            }
            else
            {
                solved = true;
            }
            return solved;
        }

        private void CopyBoard()
        {
            BoardBeforeGuessing = new Board(keepEmpty: true);
            foreach (Cell cell in Board.Cells)
            {
                Cell copyCell = new Cell()
                {
                    Digit = cell.Digit,
                    XPosition = cell.XPosition,
                    YPosition = cell.YPosition,
                    Subgrid = cell.Subgrid,
                    PossibleDigits = cell.PossibleDigits.ToList(),
                    Verified = cell.Verified
                };
                BoardBeforeGuessing.Cells.Add(copyCell);
            }
        }

        private void CopyBoardBeforeGuessing()
        {
            Board = new Board(keepEmpty: true);
            foreach (Cell cell in BoardBeforeGuessing.Cells)
            {
                Cell copyCell = new Cell()
                {
                    Digit = cell.Digit,
                    XPosition = cell.XPosition,
                    YPosition = cell.YPosition,
                    Subgrid = cell.Subgrid,
                    PossibleDigits = cell.PossibleDigits.ToList(),
                    Verified = cell.Verified
                };
                Board.Cells.Add(copyCell);
            }
        }

        private enum BoardPiece
        {
            Row,
            Column,
            Subgrid
        }
    }
}