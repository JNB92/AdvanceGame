namespace AdvanceGame
{
    /// <summary>
    /// This class is responsible for creating Piece objects based on the symbol and player.
    /// </summary>
    internal static class PieceFactory
    {
        /// <summary>
        /// Creates a Piece object based on the provided symbol and player.
        /// </summary>
        /// <param name="symbol">The symbol representing the type of Piece to create.</param>
        /// <param name="player">The player the Piece belongs to.</param>
        /// <returns>A Piece object of the appropriate type, or null if the symbol is not recognized.</returns>
        public static Piece? CreatePiece(char symbol, string player)
        {
            // Determine the player based on the case of the symbol
            if (char.IsUpper(symbol))
            {
                player = "white";
            }
            else if (char.IsLower(symbol))
            {
                player = "black";
                symbol = char.ToUpper(symbol);
            }
            else
            {
                return null;
            }
            // Create the appropriate Piece based on the symbol
            return char.ToUpper(symbol) switch
            {
                'Z' => new Zombie(symbol, player),
                'B' => new Builder(symbol, player),
                'M' => new Miner(symbol, player),
                'J' => new Jester(symbol, player),
                'S' => new Sentinel(symbol, player),
                'C' => new Catapult(symbol, player),
                'D' => new Dragon(symbol, player),
                'G' => new General(symbol, player),
                '#' => new Wall(symbol),
                '.' => null,
                _ => null
            };
        }
    }
}