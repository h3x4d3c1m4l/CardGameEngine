using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.Shuffling
{
    public interface IPlayingCardShuffler
    {
        void Shuffle<T>(Stack<T> deck);

        void Shuffle<T>(ICollection<T> deck);
    }
}
