namespace AdvanceGame;

/// <summary>
/// The Zombie class represents a Zombie piece in the Advance game.
/// </summary>
public class Zombie : Piece
{
    /// <summary>
    /// Zombie Constructor
    /// </summary>
    /// <param name="symbol">Character symbol for the Zombie.</param>
    /// <param name="player">Owner of the piece</param>
    public Zombie(char symbol, string player) : base(symbol, player) { }

    /// <summary>
    /// Checks all valid moves for the Zombie piece from a given position.
    /// </summary>
    /// <param name="board">Game board</param>
    /// <param name="startX">Initial X coordinate</param>
    /// <param name="startY">Initial Y coordinate</param>
    /// <returns>List of tuples representing all valid moves</returns>
    public override List<Tuple<int, int>> GetValidMoves(Board board, int startX, int startY)
    {
        List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();

        int direction = Player == "white" ? -1 : 1;
        int endX = startX + direction;
        if (IsValidMove(board, startX, startY, endX, startY))
        {
            validMoves.Add(new Tuple<int, int>(endX, startY));
        }
        int leapX = startX + 2 * direction;

        if (IsValidMove(board, startX, startY, leapX, startY))
        {
            validMoves.Add(new Tuple<int, int>(leapX, startY));
        }
        for (int j = -1; j <= 1; j += 2)
        {
            int endY = startY + j; if (IsValidMove(board, startX, startY, endX, endY))
            {
                validMoves.Add(new Tuple<int, int>(endX, endY));
            }

            endY = startY + j * 2;

            if (IsValidMove(board, startX, startY, leapX, endY))
            {
                validMoves.Add(new Tuple<int, int>(leapX, endY));
            }
        }
        return validMoves;
    }

    /// <summary>
    /// Determines whether a move is valid for a zombie.
    /// </summary>
    /// <returns>True if the move is valid, false otherwise.</returns>
    public override bool IsValidMove(Board board, int startX, int startY, int endX, int endY)
    {
        // Check if the destination is within the board bounds
        if (endX < 0 || endX >= board.Rows || endY < 0 || endY >= board.Cols)
        {
            return false;
        }
        int direction = Player == "white" ? -1 : 1;
        int verticalMove = endX - startX;
        if (verticalMove != direction && verticalMove != 2 * direction)
        {
            return false;
        }
        int deltaX = Math.Abs(endX - startX);
        int deltaY = Math.Abs(endY - startY);

        if (deltaX == 1 && deltaY == 0)
        {
            char destinationPiece = board.GetPiece(endX, endY);
            return destinationPiece == '.' || IsEnemyPiece(destinationPiece);
        }

        if (deltaX == 1 && deltaY == 1)
        {
            char destinationPiece = board.GetPiece(endX, endY);
            return destinationPiece == '.' || IsEnemyPiece(destinationPiece);
        }

        if (deltaX == 2 && deltaY == 0)
        {
            char destinationPiece = board.GetPiece(endX, endY);
            char intermediatePiece = board.GetPiece(startX + (endX - startX) / 2, startY);

            if (intermediatePiece != '.')
            {
                return false;
            }
            return IsEnemyPiece(destinationPiece);
        }

        if (deltaX == 2 && deltaY == 2)
        {
            char destinationPiece = board.GetPiece(endX, endY);
            char intermediatePiece = board.GetPiece(startX + (endX - startX) / 2, startY + (endY - startY) / 2);

            if (intermediatePiece != '.')
            {
                return false;
            }

            return IsEnemyPiece(destinationPiece);
        }
        return false;
    }
}