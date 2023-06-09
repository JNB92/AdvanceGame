namespace AdvanceGame
{
    /// <summary>
    /// The Piece class represents a piece in the game. It provides common properties and methods for all pieces.
    /// </summary>
    public abstract class Piece
    {
        /// <summary>
        /// Constructor for the Piece class.
        /// </summary>
        /// <param name="symbol">The symbol representing a piece.</param>
        /// <param name="player">The player color ("white" or "black").</param>
        protected Piece(char symbol, string player)
        {
            Symbol = symbol;
            Player = player;
        }

        /// <summary>
        /// The player color ("white" or "black").
        /// </summary>
        public string Player { get; protected set; }

        /// <summary>
        /// The symbol representing the piece.
        /// </summary>
        public char Symbol { get; protected set; }

        /// <summary>
        /// Gets the valid moves for the piece on the board.
        /// </summary>
        /// <param name="board">The board object.</param>
        /// <param name="startX">The starting X position of the piece.</param>
        /// <param name="startY">The starting Y position of the piece.</param>
        /// <returns>A list of valid move positions.</returns>
        public abstract List<Tuple<int, int>> GetValidMoves(Board board, int startX, int startY);

        /// <summary>
        /// Determines if a move is a capture move for the piece on the board.
        /// </summary>
        /// <param name="board">The board object.</param>
        /// <param name="startX">The starting X position of the piece.</param>
        /// <param name="startY">The starting Y position of the piece.</param>
        /// <param name="endX">The ending X position of the piece.</param>
        /// <param name="endY">The ending Y position of the piece.</param>
        /// <returns>True if the move is a capture move, false otherwise.</returns>
        public virtual bool IsCaptureMove(Board board, int startX, int startY, int endX, int endY)
        {
            return IsEnemyPiece(board.GetPiece(endX, endY));
        }

        /// <summary>
        /// Checks if a given piece is an enemy piece based on the player color.
        /// </summary>
        /// <param name="piece">The piece to check.</param>
        /// <returns>True if the piece is an enemy piece, false otherwise.</returns>
        public bool IsEnemyPiece(char piece)
        {
            return (Player == "white" && char.IsLower(piece)) || (Player == "black" && char.IsUpper(piece));
        }

        /// <summary>
        /// Checks if a move is valid for the piece on the board.
        /// </summary>
        /// <param name="board">The board object.</param>
        /// <param name="startX">The starting X position of the piece.</param>
        /// <param name="startY">The starting Y position of the piece.</param>
        /// <param name="endX">The ending X position of the piece.</param>
        /// <param name="endY">The ending Y position of the piece.</param>
        /// <returns>True if the move is valid, false otherwise.</returns>
        public abstract bool IsValidMove(Board board, int startX, int startY, int endX, int endY);
    }
}