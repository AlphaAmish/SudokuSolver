using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver;

namespace SudokuTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            //Row 0
            board.Cells.First(x => x.XPosition == 0 && x.YPosition == 0).Digit = 9;
            board.Cells.First(x => x.XPosition == 0 && x.YPosition == 0).Verified = true;
            board.Cells.First(x => x.XPosition == 3 && x.YPosition == 0).Digit = 6;
            board.Cells.First(x => x.XPosition == 3 && x.YPosition == 0).Verified = true;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 0).Digit = 1;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 0).Verified = true;
            board.Cells.First(x => x.XPosition == 5 && x.YPosition == 0).Digit = 8;
            board.Cells.First(x => x.XPosition == 5 && x.YPosition == 0).Verified = true;
            board.Cells.First(x => x.XPosition == 6 && x.YPosition == 0).Digit = 7;
            board.Cells.First(x => x.XPosition == 6 && x.YPosition == 0).Verified = true;

            //Row 1
            board.Cells.First(x => x.XPosition == 2 && x.YPosition == 1).Digit = 1;
            board.Cells.First(x => x.XPosition == 2 && x.YPosition == 1).Verified = true;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 1).Digit = 2;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 1).Verified = true;

            //Row 2
            board.Cells.First(x => x.XPosition == 0 && x.YPosition == 2).Digit = 6;
            board.Cells.First(x => x.XPosition == 0 && x.YPosition == 2).Verified = true;
            board.Cells.First(x => x.XPosition == 1 && x.YPosition == 2).Digit = 2;
            board.Cells.First(x => x.XPosition == 1 && x.YPosition == 2).Verified = true;
            board.Cells.First(x => x.XPosition == 2 && x.YPosition == 2).Digit = 8;
            board.Cells.First(x => x.XPosition == 2 && x.YPosition == 2).Verified = true;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 2).Digit = 7;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 2).Verified = true;
            board.Cells.First(x => x.XPosition == 8 && x.YPosition == 2).Digit = 5;
            board.Cells.First(x => x.XPosition == 8 && x.YPosition == 2).Verified = true;

            //Row 3
            board.Cells.First(x => x.XPosition == 0 && x.YPosition == 3).Digit = 3;
            board.Cells.First(x => x.XPosition == 0 && x.YPosition == 3).Verified = true;
            board.Cells.First(x => x.XPosition == 2 && x.YPosition == 3).Digit = 6;
            board.Cells.First(x => x.XPosition == 2 && x.YPosition == 3).Verified = true;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 3).Digit = 5;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 3).Verified = true;
            board.Cells.First(x => x.XPosition == 7 && x.YPosition == 3).Digit = 4;
            board.Cells.First(x => x.XPosition == 7 && x.YPosition == 3).Verified = true;

            //Row 4
            board.Cells.First(x => x.XPosition == 0 && x.YPosition == 4).Digit = 5;
            board.Cells.First(x => x.XPosition == 0 && x.YPosition == 4).Verified = true;
            board.Cells.First(x => x.XPosition == 2 && x.YPosition == 4).Digit = 9;
            board.Cells.First(x => x.XPosition == 2 && x.YPosition == 4).Verified = true;
            board.Cells.First(x => x.XPosition == 6 && x.YPosition == 4).Digit = 1;
            board.Cells.First(x => x.XPosition == 6 && x.YPosition == 4).Verified = true;
            board.Cells.First(x => x.XPosition == 8 && x.YPosition == 4).Digit = 6;
            board.Cells.First(x => x.XPosition == 8 && x.YPosition == 4).Verified = true;

            //Row 5
            board.Cells.First(x => x.XPosition == 1 && x.YPosition == 5).Digit = 4;
            board.Cells.First(x => x.XPosition == 1 && x.YPosition == 5).Verified = true;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 5).Digit = 8;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 5).Verified = true;
            board.Cells.First(x => x.XPosition == 6 && x.YPosition == 5).Digit = 5;
            board.Cells.First(x => x.XPosition == 6 && x.YPosition == 5).Verified = true;
            board.Cells.First(x => x.XPosition == 8 && x.YPosition == 5).Digit = 9;
            board.Cells.First(x => x.XPosition == 8 && x.YPosition == 5).Verified = true;

            //Row 6
            board.Cells.First(x => x.XPosition == 0 && x.YPosition == 6).Digit = 4;
            board.Cells.First(x => x.XPosition == 0 && x.YPosition == 6).Verified = true;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 6).Digit = 6;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 6).Verified = true;
            board.Cells.First(x => x.XPosition == 6 && x.YPosition == 6).Digit = 3;
            board.Cells.First(x => x.XPosition == 6 && x.YPosition == 6).Verified = true;
            board.Cells.First(x => x.XPosition == 7 && x.YPosition == 6).Digit = 5;
            board.Cells.First(x => x.XPosition == 7 && x.YPosition == 6).Verified = true;
            board.Cells.First(x => x.XPosition == 8 && x.YPosition == 6).Digit = 1;
            board.Cells.First(x => x.XPosition == 8 && x.YPosition == 6).Verified = true;

            //Row 7
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 7).Digit = 4;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 7).Verified = true;
            board.Cells.First(x => x.XPosition == 6 && x.YPosition == 7).Digit = 6;
            board.Cells.First(x => x.XPosition == 6 && x.YPosition == 7).Verified = true;

            //Row 8
            board.Cells.First(x => x.XPosition == 2 && x.YPosition == 8).Digit = 3;
            board.Cells.First(x => x.XPosition == 2 && x.YPosition == 8).Verified = true;
            board.Cells.First(x => x.XPosition == 3 && x.YPosition == 8).Digit = 1;
            board.Cells.First(x => x.XPosition == 3 && x.YPosition == 8).Verified = true;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 8).Digit = 9;
            board.Cells.First(x => x.XPosition == 4 && x.YPosition == 8).Verified = true;
            board.Cells.First(x => x.XPosition == 5 && x.YPosition == 8).Digit = 5;
            board.Cells.First(x => x.XPosition == 5 && x.YPosition == 8).Verified = true;
            board.Cells.First(x => x.XPosition == 8 && x.YPosition == 8).Digit = 8;
            board.Cells.First(x => x.XPosition == 8 && x.YPosition == 8).Verified = true;

            PrintBoard(board);

            SudokuDriver sudokuDriver = new SudokuDriver()
            {
                Board = board
            };

            sudokuDriver.Solve();
        }

        public static void PrintBoard(Board board)
        {
            for (int i = 8; i >= 0; i--)
            {
                List<Cell> cells = board.Cells.FindAll(c => c.YPosition == i);
                foreach (Cell cell in cells)
                {
                    if (cell.Digit.HasValue)
                    {
                        Console.Write(" " + cell.Digit.Value.ToString() + " ");
                    }
                    else
                    {
                        Console.Write(" - ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("===========================");
            Console.WriteLine();
        }
    }
}
