using System;
using System.Collections.Generic;
using System.Linq;

namespace Durak
{
    class Program
    {
        static void Main(string[] args)
        {
            //var 
            var durak = new Durak(2);

            while (durak.InProgress())
            {
                Console.WriteLine("Player #" + durak.CurrentPlayer() + " attacks");
                
                durak.Attack();
                durak.TryDefend();
            }
        }

        public class Durak
        {
            public enum Suits { Clubs, Diamonds, Hearts, Spades };
            public enum Ranks { Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

            public string[] suitImage = { "♣", "♦", "♥", "♠" };

            public Dictionary<Ranks, int> rankValue = new Dictionary<Ranks, int>();
            public Dictionary<Suits, string> suitsImages = new Dictionary<Suits, string>();

            List<Card> deck;
            Table table;

            public Suits Trump;

            Player[] players;

            int currentPlayerIndex;

            //bool roundInProgress;

            public void MoveCurrentPlayer()
            {
                //currentPlayer = (currentPlayer + 1) % players.Length;
            }

            int HowManyActivePlayers;

            public Durak(int playerCount)
            {
                Suits[] suits = new Suits[] { Suits.Clubs, Suits.Diamonds, Suits.Hearts, Suits.Spades };
                Ranks[] ranks = new Ranks[] { Ranks.Six, Ranks.Seven, Ranks.Eight, Ranks.Nine, Ranks.Ten, Ranks.Jack, Ranks.Queen, Ranks.King, Ranks.Ace };

                for(var i = 0; i < suits.Length; i++)
                {
                    suitsImages.Add(suits[i], suitImage[i]);
                }

                table = new Table();

                deck = new List<Card>();
                foreach (var suit in suits)
                {
                    foreach (var rank in ranks)
                    {
                        deck.Add(new Card(suit, rank));
                    }
                }

                Random random = new Random();
                deck = deck.OrderBy(x => random.Next()).ToList();
                Trump = deck[0].Suit;

                players = new Player[playerCount];
                for (var i = 0; i < playerCount; i++)
                {
                    players[i] = new Player();
                    DealCards(players[i]);
                }
                currentPlayerIndex = 0;
                //roundInProgress = true;
                int HowManyActivePlayers = players.Length;
            }

            public class Card
            {
                public Suits Suit { get; }
                public Ranks Rank { get; }

                public Card(Suits suit, Ranks rank)
                {
                    this.Suit = suit;
                    this.Rank = rank;
                }
                public bool WinOrNot(Card card, Suits trump)
                {
                    if ((this.Suit == card.Suit)|| this.Suit == trump)
                    {
                        if (this.Rank > card.Rank) return true;
                        return false;
                    }
                    return false;
                }
            } 

            public class Player
            {
                public List<Card> Hand { get; set; }

                public bool Active;

                public Player()
                {
                    Hand = new List<Card>();
                    Active = true;
                }

                public void TakeCard(Card card)
                {
                    Hand.Add(card);
                }

                public int HowManyCards()
                {
                    return Hand.Count();
                }              
            }

            public class Table
            {
                public List<Tuple<Card, Card>> Layout { get; set; }

                public Table()
                {
                    Layout = new List<Tuple<Card, Card>>();
                }

                public void DiscardPile()
                {
                    Layout.Clear();
                }

                public void AbandonDefense(Player player)
                {
                    foreach (var cardTuple in Layout)
                    {
                        player.Hand.Add(cardTuple.Item1);
                        if (cardTuple.Item2 != null)
                        {
                            player.Hand.Add(cardTuple.Item2);
                        }
                    }
                }
            }

            public bool InProgress()
            {
                if (HowManyActivePlayers == 1) return false;
                return true;
            }

            //public bool RoundInProgress()
            //{
            //    if (roundInProgress)
            //        return true;
            //    roundInProgress = true;
            //    return false;
            //}

            public int CurrentPlayer() => currentPlayerIndex;

            public Card TakeCard()
            {
                var card = deck[deck.Count - 1];
                deck.RemoveAt(deck.Count - 1);
                return card;
            }

            public void DealCards(Player player)
            {
                while(player.Hand.Count < 6)
                    player.Hand.Add(TakeCard());
            }
           
            public void Attack() 
            {
                var player = players[currentPlayerIndex];

                Console.WriteLine("Please, choose cards to attack from following list");
                Console.WriteLine(player.Hand);

                //Assert(player.Hand.Count > 0)

                int cardCount = int.Parse(Console.ReadLine());
                int[] cards = new int[cardCount];
                for (int i = 0; i < cardCount; i++)
                {
                    cards[i] = int.Parse(Console.ReadLine());
                    table.Layout.Add(new Tuple<Card, Card>(player.Hand[cards[i]], null));
                    player.Hand[cards[i]] = null;
                }

                // remove nulls from the hand
                int pos = 0;
                for (int i = 0; i < player.Hand.Count; ++i)
                {
                    if (player.Hand[i] != null)
                    {
                        player.Hand[pos++] = player.Hand[i];
                    }
                }

                for (int i = pos; i < player.Hand.Count; ++i)
                {
                    player.Hand.RemoveAt(i);
                }

                //if (player.Hand.Count == 0)
                //{
                //    roundInProgress = false;
                //}
            }

            public void TryDefend() 
            {
                var player = players[currentPlayerIndex + 1]; // TODO FIXME

                Console.WriteLine("You need to beat following cards:");
                Console.WriteLine(table);
                Console.WriteLine("Please, choose cards to defend from following list");
                Console.WriteLine(player.Hand);

                int cardCount = table.Layout.Count;
                int[] cards = new int[cardCount];
                for (int i = 0; i < cardCount; i++)
                {
                    cards[i] = int.Parse(Console.ReadLine());


                    player.Hand[cards[i]] = null;
                }
            }
        }
    }
}

