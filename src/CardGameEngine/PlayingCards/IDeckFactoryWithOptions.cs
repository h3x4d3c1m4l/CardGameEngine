using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.PlayingCards
{
    public interface IDeckFactoryWithOptions<TCard, TOptions> : IDeckFactory<TCard> where TCard : IPlayingCard
    {
        Stack<TCard> GetDeck(Action<TOptions> options);
    }
}
