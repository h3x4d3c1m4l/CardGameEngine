using CardGameEngine.PlayingCards;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CardGameEngine
{
    public interface IPlayer<TCard> where TCard : IPlayingCard
    {
        List<TCard> Cards { get; set; }

        string Name { get; set; }
    }
}
