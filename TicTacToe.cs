/*This program is my clone of Tic Tac Toe.
 Version 1.0 Implemented the whole beginning up to coin toss. Implemented 2 player mode with only tie
    as finish condition
Version 2.0 Implemented the winning conditions and the different AI behaviours
Version 3.0 Performed some refactoring. Improved AI_Hard, now it plays to win
Created by OuterGazer*/

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

class TicTacToe
{
    //These variables are used in different methods and are declared here for clarity
    static string beginningPlayer; //This 4 strings will be used during the loop for square choosing
    static string opposingPlayer;
    static string beginningPlayerToken;
    static string opposingPlayerToken;
    static string nameEasyAI = "Crazy Bob";
    static string nameMediumAI = "Easygoing Amy";
    static string nameHardAI = "Relentless Sam";

    static int chosenDifficulty;

    static bool beginningPlayerBool = false;    //bool variables that will store who wins, used in more than 1 method
    static bool opposingPlayerBool = false;
    static bool checkHorizontal;
    static bool checkVertical;
    static bool checkDiagonal;

    /// <summary>
    /// Code necesary to print underlined characters and strings in the console
    /// </summary>
    const int STD_OUTPUT_HANDLE = -11;
    const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    private static string WriteUnderline(string s)
    {
        var handle = GetStdHandle(STD_OUTPUT_HANDLE);
        uint mode;
        GetConsoleMode(handle, out mode);
        mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        SetConsoleMode(handle, mode);
        return s = $"\x1B[4m{s}\x1B[24m";
    }
    /// <summary>
    /// This method will check after every token placement if a winning condition is met.
    /// </summary>
    /// <param name="boardPosition">The array with the -1 and -2 values as token positions</param>
    static void VictoryCheck(int[,] boardPosition)
    {
        for(int i = 0; i < boardPosition.GetLength(0); i++) //check horizontal lines
        {            
            if (boardPosition[i, 0] < 0)
            {
                if(boardPosition[i, 1] < 0)
                {
                    if(boardPosition[i, 2] < 0)
                    {
                        WhichPlayerWins(boardPosition[i, 0],
                                    boardPosition[i, 1],
                                    boardPosition[i, 2]);
                    }
                }
            }
        }

        for (int i = 0; i < boardPosition.GetLength(0); i++) //check vertical lines
        {           
            if (boardPosition[0, i] < 0)
            {
                if (boardPosition[1, i] < 0)
                {
                    if (boardPosition[2, i] < 0)
                    {
                        WhichPlayerWins(boardPosition[0, i],
                                    boardPosition[1, i],
                                    boardPosition[2, i]);
                    }
                }
            }
        }
               
        if (boardPosition[0, 0] < 0)    //Check diagonal upper left to lower right
        {
            if (boardPosition[2, 2] < 0)
            {
               if (boardPosition[1, 1] < 0)
               {
                    WhichPlayerWins(boardPosition[0, 0],
                                boardPosition[2, 2],
                                boardPosition[1, 1]);
               }
            }            
        }
        if (boardPosition[2, 0] < 0)    //Check diagonal lower left to upper right
        {
            if (boardPosition[0, 2] < 0)
            {
                if (boardPosition[1, 1] < 0)
                {
                    WhichPlayerWins(boardPosition[2, 0],
                                boardPosition[0, 2],
                                boardPosition[1, 1]);
                }
            }
        }
    }
    /// <summary>
    /// This method is the one that accurately calculates which player wins
    /// </summary>
    /// <param name="square1">1st square to check</param>
    /// <param name="square2">2nd square to check</param>
    /// <param name="square3">3rd square to check</param>
    /// <returns>true to the player who wins, false if no win condition is met</returns>
    static bool WhichPlayerWins(int square1, int square2, int square3)
    {
        if ((square1 == -1) && (square2 == -1) && (square3 == -1))
        {
            return beginningPlayerBool = true;
        }

        if ((square1 == -2) && (square2 == -2) && (square3 == -2))
        {
            return opposingPlayerBool = true;
        }

        return false;
    }
    /// <summary>
    /// Method to avoid that players enter blank names
    /// </summary>
    /// <param name="prompt">Prompt to the appropriate player</param>
    /// <param name="name">Returning name</param>
    static void EnterPlayerName(string prompt, out string name)
    {
        do
        {
            Console.Write(prompt);
            name = Console.ReadLine().Trim();
            if(name.Length == 0)
            {
                Console.WriteLine("The name can't be blank.");
                continue;
            }
            else
            {
                break;
            }

        } while (true);
        
    }
    /// <summary>
    /// Method that looks for a square for the AI to place the token
    /// </summary>
    /// <param name="boardPosition"></param>
    /// <param name="exitValue"></param>
    /// <param name="yTemp"></param>
    /// <param name="xTemp"></param>
    /// <param name="foundASquare"></param>
    static void LookForASquare(int[,] boardPosition, int exitValue,
        ref int yTemp, ref int xTemp, ref bool foundASquare)
    {
        for (int i = 0; i < boardPosition.GetLength(0); i++) 
        {
            for (int j = 0; j < boardPosition.GetLength(1); j++)
            {
                if (boardPosition[i, j] == exitValue)
                {
                    yTemp = i;
                    xTemp = j;
                    foundASquare = true;
                    return;
                }
            }
        }
    }

