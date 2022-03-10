using Godot;
using System;
using System.Collections.Generic;

static class ShuffleContainer
{
    private static Random rng = new Random();  
    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}
class deck
{
    private List<card_base> decklist;
    public deck()
    {
        decklist = new List<card_base>();
    }

    public void populate()
    {
        decklist.Add(card_base.MakeDamageCard("Heavy Attack", 3, 0, 3, 1));
        decklist.Add(card_base.MakeDamageCard("Flaunt", 0, 3, 3, 1));
        decklist.Add(status_card.MakeStatusCard("Burn", "burning", 0, 0, 3, 1));
        decklist.Add(status_card.MakeStatusCard("Lacerate", "exposed", 0, 0, 3, 1));
        decklist.Add(card_base.MakeDamageCard("Inspiration", 0, 0, -2, 1));

        decklist.Shuffle();
    }

    public card_base draw()
    {
        if (decklist.Count <= 0)
        {
            populate();
        }

        card_base ret = decklist[decklist.Count - 1];
        decklist.Remove(ret);
        return ret;
    }
}