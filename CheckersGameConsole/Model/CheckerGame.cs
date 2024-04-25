using CheckersGameConsole.Model.MiniMaxAlgorithm;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace CheckersGameConsole.Model
{
    public class CheckerGame: MiniMaxClass
    {
        public static readonly int BOARD_SIZE = 8;
        private static readonly char BLACK_KING_CHAR = 'K';
        private static readonly char WHITE_KING_CHAR = 'Q';
        private static readonly char BLACK_CHAR = 'X';
        private static readonly char WHITE_CHAR = 'O';
        private static readonly char NONE_CHAR = ' ';
        private static readonly char SEPARATE_CHAR = '|';
        public readonly CheckerTool[,] GameBoard;
        public CheckerTool CurrentPlayer;
        public bool IsToolEat;
        public int ColEat;
        public int RowEat;

        public CheckerGame()
        {
            GameBoard = new CheckerTool[BOARD_SIZE, BOARD_SIZE];
            CurrentPlayer = CheckerTool.White;
            InitializeGameBoard();
            // if tool will eat enemy tool, the value will change to true and the index of this tool
            IsToolEat = false;
            ColEat = -1;
            RowEat = -1;
        }

        public CheckerGame(CheckerGame game)
        {
            GameBoard = (CheckerTool[,])game.GameBoard.Clone();
            CurrentPlayer = game.CurrentPlayer;
            IsToolEat = game.IsToolEat;
            ColEat = game.ColEat;
            RowEat = game.RowEat;
        }

        private void InitializeGameBoard()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    // Set black squares
                    if ((i + j) % 2 == 0 || i == 3 || i == 4)
                        GameBoard[i, j] = CheckerTool.None;
                    else
                    {
                        // Set pieces for black player 
                        if (i < 3)
                            GameBoard[i, j] = CheckerTool.Black;
                        // Set pieces for white player 
                        else if (i > 4)
                            GameBoard[i, j] = CheckerTool.White;
                    }
                }
            }
        }

        private bool IsValidIndex(int row, int col)
        {
            if (row < 0 || row >= BOARD_SIZE || col >= BOARD_SIZE || col < 0)
                return false;
            return true;
        }

        // Function to check if a move is valid
        public bool IsValidMove(Move move, CheckerTool playerPiece)
        {
            int row = move.FromRow;
            int col = move.FromColumn;
            int destRow = move.ToRow;
            int destCol = move.ToColumn;
            if(IsToolEat && !(RowEat == row && ColEat == col))
                return false;
            if (GameBoard[row, col] != CurrentPlayer && !(GameBoard[row, col] == CheckerTool.WhiteKing && CurrentPlayer == CheckerTool.White) && !(GameBoard[row, col] == CheckerTool.BlackKing && CurrentPlayer == CheckerTool.Black) )
                return false;

            // check if one of player's tools can eat enemy tool
            List<Move> indexOfPlayerToolsThatCanEat =  IndexOfPlayersToolsThatCanEat(playerPiece);
            if(indexOfPlayerToolsThatCanEat.Count > 0 )
            {
                //one of player's tools can eat enemy tool
                foreach (Move m in  indexOfPlayerToolsThatCanEat)
                {
                    if(m.FromRow  == row && m.FromColumn == col && m.ToRow == destRow && m.ToColumn == destCol)
                    {
                        // The player eat enemy tool with this move
                        return true;
                    }
                }
                // the player can eat but he is does'nt chose to eat enemy tool
                return false;
            }
            // Check if destination is within board bounds
            if (!IsValidIndex(row, col) || !IsValidIndex(destRow, destCol))
                return false;

            // Check if the destination is empty
            if (GameBoard[destRow, destCol] != CheckerTool.None)
                return false;

            // Check if the piece being moved is owned by the player
            // Cheack if the piece is black player 
            bool isBlackPlayer = playerPiece == CheckerTool.Black;
            // Check if the piece is not connect to the black piece or black king piece
            if (isBlackPlayer)
            {
                if (GameBoard[row, col] != CheckerTool.Black && GameBoard[row, col] != CheckerTool.BlackKing)
                    return false;
            }
            else
            {
                // is white player
                if (GameBoard[row, col] != CheckerTool.White && GameBoard[row, col] != CheckerTool.WhiteKing)
                    return false;
            }

           
            // Check if the piece is moving in the correct direction
            if (GameBoard[row, col] == CheckerTool.Black && destRow <= row)
                return false;
            if (GameBoard[row, col] == CheckerTool.White && destRow >= row)
                return false;

            // Check if the piece can 'eat' enemy piece


            // Check if it's a valid diagonal move (one square)
            if (Math.Abs(destRow - row) != 1 || Math.Abs(destCol - col) != 1)
            {
                // Check if the piece is not try to 'eat' enemy piece
                return IsToolEatEnemyTool(row, col, destRow, destCol);
            }
            
            return true;
        }

        public void MovePiece(int row, int col, int destRow, int destCol)
        {
            bool isEatEnemy = IsToolEatEnemyTool(row, col, destRow, destCol);
            // Move the piece to the destination
            GameBoard[destRow, destCol] = GameBoard[row, col];
            // Check if the piece can 'eat' enemy piece
            if (isEatEnemy)
            {
                // Check if the piece can 'eat' enemy piece
                int enemyRow = Math.Max(destRow, row) - 1;
                int enemyCol = Math.Max(destCol, col) - 1;
                GameBoard[enemyRow, enemyCol] = CheckerTool.None;
                GameBoard[row, col] = CheckerTool.None;
                if (IndexOfPlayersToolsThatCanEat(CurrentPlayer).Count == 0)
                {
                    ChangePlayer();
                    IsToolEat = false;
                    RowEat = -1;
                    ColEat = -1;
                }
                else
                {
                    IsToolEat = true;
                    RowEat = destRow;
                    ColEat = destCol;
                }
            }
            else
            {
                // Move the piece to the destination
                GameBoard[destRow, destCol] = GameBoard[row, col];
                ChangePlayer();
            }
            GameBoard[row, col] = CheckerTool.None;

            // Check if the black piece can be black king
            if (destRow == BOARD_SIZE - 1 && GameBoard[destRow, destCol] == CheckerTool.Black)
                GameBoard[destRow, destCol] = CheckerTool.BlackKing;


            //Check if the black piece can be white king
            if (destRow == 0 && GameBoard[destRow, destCol] == CheckerTool.White)
                GameBoard[destRow, destCol] = CheckerTool.WhiteKing;

        }

        private void ChangePlayer()
        {
            CurrentPlayer = CurrentPlayer == CheckerTool.Black ? CheckerTool.White : CheckerTool.Black;
        }

        public List<Move> GetAllPossibleMoveByPlayer(CheckerTool player)
        {
            List<Move> moves = IndexOfPlayersToolsThatCanEat(player);
            if(moves.Count == 0)
            {
                moves = IndexOfPlayersToolsThatWithoutEating(player);

            }
            return moves;
               

        }

        private List<Move> IndexOfPlayersToolsThatWithoutEating(CheckerTool player)
        {
            bool isBlackPlayer = player == CheckerTool.Black;
            List<Move> possibleMoves = new List<Move>();
            int[,] moves = { };
            CheckerTool checkerTool;
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    checkerTool = GameBoard[i, j];
                    if (isBlackPlayer)
                    {
                        // black player
                        if (checkerTool == CheckerTool.Black)
                        {
                            moves = new int[,] { { 1, -1 }, { 1, 1 } }; // only black player moves

                        }
                        else if (checkerTool == CheckerTool.BlackKing)
                        {
                            moves = new int[,] { { 1, -1 }, { 1, 1 }, { -1, -1 }, { -1, 1 } }; // all possible moves
                        }
                    }
                    else
                    {
                        // white player
                        if (checkerTool == CheckerTool.White)
                        {
                            moves = new int[,] { { -1, -1 }, { -1, 1 } }; // only white player moves
                        }
                        else if (checkerTool == CheckerTool.WhiteKing)
                        {
                            moves = new int[,] { { 1, -1 }, { 1, 1 }, { -1, -1 }, { -1, 1 } }; // all possible moves
                        }
                    }

                    for (int moveIndex = 0; moveIndex < moves.GetLength(0); moveIndex++)
                    {
                        // Accessing the horizontal and vertical movements for each move
                        int rowMovement = moves[moveIndex, 0];
                        int colMovement = moves[moveIndex, 1];
                        
                        if (IsValidIndex(i + rowMovement, j + colMovement))
                        {
                            Move move = new Move(i, j, i + rowMovement, j + colMovement);
                            if (IsValidMove(move, player))
                            {
                                possibleMoves.Add(move);
                                
                            }
                        }
                    }


                }
            }
            return possibleMoves;

        }

        private List<Move> IndexOfPlayersToolsThatCanEat(CheckerTool player)
        {
            
            bool isBlackPlayer = player == CheckerTool.Black;
            List<Move> indexOfEatTool = new List<Move>();
            int[,] moves = { };
            CheckerTool checkerTool;
            for(int i=0; i<BOARD_SIZE; i++)
            {
                for(int j=0; j<BOARD_SIZE; j++)
                {
                    checkerTool = GameBoard[i, j];
                    if (isBlackPlayer)
                    {
                        // black player
                        if (checkerTool == CheckerTool.Black)
                        {
                            moves = new int[,] { { 2, -2 }, { 2, 2 } }; // only black player moves
                            
                        }
                        else if(checkerTool == CheckerTool.BlackKing)
                        {
                            moves = new int[,] { { 2, -2 }, { 2, 2 }, { -2, -2 }, { -2, 2 } }; // all possible moves
                        }
                    }
                    else
                    {
                        // white player
                        if (checkerTool == CheckerTool.White)
                        {
                            moves = new int[,] { { -2, -2 }, { -2, 2 } }; // only white player moves
                        }
                        else if (checkerTool == CheckerTool.WhiteKing)
                        {
                            moves = new int[,] { { 2, -2 }, { 2, 2 }, { -2, -2 }, { -2, 2 } }; // all possible moves
                        }
                    }

                    for (int moveIndex = 0; moveIndex < moves.GetLength(0); moveIndex++)
                    {
                        // Accessing the horizontal and vertical movements for each move
                        int rowMovement = moves[moveIndex, 0];
                        int colMovement = moves[moveIndex, 1];
                        if(IsValidIndex(i + rowMovement, j + colMovement))
                        {
                            if(IsToolEatEnemyTool(i, j, i + rowMovement, j + colMovement))
                            {
                                indexOfEatTool.Add(new Move ( i, j, i + rowMovement, j + colMovement ));
                               
                            }
                        }
                    }


                }
            }

            // return empty list if the player can not eat
            return indexOfEatTool;
        }


        private bool IsToolEatEnemyTool(int row, int col, int destRow, int destCol)
        {
            // Check if the piece is try to 'eat' enemy piece
            if (Math.Abs(destRow - row) == 2 && Math.Abs(destCol - col) == 2)
            {
                // Check if the piece can 'eat' enemy piece
                int enemyRow = Math.Max(destRow, row) - 1;
                int enemyCol = Math.Max(destCol, col) - 1;
                // return false if the piece try to 'eat' nothing or the same tool
                if (GameBoard[row, col] == CheckerTool.None ||  GameBoard[enemyRow, enemyCol] == CheckerTool.None || GameBoard[enemyRow, enemyCol] == GameBoard[row, col] || GameBoard[destRow, destCol] != CheckerTool.None)
                    return false;
                else
                    return true;
            }
            return false;
        }

        public bool IsGameOver()
        {
            // Check if there are any pieces left for both players
            bool whitePlayerExists = false;
            bool blackPlyaerExists = false;

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (GameBoard[i, j] == CheckerTool.White || GameBoard[i, j] == CheckerTool.WhiteKing)
                        whitePlayerExists = true;
                    else if (GameBoard[i, j] == CheckerTool.Black || GameBoard[i, j] == CheckerTool.BlackKing)
                        blackPlyaerExists = true;
                }
            }
            return !whitePlayerExists || !blackPlyaerExists;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("   0 1 2 3 4 5 6 7");
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                stringBuilder.Append(i + " ");
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    stringBuilder.Append(SEPARATE_CHAR);
                    switch(GameBoard[i,j])
                    {
                        case CheckerTool.Black:
                            stringBuilder.Append(BLACK_CHAR);
                            break;
                        case CheckerTool.White:
                            stringBuilder.Append(WHITE_CHAR);
                            break;
                        case CheckerTool.None:
                            stringBuilder.Append(NONE_CHAR);
                            break;
                        case CheckerTool.BlackKing:
                            stringBuilder.Append(BLACK_KING_CHAR);
                            break;
                        case CheckerTool.WhiteKing:
                            stringBuilder.Append(WHITE_KING_CHAR);
                            break;

                    }
                }
                stringBuilder.AppendLine(SEPARATE_CHAR+"");
            }
            return stringBuilder.ToString();
        }

        public int CountPlayerTools(CheckerTool player)
        {
            int countBlack = 0;
            int countWhite = 0;
            
            for(int i=0; i < BOARD_SIZE; i++)
            {
                for( int j=0; j < BOARD_SIZE; j++)
                {
                    switch(player)
                    {
                        case CheckerTool.White:
                            if (GameBoard[i, j] == CheckerTool.White || GameBoard[i, j] == CheckerTool.WhiteKing)
                                countWhite++;
                            break;
                        case CheckerTool.Black:
                            if (GameBoard[i, j] == CheckerTool.Black || GameBoard[i, j] == CheckerTool.BlackKing)
                                countBlack++;
                            break;
                    }
                }
            }
            // if we have more black tool so it will be negative number
            return countWhite - countBlack;
        }

        public List<MiniMaxTreeNode> GetChildren(MiniMaxTreeNode parent)
        {
            List<MiniMaxTreeNode> children = new List<MiniMaxTreeNode>();
            foreach (Move move in GetAllPossibleMoveByPlayer(CurrentPlayer))
            {
                CheckerGame game = new CheckerGame(this);
                game.MovePiece(move.FromRow, move.FromColumn, move.ToRow, move.ToColumn);
                children.Add(new MiniMaxTreeNode(game, parent, maximazieNode: CurrentPlayer == CheckerTool.White));
            }
            return children;
        }

        public int GetEvaluation()
        {
            return CountPlayerTools(CurrentPlayer);
        }

        public bool IsOver()
        {
            return IsGameOver();
        }

        public bool IsMaximaize()
        {
            return CurrentPlayer == CheckerTool.White;
        }
    }
}
