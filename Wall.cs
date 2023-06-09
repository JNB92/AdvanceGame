namespace AdvanceGame
{
    /// <summary>
    /// The Wall class represents a Wall piece in the game.
    /// </summary>
    public class Wall : Piece
    {
        /// <summary>
        /// Wall constructor
        /// </summary>
        /// <param name="symbol">Character symbol for the Wall.</param>
        public Wall(char symbol) : base(symbol, "none") { }

        /// <summary>
        /// Gets a list of valid moves. A wall cannot move, so this always returns an empty list.
        /// </summary>
        /// <returns>An empty list, because a wall cannot move.</returns>
        public override List<Tuple<int, int>> GetValidMoves(Board board, int row, int col)
        {
            return new List<Tuple<int, int>>();
        }

        /// <summary>
        /// Determines whether a move is valid. A wall cannot move, so this always returns false.
        /// </summary>
        /// <returns>False, because a wall cannot move.</returns>
        public override bool IsValidMove(Board board, int fromRow, int fromCol, int toRow, int toCol)
        {
            return false;
        }
    }
}