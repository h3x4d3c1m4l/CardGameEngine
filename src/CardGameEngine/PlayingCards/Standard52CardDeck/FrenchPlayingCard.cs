using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.PlayingCards.Standard52CardDeck
{
    public class FrenchPlayingCard : IPlayingCard
    {
        public Guid Id { get; }

        public FrenchPlayingCards Card { get; }

        public FrenchPlayingCardRanks? Rank { get; }

        public FrenchPlayingCardSuits? Suit { get; }

        public FrenchPlayingCard(FrenchPlayingCards card)
        {
            Id = Guid.NewGuid();
            Card = card;

            if (HasSuitClubs)
                Suit = FrenchPlayingCardSuits.Clubs;
            else if (HasSuitDiamonds)
                Suit = FrenchPlayingCardSuits.Diamonds;
            else if (HasSuitHearts)
                Suit = FrenchPlayingCardSuits.Hearts;
            else if (HasSuitSpades)
                Suit = FrenchPlayingCardSuits.Spades;

            if (HasRankAce)
                Rank = FrenchPlayingCardRanks.Ace;
            else if (HasRankTwo)
                Rank = FrenchPlayingCardRanks.Two;
            else if (HasRankThree)
                Rank = FrenchPlayingCardRanks.Three;
            else if (HasRankFour)
                Rank = FrenchPlayingCardRanks.Four;
            else if (HasRankFive)
                Rank = FrenchPlayingCardRanks.Five;
            else if (HasRankSix)
                Rank = FrenchPlayingCardRanks.Six;
            else if (HasRankSeven)
                Rank = FrenchPlayingCardRanks.Seven;
            else if (HasRankEight)
                Rank = FrenchPlayingCardRanks.Eight;
            else if (HasRankNine)
                Rank = FrenchPlayingCardRanks.Nine;
            else if (HasRankTen)
                Rank = FrenchPlayingCardRanks.Ten;
            else if (HasRankJack)
                Rank = FrenchPlayingCardRanks.Jack;
            else if (HasRankQueen)
                Rank = FrenchPlayingCardRanks.Queen;
            else if (HasRankKing)
                Rank = FrenchPlayingCardRanks.King;
        }

        public bool HasSuitClubs => Card >= FrenchPlayingCards.AceOfClubs && Card <= FrenchPlayingCards.KingOfClubs;
        public bool HasSuitDiamonds => Card >= FrenchPlayingCards.AceOfDiamonds && Card <= FrenchPlayingCards.KingOfDiamonds;
        public bool HasSuitHearts => Card >= FrenchPlayingCards.AceOfHearts && Card <= FrenchPlayingCards.KingOfHearts;
        public bool HasSuitSpades => Card >= FrenchPlayingCards.AceOfSpades && Card <= FrenchPlayingCards.KingOfSpades;
        public bool HasRankAce => Card == FrenchPlayingCards.AceOfClubs || Card == FrenchPlayingCards.AceOfDiamonds || Card == FrenchPlayingCards.AceOfHearts || Card == FrenchPlayingCards.AceOfSpades;
        public bool HasRankTwo => Card == FrenchPlayingCards.TwoOfClubs || Card == FrenchPlayingCards.TwoOfDiamonds || Card == FrenchPlayingCards.TwoOfHearts || Card == FrenchPlayingCards.TwoOfSpades;
        public bool HasRankThree => Card == FrenchPlayingCards.ThreeOfClubs || Card == FrenchPlayingCards.ThreeOfDiamonds || Card == FrenchPlayingCards.ThreeOfHearts || Card == FrenchPlayingCards.ThreeOfSpades;
        public bool HasRankFour => Card == FrenchPlayingCards.FourOfClubs || Card == FrenchPlayingCards.FourOfDiamonds || Card == FrenchPlayingCards.FourOfHearts || Card == FrenchPlayingCards.FourOfSpades;
        public bool HasRankFive => Card == FrenchPlayingCards.FiveOfClubs || Card == FrenchPlayingCards.FiveOfDiamonds || Card == FrenchPlayingCards.FiveOfHearts || Card == FrenchPlayingCards.FiveOfSpades;
        public bool HasRankSix => Card == FrenchPlayingCards.SixOfClubs || Card == FrenchPlayingCards.SixOfDiamonds || Card == FrenchPlayingCards.SixOfHearts || Card == FrenchPlayingCards.SixOfSpades;
        public bool HasRankSeven => Card == FrenchPlayingCards.SevenOfClubs || Card == FrenchPlayingCards.SevenOfDiamonds || Card == FrenchPlayingCards.SevenOfHearts || Card == FrenchPlayingCards.SevenOfSpades;
        public bool HasRankEight => Card == FrenchPlayingCards.EightOfClubs || Card == FrenchPlayingCards.EightOfDiamonds || Card == FrenchPlayingCards.EightOfHearts || Card == FrenchPlayingCards.EightOfSpades;
        public bool HasRankNine => Card == FrenchPlayingCards.NineOfClubs || Card == FrenchPlayingCards.NineOfDiamonds || Card == FrenchPlayingCards.NineOfHearts || Card == FrenchPlayingCards.NineOfSpades;
        public bool HasRankTen => Card == FrenchPlayingCards.TenOfClubs || Card == FrenchPlayingCards.TenOfDiamonds || Card == FrenchPlayingCards.TenOfHearts || Card == FrenchPlayingCards.TenOfSpades;
        public bool HasRankJack => Card == FrenchPlayingCards.JackOfClubs || Card == FrenchPlayingCards.JackOfDiamonds || Card == FrenchPlayingCards.JackOfHearts || Card == FrenchPlayingCards.JackOfSpades;
        public bool HasRankQueen => Card == FrenchPlayingCards.QueenOfClubs || Card == FrenchPlayingCards.QueenOfDiamonds || Card == FrenchPlayingCards.QueenOfHearts || Card == FrenchPlayingCards.QueenOfSpades;
        public bool HasRankKing => Card == FrenchPlayingCards.KingOfClubs || Card == FrenchPlayingCards.KingOfDiamonds || Card == FrenchPlayingCards.KingOfHearts || Card == FrenchPlayingCards.KingOfSpades;
        public bool IsJoker => Card == FrenchPlayingCards.Joker;

        public bool HasSameSuit(FrenchPlayingCard playingCard) =>
            (playingCard.HasSuitClubs && HasSuitClubs) ||
            (playingCard.HasSuitDiamonds && HasSuitDiamonds) ||
            (playingCard.HasSuitHearts && HasSuitHearts) ||
            (playingCard.HasSuitSpades && HasSuitSpades);

        public bool HasSameRank(FrenchPlayingCard playingCard) =>
            (playingCard.HasRankAce && HasRankAce) ||
            (playingCard.HasRankTwo && HasRankTwo) ||
            (playingCard.HasRankThree && HasRankThree) ||
            (playingCard.HasRankFour && HasRankFour) ||
            (playingCard.HasRankFive && HasRankFive) ||
            (playingCard.HasRankSix && HasRankSix) ||
            (playingCard.HasRankSeven && HasRankSeven) ||
            (playingCard.HasRankEight && HasRankEight) ||
            (playingCard.HasRankNine && HasRankNine) ||
            (playingCard.HasRankTen && HasRankTen) ||
            (playingCard.HasRankJack && HasRankJack) ||
            (playingCard.HasRankQueen && HasRankQueen) ||
            (playingCard.HasRankKing && HasRankKing);
    }

    public enum FrenchPlayingCardRanks
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    public enum FrenchPlayingCardSuits
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public enum FrenchPlayingCards
    {
        AceOfClubs,
        TwoOfClubs,
        ThreeOfClubs,
        FourOfClubs,
        FiveOfClubs,
        SixOfClubs,
        SevenOfClubs,
        EightOfClubs,
        NineOfClubs,
        TenOfClubs,
        JackOfClubs,
        QueenOfClubs,
        KingOfClubs,

        AceOfDiamonds,
        TwoOfDiamonds,
        ThreeOfDiamonds,
        FourOfDiamonds,
        FiveOfDiamonds,
        SixOfDiamonds,
        SevenOfDiamonds,
        EightOfDiamonds,
        NineOfDiamonds,
        TenOfDiamonds,
        JackOfDiamonds,
        QueenOfDiamonds,
        KingOfDiamonds,

        AceOfHearts,
        TwoOfHearts,
        ThreeOfHearts,
        FourOfHearts,
        FiveOfHearts,
        SixOfHearts,
        SevenOfHearts,
        EightOfHearts,
        NineOfHearts,
        TenOfHearts,
        JackOfHearts,
        QueenOfHearts,
        KingOfHearts,

        AceOfSpades,
        TwoOfSpades,
        ThreeOfSpades,
        FourOfSpades,
        FiveOfSpades,
        SixOfSpades,
        SevenOfSpades,
        EightOfSpades,
        NineOfSpades,
        TenOfSpades,
        JackOfSpades,
        QueenOfSpades,
        KingOfSpades,

        Joker
    }
}
