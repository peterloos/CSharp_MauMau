using System;

struct Card
{
    private CardColor color;
    private CardPicture picture;

    // c'tor
    public Card(CardColor color, CardPicture picture)
    {
        this.color = color;
        this.picture = picture;
    }

    // properties
    public CardColor Color
    {
        get
        {
            return this.color;
        }
    }

    public CardPicture Picture
    {
        get
        {
            return this.picture;
        }
    }

    // overrides
    public override bool Equals(Object obj)
    {
        if (obj == null || !(obj is Card))
            return false;

        Card tmp = (Card) obj;
        return (this.color == tmp.color && this.picture == tmp.picture);
    }

    public override String ToString()
    {
        return String.Format("{0} {1}", this.color, this.picture);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

