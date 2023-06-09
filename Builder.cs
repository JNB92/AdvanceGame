namespace AdvanceGame
{
    /// <summary>
    /// The Builder class represents a Builder piece in the Advance game.
    /// </summary>
    public class Builder : Piece
    {
        /// <summary>
        /// Builder Constructor
        /// </summary>
        /// <param name="symbol">Character symbol for the Builder.</param>
        /// <param name="player">Owner of the piece</param>
        public Builder(char symbol, string player) : base(symbol, player)
        {
        }

        /// <summary>
        /// Checks all valid moves for the Builder piece from a given position.
        /// </summary>
        /// <param name="board">Game board</param>
        /// <param name="startX">Initial X coordinate</param>
        /// <param name="startY">Initial Y coordinate</param>
        /// <returns>List of tuples representing all valid moves</returns>
        public override List<Tuple<int, int>> GetValidMoves(Board board, int startX, int startY)
        {
            List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();

            int forwardDirection = Player == "white" ? 1 : -1;
            int[] deltaYOptions = new int[] { -1, 0, 1 };

            // Check all possible moves
            for (int i = 0; i < deltaYOptions.Length; i++)
            {
                int endX = startX + forwardDirection;
                int endY = startY + deltaYOptions[i];

                if (IsValidMove(board, startX, startY, endX, endY))
                {
                    validMoves.Add(new Tuple<int, int>(endX, endY));
                }
            }

            return validMoves;
        }

        /// <summary>
        /// Checks if the specified piece is an enemy piece.
        /// </summary>
        /// <param name="piece">The piece to check.</param>
        /// <returns>True if the specified piece is an enemy piece, false otherwise.</returns>
        public new bool IsEnemyPiece(char piece)
        {
            return (Player == "white" && char.IsLower(piece)) || (Player == "black" && char.IsUpper(piece));
        }

        /// <summary>
        /// Checks if a move to a specified location is valid.
        /// </summary>
        /// <param name="board">Current game board.</param>
        /// <param name="startX">The piece's initial x-coordinate.</param>
        /// <param name="startY">The piece's initial y-coordinate.</param>
        /// <param name="endX">The piece's destination x-coordinate.</param>
        /// <param name="endY">The piece's destination y-coordinate.</param>
        /// <returns>True if the move is valid, false otherwise.</returns>
        public override bool IsValidMove(Board board, int startX, int startY, int endX, int endY)
        {
            // Check if the destination is within the board bounds
            if (endX < 0 || endX >= board.Rows || endY < 0 || endY >= board.Cols)
            {
                return false;
            }

            int deltaX = Math.Abs(endX - startX);
            int deltaY = Math.Abs(endY - startY);

            if ((deltaX == 1 && deltaY <= 1) || (deltaX == 0 && deltaY == 1))
            {
                char destinationPiece = board.GetPiece(endX, endY);

                if (destinationPiece == '.' || IsEnemyPiece(destinationPiece))
                {
                    return true;
                }

                if (destinationPiece == '.' && CanBuildWall(board, endX, endY))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a Builder can build a wall on the specified square.
        /// </summary>
        /// <param name="board">Current game board.</param>
        /// <param name="x">The x-coordinate of the square.</param>
        /// <param name="y">The y-coordinate of the square.</param>
        /// <returns>True if a wall can be built on the specified square, false otherwise.</returns>
        private bool CanBuildWall(Board board, int x, int y)
        {
            // Check if the square is unoccupied and doesn't have a wall
            if (board.GetPiece(x, y) != '.' && board.GetPiece(x, y) == '#')
            {
                return false;
            }
            return true;
        }
    }
}