    static void SetSquareTo_3_Or_2(int[,] boardPosition, int counter, int referenceSquare)
    {
        if ((beginningPlayer == nameHardAI) || (beginningPlayer == nameMediumAI) ||
            (opposingPlayer == nameHardAI) || (opposingPlayer == nameMediumAI))
        {
            if((checkHorizontal == true) || (checkDiagonal == true)) //if AI coul win, we set a 3 in the remaining square
            {
                boardPosition[counter, referenceSquare] = 3;
            }
            if (checkVertical == true)
            {
                boardPosition[referenceSquare, counter] = 3;
            }

        }
        else
        {
            if ((checkHorizontal == true) || (checkDiagonal == true)) //if instead player could win, we set a 2 in the remaining square
            {
                boardPosition[counter, referenceSquare] = 2;
            }
            if (checkVertical == true)
            {
                boardPosition[referenceSquare, counter] = 2;
            }
        }
    }
    /// <summary>
    /// This method is the programming for the AI behaviour in hard difficulty. It will first set a value of 3
    /// in the most important square at that moment, then it will look for this square and put the token there.
    /// The most important square will be the AI to do 3 in line, if there isn't chance and the player will do
    /// 3 in line, a 2 will be put in that square.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="board"></param>
    /// <param name="boardValues"></param>
    /// <param name="y"></param>
    /// <param name="x"></param>
    static void AI_Hard(string token, string[,] board, int[,] boardPosition, out int y, out int x)
    {
        int yTemp = 0;
        int xTemp = 0;
        bool foundASquare = false;
        checkHorizontal = false; //These booleans will be used to know specifically which squares to assign a 3 or 2
        checkVertical = false;   //in the SetSquareTo_3_Or_2 method
        checkDiagonal = false;

        checkHorizontal = true;
        for (int i = 0; i < boardPosition.GetLength(0); i++)    //Loop to check horizontal lines
        {            
            if((boardPosition[i, 0] < 0) || (boardPosition[i, 1] < 0) || (boardPosition[i, 2] < 0))
            {
                if ((boardPosition[i, 0] == boardPosition[i, 1]) && (boardPosition[i, 2] >= 0))
                {
                    SetSquareTo_3_Or_2(boardPosition, i, 2);
                }
                if ((boardPosition[i, 0] == boardPosition[i, 2]) && (boardPosition[i, 1] >= 0))
                {
                    SetSquareTo_3_Or_2(boardPosition, i, 1);
                }
                if ((boardPosition[i, 1] == boardPosition[i, 2]) && (boardPosition[i, 0] >= 0))
                {
                    SetSquareTo_3_Or_2(boardPosition, i, 0);
                }
            }            
        }
        checkHorizontal = false;

        checkVertical = true;
        for (int i = 0; i < boardPosition.GetLength(0); i++)    //Loop to check vertical lines
        {            
            if ((boardPosition[0, i] < 0) || (boardPosition[1, i] < 0) || (boardPosition[2, i] < 0))
            {
                if ((boardPosition[0, i] == boardPosition[1, i]) && (boardPosition[2, i] >= 0))
                {
                    SetSquareTo_3_Or_2(boardPosition, i, 2);
                }
                if ((boardPosition[0, i] == boardPosition[2, i]) && (boardPosition[1, i] >= 0))
                {
                    SetSquareTo_3_Or_2(boardPosition, i, 1);
                }
                if ((boardPosition[1, i] == boardPosition[2, i]) && (boardPosition[0, i] >= 0))
                {
                    SetSquareTo_3_Or_2(boardPosition, i, 0);
                }
            }
        }
        checkVertical = false;

        //Check diagonal for upper left to lower right
        checkDiagonal = true;
        if ((boardPosition[0, 0] < 0) || (boardPosition[2, 2] < 0))
        {
            if ((boardPosition[0, 0] == boardPosition[1, 1]) && (boardPosition[2, 2] >= 0))
            {
                SetSquareTo_3_Or_2(boardPosition, 2, 2);
            }            
            if ((boardPosition[2, 2] == boardPosition[1, 1]) && (boardPosition[0, 0] >= 0))
            {
                SetSquareTo_3_Or_2(boardPosition, 0, 0);
            }
        }

        //Check diagonal for lower left to upper right
        if ((boardPosition[2, 0] < 0) || (boardPosition[0, 2] < 0))
        {
            if ((boardPosition[2, 0] == boardPosition[1, 1]) && (boardPosition[0, 2] >= 0))
            {
                SetSquareTo_3_Or_2(boardPosition, 0, 2);
            }
            if ((boardPosition[0, 2] == boardPosition[1, 1]) && (boardPosition[2, 0] >= 0))
            {
                SetSquareTo_3_Or_2(boardPosition, 2, 0);
            }
        }
        checkDiagonal = false;

        //after assigning a 3 or a 2, we look for it with this loop. If there aren't 3 or 2s, a square with a 1 or 0
        //will be assigned instead
        for (int i = 3; i >= 0; i--)
        {
            LookForASquare(boardPosition, i, ref yTemp, ref xTemp, ref foundASquare);
            
            if (foundASquare == true)
            {
                break;
            }
        }

        //Here we assign the AI to the square itself
        y = yTemp;
        x = xTemp;

        board[y + 1, (x + 1) * 2] = WriteUnderline(token);
    }
    /// <summary>
    /// Method for the AI Behavior in the easy difficulty
    /// </summary>
    /// <param name="token">Respective token</param>
    /// <param name="board">The string board</param>
    /// <param name="y">y coordinate of the board</param>
    /// <param name="x">x coordinate of the board</param>
    static void AI_Easy(string token, string[,] board, out int y, out int x)
    {
        Random Y = new Random();
        Random X = new Random();
        
        do
        {
            y = Y.Next(0, 3);
            x = X.Next(0, 3);
            if (board[y + 1, (x + 1) * 2] == "_")
            {
                board[y + 1, (x + 1) * 2] = WriteUnderline(token);
                break;
            }
            else
            {
                continue;
            }

        } while (true);
    }
    /// <summary>
    /// This is the main method that programs the AI behaviour each turn
    /// </summary>
    /// <param name="player">Respective name of the AI</param>
    /// <param name="token">Respective token assigned at the beginning</param>
    /// <param name="board">String board with tokens</param>
    /// <param name="boardPosition">Value Board for -1 and -2</param>
    static void AI_Behaviour(string player, string token, ref string[,] board, ref int[,] boardPosition)
    {
        Random AI_Medium = new Random();
        int yCoordinate = 0;
        int xCoordinate = 0;

        switch (chosenDifficulty)
        {
            case (1):
                AI_Easy(token, board, out yCoordinate, out xCoordinate);
                break;
            case (2):
                int AI_Choice = AI_Medium.Next(1, 3);
                    if(AI_Choice == 1)
                    {
                        AI_Easy(token, board, out yCoordinate, out xCoordinate);
                    }
                    else
                    {
                        AI_Hard(token, board, boardPosition, out yCoordinate, out xCoordinate);
                    }
                break;
            case (3):
                AI_Hard(token, board, boardPosition, out yCoordinate, out xCoordinate);
                break;
            default:
                break;
        }

        //Depending if AI plays first o second we assign -1 or -2 to their square for the VictoryCheck method
        if (player.Equals(beginningPlayer) == true)
        {
            boardPosition[yCoordinate, xCoordinate] = -1;
        }
        if (player.Equals(opposingPlayer) == true)
        {
            boardPosition[yCoordinate, xCoordinate] = -2;
        }
    }

