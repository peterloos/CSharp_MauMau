#define VERBOSE
// #define SINGLE_STEP

using System;

class MauMaster
{
    private const String Version = "    Simple Mau-Mau Cards Game (Version 1.00)";
    private const int MaxCardsAtBegin = 5;  // used for testing - should be 5 regularly

    private CardDeck playing;  // deck to play (Kartenstapel zum Ablegen - offen)
    private CardDeck drawing;  // deck to draw (Kartenstapel zum Ziehen - verdeckt)
    private Player[] players;  // array of players
    private int rounds;        // counting rounds of a game

    // c'tor(s)
    public MauMaster(String[] names)
    {
        // create two card decks
        this.playing = new CardDeck();  // deck to play (Kartenstapel zum Ablegen)
        this.drawing = new CardDeck();  // deck to draw (Kartenstapel zum Ziehen)

        // create array of players
        this.players = new Player[names.Length];
        for (int i = 0; i < this.players.Length; i++)
        {
            this.players[i] = new Player(names[i]) { PlayingDeck = playing, DrawingDeck = drawing };
        }
    }

    // properties
    public int Rounds
    {
        get
        {
            return this.rounds;
        }
    }

    // public interface
    public void Init(int randomSeed)
    {
        // create new random generator (prefer unique results to make testing more easier)
        Random rand = new Random(randomSeed);
        this.playing.Rand = rand;
        this.drawing.Rand = rand;

        // intialize card decks
        this.playing.Clear();
        this.drawing.Fill();       // fill deck with all available cards ...
        this.drawing.Shuffle();    // ... and mix them ...

        for (int i = 0; i < this.players.Length; i++)
        {
            this.players[i].IsPlaying = true;
            this.players[i].DrawCards(MaxCardsAtBegin);  // draw initial amount of cards
        }

        this.rounds = 0;
    }

    public void Play()
    {
        // controlling game variables
        int numberOfCardsToDraw = 2;
        int activePlayers = this.players.Length;
        int currentPlayer = 0;
        CardColor requestedCardColor = CardColor.Empty;

        // controlling special games state 'Bube'
        bool bubeIsActive = false;

        // controlling special games states '8'
        bool eightIsActive = false;
        bool skipNextPlayer = false;

        // uncover first card
        Card firstCard = this.drawing.Pop();
        this.playing.Push(firstCard);

        // and now lets play an aweful game
        while (activePlayers > 1)
        {
            Card topMostCard = this.playing.TopOfDeck;
            this.LogGameStatusDebug(topMostCard, currentPlayer);   // debug/verbose output            

            this.rounds++;  // count rounds of this game

            // handle special cards
            if (topMostCard.Picture == CardPicture.Sieben)
            {
                // '7' is on top of card deck
                Log(">   '7' is on top of deck");
                if (this.players[currentPlayer].CounterSeven(numberOfCardsToDraw))
                {
                    numberOfCardsToDraw = this.IncrementNumberOfCardsToDraw(numberOfCardsToDraw);
                }
                else
                {
                    numberOfCardsToDraw = 2;  // no more '7' on top of card deck

                    // player may now draw a card, if he can
                    this.players[currentPlayer].PlayArbitraryCard();

                    Card peek = this.playing.TopOfDeck;

                    if (peek.Picture == CardPicture.Bube)
                    {
                        requestedCardColor = this.players[currentPlayer].ChooseAColor();

                        bubeIsActive = true;

                        String msg = String.Format(">   {0} has choosen color {1}",
                            this.players[currentPlayer].Name, requestedCardColor);
                        Log(msg);
                    }
                    else if (peek.Picture == CardPicture.Acht)
                    {
                        Log(">   '8' is on top of deck - skip next player");
                        eightIsActive = true;
                        skipNextPlayer = true;
                    }
                }
            }
            else if (topMostCard.Picture == CardPicture.Acht && !eightIsActive)
            {
                Log(">   '8' is on top of deck - skip next player");
                eightIsActive = true;
                skipNextPlayer = false;
            }
            else if (topMostCard.Picture == CardPicture.Bube && !bubeIsActive)
            {
                requestedCardColor = this.players[currentPlayer].ChooseAColor();

                bubeIsActive = true;

                String msg = String.Format(">   {0} has choosen color {1}",
                    this.players[currentPlayer].Name, requestedCardColor);
                Log(msg);
            }
            else
            {
                // regular mode -- no special cards --
                // current player plays according to "standard" rules
                bool success;
                if (! bubeIsActive)
                {
                    // picture or color can be played
                    success = 
                        this.players[currentPlayer].PlayCard(topMostCard.Color, topMostCard.Picture);
                }
                else
                {
                    // a color has been choosen right before, only color must match
                    success = 
                        this.players[currentPlayer].PlayCard(requestedCardColor);
                }

                if (success)
                {
                    // reset special state(s), if any 
                    eightIsActive = false;
                    bubeIsActive = false;

                    Card peek = this.playing.TopOfDeck;
                    if (peek.Picture == CardPicture.Bube)
                    {
                        requestedCardColor = this.players[currentPlayer].ChooseAColor();

                        bubeIsActive = true;

                        String msg = String.Format(">   {0} has choosen color {1}",
                            this.players[currentPlayer].Name, requestedCardColor);
                        Log(msg);
                    }
                    else if (peek.Picture == CardPicture.Acht)
                    {
                        Log(">   '8' is on top of deck - skip next player");

                        eightIsActive = true;
                        skipNextPlayer = true;
                    }
                }
                else
                {
                    String msg = String.Format(">   {0} cannot serve, draws {1} card(s)",
                        this.players[currentPlayer].Name, 1);
                    Log(msg);

                    this.players[currentPlayer].DrawCards(1);
                }
            }

            // test, if current player quits
            if (this.players[currentPlayer].NumberCards == 0)
            {
                String msg = String.Format(">   {0} quits game !", this.players[currentPlayer].Name);
                Log(msg);

                this.players[currentPlayer].IsPlaying = false;
                activePlayers--;
            }

            // switch to next player
            currentPlayer = this.NextPlayer(currentPlayer);

            // '8' has just been played, next player pauses
            if (skipNextPlayer == true)
            {
                // skip next player
                currentPlayer = this.NextPlayer(currentPlayer);

                skipNextPlayer = false;
            }
        }

        // last player loses the game
        this.LogFinalGameStatus(this.players[currentPlayer].Name);
    }

