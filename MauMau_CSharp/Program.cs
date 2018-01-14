using System;

class Program
{
    private static void TestUnit_01_Cards ()
    {
        // test frame for Card objects
        Card card1 = new Card(CardColor.Kreuz, CardPicture.Neun);
        Card card2 = new Card(CardColor.Pik, CardPicture.König);
        Card card3 = new Card(CardColor.Kreuz, CardPicture.Neun);

        Console.WriteLine(card1);
        Console.WriteLine(card2);

        Console.WriteLine("Farbe: {0}", card1.Color);
        Console.WriteLine("Bild:  {0}", card2.Picture);

        if (card1.Equals(card2))
            Console.WriteLine("Die Karten sind gleich");
        else
            Console.WriteLine("Die Karten sind verschieden");

        Console.WriteLine(card1.Equals (card3));
    }

    private static void TestUnit_02_CardDeck()
    {
        Random rand = new Random(1);

        // test frame for CardDeck objects
        Card card1 = new Card(CardColor.Kreuz, CardPicture.Neun);
        Card card2 = new Card(CardColor.Pik, CardPicture.König);
        Card card3 = new Card(CardColor.Herz, CardPicture.Sieben);

        CardDeck deck = new CardDeck() { Rand = rand };
        deck.Push(card1);
        deck.Push(card2);
        deck.Push(card3);

        Console.WriteLine(deck);
    }

    private static void TestUnit_03_CardDeck()
    {
        Random rand = new Random(1);

        // test frame for CardDeck objects
        CardDeck deck = new CardDeck() { Rand = rand };
        deck.Fill();
        Console.WriteLine(deck);
        deck.Shuffle();
        Console.WriteLine(deck);
    }

    private static void TestUnit_04_CardSet()
    {
        // test frame for a single CardSet object
        Card card1 = new Card(CardColor.Kreuz, CardPicture.Neun);
        Card card2 = new Card(CardColor.Pik, CardPicture.König);
        Card card3 = new Card(CardColor.Herz, CardPicture.Sieben);

        CardSet set = new CardSet();
        set.Add(card1);
        set.Add(card2);
        set.Add(card3);

        for (int i = 0; i < set.Size; i++)
        {
            Console.WriteLine("Karte {0}: {1}", i, set[i]);
        }

        Console.WriteLine("Karten auf der Hand: {0}", set);
        set.Remove(1);
        Console.WriteLine("Karten auf der Hand: {0}", set);
    }

    private static void TestUnit_10_PlayTheGame()
    {
        MauMaster.PrintVersion();
        String[] names = MauMaster.ReadPlayers();
        MauMaster mm = new MauMaster(names);
        mm.Init(1);
        mm.Play();
    }

    private static void TestUnit_11_SingleTestMauMaster()
    {
        MauMaster.PrintVersion();
        MauMaster mm = new MauMaster(new String[] { "Hans", "Sepp", "Ulli" });
        int randomSeed = 188;
        mm.Init(randomSeed);
        mm.Play();
    }

    private static void TestUnit_12_StressTestMauMaster()
    {
        MauMaster.PrintVersion();
        MauMaster mm = new MauMaster(new String[] { "Hans", "Sepp", "Ulli" });

        int minRounds = Int32.MaxValue;
        int minRoundsIndex = -1;
        int maxRounds = -1;
        int maxRoundsIndex = -1;

        for (int i = 1; i < 1000; i++)
        {
            mm.Init(i);
            mm.Play();

            if (mm.Rounds < minRounds)
            {
                minRounds = mm.Rounds;
                minRoundsIndex = i;
            }

            if (mm.Rounds > maxRounds)
            {
                maxRounds = mm.Rounds;
                maxRoundsIndex = i;
            }

            Console.WriteLine("Game at {0,5}: {1}", i, mm.Rounds);    
        }

        Console.WriteLine("Minumum number of rounds: {0} [Index {1}]", minRounds, minRoundsIndex);
        Console.WriteLine("Maximum number of rounds: {0} [Index {1}]", maxRounds, maxRoundsIndex);
    }

    public static void Main()
    {
        // TestUnit_01_Cards();
        // TestUnit_02_CardDeck();
        // TestUnit_03_CardDeck();
        // TestUnit_04_CardSet();
        // TestUnit_10_PlayTheGame();
        // TestUnit_11_SingleTestMauMaster();
        TestUnit_12_StressTestMauMaster();
    }
}

