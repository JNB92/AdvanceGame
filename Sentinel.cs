namespace AdvanceGame;

/// <summary>
/// The Sentinel class represents a Sentinel piece in the Advance game.
/// </summary>
public class Sentinel : Piece
{
    /// <summary>
    /// Sentinel Constructor
    /// </summary>
    /// <param name="symbol">Character symbol for the Sentinel.</param>
    /// <param name="player">Owner of the piece.</param>
    public Sentinel(char symbol, string player) : base(symbol, player)
    {
    }

    /// <summary>
    /// Checks all valid moves for the Sentinel piece from a given position.
    /// </summary>
    /// <param name="board">Game board</param>
    /// <param name="startX">Initial X coordinate</param>
    /// <param name="startY">Initial Y coordinate</param>
    /// <returns>List of tuples representing all valid moves</returns>
    public override List<Tuple<int, int>> GetValidMoves(Board board, int startX, int startY)
    {
        List<Tuple<int, int>> validMoves = new();

        // Define all possible L-shaped moves
        int[] deltaXOptions = { -2, -1, 1, 2, 2, 1, -1, -2 };
        int[] deltaYOptions = { 1, 2, 2, 1, -1, -2, -2, -1 };

        // Check all possible L-shaped moves
        for (var i = 0; i < deltaXOptions.Length; i++)
        {
            var deltaX = deltaXOptions[i];
            var deltaY = deltaYOptions[i];

            var endX = startX + deltaX;
            var endY = startY + deltaY;

            if (IsValidMove(board, startX, startY, endX, endY)) validMoves.Add(new Tuple<int, int>(endX, endY));
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
        if (endX < 0 || endX >= board.Rows || endY < 0 || endY >= board.Cols) return false;

        var deltaX = Math.Abs(endX - startX);
        var deltaY = Math.Abs(endY - startY);

        if ((deltaX == 2 && deltaY == 1) || (deltaX == 1 && deltaY == 2))
        {
            var destinationPiece = board.GetPiece(endX, endY);

            if (destinationPiece == '.' || IsEnemyPiece(destinationPiece))
            {
                var isInProtectionRange = IsInProtectionRange(startX, startY, endX, endY);
                if (isInProtectionRange)
                {
                    var currentPiece = board.GetPiece(startX, startY);
                    if (IsFriendlyPiece(destinationPiece) || currentPiece == 'S') return false;
                }

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if the specified piece is a friendly piece.
    /// </summary>
    /// <param name="piece">The piece to check.</param>
    /// <returns>True if the specified piece is a friendly piece, false otherwise.</returns>
    private bool IsFriendlyPiece(char piece)
    {
        return (Player == "white" && char.IsUpper(piece)) || (Player == "black" && char.IsLower(piece));
    }

    /// <summary>
    /// Checks if a given destination is within the Sentinel's protection range.
    /// </summary>
    /// <param name="startX">The Sentinel's current x-coordinate.</param>
    /// <param name="startY">The Sentinel's current y-coordinate.</param>
    /// <param name="endX">The destination x-coordinate.</param>
    /// <param name="endY">The destination y-coordinate.</param>
    /// <returns>True if the destination is within the protection range, false otherwise.</returns>
    private bool IsInProtectionRange(int startX, int startY, int endX, int endY)
    {
        var deltaX = Math.Abs(endX - startX);
        var deltaY = Math.Abs(endY - startY);

        // Check if the destination square is within the protection range (cardinal directions, one square away)
        return (deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1);
    }
}