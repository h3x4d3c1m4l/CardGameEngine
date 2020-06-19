using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.PlayingCards
{
    public interface IDeckFactory<T> where T : IPlayingCard
    {
        Stack<T> GetDeck();
    }
}
