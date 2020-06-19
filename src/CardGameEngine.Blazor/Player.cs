using CardGameEngine.PlayingCards.Standard52CardDeck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardGameEngine.Blazor
{
    public class Player : IPlayer<FrenchPlayingCard>
    {
        public List<FrenchPlayingCard> Cards { get; set; } = new List<FrenchPlayingCard>();

        public string Name { get; set; }
    }
}
