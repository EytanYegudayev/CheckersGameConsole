using CheckersGameConsole.Model;
using CheckersGameConsole.Model.MiniMaxAlgorithm;
using CheckersGameConsole.Model.MonterCarloTree;
using System;

namespace CheckersGameConsole
{
    class Program
    {
        static void Main()
        {
            // Initialize the game board
            CheckerGame checkerGame = new CheckerGame();
            Console.WriteLine(checkerGame.ToString());

            // Main game loop
            while (!checkerGame.IsGameOver())
            {
                // Player 1's turn
                while(checkerGame.CurrentPlayer == CheckerTool.White && !checkerGame.IsGameOver())
                {
                    Console.WriteLine("White Player turn" + checkerGame.CurrentPlayer);
                    PlayerTurn(checkerGame, checkerGame.CurrentPlayer);
                    Console.WriteLine(checkerGame.ToString());
                }

                // Check if Player 1 wins
                if (checkerGame.IsGameOver())
                {
                    Console.WriteLine("White Player  wins!");
                    break;
                }

                while (checkerGame.CurrentPlayer == CheckerTool.Black && !checkerGame.IsGameOver())
                {
                    // Player 2's turn
                    MiniMaxTreeSearch miniMaxTreeSearch = new MiniMaxTreeSearch(checkerGame);
                    checkerGame = (CheckerGame)miniMaxTreeSearch.MiniMaxSearch(miniMaxTreeSearch.Root, 6, -MiniMaxTreeSearch.INFINITY, MiniMaxTreeSearch.INFINITY, miniMaxTreeSearch.Root.MaximaizeNode).MiniMaxClass;
                    Console.WriteLine(checkerGame.ToString());

                }
                // Check if Player 2 wins
                if (checkerGame.IsGameOver())
                {
                    Console.WriteLine("Black Player wins!");
                    break;
                }
            }

            Console.ReadLine();
        }

        // Function to handle a player's turn
        static void PlayerTurn(CheckerGame gameBoard, CheckerTool playerPiece)
        {
            bool validMove = false;

            while (!validMove)
            {
                Console.WriteLine("Enter the coordinates of the piece you want to move (row column):");
                try
                {
                    string[] input = Console.ReadLine().Split();
                    int row = int.Parse(input[0]);
                    int col = int.Parse(input[1]);

                    Console.WriteLine("Enter the coordinates of the destination (row column):");
                    input = Console.ReadLine().Split();
                    int destRow = int.Parse(input[0]);
                    int destCol = int.Parse(input[1]);
                    List<Move> moves = gameBoard.GetAllPossibleMoveByPlayer(playerPiece);
                    foreach(Move move in moves)
                    {
                        Console.WriteLine(move);
                    }
                    // Check if the move is valid
                    if (gameBoard.IsValidMove(new Move(row, col, destRow, destCol), playerPiece))
                    {
                        gameBoard.MovePiece(row, col, destRow, destCol);
                        validMove = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move. Please try again.");
                    }
                }catch { Console.WriteLine("Invalid Input!"); }

            }
        }
    }
}