    /// <summary>
    /// This method handles each player's turn
    /// </summary>
    /// <param name="player">The name of the player</param>
    /// <param name="token">The chosen token</param>
    static void PlayerBehaviour(string player, string token, ref string[,] board, ref int[,] boardPosition)
    {
        Console.Clear();

        int xCoordinate;
        int yCoordinate;

        PrintBoard(board);
        Console.WriteLine();

        do
        {
            //player choose where to play
            yCoordinate = ReadInt(player + ", write the row of the square that you " +
            "want to place your token", 0, 2) + 1; //+1 because the first row of our board is the coordinate
            xCoordinate = (ReadInt(player + ", write the column of the square that you " +
                "want to place your token", 0, 2) + 1) * 2; //(x+1)*2 will place the token in the columns 2, 4 and 6

            //We check that the square isn't taken
            switch (board[yCoordinate, xCoordinate])
            {
                case ("_"):
                    board[yCoordinate, xCoordinate] = WriteUnderline(token);
                    break;
                default:                    
                    Console.WriteLine("That square is taken, choose another one.");
                    continue;
            }

            break;
        } while (true);

        //we place the player's token in the chosen square
        if(player.Equals(beginningPlayer) == true)
        {
            boardPosition[yCoordinate - 1, (xCoordinate / 2) - 1] = -1;
        }
        if (player.Equals(opposingPlayer) == true)
        {
            boardPosition[yCoordinate - 1, (xCoordinate / 2) - 1] = -2;
        }

        //The next 4 if statements cover the case at the beginning of the game where the AI has the center square
        //and he player chooses a corner. It then sets the value of the opposite corner to 0 to make sure
        //that the AI doesn't choose that square, as it wouldn't be a sensible choice
        if((boardPosition[1, 1] < 0) && (boardPosition[0, 0] < 0) && boardPosition[2, 2] == 1)
        {
            boardPosition[2, 2] = 0;
        }
        if ((boardPosition[1, 1] < 0) && (boardPosition[0, 2] < 0) && boardPosition[2, 0] == 1)
        {
            boardPosition[2, 0] = 0;
        }
        if ((boardPosition[1, 1] < 0) && (boardPosition[2, 2] < 0) && boardPosition[0, 0] == 1)
        {
            boardPosition[0, 0] = 0;
        }
        if ((boardPosition[1, 1] < 0) && (boardPosition[2, 0] < 0) && boardPosition[0, 2] == 1)
        {
            boardPosition[0, 2] = 0;
        }
    }

