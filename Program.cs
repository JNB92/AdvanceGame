namespace AdvanceGame;

/// <summary>
/// The Main class for the Advance Game program.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Main method for the Advance Game
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    private static void Main(string[] args)
    {
        // Validate arguments
        if (args.Length != 1 && args.Length != 3)
        {
            Console.WriteLine(
                "Please provide the following: Your chosen colour (either white or black) as well as the <input_board_path> and <output_board_path>");
            return;
        }

        var player = args[0];

        // Print the name of the chess bot
        if (player == "name")
        {
            Console.WriteLine("Chess bot from Wish");
            return;
        }

        // If the player argument is not 'name', then we expect 3 arguments
        if (args.Length == 3)
        {
            var inputBoardPath = args[1];
            var outputBoardPath = args[2];

            if (player != "white" && player != "black")
            {
                Console.WriteLine("Invalid player argument. Must be 'white', 'black', or 'name'.");
                return;
            }

            // Read the board from the input file
            Board gameBoard;

            try
            {
                gameBoard = new Board(inputBoardPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading board from file: {ex.Message}");
                return;
            }

            // Make a move
            GameLogic.MakeMove(player);

            // Write the board to the output file
            try
            {
                GameLogic.WriteToNewFile(outputBoardPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing board to file: {ex.Message}");
                return;
            }
        }
    }
}