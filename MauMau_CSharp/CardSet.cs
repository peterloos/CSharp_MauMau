using System;
using System.Collections.Generic;

class CardSet
{
    private List<Card> set;

    // c'tor(s)
    public CardSet ()
    {
        this.set = new List<Card>();
    }

    // properties
    public int Size
    {
        get
        {
            return this.set.Count;
        }
    }

    // indexer
    public Card this [int index]
    {
        get
        {
            return this.set[index];
        }
    }

    // public interface
    public void Add(Card card)
    {
        this.set.Add(card);
    }

    public void Remove(int index)
    {
        if (index < 0 || index >= this.set.Count)
        {
            String msg = String.Format("Wrong Index {0} !", index);
            throw new IndexOutOfRangeException(msg);
        }

        this.set.RemoveAt(index);
    }

    public void Clear()
    {
        this.set.Clear();
    }

    // overrides
    public override String ToString()
    {
        String s = "";
        for (int i = 0; i < this.set.Count; i++)
        {
            s += this.set[i];
            if (i < this.set.Count - 1)
                s += ", ";
        }
        return s;
    }
}