    /// <summary>
    /// Method to assign the token to the opponent once the player 1 chooses their token
    /// </summary>
    /// <param name="tokenP1">the token chosen from Player 1</param>
    /// <param name="token2">the token that will be assigned to the opponent</param>
    static void AssignToken(string tokenP1, out string token2)
    {
        if (tokenP1 == "X")
        {
            token2 = "O";
        }
        else
        {
            token2 = "X";
        }
    }
    /// <summary>
    /// Method to avoid string inputs from breaking the program when a numeric value is the expected input to a question
    /// </summary>
    /// <param name="prompt">THe question that expects a number as answer</param>
    /// <param name="min">the minimal value of the answer</param>
    /// <param name="max">the maximal value of the answer</param>
    /// <returns>The answer itself as integer</returns>
    static int ReadInt(string prompt, int min, int max)
    {
        string playerInput;
        int answerToPrompt;

        do
        {
            Console.WriteLine(prompt);
            playerInput = Console.ReadLine();
            try
            {
                answerToPrompt = int.Parse(playerInput);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                continue;
            }

            if ((answerToPrompt < min) || (answerToPrompt > max))
            {
                Console.WriteLine("That number is out of range. Write a number between {0} and {1}", min, max);
                continue;
            }

            break;

        } while (true);

        return answerToPrompt;
    }
    /// <summary>
    /// Method to check for proper input from the player when a question with only two valid answers is made
    /// </summary>
    /// <param name="prompt">The question prompting 1 of 2 possible answers</param>
    /// <param name="option1">answer 1</param>
    /// <param name="option2"> answer 2</param>
    /// <param name="playerAnswer">strig to use in the program</param>
    static void YesNoPrompt(string prompt, string option1, string option2, out string playerAnswer)
    {
        do
        {
            Console.Write(prompt);
            playerAnswer = Console.ReadLine().Trim().ToUpper();
            if((playerAnswer != option1) && (playerAnswer != option2))
            {
                Console.WriteLine("Please choose exclusively {0} or {1}", option1, option2);
                continue;
            }
            else
            {
                break;
            }
        } while (true);

    }
    /// <summary>
    /// Structure that will hold the information on the players. Namely name and token chosen to play
    /// </summary>
    struct Player
    {
        public string Name;
        public string Token;
    }

