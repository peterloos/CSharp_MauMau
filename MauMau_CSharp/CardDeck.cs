using System;
using System.Collections.Generic;

class CardDeck
{
    private List<Card> deck;
    private Random rand;

    // c'tor(s)
    public CardDeck()
    {
        this.deck = new List<Card>();
    }

    // properties
    public Random Rand
    {
        set
        {
            this.rand = value;
        }
    }

    public int Size
    {
        get
        {
            return this.deck.Count;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return this.deck.Count == 0;
        }
    }

    public Card TopOfDeck
    {
        get
        {
            if (this.IsEmpty)
            {
                throw new IndexOutOfRangeException("TopOfDeck::CardDeck is emtpy !");
            }

            Card card = this.deck[this.deck.Count - 1];
            return card;
        }
    }

    // public interface
    public void Push(Card card)
    {
        this.deck.Add(card);
    }

    public Card Pop()
    {
        if (this.IsEmpty)
        {
            throw new IndexOutOfRangeException("Pop::CardDeck is emtpy !");
        }

        Card card = this.deck[this.deck.Count - 1];
        this.deck.RemoveAt(this.deck.Count - 1);
        return card;
    }

    public void Fill()
    {
        // fill deck with all available cards
        for (int i = 1; i <= 4; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                Card card = new Card((CardColor) i, (CardPicture) j);
                this.deck.Add(card);
            }
        }
    }

    public void Clear()
    {
        this.deck.Clear();
    }

    public void Shuffle()
    {
        // mix deck by random
        const int ShuffleCount = 30;

        for (int i = 0; i < ShuffleCount; i++)
        {
            int index1 = this.rand.Next(this.deck.Count);
            int index2 = this.rand.Next(this.deck.Count);

            if (index1 != index2)
            {
                Card temp = this.deck[index1];
                this.deck[index1] = this.deck[index2];
                this.deck[index2] = temp;
            }
        }
    }

    // overrides
    public override String ToString()
    {
        String s = String.Empty;
        for (int i = 0; i < this.deck.Count; i++)
        {
            s += String.Format("{0,2}: {1}", (i + 1), this.deck[i]);
            s += Environment.NewLine;
        }

        return s;
    }
}
