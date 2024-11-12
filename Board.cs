namespace AdvanceGame;

/// <summary>
/// The Board class represents the Game Board in the Advance game.
/// </summary>
public class Board
{
    /// <summary>
    /// Represents the game board as a 2D character array.
    /// </summary>
    private readonly char[,] _board;

    /// <summary>
    /// Initializes a new instance of the Board class from the specified input board file.
    /// </summary>
    /// <param name="inputBoardPath">The path to the input board file.</param>
    public Board(string inputBoardPath)
    {
        string[] lines = File.ReadAllLines(inputBoardPath);
        Rows = lines.Length;
        Cols = lines[0].Length;
        _board = new char[Rows, Cols];
        ReadFromFile(inputBoardPath);
        CurrentBoard = _board;
    }

    /// <summary>
    /// Initializes a new instance of the Board class by copying the contents of another Board instance.
    /// </summary>
    /// <param name="other">The Board instance to copy.</param>
    public Board(Board other)
    {
        Rows = other.Rows;
        Cols = other.Cols;
        _board = new char[Rows, Cols];
        for (var i = 0; i < Rows; i++)
        for (var j = 0; j < Cols; j++)
            _board[i, j] = other._board[i, j];
    }

    /// <summary>
    /// Gets the number of columns in the game board.
    /// </summary>
    public int Cols { get; }

    /// <summary>
    /// Gets or sets the current state of the game board.
    /// </summary>
    public char[,]? CurrentBoard { get; internal set; }

