using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.PlayingCards.Standard52CardDeck
{
    public class Standard52CardDeckFactory : IDeckFactoryWithOptions<FrenchPlayingCard, Standard52CardDeckFactoryOptions>
    {
        public Stack<FrenchPlayingCard> GetDeck(Action<Standard52CardDeckFactoryOptions> optionsAction = null)
        {
            var stack = new Stack<FrenchPlayingCard>(52);
            foreach (FrenchPlayingCards c in Enum.GetValues(typeof(FrenchPlayingCards)))
            {
                if (c == FrenchPlayingCards.Joker)
                    continue;

                var card = new FrenchPlayingCard(c);
                stack.Push(card);
            }

            // create jokers
            var options = new Standard52CardDeckFactoryOptions();
            optionsAction?.Invoke(options);

            for (var i = 0; i < options.AmountOfJokers; i++)
            {
                stack.Push(new FrenchPlayingCard(FrenchPlayingCards.Joker));
            }

            return stack;
        }

        public Stack<FrenchPlayingCard> GetDeck()
        {
            return GetDeck(null);
        }
    }
}
