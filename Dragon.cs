namespace AdvanceGame;

/// <summary>
/// The Dragon class represents a Dragon piece in the Advance game.
/// </summary>
public class Dragon : Piece
{
    /// <summary>
    /// Dragon Constructor
    /// </summary>
    /// <param name="symbol">Character symbol for the Dragon.</param>
    /// <param name="player">Owner of the piece</param>
    public Dragon(char symbol, string player) : base(symbol, player)
    {
    }

    /// <summary>
    /// Checks all valid moves for the Dragon piece from a given position.
    /// </summary>
    /// <param name="board">Game board</param>
    /// <param name="startX">Initial X coordinate</param>
    /// <param name="startY">Initial Y coordinate</param>
    /// <returns>List of tuples representing all valid moves</returns>
    public override List<Tuple<int, int>> GetValidMoves(Board board, int startX, int startY)
    {
        List<Tuple<int, int>> validMoves = new();
        // Check for forward moves
        for (var i = -board.Rows; i <= board.Rows; i++)
            if (i != 0)
            {
                var endX = startX + i;
                var endY = startY;

                if (IsValidMove(board, startX, startY, endX, endY)) validMoves.Add(new Tuple<int, int>(endX, endY));
                endX = startX;
                endY = startY + i;
                if (IsValidMove(board, startX, startY, endX, endY)) validMoves.Add(new Tuple<int, int>(endX, endY));
            }

        // Check for diagonal moves
        for (var i = -board.Rows; i <= board.Rows; i++)
            if (i != 0)
            {
                var endX = startX + i;
                var endY = startY + i;

                if (IsValidMove(board, startX, startY, endX, endY)) validMoves.Add(new Tuple<int, int>(endX, endY));
                endX = startX - i;
                endY = startY + i;

                if (IsValidMove(board, startX, startY, endX, endY)) validMoves.Add(new Tuple<int, int>(endX, endY));
            }

        return validMoves;
    }

    /// <summary>
    /// Determines if the provided character represents an enemy piece for the current player.
    /// </summary>
    /// <param name="piece">The character representing the piece.</param>
    /// <returns>True if the piece is an enemy piece, false otherwise.</returns>
    public new bool IsEnemyPiece(char piece)
    {
        return (Player == "white" && char.IsLower(piece)) || (Player == "black" && char.IsUpper(piece));
    }

    /// <summary>
    /// Checks if the path between two positions on the board is clear.
    /// </summary>
    /// <param name="board">The board object.</param>
    /// <param name="startX">The starting X position.</param>
    /// <param name="startY">The starting Y position.</param>
    /// <param name="endX">The ending X position.</param>
    /// <param name="endY">The ending Y position.</param>
    /// <returns>True if the path is clear, false otherwise.</returns>
    public bool IsPathClear(Board board, int startX, int startY, int endX, int endY)
    {
        var xDirection = endX > startX ? 1 : -1;
        var yDirection = endY > startY ? 1 : -1;

        if (startX == endX) xDirection = 0;
        if (startY == endY) yDirection = 0;
        var currentX = startX + xDirection;
        var currentY = startY + yDirection;

        while (currentX != endX || currentY != endY)
        {
            if (board.GetPiece(currentX, currentY) != '.') return false;
            currentX += xDirection;
            currentY += yDirection;
        }

        return true;
    }

    /// <summary>
    /// Determines if the move from the starting position to the ending position is a valid move for the piece.
    /// </summary>
    /// <param name="board">The board object.</param>
    /// <param name="startX">The starting X position.</param>
    /// <param name="startY">The starting Y position.</param>
    /// <param name="endX">The ending X position.</param>
    /// <param name="endY">The ending Y position.</param>
    /// <returns>True if the move is valid, false otherwise.</returns>
    public override bool IsValidMove(Board board, int startX, int startY, int endX, int endY)
    {
        // Check if the destination is within the board bounds
        if (endX < 0 || endX >= board.Rows || endY < 0 || endY >= board.Cols) return false;
        var deltaX = Math.Abs(endX - startX);
        var deltaY = Math.Abs(endY - startY);

        if (deltaX <= 1 && deltaY <= 1) return board.GetPiece(endX, endY) == '.';
        // Check if the move is a valid move for the piece
        var isVerticalMove = startX == endX && deltaY > 1;
        var isHorizontalMove = startY == endY && deltaX > 1;
        var isDiagonalMove = deltaX == deltaY && deltaX > 1;

        if (!isVerticalMove && !isHorizontalMove && !isDiagonalMove) return false;
        // Check if there are no pieces between the start and end positions
        if (!IsPathClear(board, startX, startY, endX, endY)) return false;

        var destinationPiece = board.GetPiece(endX, endY);

        if (destinationPiece == '.' || IsEnemyPiece(destinationPiece)) return true;
        return false;
    }
}