namespace AdvanceGame;

/// <summary>
/// The Jester class represents a Jester piece in the Advance game.
/// </summary>
public class Jester : Piece
{
    /// <summary>
    /// Jester Constructor
    /// </summary>
    /// <param name="symbol">Character symbol for the Jester.</param>
    /// <param name="player">Owner of the piece.</param>
    public Jester(char symbol, string player) : base(symbol, player)
    {
    }

    /// <summary>
    /// Checks all valid moves for the Jester piece from a given position.
    /// </summary>
    /// <param name="board">Game board</param>
    /// <param name="startX">Initial X coordinate</param>
    /// <param name="startY">Initial Y coordinate</param>
    /// <returns>List of tuples representing all valid moves</returns>
    public override List<Tuple<int, int>> GetValidMoves(Board board, int startX, int startY)
    {
        List<Tuple<int, int>> validMoves = new();

        var deltaXOptions = new int[] { -1, 0, 1 };
        var deltaYOptions = new int[] { -1, 0, 1 };

        //for loop to check all possible moves
        for (var i = 0; i < deltaXOptions.Length; i++)
        for (var j = 0; j < deltaYOptions.Length; j++)
        {
            if (deltaXOptions[i] == 0 && deltaYOptions[j] == 0) continue;

            var endX = startX + deltaXOptions[i];
            var endY = startY + deltaYOptions[j];

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
    /// Checks if the Jester can convert an enemy piece at a specified location.
    /// </summary>
    /// <param name="board">Current game board.</param>
    /// <param name="startX">The Jester's initial x-coordinate.</param>
    /// <param name="startY">The Jester's initial y-coordinate.</param>
    /// <param name="endX">The Jester's destination x-coordinate.</param>
    /// <param name="endY">The Jester's destination y-coordinate.</param>
    /// <returns>True if the enemy piece can be converted, false otherwise.</returns>
    public bool IsValidConversion(char[,] board, int startX, int startY, int endX, int endY)
    {
        // Check if the destination is within the board bounds
        if (endX < 0 || endX >= board.GetLength(0) || endY < 0 || endY >= board.GetLength(1)) return false;

        var deltaX = Math.Abs(endX - startX);
        var deltaY = Math.Abs(endY - startY);

        // Check if the destination is a valid conversion
        if ((deltaX == 1 && deltaY <= 1) || (deltaX == 0 && deltaY == 1))
        {
            var destinationPiece = board[endX, endY];

            if (IsEnemyPiece(destinationPiece) && char.ToUpper(destinationPiece) != 'G') return true;
        }

        return false;
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

        // Check if the destination is a valid move
        if ((deltaX == 1 && deltaY <= 1) || (deltaX == 0 && deltaY == 1))
        {
            var destinationPiece = board.GetPiece(endX, endY);

            if (destinationPiece == '.' || IsFriendlyPiece(destinationPiece)) return true;
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
}