    /// <summary>
    /// The board itself, how it will be grafically displayed
    /// </summary>
    /// <returns></returns>
    static string[,] GameBoard()
    {
        string[,] gameBoard = new string[4, 8] {
            {" ", " ", WriteUnderline("0"), " ", WriteUnderline("1"), " ", WriteUnderline("2"), " " },
            {"0", "|", "_", "|", "_", "|", "_", "|" },
            {"1", "|", "_", "|", "_", "|", "_", "|" },
            {"2", "|", "_", "|", "_", "|", "_", "|" }
        };

        return gameBoard;
    }
    /// <summary>
    /// This method is the standard board, with a 2 given to the most valuable square. It will be used
    /// to keep track of where the players play and also will be used to decide the AI moves
    /// </summary>
    /// <returns>An array that works as the gaming board</returns>
    static int[,] BoardValues()
    {
        int[,] board = new int[,] {
        { 1, 0, 1, },
        { 0, 2, 0, },
        { 1, 0, 1 }
        };

        return board;
    }
    /// <summary>
    /// This method prints the board after every move
    /// </summary>
    /// <param name="board"></param>
    static void PrintBoard(string[,] board)
    {
        Console.Clear();

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Console.Write(board[i, j]);
            }

            Console.WriteLine();
        }
    }
    static void Main()
    {
        string playAnotherRound;

        do
        {
            string chosenMode;  //this will hold whether the game is 1 player or 2 players
            string chosenCoinSide; //At the beginning, a coin will be tossed to decide which player begins
            string stringCoinResult = null;

            beginningPlayerBool = false;    //bool variables that will store who wins, used in more than 1 method
            opposingPlayerBool = false;

            Random coin = new Random(); //The coin that will be tossed to decide who starts

            Player Player1 = new Player();
            Player Opponent = new Player(); //This variable will contain either the 2P info or the AI info throughout the game            

            Console.Clear();

            Console.WriteLine("Welcome to our game of Tic Tac Toe!\n\nInstructions are as follow:");
            Console.WriteLine("\t- The objective of the game is to choose squares in order to make 3 in a line");
            Console.WriteLine("\t- The first player to succeed in making 3 in a line wins");
            Console.WriteLine("\t- Players will take turns to choose their squares");
            Console.WriteLine("\t- The player to make the first move will be chosen randomly");
            Console.WriteLine("\t- 1 Player and 2 Player modes supported");
            Console.WriteLine("\t- 3 different difficulty modes supported");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();

            YesNoPrompt("Select game mode [1P/2P]: ", "1P", "2P", out chosenMode);

            EnterPlayerName("\nEnter the name of player number 1: ", out Player1.Name);

            if (chosenMode == "2P")
            {
                EnterPlayerName("\nEnter the name of player number 2: ", out Opponent.Name);
            }

            YesNoPrompt("\nSelect your preferred token for the game between X and O, [X,O]", "X", "O", out Player1.Token);
            
            AssignToken(Player1.Token, out Opponent.Token);

            if (chosenMode != "2P") //Preparations to play against the AI
            {
                do
                {
                    chosenDifficulty =
                ReadInt("\nChoose the desired difficulty level:\n\t- 1. Easy\n\t- 2. Medium\n\t- 3. Hard", 1, 3);
                    Console.WriteLine();

                    switch (chosenDifficulty)
                    {
                        case (1):
                            Opponent.Name = nameEasyAI;
                            Console.WriteLine("You have chosen the easy difficulty. Your opponent will be "
                                + Opponent.Name + ".");
                            break;
                        case (2):
                            Opponent.Name = nameMediumAI;
                            Console.WriteLine("You have chosen the medium difficulty. Your opponent will be "
                                + Opponent.Name + ".");
                            break;
                        case (3):
                            Opponent.Name = nameHardAI;
                            Console.WriteLine("You have chosen the hard difficulty. Your opponent will be "
                                + Opponent.Name + ".");
                            break;
                        default:
                            Console.WriteLine("Wrong option.");
                            continue;
                    }

                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                } while (true);
            }

            //End of preparations
            Console.Clear();

            Console.WriteLine("Before beginning the game, we will throw a coin to decide who begins.");
            YesNoPrompt("What do you choose, heads or tails?[H/T]", "H", "T", out chosenCoinSide);
            int coinResult = coin.Next(1, 3);
           
            Console.WriteLine("Let's flip the coin! Press Enter to continue...");
            Console.ReadLine();
            switch (coinResult)
            {
                case (1):
                    Console.WriteLine("The coin shows heads!");
                    stringCoinResult = "H";
                    break;
                case (2):
                    Console.WriteLine("The coin shows tails!");
                    stringCoinResult = "T";
                    break;
            }
            if (chosenCoinSide == stringCoinResult)
            {
                Console.WriteLine(Player1.Name + ", you got lucky!. You will make the first move.");
                beginningPlayer = Player1.Name;
                beginningPlayerToken = Player1.Token;
                opposingPlayer = Opponent.Name;
                opposingPlayerToken = Opponent.Token;
            }
            else
            {
                Console.WriteLine(Player1.Name + ", you didn't get lucky!. " + Opponent.Name + " will begin");
                beginningPlayer = Opponent.Name;
                beginningPlayerToken = Opponent.Token;
                opposingPlayer = Player1.Name;
                opposingPlayerToken = Player1.Token;
            }

            Console.Write("Press Enter to continue...");
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Before beginning, let's make a brief summary of the information so far:" +
                " Press Enter afer each line...");
            Console.ReadLine();
            Console.WriteLine(Player1.Name + " will be playing against " + Opponent.Name + ".");
            Console.ReadLine();
            Console.WriteLine(Player1.Name + " will play with the token: " + Player1.Token + ".\n" +
                Opponent.Name + " will play with the token: " + Opponent.Token + ".");
            Console.ReadLine();
            Console.WriteLine("The player that will make the first move is " + beginningPlayer + ".");
            Console.WriteLine("Have fun!");
            Console.ReadLine();

            //The game begins here!
            string[,] board = GameBoard();
            int[,] boardValues = BoardValues();

            if (chosenMode == "2P") //Piece of code for 2 player mode
            {
                int counter = 0;
                do
                {
                    PlayerBehaviour(beginningPlayer, beginningPlayerToken, ref board, ref boardValues);

                    VictoryCheck(boardValues);
                    if (beginningPlayerBool == true)
                    {
                        PrintBoard(board);
                        Console.WriteLine();
                        Console.WriteLine(beginningPlayer + " has a 3 in line. " + beginningPlayer + " wins!");
                        break;
                    }

                    counter += 1;

                    if (counter == 9)
                    {
                        PrintBoard(board);
                        Console.WriteLine();
                        Console.WriteLine("Nobody has been able to do a 3 in line. It's a tie!");
                        break;
                    }
                    else
                    {
                        PlayerBehaviour(opposingPlayer, opposingPlayerToken, ref board, ref boardValues);

                        VictoryCheck(boardValues);
                        if (opposingPlayerBool == true)
                        {
                            PrintBoard(board);
                            Console.WriteLine();
                            Console.WriteLine(opposingPlayer + " has a 3 in line. " + opposingPlayer + " wins!");
                            break;
                        }

                        counter += 1;
                    }

                } while (true);
            }

            if (chosenMode != "2P")   //piece of code for 1 player mode
            {
                int counter = 0;
                do
                {
                    if ((beginningPlayer == nameEasyAI) || (beginningPlayer == nameMediumAI) ||
                        (beginningPlayer == nameHardAI))
                    {
                        AI_Behaviour(beginningPlayer, beginningPlayerToken, ref board, ref boardValues);
                    }
                    else
                    {
                        PlayerBehaviour(beginningPlayer, beginningPlayerToken, ref board, ref boardValues);
                    }

                    VictoryCheck(boardValues);
                    if (beginningPlayerBool == true)
                    {
                        PrintBoard(board);
                        Console.WriteLine();
                        Console.WriteLine(beginningPlayer + " has a 3 in line. " + beginningPlayer + " wins!");
                        break;
                    }

                    counter += 1;

                    if (counter == 9)
                    {
                        PrintBoard(board);
                        Console.WriteLine();
                        Console.WriteLine("Nobody has been able to do a 3 in line. It's a tie!");
                        break;
                    }
                    else
                    {
                        if ((opposingPlayer == nameEasyAI) || (opposingPlayer == nameMediumAI) ||
                        (opposingPlayer == nameHardAI))
                        {
                            AI_Behaviour(opposingPlayer, opposingPlayerToken, ref board, ref boardValues);
                        }
                        else
                        {
                            PlayerBehaviour(opposingPlayer, opposingPlayerToken, ref board, ref boardValues);
                        }

                        VictoryCheck(boardValues);
                        if (opposingPlayerBool == true)
                        {
                            PrintBoard(board);
                            Console.WriteLine();
                            Console.WriteLine(opposingPlayer + " has a 3 in line. " + opposingPlayer + " wins!");
                            break;
                        }

                        counter += 1;
                    }

                } while (true);
            }

            YesNoPrompt("\nDo you wish play again? [Y/N]", "Y", "N", out playAnotherRound);

        } while (playAnotherRound == "Y");

        Console.WriteLine("\nPress Enter to exit to Windows.");
        Console.ReadLine();
    }
}