using CardGameEngine.Exceptions;
using CardGameEngine.GameConcepts;
using CardGameEngine.PlayingCards.Standard52CardDeck;
using CardGameEngine.Shuffling;
using MauMau.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine.MauMau
{
    public class MauMauGame<TPlayer> : ITurnBasedGame<TPlayer, FrenchPlayingCard>, IPlayersCanJoinMidGame<FrenchPlayingCard> where TPlayer : IPlayer<FrenchPlayingCard>
    {
        public MauMauGame(Standard52CardDeckFactory deckFactory, IPlayingCardShuffler shuffler, List<TPlayer> players, Action<MauMauOptions> optionsAction = null)
        {
            _cardShuffler = shuffler;
            _options = new MauMauOptions();
            optionsAction?.Invoke(_options);

            var stock = new Stack<FrenchPlayingCard>();
            for (var i = 0; i < _options.AmountOfDecks; i++)
            {
                var deck = deckFactory.GetDeck();
                foreach (var c in deck)
                    stock.Push(c);
            }
            shuffler.Shuffle(stock);
            DrawingStack = stock;

            var firstPlayedCardIsOk = false;
            var testedCards = new Stack<FrenchPlayingCard>();
            // make sure to never start the game with a bully card
            while (!firstPlayedCardIsOk)
            {
                var card = DrawingStack.Pop();
                if (card.IsJoker || card.HasRankAce || card.HasRankTwo || card.HasRankSeven || card.HasRankEight || card.HasRankJack)
                {
                    testedCards.Push(card);
                }
                else
                {
                    // ok
                    firstPlayedCardIsOk = true;
                    PlayingStack = new Stack<FrenchPlayingCard>();
                    PlayingStack.Push(card);
                    while (testedCards.Any())
                    {
                        DrawingStack.Push(testedCards.Pop());
                    }
                }
            }

            LinkedPlayers = new LinkedList<IPlayer<FrenchPlayingCard>>();
            foreach (var p in players)
            {
                AddPlayer(p);
            }
            CurrentPlayer = LinkedPlayers.First;
        }

        private IPlayingCardShuffler _cardShuffler;

        private MauMauOptions _options;

        public List<string> Events { get; set; } = new List<string>();

        public LinkedList<IPlayer<FrenchPlayingCard>> LinkedPlayers { get; set; }

        public Stack<FrenchPlayingCard> DrawingStack { get; set; }

        public Stack<FrenchPlayingCard> PlayingStack { get; set; }

        public EventHandler<IPlayer<FrenchPlayingCard>> NextTurnStarted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LinkedListNode<IPlayer<FrenchPlayingCard>> CurrentPlayer { get; set; }

        public uint? AmountOfCardsCurrentPlayerNeedsToDraw;

        public FrenchPlayingCardSuits? SuitForCurrentPlayer;

        public bool PlayingCounterwise { get; set; }

        public IPlayer<FrenchPlayingCard> GameWonBy { get; set; }

        TPlayer ITurnBasedGame<TPlayer, FrenchPlayingCard>.CurrentPlayer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        EventHandler<TPlayer> ITurnBasedGame<TPlayer, FrenchPlayingCard>.NextTurnStarted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public EventHandler StateChanged;

        private void AddEvent(string description)
        {
            if (Events.Count == 10)
                Events.RemoveAt(0);

            Events.Add(description);
        }

        public void PlayCard(IPlayer<FrenchPlayingCard> player, FrenchPlayingCard card, FrenchPlayingCardSuits? pickedSuits = null)
        {
            if (!Equals(CurrentPlayer.Value, player))
                throw new NotThePlayersTurnException();

            var playingPlayer = CurrentPlayer;

            // check if player has to draw
            if (AmountOfCardsCurrentPlayerNeedsToDraw != null)
            {
                // special turn: only allow card with rank two or joker
                // else the player should just draw, then they are allowed to regularly play a card
                if (card.HasRankTwo)
                {
                    AmountOfCardsCurrentPlayerNeedsToDraw += 2;
                    AdvanceTurn();
                }
                else if (card.IsJoker)
                {
                    AmountOfCardsCurrentPlayerNeedsToDraw += 5;
                    AdvanceTurn();
                }
                else
                {
                    throw new CardNotAllowedException();
                }
            }
            else if (card == null && DrawnACard == player)
            {
                // player has just drawn a card, and wants to skip turn
                AdvanceTurn();
                AddEvent($"{playingPlayer.Value.Name} skipped their turn");
            }
            else
            {
                // normal turn
                var lastPlayed = PlayingStack.Peek();
                if (
                    !lastPlayed.IsJoker &&
                    !card.IsJoker &&
                    !card.HasRankJack &&
                    !(SuitForCurrentPlayer != null && card.Suit == SuitForCurrentPlayer) &&
                    !(SuitForCurrentPlayer == null && card.HasSameRank(lastPlayed)) &&
                    !(SuitForCurrentPlayer == null && card.HasSameSuit(lastPlayed)))
                {
                    // card not allowed
                    throw new CardNotAllowedException();
                }

                // we are very sure the played card is allowed, now execute actions
                if (card.IsJoker)
                {
                    AmountOfCardsCurrentPlayerNeedsToDraw = 5;
                    AdvanceTurn();
                }
                else
                {
                    switch (card.Rank)
                    {
                        case FrenchPlayingCardRanks.Ace:
                            PlayingCounterwise = !PlayingCounterwise;
                            AdvanceTurn();
                            break;
                        case FrenchPlayingCardRanks.Two:
                            // next player has to draw 
                            AmountOfCardsCurrentPlayerNeedsToDraw = 2;
                            AdvanceTurn();
                            break;
                        case FrenchPlayingCardRanks.Seven:
                            // don't change CurrentPlayer
                            break;
                        case FrenchPlayingCardRanks.Eight:
                            AdvanceTurn();
                            AdvanceTurn();
                            break;
                        case FrenchPlayingCardRanks.Jack:
                            if (pickedSuits == null)
                            {
                                throw new CardNotAllowedException();
                            }
                            AdvanceTurn();
                            break;
                        default:
                            AdvanceTurn();
                            break;
                    }
                }
            }

            // take the card from the player
            if (card != null)
            {
                playingPlayer.Value.Cards.Remove(card);
                PlayingStack.Push(card);
                if (card.IsJoker)
                    AddEvent($"{playingPlayer.Value.Name} played a joker");
                else
                    AddEvent($"{playingPlayer.Value.Name} played a {card.Rank} of {card.Suit}{(pickedSuits != null ? $", picked {pickedSuits}" : string.Empty)}");
                if (card.Rank == FrenchPlayingCardRanks.Jack)
                    SuitForCurrentPlayer = pickedSuits;
                else
                    SuitForCurrentPlayer = null;
            }

            // check if player won the game
            if (!playingPlayer.Value.Cards.Any())
            {
                // player ran out of cards
                if (card.IsJoker || card.HasRankAce || card.HasRankTwo || card.HasRankSeven || card.HasRankEight || card.HasRankJack)
                {
                    // automatically pick card
                    PickFromStack(playingPlayer.Value, true);
                }
                else
                {
                    GameWonBy = playingPlayer.Value;
                }
            }

            DrawnACard = null;
            StateChanged?.Invoke(this, null);
        }

        public void AdvanceTurn()
        {
            if (PlayingCounterwise)
            {
                CurrentPlayer = CurrentPlayer.Previous ?? LinkedPlayers.Last;
            }
            else
            {
                CurrentPlayer = CurrentPlayer.Next ?? LinkedPlayers.First;
            }
        }

        public IPlayer<FrenchPlayingCard> DrawnACard { get; set; }

        public void PickFromStack(IPlayer<FrenchPlayingCard> player, bool forceOneCard = false)
        {
            if (!forceOneCard && !Equals(CurrentPlayer.Value, player))
                throw new NotThePlayersTurnException();

            if (!forceOneCard && AmountOfCardsCurrentPlayerNeedsToDraw != null)
            {
                // player has to pick cards due to other player's cards
                for (var i = 0; i < AmountOfCardsCurrentPlayerNeedsToDraw; i++)
                {
                    var card = ShufflePlayingStackToDrawingStackAndDrawCard();
                    player.Cards.Add(card);
                }

                AddEvent($"{player.Name} had to draw {AmountOfCardsCurrentPlayerNeedsToDraw} cards");
                AmountOfCardsCurrentPlayerNeedsToDraw = null;
            }
            else if (forceOneCard || DrawnACard != player)
            {
                // player just picks card
                var card = ShufflePlayingStackToDrawingStackAndDrawCard();
                player.Cards.Add(card);
                if (!forceOneCard)
                {
                    DrawnACard = player;
                    AddEvent($"{player.Name} has drawen a card");
                }
            }

            StateChanged?.Invoke(this, null);
        }

        // TODO: what if no cards?
        public FrenchPlayingCard ShufflePlayingStackToDrawingStackAndDrawCard()
        {
            if (DrawingStack.Count == 0)
            {
                // shuffle playing stack to drawing stack+
                var lastPlayedCard = PlayingStack.Pop();
                _cardShuffler.Shuffle(PlayingStack);
                foreach (var c in PlayingStack)
                {
                    DrawingStack.Push(c);
                }
                PlayingStack.Clear();
                PlayingStack.Push(lastPlayedCard);
            }

            return DrawingStack.Pop();
        }

        public void AddPlayer(IPlayer<FrenchPlayingCard> player)
        {
            for (var i = 0; i < _options.AmountOfCardsPerPlayer; i++)
            {
                var card = ShufflePlayingStackToDrawingStackAndDrawCard();
                player.Cards.Add(card);
            }
            LinkedPlayers.AddLast((TPlayer)player);
            AddEvent($"{player.Name} joined");
            StateChanged?.Invoke(this, null);
        }

        public void RemovePlayer(IPlayer<FrenchPlayingCard> player)
        {
            if (CurrentPlayer.Value == player)
                AdvanceTurn();
            LinkedPlayers.Remove(player);
            AddEvent($"{player.Name} left");
            StateChanged?.Invoke(this, null);
        }

        public void Reset()
        {
            var deckFactory = new Standard52CardDeckFactory();
            var stock = new Stack<FrenchPlayingCard>();
            for (var i = 0; i < _options.AmountOfDecks; i++)
            {
                var deck = deckFactory.GetDeck();
                foreach (var c in deck)
                    stock.Push(c);
            }
            _cardShuffler.Shuffle(stock);
            DrawingStack = stock;
            string wonByName = GameWonBy.Name;
            GameWonBy = null;
            foreach (var player in LinkedPlayers)
            {
                player.Cards = new List<FrenchPlayingCard>();
                for (var i = 0; i < _options.AmountOfCardsPerPlayer; i++)
                {
                    var card = ShufflePlayingStackToDrawingStackAndDrawCard();
                    player.Cards.Add(card);
                }
            }
            Events = new List<string>();
            Events.Add($"Game won by: {wonByName}");
            Events.Add($"Started new game with: {CurrentPlayer.Value.Name}'s turn");
            StateChanged?.Invoke(this, null);
        }
    }
}
