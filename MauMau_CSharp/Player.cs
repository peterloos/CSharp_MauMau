using System;

class Player
{
    private CardSet  hand;       // player's hand of cards (usually 5)
    private CardDeck playing;    // deck to play (Kartenstapel zum Ablegen)
    private CardDeck drawing;    // deck to draw (Kartenstapel zum Ziehen)
    private String   name;       // players name
    private bool     isPlaying;  // false, after getting rid of all cards

    // c'tors
    public Player(String name)
    {
        this.hand = new CardSet();
        this.name = name;
        this.isPlaying = true;
        this.playing = null;   // yet to be provided - see property 'PlayingDeck'
        this.drawing = null;   // yet to be provided - see property 'DrawingDeck'
    }

    public Player(String name, CardDeck playing, CardDeck drawing) : this(name)
    {
        this.playing = playing;
        this.drawing = drawing;
    }

    // properties
    public int NumberCards
    {
        get
        {
            return this.hand.Size;
        }
    }

    public String Name
    {
        get
        {
            return this.name;
        }

        set
        {
            this.name = value;
        }
    }

    public CardDeck PlayingDeck
    {
        set
        {
            this.playing = value;
        }
    }

    public CardDeck DrawingDeck
    {
        set
        {
            this.drawing = value;
        }
    }

    public bool IsPlaying
    {
        get
        {
            return this.isPlaying;
        }

        set
        {
            this.isPlaying = value;
        }
    }

    // public interface
    public CardColor ChooseAColor()
    {
        if (this.hand.Size > 0)
        {
            // players has (still) some cards in his hand
            return this.hand[0].Color;
        }
        else
        {
            // players has no more cards, chooses arbitrary card color
            return CardColor.Herz;
        }
    }

    public void DrawCards(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Card card = this.DrawCard();
            this.hand.Add(card);

            String msg = String.Format(">   {0} draws {1} from drawing deck!", this.name, card);
            MauMaster.Log(msg);
        }
    }

    public bool CounterSeven(int numCardsToDraw)
    {
        if (this.HasSeven())
        {
            // players holds '7' in his hand
            String msg = String.Format(">   {0} counters '7' with another '7' !", this.name);
            MauMaster.Log(msg);

            this.PlaySeven();
            return true;
        }
        else
        {
            // players must draw cards
            String msg = String.Format(">   {0} cannot respond to '7', draws {1} card(s)!",
                this.name, numCardsToDraw);
            MauMaster.Log(msg);

            this.DrawCards(numCardsToDraw);
            return false;
        }
    }

    public bool PlayCard(CardColor requestedColor, CardPicture requestedPicture)
    {
        for (int i = 0; i < this.hand.Size; i++)
        {
            Card card = this.hand[i];
            if (card.Color == requestedColor || card.Picture == requestedPicture)
            {
                this.hand.Remove(i);
                this.playing.Push(card);
                String s = String.Format(">   {0} plays {1}", this.name, card);
                MauMaster.Log(s);
                this.PrintMauMauIf();
                return true;
            }
        }

        // 'Bube' maybe played upon every card!
        for (int i = 0; i < this.hand.Size; i++)
        {
            Card card = this.hand[i];
            if (card.Picture == CardPicture.Bube)
            {
                this.hand.Remove(i);
                this.playing.Push(card);
                String s = String.Format(">   {0} plays {1}", this.name, card);
                MauMaster.Log(s);
                this.PrintMauMauIf();
                return true;
            }
        }

        return false;
    }

    public bool PlayCard(CardColor requestedColor)
    {
        for (int i = 0; i < this.hand.Size; i++)
        {
            Card card = this.hand[i];

            // 'Bube' upon 'Bube' not allowed ("Bube auf Bube" stinkt)
            if (card.Picture == CardPicture.Bube)
                continue;

            if (card.Color == requestedColor)
            {
                this.hand.Remove(i);
                this.playing.Push(card);
                String s = String.Format(">   {0} plays {1}", this.name, card);
                MauMaster.Log(s);
                this.PrintMauMauIf();
                return true;
            }
        }

        return false;
    }

    public void PlayArbitraryCard()
    {
        int lastIndex = this.hand.Size - 1;
        Card card = this.hand[lastIndex];
        this.hand.Remove(lastIndex);
        this.playing.Push(card);
        String s = String.Format(">   {0} plays {1}", this.name, card);
        MauMaster.Log(s);
        this.PrintMauMauIf();
    }

    // private helper methods
    private Card DrawCard()
    {
        // turn over playing deck to serve as new drawing deck
        if (this.drawing.Size == 0)
        {
            MauMaster.Log(">   turn over playing deck to serve as new drawing deck");

            // save topmost card of playing stack
            Card topmostPlayingCard = this.playing.Pop();

            // copy rest of playing deck to drawing deck
            while (!this.playing.IsEmpty)
            {
                Card tmp = this.playing.Pop();
                this.drawing.Push(tmp);
            }

            // shuffle drawing stack
            this.drawing.Shuffle();

            // restore topmost card of playing stack
            this.playing.Push(topmostPlayingCard);
        }

        return this.drawing.Pop();
    }

    private bool HasSeven()
    {
        for (int i = 0; i < this.hand.Size; i++)
        {
            Card c = this.hand[i];
            if (c.Picture == CardPicture.Sieben)
                return true;
        }

        return false;
    }

    private void PlaySeven()
    {
        for (int i = 0; i < this.hand.Size; i++)
        {
            Card card = this.hand[i];
            if (card.Picture == CardPicture.Sieben)
            {
                this.hand.Remove(i);
                this.playing.Push(card);
                String s = String.Format(">   {0} drops {1} onto deck!", this.name, card);
                MauMaster.Log(s);
                this.PrintMauMauIf();
                return;
            }
        }

        throw new InvalidOperationException("ERROR (PlaySeven): Should never be reached");
    }

    private void PrintMauMauIf()
    {
        if (this.hand.Size == 1)
        {
            String s = String.Format("==> {0} says 'Mau'", this.name);
            MauMaster.Log(s);
        }
        else if (this.hand.Size == 0)
        {
            String s = String.Format("##> {0} says 'MAU MAU !!!'", this.name);
            MauMaster.Log(s);
        }
    }

    // overrides
    public override String ToString()
    {
        String s = String.Format("{0} [{1}]", this.name, this.isPlaying ? "X" : "-");
        if (this.hand.Size > 0)
        {
            s += ": ";
            s += this.hand.ToString();
        }
        return s;
    }
}