    public static String[] ReadPlayers()
    {
        Console.Write("Number of Players: ");
        String s = Console.ReadLine();
        int n = Int32.Parse(s);

        String[] names = new String[n];
        for (int i = 0; i < n; i++)
        {
            Console.Write("{0}. Player: ", (i+1));
            names[i] = Console.ReadLine();
        }

        return names;
    }

    public static void PrintVersion()
    {
        Console.WriteLine("------------------------------------------------------------------");
        Console.WriteLine(MauMaster.Version);
        Console.WriteLine("------------------------------------------------------------------");
    }

    // private helper methods
    private int NextPlayer(int currentPlayer)
    {
        // move to next player
        currentPlayer++;
        if (currentPlayer == this.players.Length)
            currentPlayer = 0;

        // search next array slot with still active player
        while (!this.players[currentPlayer].IsPlaying)
        {
            currentPlayer++;
            if (currentPlayer == this.players.Length)
                currentPlayer = 0;
        }

        return currentPlayer;
    }

    private int IncrementNumberOfCardsToDraw(int numberOfCardsToDraw)
    {
        numberOfCardsToDraw += 2;
        return numberOfCardsToDraw;
    }

    private void LogFinalGameStatus(String name)
    {
#if VERBOSE
        Console.WriteLine("{0} has lost --- Game over [{1}]", name, this.rounds);
#endif
    }

    private void LogGameStatusDebug(Card topMostCard, int currentPlayer)
    {
#if VERBOSE
        Console.WriteLine("------------------------------------------------------------------");
        Console.WriteLine("Topmost card: {0}", topMostCard);
        Console.WriteLine("------------------------------------------------------------------");

        for (int i = 0; i < this.players.Length; i++)
        {
            String prefix = (i == currentPlayer) ? "-->" : "   ";
            String s = String.Format("{0} {1}", prefix, this.players[i]);
            Console.WriteLine(s);
        }

        Console.WriteLine("------------------------------------------------------------------");
#endif

#if SINGLE_STEP
        Console.ReadKey();  // just for testing
#endif
    }

    public static void Log (String message)
    {
#if VERBOSE
        Console.WriteLine(message);
#endif
    }
}

