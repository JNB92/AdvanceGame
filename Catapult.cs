namespace AdvanceGame
{
    /// <summary>
    /// The Catapult class represents a Catapult piece in the Advance game.
    /// </summary>
    public class Catapult : Piece
    {
        /// <summary>
        /// Catapult Constructor
        /// </summary>
        /// <param name="symbol">Character symbol for the Catapult.</param>
        /// <param name="player">Owner of the piece</param>
        public Catapult(char symbol, string player) : base(symbol, player) { }

        /// <summary>
        /// Checks all valid moves for the Catapult piece from a given position.
        /// </summary>
        /// <param name="board">Game board</param>
        /// <param name="startX">Initial X coordinate</param>
        /// <param name="startY">Initial Y coordinate</param>
        /// <returns>List of tuples representing all valid moves</returns>
        public override List<Tuple<int, int>> GetValidMoves(Board board, int startX, int startY)
        {
            List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();

            // Check for vertical and horizontal moves
            int[] deltas = new int[] { -3, -2, -1, 0, 1, 2, 3 };

            foreach (int deltaX in deltas)
            {
                foreach (int deltaY in deltas)
                {
                    if ((deltaX == 0) ^ (deltaY == 0))
                    {
                        int endX = startX + deltaX;
                        int endY = startY + deltaY;

                        if (IsValidMove(board, startX, startY, endX, endY))
                        {
                            if (!IsCaptureMove(board, startX, startY, endX, endY) || board.GetPiece(endX, endY) == '.')
                            {
                                validMoves.Add(new Tuple<int, int>(endX, endY));
                            }
                        }
                    }
                }
            }

            return validMoves;
        }

        /// <summary>
        /// Determines if a move is a capture move for the Knight piece on the board.
        /// </summary>
        /// <param name="board">The board object.</param>
        /// <param name="startX">The starting X position of the piece.</param>
        /// <param name="startY">The starting Y position of the piece.</param>
        /// <param name="endX">The ending X position of the piece.</param>
        /// <param name="endY">The ending Y position of the piece.</param>
        /// <returns>True if the move is a capture move, false otherwise.</returns>
        public override bool IsCaptureMove(Board board, int startX, int startY, int endX, int endY)
        {
            // Check if the move is a capture
            bool isVerticalCapture = startX == endX && Math.Abs(endY - startY) == 3;
            bool isHorizontalCapture = startY == endY && Math.Abs(endX - startX) == 3;
            bool isDiagonalCapture = Math.Abs(endX - startX) == 2 && Math.Abs(endY - startY) == 2;
            bool isPerpendicularCapture = (Math.Abs(endX - startX) == 2 && startY == endY) ||
                                          (Math.Abs(endY - startY) == 2 && startX == endX);

            return isVerticalCapture || isHorizontalCapture || isDiagonalCapture || isPerpendicularCapture;
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

            int deltaX = Math.Sign(endX - startX);
            int deltaY = Math.Sign(endY - startY);
            int x = startX + deltaX;
            int y = startY + deltaY;

            while (x >= 0 && x < board.Rows && y >= 0 && y < board.Cols && (x != endX || y != endY))
            {
                if (board.GetPiece(x, y) != '.')
                {
                    return false;
                }

                x += deltaX;
                y += deltaY;
            }

            // Check if the move is in one of the 4 cardinal directions and only one square
            bool isVerticalMove = startX == endX && Math.Abs(endY - startY) == 1;
            bool isHorizontalMove = startY == endY && Math.Abs(endX - startX) == 1;

            // Check if the capture is either 3 squares away in a cardinal direction or
            // 2 squares away in two perpendicular cardinal directions
            bool isVerticalCapture = startX == endX && Math.Abs(endY - startY) == 3;
            bool isHorizontalCapture = startY == endY && Math.Abs(endX - startX) == 3;
            bool isDiagonalCapture = Math.Abs(endX - startX) == 2 && Math.Abs(endY - startY) == 2;
            bool isPerpendicularCapture = (Math.Abs(endX - startX) == 2 && startY == endY) ||
                                          (Math.Abs(endY - startY) == 2 && startX == endX);

            if (!isVerticalMove && !isHorizontalMove && !isVerticalCapture && !isHorizontalCapture && !isDiagonalCapture && !isPerpendicularCapture)
            {
                return false;
            }
            // Check if the destination square is empty or has an opponent piece
            char destinationPiece = board.GetPiece(endX, endY);

            // Check if the destination square is empty
            if (destinationPiece == '.')
            {
                return true;
            }

            return false;
        }
    }
}