    /// <summary>
    /// Gets the number of rows in the game board.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Determines whether the general would be protected by a sentinel piece on the chess board.
    /// </summary>
    /// <param name="board">The chess board.</param>
    /// <param name="sentinelX">The X-coordinate of the sentinel piece.</param>
    /// <param name="sentinelY">The Y-coordinate of the sentinel piece.</param>
    /// <param name="generalX">The X-coordinate of the general piece.</param>
    /// <param name="generalY">The Y-coordinate of the general piece.</param>
    /// <param name="currentPlayer">The current player.</param>
    /// <returns>True if the general would be protected by the sentinel piece, otherwise false.</returns>
    public static bool WouldBeProtected(Board board, int sentinelX, int sentinelY, int generalX, int generalY,
        string currentPlayer)
    {
        int[] dx = { 0, -1, 0, 1 };
        int[] dy = { -1, 0, 1, 0 };

        for (var i = 0; i < 4; i++)
        {
            var x = generalX + dx[i];
            var y = generalY + dy[i];

            if (x >= 0 && x < board.Rows && y >= 0 && y < board.Cols)
            {
                if (sentinelX != x || sentinelY != y) continue;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the character representing the piece at the specified position on the game board.
    /// </summary>
    /// <param name="row">The row index of the position.</param>
    /// <param name="col">The column index of the position.</param>
    /// <returns>The character representing the piece at the specified position.</returns>
    public char GetPiece(int row, int col)
    {
        return _board[row, col];
    }

    /// <summary>
    /// Determines if a specific square on the game board contains an enemy piece.
    /// </summary>
    /// <param name="row">The row index of the square.</param>
    /// <param name="col">The column index of the square.</param>
    /// <param name="currentPlayer">The current player's color ("white" or "black").</param>
    /// <returns>True if the square contains an enemy piece; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid player is specified. Must be "white" or "black".</exception>
    public bool IsEnemyPiece(int row, int col, string currentPlayer)
    {
        var piece = GetPiece(row, col);

        return currentPlayer switch
        {
            "white" => char.IsLower(piece),
            "black" => char.IsUpper(piece),
            _ => throw new ArgumentException("Invalid player: must be 'white' or 'black'.")
        };
    }

    /// <summary>
    /// Determines if the general would be in danger after a move from the specified starting position to the specified end position.
    /// </summary>
    /// <param name="startX">The starting x-coordinate of the piece.</param>
    /// <param name="startY">The starting y-coordinate of the piece.</param>
    /// <param name="endX">The end x-coordinate of the piece.</param>
    /// <param name="endY">The end y-coordinate of the piece.</param>
    /// <param name="currentPlayer">The current player's color ("white" or "black").</param>
    /// <param name="checkNextTurn">Optional. Specifies whether to check the next turn's move. Default is true.</param>
    /// <returns>True if the general would be in danger after the move; otherwise, false.</returns>
    public bool IsGeneralInDangerAfterMove(int startX, int startY, int endX, int endY, string currentPlayer,
        bool checkNextTurn = true)
    {
        Board tempBoard = new(this);
        tempBoard.MovePiece(startX, startY, endX, endY);
        General general = new(currentPlayer == "white" ? 'G' : 'g', currentPlayer);
        var IsInDanger = general.IsInDanger(tempBoard, endX, endY);
        return IsInDanger;
    }

    /// <summary>
    /// Determines if the general would be protected after constructing a wall at the specified positions.
    /// </summary>
    /// <param name="builderX">The x-coordinate of the builder.</param>
    /// <param name="builderY">The y-coordinate of the builder.</param>
    /// <param name="wallX">The x-coordinate of the wall.</param>
    /// <param name="wallY">The y-coordinate of the wall.</param>
    /// <param name="generalX">The x-coordinate of the general.</param>
    /// <param name="generalY">The y-coordinate of the general.</param>
    /// <param name="currentPlayer">The current player's color ("white" or "black").</param>
    /// <returns>True if the general would be protected after constructing the wall; otherwise, false.</returns>
    public bool IsGeneralProtectedAfterWallConstruction(int builderX, int builderY, int wallX, int wallY, int generalX,
        int generalY, string currentPlayer)
    {
        // Store the original value at the wall position
        var original = _board[wallX, wallY];

        // Temporarily place a wall
        _board[wallX, wallY] = '#';

        // Check if the general would still be in danger
        General general = new(currentPlayer == "white" ? 'G' : 'g', currentPlayer);
        var isInDanger = general.IsInDanger(this, generalX, generalY);

        // Restore the original value
        _board[wallX, wallY] = original;

        return !isInDanger;
    }

    /// <summary>
    /// Makes a move for the specified current player.
    /// </summary>
    /// <param name="currentPlayer">The current player's color ("white" or "black").</param>
    public void MakeMove(string currentPlayer)
    {
        var general = new General(currentPlayer == "white" ? 'G' : 'g', currentPlayer);
        var generalPosition = FindGeneralPosition(general);

        if (generalPosition != null && general.IsInDanger(this, generalPosition.Item1, generalPosition.Item2))
        {
            var threat = general.IdentifyThreat(this, generalPosition.Item1, generalPosition.Item2);
            if (threat != null && TryCaptureThreateningPiece(currentPlayer, threat))
                return;

            var bestProtectionMove = FindProtectionMove(currentPlayer, generalPosition);
            if (bestProtectionMove != null)
                return;

            if (BuildWall(currentPlayer, generalPosition))
                return;

            var safeMove = general.FindSafestMove(this, generalPosition.Item1, generalPosition.Item2);
            if (safeMove != null)
            {
                MoveAndUpdateBoard(generalPosition.Item1, generalPosition.Item2, safeMove.Item1, safeMove.Item2);
                return;
            }
        }

        TryMakeMove(currentPlayer);
    }

    /// <summary>
    /// Moves a piece from the specified starting position to the specified end position.
    /// </summary>
    /// <param name="startX">The starting x-coordinate of the piece.</param>
    /// <param name="startY">The starting y-coordinate of the piece.</param>
    /// <param name="endX">The end x-coordinate of the piece.</param>
    /// <param name="endY">The end y-coordinate of the piece.</param>
    public void MovePiece(int startX, int startY, int endX, int endY)
    {
        var piece = GetPiece(startX, startY);

        var playerOfPiece = IsCorrectColor(piece, "white") ? "white" : "black";

        if (piece == 'J' || piece == 'j')
        {
            var endPiece = GetPiece(endX, endY);

            if (IsCorrectColor(endPiece, playerOfPiece))
            {
                _board[startX, startY] = endPiece;
                _board[endX, endY] = piece;
            }
            else if (IsEnemyPiece(endX, endY, playerOfPiece) && char.ToUpper(endPiece) != 'G')
            {
                _board[endX, endY] = char.IsUpper(endPiece) ? char.ToLower(endPiece) : char.ToUpper(endPiece);
            }
            else
            {
                _board[startX, startY] = '.';
                _board[endX, endY] = piece;
            }
        }
        else
        {
            _board[startX, startY] = '.';
            _board[endX, endY] = piece;
        }
    }

    /// <summary>
    /// Writes the current state of the game board to a new file.
    /// </summary>
    /// <param name="filePath">The path of the new file to write.</param>
    public void WriteToNewFile(string filePath)
    {
        using StreamWriter writer = new(filePath);
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++) writer.Write(_board[row, col]);

            writer.WriteLine();
        }
    }

    /// <summary>
    /// Determines if the specified piece belongs to the specified player.
    /// </summary>
    /// <param name="piece">The piece to check.</param>
    /// <param name="player">The player's color ("white" or "black").</param>
    /// <returns>True if the piece belongs to the player; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid player is specified. Must be "white" or "black".</exception>
    private static bool IsCorrectColor(char piece, string player)
    {
        return player switch
        {
            "white" => char.IsUpper(piece),
            "black" => char.IsLower(piece),
            _ => throw new ArgumentException("Invalid player: must be 'white' or 'black'.")
        };
    }

    /// <summary>
    /// Builds a wall to protect the general in the current board configuration.
    /// </summary>
    /// <param name="currentPlayer">The current player's color ("white" or "black").</param>
    /// <param name="generalPosition">The position of the general on the board.</param>
    /// <returns>True if a wall was successfully built; otherwise, false.</returns>
    private bool BuildWall(string currentPlayer, Tuple<int, int> generalPosition)
    {
        for (var startX = 0; startX < Rows; startX++)
        for (var startY = 0; startY < Cols; startY++)
        {
            var piece = GetPiece(startX, startY);
            _ = PieceFactory.CreatePiece(piece, currentPlayer);

            if ((currentPlayer == "white" && piece == 'B') ||
                (currentPlayer == "black" && piece == 'b')) // If the piece is a builder
            {
                // Check the 8 surrounding squares
                int[] dx = { -1, 0, 1, 1, 1, 0, -1, -1 };
                int[] dy = { 1, 1, 1, 0, -1, -1, -1, 0 };

                for (var i = 0; i < 8; i++)
                {
                    var wallX = startX + dx[i];
                    var wallY = startY + dy[i];

                    // Add a bounds check here
                    if (wallX < 0 || wallX >= Rows || wallY < 0 || wallY >= Cols ||
                        GetPiece(wallX, wallY) != '.' || !IsGeneralProtectedAfterWallConstruction(startX,
                            startY, wallX, wallY, generalPosition.Item1, generalPosition.Item2, currentPlayer))
                        continue;
                    // Check if the builder can construct a wall that would protect the general
                    _board[wallX, wallY] = '#'; // construct a wall
                    CurrentBoard = _board; // update the current board
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Evaluates and executes a move from the specified starting position to the specified end position.
    /// </summary>
    /// <param name="startX">The starting x-coordinate of the piece.</param>
    /// <param name="startY">The starting y-coordinate of the piece.</param>
    /// <param name="endX">The end x-coordinate of the piece.</param>
    /// <param name="endY">The end y-coordinate of the piece.</param>
    /// <param name="currentPlayer">The current player's color ("white" or "black").</param>
    /// <param name="bestCaptureMove">The best capture move found so far.</param>
    /// <param name="bestNonCaptureMove">The best non-capture move found so far.</param>
    /// <returns>True if a move was executed; otherwise, false.</returns>
    private bool EvaluateAndExecuteMove(int startX, int startY, int endX, int endY, string currentPlayer,
        ref Tuple<int, int, int, int>? bestCaptureMove, ref Tuple<int, int, int, int>? bestNonCaptureMove)
    {
        var moveMade = false;

        if (GetPiece(endX, endY) != '.')
        {
            bestCaptureMove = Tuple.Create(startX, startY, endX, endY);
            MovePiece(startX, startY, endX, endY);
            CurrentBoard = _board; // update the current board
            moveMade = true;
        }
        else
        {
            bestNonCaptureMove = bestNonCaptureMove switch
            {
                null => Tuple.Create(startX, startY, endX, endY),
                _ => bestNonCaptureMove
            };
        }

        return moveMade;
    }

    /// <summary>
    /// Finds the position of the general on the board.
    /// </summary>
    /// <param name="general">The general piece object.</param>
    /// <returns>The position of the general as a tuple of row and column indices, or null if not found.</returns>
    private Tuple<int, int>? FindGeneralPosition(General general)
    {
        for (var row = 0; row < Rows; row++)
        for (var col = 0; col < Cols; col++)
            if (_board[row, col] == general.Symbol)
                return Tuple.Create(row, col);
        return null;
    }

    /// <summary>
    /// Finds a protection move for the general to defend against threats.
    /// </summary>
    /// <param name="currentPlayer">The current player's color ("white" or "black").</param>
    /// <param name="generalPosition">The position of the general on the board.</param>
    /// <returns>The best protection move as a tuple of starting and ending positions, or null if no protection move is found.</returns>
    private Tuple<int, int, int, int>? FindProtectionMove(string currentPlayer, Tuple<int, int> generalPosition)
    {
        Tuple<int, int, int, int>? bestProtectionMove = null;

        for (var startX = 0; startX < Rows; startX++)
        {
            for (var startY = 0; startY < Cols; startY++)
            {
                var piece = GetPiece(startX, startY);
                var pieceObj = PieceFactory.CreatePiece(piece, currentPlayer);

                if ((currentPlayer == "white" && piece == 'S') ||
                    (currentPlayer == "black" && piece == 's')) // If the piece is a sentinel
                {
                    // Check moves
                    int[] dx = { -2, -1, 1, 2, 2, 1, -1, -2 };
                    int[] dy = { 1, 2, 2, 1, -1, -2, -2, -1 };

                    for (var i = 0; i < 8; i++)
                    {
                        var endX = startX + dx[i];
                        var endY = startY + dy[i];

                        // Check if pieceObj is not null and if the sentinel can move to a square where it can protect the general
                        if (pieceObj != null && pieceObj.IsValidMove(this, startX, startY, endX, endY) &&
                            WouldBeProtected(this, endX, endY, generalPosition.Item1, generalPosition.Item2,
                                currentPlayer) &&
                            !IsGeneralInDangerAfterMove(startX, startY, endX, endY, currentPlayer, false))
                        {
                            bestProtectionMove = Tuple.Create(startX, startY, endX, endY);
                            break;
                        }
                    }
                }

                if (bestProtectionMove != null) break;
            }

            if (bestProtectionMove != null) break;
        }

        if (bestProtectionMove != null)
        {
            MovePiece(bestProtectionMove.Item1, bestProtectionMove.Item2, bestProtectionMove.Item3,
                bestProtectionMove.Item4);
            CurrentBoard = _board;
            return bestProtectionMove;
        }

        return bestProtectionMove;
    }
    // Generate a method that checks if the enemy general is in check

    /// <summary>
    /// Makes the best non-capture move or dangerous move.
    /// </summary>
    /// <param name="bestNonCaptureMove">The best non-capture move found.</param>
    /// <param name="bestDangerousMove">The best dangerous move found.</param>
    private void MakeBestNonCaptureOrDangerousMove(Tuple<int, int, int, int>? bestNonCaptureMove,
        Tuple<int, int, int, int>? bestDangerousMove)
    {
        if (bestNonCaptureMove != null)
        {
            MovePiece(bestNonCaptureMove.Item1, bestNonCaptureMove.Item2, bestNonCaptureMove.Item3,
                bestNonCaptureMove.Item4);
            CurrentBoard = _board; // update the current board
        }
        else if (bestDangerousMove != null)
        {
            MovePiece(bestDangerousMove.Item1, bestDangerousMove.Item2, bestDangerousMove.Item3,
                bestDangerousMove.Item4);
            CurrentBoard = _board; // update the current board
        }
    }

    /// <summary>
    /// Moves a piece from the specified starting position to the specified end position and updates the game board.
    /// </summary>
    /// <param name="startX">The starting x-coordinate of the piece.</param>
    /// <param name="startY">The starting y-coordinate of the piece.</param>
    /// <param name="endX">The end x-coordinate of the piece.</param>
    /// <param name="endY">The end y-coordinate of the piece.</param>
    private void MoveAndUpdateBoard(int startX, int startY, int endX, int endY)
    {
        MovePiece(startX, startY, endX, endY);
        CurrentBoard = _board; // update the current board
    }

    /// <summary>
    /// Reads the game board from the specified file.
    /// </summary>
    /// <param name="inputBoardPath">The path of the input file containing the game board.</param>
    private void ReadFromFile(string inputBoardPath)
    {
        string[] lines = File.ReadAllLines(inputBoardPath);

        for (var row = 0; row < Rows; row++)
        {
            var line = lines[row].TrimEnd('\n');
            for (var col = 0; col < Cols; col++) _board[row, col] = line[col];
        }
    }

    /// <summary>
    /// Tries to capture the threatening piece by moving a piece from the current player to the threatening position.
    /// </summary>
    /// <param name="currentPlayer">The current player's color ("white" or "black").</param>
    /// <param name="threat">The position of the threatening piece.</param>
    /// <returns>True if the threatening piece was captured; otherwise, false.</returns>
    private bool TryCaptureThreateningPiece(string currentPlayer, Tuple<int, int> threat)
    {
        for (var startX = 0; startX < Rows; startX++)
        for (var startY = 0; startY < Cols; startY++)
        {
            var piece = GetPiece(startX, startY);
            var pieceObj = PieceFactory.CreatePiece(piece, currentPlayer);

            if (piece == '.' || pieceObj?.Player != currentPlayer || !IsCorrectColor(piece, currentPlayer) ||
                !pieceObj.IsValidMove(this, startX, startY, threat.Item1, threat.Item2) ||
                IsGeneralInDangerAfterMove(startX, startY, threat.Item1, threat.Item2, currentPlayer,
                    false))
                continue;

            MovePiece(startX, startY, threat.Item1, threat.Item2);
            CurrentBoard = _board; // update the current board
            return true;
        }

        return false;
    }

    /// <summary>
    /// Tries to convert an enemy piece using a Jester piece to save the General.
    /// </summary>
    /// <param name="currentPlayer">The current player's color ("white" or "black").</param>
    /// <returns>The best conversion move as a tuple of starting and ending positions, or null if no conversion move is found.</returns>
    private Tuple<int, int, int, int>? TryConvertEnemyWithJester(string currentPlayer)
    {
        Tuple<int, int, int, int>? bestConversionMove = null;
        var general = new General(currentPlayer == "white" ? 'G' : 'g', currentPlayer);
        var generalPosition = FindGeneralPosition(general);

        if (generalPosition == null)
            return null;

        Tuple<int, int>? threatPosition = null;
        if (general.IsInDanger(this, generalPosition.Item1, generalPosition.Item2))
            threatPosition = general.IdentifyThreat(this, generalPosition.Item1, generalPosition.Item2);

        for (var startX = 0; startX < Rows; startX++)
        for (var startY = 0; startY < Cols; startY++)
        {
            var piece = GetPiece(startX, startY);

            if ((currentPlayer == "white" && piece == 'J') ||
                (currentPlayer == "black" && piece == 'j')) // If the piece is a Jester
            {
                // Check the 8 surrounding squares
                int[] dx = { -1, 0, 1, 1, 1, 0, -1, -1 };
                int[] dy = { 1, 1, 1, 0, -1, -1, -1, 0 };

                for (var i = 0; i < 8; i++)
                {
                    var endX = startX + dx[i];
                    var endY = startY + dy[i];

                    // Create a Jester object
                    var pieceObj = PieceFactory.CreatePiece(piece, currentPlayer);

                    // Check if the Jester can convert the enemy to save the General
                    if (pieceObj is Jester &&
                        ((Jester)pieceObj).IsValidConversion(_board, startX, startY, endX, endY))
                    {
                        if (threatPosition != null && endX == threatPosition.Item1 && endY == threatPosition.Item2)
                            return Tuple.Create(startX, startY, endX, endY);
                        if (bestConversionMove == null) bestConversionMove = Tuple.Create(startX, startY, endX, endY);
                    }
                }
            }
        }

        return bestConversionMove;
    }

    /// <summary>
    /// Tries to make a move for the current player.
    /// </summary>
    /// <param name="currentPlayer">The current player's color ("white" or "black").</param>
    private void TryMakeMove(string currentPlayer)
    {
        Tuple<int, int, int, int>? bestCaptureMove = null;
        Tuple<int, int, int, int>? bestNonCaptureMove = null;
        Tuple<int, int, int, int>? bestDangerousMove = null;

        var moveMade = false;

        for (var startX = 0; startX < Rows && !moveMade; startX++)
        for (var startY = 0; startY < Cols && !moveMade; startY++)
        {
            var jesterConversionMove = TryConvertEnemyWithJester(currentPlayer);
            if (jesterConversionMove != null)
            {
                MoveAndUpdateBoard(jesterConversionMove.Item1, jesterConversionMove.Item2, jesterConversionMove.Item3,
                    jesterConversionMove.Item4);
                return;
            }

            var piece = GetPiece(startX, startY);
            var pieceObj = PieceFactory.CreatePiece(piece, currentPlayer);

            if (piece != '.' && pieceObj?.Player == currentPlayer && IsCorrectColor(piece, currentPlayer))
                moveMade = TryMovesForPiece(startX, startY, currentPlayer, pieceObj, ref bestCaptureMove,
                    ref bestNonCaptureMove, ref bestDangerousMove);
        }

        if (!moveMade) MakeBestNonCaptureOrDangerousMove(bestNonCaptureMove, bestDangerousMove);
    }

    /// <summary>
    /// Tries different moves for a specific piece.
    /// </summary>
    /// <param name="startX">The starting X position of the piece.</param>
    /// <param name="startY">The starting Y position of the piece.</param>
    /// <param name="currentPlayer">The current player's color ("white" or "black").</param>
    /// <param name="pieceObj">The piece object representing the piece to move.</param>
    /// <param name="bestCaptureMove">The best capture move found so far.</param>
    /// <param name="bestNonCaptureMove">The best non-capture move found so far.</param>
    /// <param name="bestDangerousMove">The best dangerous move found so far.</param>
    /// <returns>True if a move is made, false otherwise.</returns>
    private bool TryMovesForPiece(int startX, int startY, string currentPlayer, Piece pieceObj,
        ref Tuple<int, int, int, int>? bestCaptureMove, ref Tuple<int, int, int, int>? bestNonCaptureMove,
        ref Tuple<int, int, int, int>? bestDangerousMove)
    {
        var moveMade = false;

        for (var endX = 0; endX < Rows && !moveMade; endX++)
        for (var endY = 0; endY < Cols && !moveMade; endY++)
            if (pieceObj.IsValidMove(this, startX, startY, endX, endY) &&
                !IsGeneralInDangerAfterMove(startX, startY, endX, endY, currentPlayer, false))
                moveMade = EvaluateAndExecuteMove(startX, startY, endX, endY, currentPlayer, ref bestCaptureMove,
                    ref bestNonCaptureMove);
            else if (pieceObj.IsValidMove(this, startX, startY, endX, endY) &&
                     IsGeneralInDangerAfterMove(startX, startY, endX, endY, currentPlayer, false) &&
                     bestDangerousMove == null)
                bestDangerousMove = Tuple.Create(startX, startY, endX, endY);
        return moveMade;
    }
}