namespace AdvanceGame
{
    /// <summary>
    /// The Miner class represents a Miner piece in the Advance game.
    /// </summary>
    public class Miner : Piece
    {
        /// <summary>
        /// Miner Constructor
        /// </summary>
        /// <param name="symbol">Character symbol for the Miner.</param>
        /// <param name="player">The player the miner belongs to.</param>
        public Miner(char symbol, string player) : base(symbol, player) { }

        /// <summary>
        /// Checks all valid moves for the Miner piece from a given position.
        /// </summary>
        /// <param name="board">Game board</param>
        /// <param name="startX">Initial X coordinate</param>
        /// <param name="startY">Initial Y coordinate</param>
        /// <returns>List of tuples representing all valid moves</returns>
        public override List<Tuple<int, int>> GetValidMoves(Board board, int startX, int startY)
        {
            List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();

            int[] deltaOptions = { -1, 0, 1 };

            // Check all possible moves
            foreach (int deltaX in deltaOptions)
            {
                foreach (int deltaY in deltaOptions)
                {
                    if (deltaX == 0 && deltaY == 0)
                    {
                        continue;
                    }

                    int endX = startX + deltaX;
                    int endY = startY + deltaY;

                    if (IsValidMove(board, startX, startY, endX, endY))
                    {
                        validMoves.Add(new Tuple<int, int>(endX, endY));
                    }
                }
            }
            return validMoves;
        }

        /// <summary>
        /// Determines whether a move is valid for a miner.
        /// </summary>
        /// <returns>True if the move is valid, false otherwise.</returns>
        public override bool IsValidMove(Board board, int startX, int startY, int endX, int endY)
        {
            // Check if the move is within the board
            if (endX < 0 || endX >= board.Rows || endY < 0 || endY >= board.Cols)
            {
                return false;
            }

            bool isVerticalMove = startX == endX;
            bool isHorizontalMove = startY == endY;

            if (!isVerticalMove && !isHorizontalMove)
            {
                return false;
            }
            // Determine the direction of the move
            int stepX = isVerticalMove ? 0 : (endX - startX) / Math.Abs(endX - startX);
            int stepY = isHorizontalMove ? 0 : (endY - startY) / Math.Abs(endY - startY);

            for (int x = startX + stepX, y = startY + stepY; x != endX || y != endY; x += stepX, y += stepY)
            {
                if (board.GetPiece(x, y) != '.')
                {
                    return false;
                }
            }
            char destinationPiece = board.GetPiece(endX, endY);

            if (destinationPiece == '.' || IsEnemyPiece(destinationPiece) || destinationPiece == '#')
            {
                return true;
            }
            return false;
        }
    }
}