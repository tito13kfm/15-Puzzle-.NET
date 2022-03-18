using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Media;

namespace _15_Puzzle.NET
{
    internal class Program
    {
        static int time = 0;
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            time = time + 1;
            Console.Title = "15 Puzzle - " + time + " seconds elapsed";
        }
        private static void Main(string[] args)
        {
            int size=0;
            bool valid = false;
            GetBoardSize();
            Console.SetWindowSize(size * 10, size * 6);
            Console.SetBufferSize(size * 10, size * 6);
            int[,] board = new int[size, size];
            int[,] winningBoard = new int[size, size];
            int correct;
            int blankRow = 0, blankCol = 0, moves = 0;
            
            //Random number generator initialized
            Random random = new Random();

            Console.Title = "15 Puzzle";
            Console.CursorVisible = false;

            InitBoard();

            void InitBoard()
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        board[i, j] = i * size + j + 1;                  
                        winningBoard[i, j] = i*size+j+1;
                        if (board[i, j] == size * size) { board[i, j] = 0; }
                        if (winningBoard[i, j] == size*size) { winningBoard[i, j] = 0;}
                    }
                }
            }

            RandBoard();
            PrintBoard();

            Console.WriteLine("Press any key to begin.  Good luck");
            Console.ReadKey();
            Console.SetCursorPosition(0, size*4 +5);
            Console.WriteLine("                                  ");

            //Initialize a timer
            Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;


            GetMove();

            //Function to randomize board starting position
            void RandBoard()
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        //Doing size^2 swaps to randomize board
                        int k = random.Next(size);
                        int l = random.Next(size);
                        int tmp = board[i, j];
                        board[i, j] = board[k, l];
                        board[k, l] = tmp;
                    }
                }
                //Check current board to see if it's winnable
                if (CheckValid(board, winningBoard)) { return; }
                else { RandBoard(); }
            }

            bool CheckValid(int[,] tryBoard, int[,] checkBoard)
            {
                
                int[] puzzle=new int[size*size];
                int k = 0;
                int parity = 0;
                int gridWidth = (int)Math.Sqrt(puzzle.Length);
                int row = 0;
                //Put the array in to a 1 dimensional array
                for (int i = 0; i < size; i++)
                {
                    for (int j=0; j < size; j++)
                    {
                        puzzle[k] = board[i, j];
                        k++;
                    }
                }

                //This counts the number of times a tile is greater than the tiles to the right of it, not counting the blank tile.
                for (int i = 0; i < puzzle.Length; i++)
                {
                    if (i % gridWidth == 0)
                    { // advance to next row
                        row++;
                    }
                    if (puzzle[i] == 0)
                    { // the blank tile
                        blankRow = row; // save the row on which encountered
                        continue;
                    }
                    for (int j = i + 1; j < puzzle.Length; j++)
                    {
                        if (puzzle[i] > puzzle[j] && puzzle[j] != 0)
                        {
                            parity++;
                        }
                    }
                }

                //Included code in case this ever becomes more than a 15 puzzle, so that it will work with any board width
                if (gridWidth % 2 == 0)
                { // even grid
                    if (blankRow % 2 == 0)
                    { // blank on odd row; counting from bottom
                        return parity % 2 == 0;
                    }
                    else
                    { // blank on even row; counting from bottom
                        return parity % 2 != 0;
                    }
                }
                else
                { // odd grid
                    return parity % 2 == 0;
                }

          

            }

            int CountCorrect(int[,] tryBoard, int[,] checkBoard)
            {
                correct = 0;
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (tryBoard[i, j] == checkBoard[i, j])
                        {
                            //count up the number of tiles in correct location
                            correct++;
                        }
                    }
                }
                return correct;
            }

            void GetMove()
            {
                AppendBoard();
                int targetRow = 0;
                int targetCol = 0;
            
                //check which key is pressed and set target piece based on location of blank tile
                ConsoleKeyInfo move = Console.ReadKey();
                switch (move.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        targetRow = blankRow + 1;
                        targetCol = blankCol;
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        targetRow = blankRow;
                        targetCol = blankCol + 1;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        targetRow = blankRow - 1;
                        targetCol = blankCol;
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        targetRow = blankRow;
                        targetCol = blankCol - 1;
                        break;
                    default:
                        targetRow = blankRow;
                        targetCol = blankCol;
                        break;
                }

               
                //Check if it's a valid move, and increment move counter
                if (targetRow < 0 || targetCol < 0) { GetMove(); }
                else if (targetRow > size - 1 || targetCol > size - 1) { GetMove(); }
                else { moves++; }
                
                //Swap selected tile with blank tile
                int piece = board[targetRow, targetCol];
                board[blankRow, blankCol] = piece;
                board[targetRow, targetCol] = 0;
                CheckWin();

            }

            void AppendBoard()
            {
                //setup new variable using StringBuilder function
                var sb = new StringBuilder();

                //Append all the labels to the 16 spots on the board.
                for (int i = 0; i < size; i++)
                {
                    sb = new StringBuilder();
                    Console.SetCursorPosition(3, 4 * i + 3);
                    for (int j = 0; j < size; j++)
                    {
                        sb.Append("║  " + displayValue(board[i, j]) + "   ");
                        if (board[i, j] == 0)
                        {
                            blankRow = i;
                            blankCol = j;
                        }

                    }
                    Console.Write(sb);
                }
                
                //Likely better way of doing this, but for now just move the cursor where I want it and write the move count
                Console.SetCursorPosition(13, size*4+4);
                Console.Write(moves);
                return;
            }

            void CheckWin()
            {
                CountCorrect(board, winningBoard);
                if (correct == size*size)
                {
                    Console.Clear();
                    aTimer.Enabled = false;
                    Console.WriteLine("");
                    Console.WriteLine("You Won in " + moves + " moves and  " + time + " seconds!!!!!");
                    Console.WriteLine("");
                    Console.WriteLine("                   Press any key to exit");
                    Console.ReadKey();
                }
                else { GetMove(); }
            }

            void PrintBoard()
            {
                //Function to build and print the empty board.
                Console.Clear();
                string topbar="", middle="",seperator="",bottombar="";
                for (int i = 0; i < size-1; i++)
                {
                    topbar = topbar + "╦═══════";
                    middle = middle + "║       ";
                    seperator = seperator + "╠═══════";
                    bottombar = bottombar + "╚═══════";
                }
                Console.WriteLine("\n   ╔═══════"+ topbar +"╗");
                Console.WriteLine("   ║       " + middle + "║");

                for (int i = 0; i < size; i++)
                {
                    if (i > 0) { Console.WriteLine("   ║       " + middle + "║"); }
                    Console.Write("   ");

                    for (int j = 0; j < size; j++)
                    {
                        Console.Write("║       ");

                    }

                    Console.Write("║\n");
                    Console.WriteLine("   ║       " + middle + "║");

                    if (i < size -1) { Console.WriteLine("   ╠═══════"+seperator+ "╣"); }
                    else { Console.WriteLine("   ╚═══════"+bottombar+"╝"); }

                }

                Console.WriteLine("\nUse WASD or Arrow Keys to move");
                Console.WriteLine("Move Number: " + moves);
                return;
            }

            string displayValue(int i)
            {
                if (i == 0) { return "  "; }
                if (i < 10) { return " " + i.ToString(); }
                return i.ToString();
            }
            void GetBoardSize()
            {
                Console.WriteLine("How many Rows and Columns should I make the board (2 - 7) ?");
                Console.WriteLine("Enter 4 for a standard 15 Puzzle");
                while (!valid)
                {
                    valid = Int32.TryParse(Console.ReadLine(), out size);
                }
                if (size > 7 || size < 2) { Console.WriteLine("Invalid size input, defaulting to 4"); size = 4; }
            }

        }

    }
}
