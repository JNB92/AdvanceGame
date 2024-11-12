namespace AdvanceGame;

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
    public Catapult(char symbol, string player) : base(symbol, player)
    {
    }

    /// <summary>
    /// Checks all valid moves for the Catapult piece from a given position.
    /// </summary>
    /// <param name="board">Game board</param>
    /// <param name="startX">Initial X coordinate</param>
    /// <param name="startY">Initial Y coordinate</param>
    /// <returns>List of tuples representing all valid moves</returns>
    public override List<Tuple<int, int>> GetValidMoves(Board board, int startX, int startY)
    {
        List<Tuple<int, int>> validMoves = new();

        // Check for vertical and horizontal moves
        var deltas = new int[] { -3, -2, -1, 0, 1, 2, 3 };

        foreach (var deltaX in deltas)
        foreach (var deltaY in deltas)
            if ((deltaX == 0) ^ (deltaY == 0))
            {
                var endX = startX + deltaX;
                var endY = startY + deltaY;

                if (IsValidMove(board, startX, startY, endX, endY))
                    if (!IsCaptureMove(board, startX, startY, endX, endY) || board.GetPiece(endX, endY) == '.')
                        validMoves.Add(new Tuple<int, int>(endX, endY));
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
        var isVerticalCapture = startX == endX && Math.Abs(endY - startY) == 3;
        var isHorizontalCapture = startY == endY && Math.Abs(endX - startX) == 3;
        var isDiagonalCapture = Math.Abs(endX - startX) == 2 && Math.Abs(endY - startY) == 2;
        var isPerpendicularCapture = (Math.Abs(endX - startX) == 2 && startY == endY) ||
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
        if (endX < 0 || endX >= board.Rows || endY < 0 || endY >= board.Cols) return false;

        var deltaX = Math.Sign(endX - startX);
        var deltaY = Math.Sign(endY - startY);
        var x = startX + deltaX;
        var y = startY + deltaY;

        while (x >= 0 && x < board.Rows && y >= 0 && y < board.Cols && (x != endX || y != endY))
        {
            if (board.GetPiece(x, y) != '.') return false;

            x += deltaX;
            y += deltaY;
        }

        // Check if the move is in one of the 4 cardinal directions and only one square
        var isVerticalMove = startX == endX && Math.Abs(endY - startY) == 1;
        var isHorizontalMove = startY == endY && Math.Abs(endX - startX) == 1;

        // Check if the capture is either 3 squares away in a cardinal direction or
        // 2 squares away in two perpendicular cardinal directions
        var isVerticalCapture = startX == endX && Math.Abs(endY - startY) == 3;
        var isHorizontalCapture = startY == endY && Math.Abs(endX - startX) == 3;
        var isDiagonalCapture = Math.Abs(endX - startX) == 2 && Math.Abs(endY - startY) == 2;
        var isPerpendicularCapture = (Math.Abs(endX - startX) == 2 && startY == endY) ||
                                     (Math.Abs(endY - startY) == 2 && startX == endX);

        if (!isVerticalMove && !isHorizontalMove && !isVerticalCapture && !isHorizontalCapture && !isDiagonalCapture &&
            !isPerpendicularCapture) return false;
        // Check if the destination square is empty or has an opponent piece
        var destinationPiece = board.GetPiece(endX, endY);

        // Check if the destination square is empty
        if (destinationPiece == '.') return true;

        return false;
    }
}