using CardGameEngine.PlayingCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.GameConcepts
{
    public interface ITurnBasedGame<TPlayer, TPlayingCard> where TPlayer : IPlayer<TPlayingCard> where TPlayingCard : IPlayingCard
    {
        TPlayer CurrentPlayer { get; set; }

        EventHandler<TPlayer> NextTurnStarted { get; set; }
    }
}
