namespace AdvanceGame
{
    /// <summary>
    /// The General class represents a General piece in the Advance game.
    /// </summary>
    public class General : Piece
    {
        /// <summary>
        /// General Constructor
        /// </summary>
        /// <param name="symbol">Character symbol for the General.</param>
        /// <param name="player">Owner of the piece</param>
        public General(char symbol, string player) : base(symbol, player)
        {
        }

        /// <summary>
        /// Finds the safest move for the General piece if it is in danger.
        /// </summary>
        /// <param name="board">Game board</param>
        /// <param name="startX">Initial X coordinate</param>
        /// <param name="startY">Initial Y coordinate</param>
        /// <returns>Tuple of new coordinates if a safe move is found, null otherwise</returns>
        public Tuple<int, int>? FindSafestMove(Board board, int startX, int startY)
        {
            General tempGeneral = new General(Symbol, Player);
            if (tempGeneral.IsInDanger(board, startX, startY))
            {
                List<Tuple<int, int>> validMoves = GetValidMoves(board, startX, startY);
                foreach (Tuple<int, int> move in validMoves)
                {
                    Board tempBoard = new Board(board);
                    tempBoard.MovePiece(startX, startY, move.Item1, move.Item2);
                    if (tempGeneral.IsInDanger(tempBoard, move.Item1, move.Item2))
                    {
                        continue;
                    }

                    return move;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks all valid moves for the General piece from a given position.
        /// </summary>
        /// <param name="board">Game board</param>
        /// <param name="startX">Initial X coordinate</param>
        /// <param name="startY">Initial Y coordinate</param>
        /// <returns>List of tuples representing all valid moves</returns>
        public override List<Tuple<int, int>> GetValidMoves(Board board, int startX, int startY)
        {
            List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();

            // Define the possible moves
            int[] deltaXOptions = { -1, 0, 1 };
            int[] deltaYOptions = { -1, 0, 1 };

            // Check for all possible moves
            foreach (int deltaX in deltaXOptions)
            {
                foreach (int deltaY in deltaYOptions)
                {
                    if (deltaX != 0 || deltaY != 0)
                    {
                        int endX = startX + deltaX;
                        int endY = startY + deltaY;

                        if (IsValidMove(board, startX, startY, endX, endY))
                        {
                            validMoves.Add(new Tuple<int, int>(endX, endY));
                        }
                    }
                }
            }

            return validMoves;
        }

        /// <summary>
        /// Identifies any threat to the General piece from a given position.
        /// </summary>
        /// <param name="board">Game board</param>
        /// <param name="posX">X coordinate of General's position</param>
        /// <param name="posY">Y coordinate of General's position</param>
        /// <returns>Tuple of coordinates if a threat is found, null otherwise</returns>
        public Tuple<int, int>? IdentifyThreat(Board board, int posX, int posY)
        {
            // Define a list to hold all the piece types
            List<char> pieceTypes = new List<char>
                { 'D', 'd', 'Z', 'z', 'S', 's', 'M', 'm', 'B', 'b', 'J', 'j', 'C', 'c', '#' };

            // Check for all possible threats
            foreach (char pieceType in pieceTypes)
            {
                for (int x = 0; x < board.Rows; x++)
                {
                    for (int y = 0; y < board.Cols; y++)
                    {
                        char piece = board.GetPiece(x, y);

                        if (IsEnemyPiece(piece) && piece == pieceType)
                        {
                            Piece? enemyPiece = PieceFactory.CreatePiece(piece, Player == "white" ? "black" : "white");

                            if (enemyPiece != null && enemyPiece.IsValidMove(board, x, y, posX, posY))
                            {
                                return new Tuple<int, int>(x, y);
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if the General piece is under threat.
        /// </summary>
        /// <param name="board">The game board</param>
        /// <param name="posX">The X-coordinate of the General's position</param>
        /// <param name="posY">The Y-coordinate of the General's position</param>
        /// <returns>True if a threat is found, false otherwise</returns>
        public bool IsInDanger(Board board, int posX, int posY)
        {
            // Define a list to hold all the piece types
            List<char> pieceTypes = new List<char>
                { 'D', 'd', 'Z', 'z', 'S', 's', 'M', 'm', 'B', 'b', 'C', 'c', 'J', 'j', '#' };

            // Check for all possible Danger
            foreach (char pieceType in pieceTypes)
            {
                for (int x = 0; x < board.Rows; x++)
                {
                    for (int y = 0; y < board.Cols; y++)
                    {
                        char piece = board.GetPiece(x, y);

                        if (IsEnemyPiece(piece) && (piece == pieceType))
                        {
                            Piece? enemyPiece = PieceFactory.CreatePiece(piece, Player == "white" ? "black" : "white");

                            if (enemyPiece != null && enemyPiece.IsValidMove(board, x, y, posX, posY))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a proposed move is valid for the General piece.
        /// </summary>
        /// <param name="board">Game board</param>
        /// <param name="startX">Initial X coordinate</param>
        /// <param name="startY">Initial Y coordinate</param>
        /// <param name="endX">Proposed final X coordinate</param>
        /// <param name="endY">Proposed final Y coordinate</param>
        /// <returns>True if move is valid, false otherwise</returns>
        public override bool IsValidMove(Board board, int startX, int startY, int endX, int endY)
        {
            // Check if the destination is within the board bounds
            if (endX < 0 || endX >= board.Rows || endY < 0 || endY >= board.Cols)
            {
                return false;
            }

            int deltaX = Math.Abs(endX - startX);
            int deltaY = Math.Abs(endY - startY);

            if (deltaX > 1 || deltaY > 1)// General can only move one step
            {
                return false;
            }

            char destinationPiece = board.GetPiece(endX, endY);

            if (destinationPiece == '.' || base.IsEnemyPiece(destinationPiece))
            {
                if (!board.IsGeneralInDangerAfterMove(startX, startY, endX, endY, Player))
                {
                    return true;
                }
            }

            return false;
        }
    }
}