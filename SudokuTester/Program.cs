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
            string input = "";
            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine("Please enter row:");
                input = Console.ReadLine();
                for (int j = 0; j < input.Length; j++)
                {
                    if (input[j] != ' ')
                    {
                        board.Cells.First(x => x.XPosition == j && x.YPosition == i).Digit = int.Parse(input[j].ToString());
                        board.Cells.First(x => x.XPosition == j && x.YPosition == i).Verified = true;
                    }
                }
            }
            PrintBoard(board);

            SudokuDriver sudokuDriver = new SudokuDriver()
            {
                Board = board
            };

            sudokuDriver.Solve();

            PrintBoard(board);
            Console.WriteLine("Please select any key to exit.");
            Console.ReadKey();
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
