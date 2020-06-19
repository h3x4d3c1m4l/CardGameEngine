using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGameEngine.Shuffling
{
    public class PlayingCardRandomShuffler : IPlayingCardShuffler
    {
        public void Shuffle<T>(Stack<T> deck)
        {
            //'https://stackoverflow.com/questions/33643104/shuffling-a-stackt
            var values = deck.ToArray();
            deck.Clear();
            var rnd = new Random();
            foreach (var value in values.OrderBy(x => rnd.Next()))
                deck.Push(value);
        }

        public void Shuffle<T>(ICollection<T> deck)
        {
            throw new NotImplementedException();
        }
    }
}
