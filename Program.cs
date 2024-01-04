using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Card
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsMatched { get; set; }
    public string Symbol { get; set; }
}

internal class Program
{
    static Random random = new Random();
    static string[] symbolsEasy = new string[] { "A", "B", "C" ,"D"};
    static string[] symbolsHard = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" };
    static string[] symbols;
    static List<Card> cards;
    static List<Card> firstCards;
    static List<Card> secondCards;
    static List<string> matchedSymbols;
    static List<string> unmatchedSymbols = new List<string>();
    const int initialTrial = 25;
    static int trial = initialTrial;
    static string inputFileName;
    static int attempts;
    static string playerName;
    static int level;


    static string[,] CardPosition;
    static string[,] CardPosition1;
    //Sets up the CardPosition and CardPosition1 arrays based on the selected difficulty level.
    static void SetCardPositions()
    {
        int rows, columns;

        switch (level)
        {
            case 1:
                rows = 2;
                columns = 2;
               
                break;
           
            case 2:
                rows = 4;
                columns = 4;
               
                break;
            default:
                Console.WriteLine("Invalid difficulty level.");
                return;
        }

        CardPosition = new string[rows, columns];
        CardPosition1 = new string[rows, columns];


        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                CardPosition[i, j] = "";
                CardPosition1[i, j] = "";
            }
        }
    }




    static void Main()
    {

        char fillChar = '=';
        string gameInstruction = "GAME INSTRUCTION";
        string gameSettings = "GAME SETTINGS";
        while (true)

        {
            Console.Clear();
            DisplayGameInstructions();

            Console.Write("Enter 'y' if you want to see more instructions, enter any key to skip:");
            string userInput = Console.ReadLine();
            HorizontalLine(fillChar);

            if (userInput != "n")
            {
                Console.Clear();
                Console.WriteLine("");
                //Console.WriteLine("skipped");
                //HorizontalLine(fillChar);
                break;
            }
            else if (userInput.ToLower() == "y")
            {
                Console.Clear();
                HorizontalLine(fillChar);
                CenterText(gameInstruction);
                Console.WriteLine("");
                HorizontalLine(fillChar);
                DisplayGameInstructionsFromFile();
                break;

            }
        }
        while (true)
        {
            HorizontalLine(fillChar);
            CenterText(gameSettings);
            Console.WriteLine("");
            HorizontalLine(fillChar);
            Console.Write("Enter username: ");
            playerName = Console.ReadLine();
            Console.Write("");
            HorizontalLine(fillChar);
            level = GetDifficultyLevel();
            SetSymbols();
            SetCardPositions();
            
            Console.Write("Enter 's' to start or 'q' to quit:");
            string startInput = Console.ReadLine();
            HorizontalLine(fillChar);


            if (startInput.ToLower() == "q")
            {
                Console.WriteLine("oops! you quit the game.");
                return;
            }
            else if (startInput.ToLower() == "s")
            {
                Console.Clear();
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 's' or 'q'.");
            }
        }

        StartMemoryCardGame();


    }








    //Displays the welcome message and game instructions.
    public static void DisplayGameInstructions()
    {


        char fillChar = '=';

        HorizontalLine(fillChar);

        string text = "WELCOME TO MEMORY GAME!";
        CenterText(text);
        Console.WriteLine();
        HorizontalLine(fillChar);
        Console.WriteLine();
        Console.WriteLine("The objective of the game is to find matching pairs of cards.\nEach card contains a symbol  and a content. \nYou need to enter two alphabets (symbols) in each turn \nand the game will tell you if they match or not.");
        Console.WriteLine("\n \n ");
        HorizontalLine(fillChar);
    }
    //Reads and displays additional game instructions from a file.
    public static void DisplayGameInstructionsFromFile()
    {
        const string instructionsFileName = "instructions.txt";

        try
        {
            using (StreamReader instructionsFile = new StreamReader(instructionsFileName))
            {
                string line;
                while ((line = instructionsFile.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine("\n \n");
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("An error occurred while reading the file: " + e.Message);
        }
    }
    //Prints a horizontal line of a specified character to the console.
    public static void HorizontalLine(char fillChar)
    {
        Console.WriteLine(new string(fillChar, Console.WindowWidth));
    }
    //Centers a given text within the console window.
    public static void CenterText(string text)
    {
        int screenWidth = Console.WindowWidth;
        int textWidth = text.Length;
        int leftPadding = (screenWidth - textWidth) / 2;

        Console.WriteLine("{0," + leftPadding + "}{1}", "", text);
    }
    //Takes user input to choose a difficulty level.
    public static int GetDifficultyLevel()
    {
        int level;

        while (true)
        {
            Console.WriteLine("Choose a difficulty level:");
            Console.WriteLine("1. Easy");
            //Console.WriteLine("2. Medium");
            Console.WriteLine("2. Hard");
            string input = Console.ReadLine();

            if (int.TryParse(input, out level) && level >= 1 && level <= 2)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 2.");
            }
        }

        return level;
    }
    //Sets the input file name based on the selected difficulty level.
    public static void SetInput()
    {


        switch (level)
        {
            case 1:
                inputFileName = "easyinput.txt";
                break;

            case 2:
                inputFileName = "input.txt";
                break;
            default:
                Console.WriteLine("Invalid level");
                return;
        }
    }
    //Retrieves the content of a card by its ID.
    public static string GetCardContentById(int id)
    {
        foreach (Card card in cards)
        {
            if (card.Id == id)
            {
                return card.Content;
            }
        }

        return null;
    }
    //Retrieves the ID of a card by its symbol.
    public static int GetIdBySymbol(string symbol)
    {
        foreach (Card card in firstCards)
        {
            if (card.Symbol == symbol)
            {
                // Check if the current card is matched, and if not, return the Id
                if (!card.IsMatched)
                {
                    return card.Id;
                }
            }
        }
        foreach (Card card in secondCards)
        {
            if (card.Symbol == symbol)
            {
                // Check if the current card is matched, and if not, return the Id
                if (!card.IsMatched)
                {
                    return card.Id;
                }
            }
        }

        return -1; // Symbol not found
    }
    //Sets the symbols array based on the selected difficulty level.
    public static void SetSymbols()
    {
        switch (level)
        {
            case 1:
                symbols = symbolsEasy;
                break;
            
            case 2:
                symbols = symbolsHard;
                break;
            default:
                Console.WriteLine("Invalid difficulty level.");
                break;
        }
    }
    //Sets up the initial game field based on the difficulty level.
    public static void SetField()
    {
        string[,] playFieldInitial;

        switch (level)
        {
            case 1:
                playFieldInitial = new string[,] 
                {
                    { "A", "B" },
                    { "C", "D" } 
                };
                CardPosition1 = playFieldInitial;
                UpdateMatchedField(matchedSymbols);
                DisplayFieldEasy();
                break;
           
            case 2:
                playFieldInitial = new string[,]
                { 
                    { "A", "B", "C", "D" },
                    { "E", "F", "G", "H" }, 
                    { "I", "J", "K", "L" }, 
                    { "M", "N", "O", "P" } 
                };

                CardPosition1 = playFieldInitial;
                UpdateMatchedField(matchedSymbols);
                DisplayFieldHard();
                
                break;
            default:
                playFieldInitial = new string[,] { };
                CardPosition1 = playFieldInitial;
                break;
        }


        UpdateMatchedField(matchedSymbols);
        // DisplayFieldHard();

    }
    //Reads card data from a file and creates card objects.
    public static List<Card> ReadCardsFromFile(string inputFileName)
    {
        cards = new List<Card>();
        firstCards = new List<Card>();
        secondCards = new List<Card>();
        matchedSymbols = new List<string>();

        try
        {
            using (StreamReader inputFile = new StreamReader(inputFileName))
            {
                while (!inputFile.EndOfStream)
                {

                    string line = inputFile.ReadLine();
                    string[] content = line.Split(',');

                    for (int i = 0; i < content.Length; i++)
                    {
                        if (string.IsNullOrEmpty(content[i]))
                        {
                            continue;
                        }
                        Card firstCard = new Card { Id = cards.Count / 2 + 1, Content = content[i] };
                        Card secondCard = new Card { Id = firstCard.Id, Content = firstCard.Content };
                        Card card = new Card { Id = cards.Count / 2 + 1, Content = content[i] };
                        firstCards.Add(firstCard);
                        secondCards.Add(secondCard);
                        cards.Add(firstCard);
                        cards.Add(secondCard);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while reading the file: " + e.Message);
        }

        return cards;
    }
    //Assigns unique positions and symbols to the cards.
    public static void AssignCardPositions(List<Card> anycards)
    {

        foreach (Card card in anycards)
        {
            int i, j;

            // Find a unique random position in CardPosition
            do
            {
                i = random.Next(0, CardPosition.GetLength(0));
                j = random.Next(0, CardPosition.GetLength(1));
            } while (!string.IsNullOrEmpty(CardPosition[i, j]));

            CardPosition[i, j] = card.Id.ToString();
            card.Symbol = symbols[i * CardPosition.GetLength(1) + j];

            //Console.WriteLine($"Card {card.Id}: {card.Content} :{card.Symbol}");
        }

    }
    //Takes user input for card selections.
    public static string GetUserInput()
    {
        unmatchedSymbols = symbols.Except(matchedSymbols).ToList();
        while (true)
        {
            HorizontalLine('-');
            Console.WriteLine("player : " + playerName);
            Console.WriteLine("available cell: " + string.Join(", ", unmatchedSymbols));
            Console.WriteLine("Number of attempts : " + initialTrial);
            Console.WriteLine("Number of attempts left: " + trial);
            Console.WriteLine("");
            HorizontalLine('-');
            Console.Write("Enter a letter:");
            char userInputChar = Console.ReadKey().KeyChar;
            Console.WriteLine("");


            if (char.IsLetter(userInputChar))
            {
                string userInput = userInputChar.ToString().ToUpper();

                if (matchedSymbols.Contains(userInput) || !symbols.Contains(userInput))
                {
                    //unmatchedSymbols = symbols.Except(matchedSymbols).ToList();
                    Console.WriteLine("try: " + string.Join(", ", unmatchedSymbols));
                    continue;
                }


                else
                {

                    return userInput;
                }

            }

            Console.WriteLine("Please enter a valid letter.");
        }
    }
    //Checks if the selected cards match and updates the game state accordingly.
    public static void CheckCardInputs(string cardInput1, string cardInput2)
    {
        int firstPick = GetIdBySymbol(cardInput1);
        int secondPick = GetIdBySymbol(cardInput2);

        if (cardInput1 != cardInput2)
        {
            if (firstPick == secondPick && firstPick != -1 && secondPick != -1)
            {
                string matchedContent = GetCardContentById(firstPick);
                HorizontalLine('-');
                Console.WriteLine("We have a match!");
                Console.WriteLine($"Symbols: {cardInput1}, {cardInput2}");
                Console.WriteLine($"Content: {matchedContent}");
                matchedSymbols.Add(cardInput1);
                matchedSymbols.Add(cardInput2);

            }
            else
            {
                string firstCardContent = GetCardContentById(firstPick);
                string secondCardContent = GetCardContentById(secondPick);
                HorizontalLine('-');
                Console.WriteLine("Try again!");
                Console.WriteLine($"Symbol: {cardInput1}");
                Console.WriteLine($"Content: {firstCardContent}");
                Console.WriteLine();
                Console.WriteLine($"Symbol: {cardInput2}");
                Console.WriteLine($"Content: {secondCardContent}");
            }
        }
        else
        {
            Console.WriteLine("You can't input the same letters.");

        }
    }
    //Displays the game field for the hard difficulty level.
    public static void DisplayFieldHard()
    {

        Console.WriteLine("     |     |     |    ");
        Console.WriteLine("  {0}  |  {1}  |  {2}  |  {3}  ", CardPosition1[0, 0], CardPosition1[0, 1], CardPosition1[0, 2], CardPosition1[0, 3]);
        Console.WriteLine("_____|_____|_____|_____");
        Console.WriteLine("     |     |     |");
        Console.WriteLine("  {0}  |  {1}  |  {2}  |  {3}  ", CardPosition1[1, 0], CardPosition1[1, 1], CardPosition1[1, 2], CardPosition1[1, 3]);
        Console.WriteLine("_____|_____|_____|_____");
        Console.WriteLine("     |     |     |");
        Console.WriteLine("  {0}  |  {1}  |  {2}  |  {3}  ", CardPosition1[2, 0], CardPosition1[2, 1], CardPosition1[2, 2], CardPosition1[2, 3]);
        Console.WriteLine("_____|_____|_____|_____");
        Console.WriteLine("     |     |     |");
        Console.WriteLine("  {0}  |  {1}  |  {2}  |  {3}  ", CardPosition1[3, 0], CardPosition1[3, 1], CardPosition1[3, 2], CardPosition1[3, 3]);
        Console.WriteLine("     |     |     |");

    }
    //Displays the game field for the easy difficulty level.
    public static void DisplayFieldEasy()
    {
        Console.WriteLine("     |     |");
        Console.WriteLine("  {0}  |  {1}  |", CardPosition1[0, 0], CardPosition1[0, 1]);
        Console.WriteLine("_____|_____|");
        Console.WriteLine("     |     |    ");
        Console.WriteLine("  {0}  |  {1}  |", CardPosition1[1, 0], CardPosition1[1, 1]);
        Console.WriteLine("     |     |");

    }
    //Controls the main game loop, managing user input, card matching, and game state.
    public static void PlayGame()
    {
        Console.Clear();
        string text = "START GAME";
        HorizontalLine('=');
        CenterText(text);
        Console.WriteLine("");
        HorizontalLine('=');
        string capPlayerName = playerName.ToUpper();
        string congrateText = "CONGRATULATION " + capPlayerName + " YOU WON!";
        string maxText = "GAMEOVER! " + capPlayerName + " YOU HAVE REACHED THE MAXIMUM ATTEMPTS";
        
        while (attempts < initialTrial)
        {

            SetField();

            Console.WriteLine("\nEnter two alphabets below:");

            string cardInput1 = GetUserInput();
            string cardInput2 = GetUserInput();
            CheckCardInputs(cardInput1, cardInput2);
            if (matchedSymbols.Count == symbols.Length)
            {
                HorizontalLine('=');
                CenterText(congrateText);
                Console.WriteLine("");
                HorizontalLine('=');
                SaveGame();
                break;
            }
            trial--;
            attempts++;
        }

        if (matchedSymbols.Count == symbols.Length)
        {
            Console.WriteLine("Press any key to start again or 'q' to quit.");
            string userInput = Console.ReadLine();
            if (userInput.ToLower() == "q")
            {
                Console.WriteLine("Game over. You quit the game.");
                return;
            }
            else
            {
                attempts = 0;
                matchedSymbols.Clear();
                trial = initialTrial;

                //cards.Clear();
                //firstCards.Clear();
                //econdCards.Clear();

                PlayGame();
                //StartMemoryCardGame();
            }
        }
        else
        {
            HorizontalLine('=');
            CenterText(maxText);
            Console.WriteLine("");
            HorizontalLine('=');
            Console.WriteLine("Press any key to start again or 'q' to quit.");
            string userInput = Console.ReadLine();
            if (userInput.ToLower() == "q")
            {
                Console.WriteLine("Game over. You quit the game.");
                return;
            }
            else
            {
                attempts = 0;
                matchedSymbols.Clear();
                trial = initialTrial;
                //cards.Clear();
                //firstCards.Clear();
                //secondCards.Clear();

                PlayGame();
                //StartMemoryCardGame();
            }
            SaveGame();
        }
    }
    //Sets up the game environment and starts the memory card game.
    public static void StartMemoryCardGame()
    {
        //Console.Clear();
        SetInput();
        ReadCardsFromFile(inputFileName);

        AssignCardPositions(firstCards);
        AssignCardPositions(secondCards);
        Console.WriteLine();
        PlayGame();
    }
    //Updates the displayed game field to mark matched symbols.
    public static void UpdateMatchedField(List<string> matchedSymbols)
    {
        for (int row = 0; row < CardPosition1.GetLength(0); row++)
        {
            for (int col = 0; col < CardPosition1.GetLength(1); col++)
            {
                if (matchedSymbols.Contains(CardPosition1[row, col]))
                {
                    CardPosition1[row, col] = "X";
                }
            }
        }
    }
    //Retrieves a specific property(Id, Symbol, Content) of a card at a given position.
    public static string GetPropertyByPosition(int row, int column, string property)
    {
        string value = CardPosition[row, column];

        switch (property)
        {
            case "Id":
                return value;
            case "Symbol":
                Card card = GetCardByPosition(row, column);
                return card.Symbol;
            case "Content":
                int id = int.Parse(value);
                return GetCardContentById(id);
            default:
                return value;
        }
    }
    //Retrieves the card object at a given position.
    public static Card GetCardByPosition(int row, int column)
    {
        string value = CardPosition[row, column];
        int id = int.Parse(value);

        foreach (Card card in cards)
        {
            if (card.Id == id)
            {
                return card;
            }
        }

        return null;
    }
    //Saves the current game state to a file.
    public static void SaveGame()
    {
        string saveFile = "savegame.txt"
; using (StreamWriter file = new StreamWriter(saveFile))
        {

            file.WriteLine(playerName);


            string matchedSymbolsStr = string.Join(",", matchedSymbols);
            file.WriteLine(matchedSymbolsStr);


            file.WriteLine(trial);

            // Save the current field
            for (int i = 0; i < CardPosition1.GetLength(0); i++)
            {
                for (int j = 0; j < CardPosition1.GetLength(1); j++)
                {
                    file.WriteLine("{0},{1},{2}", i, j, CardPosition1[i, j]);
                }
            }
        }
    }
    //Handles the event of the console closing by saving the game.
    public static void ConsoleClosing(object sender, EventArgs e)
    {
        SaveGame();
    }

}