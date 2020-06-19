using CardGameEngine.PlayingCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.GameConcepts
{
    public interface IPlayersCanJoinMidGame<TPlayingCard> where TPlayingCard : IPlayingCard
    {
        void AddPlayer(IPlayer<TPlayingCard> player);
    }
}
