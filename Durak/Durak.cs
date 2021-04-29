using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Bots
{
    public class Durak
    {
        public enum Suits { Clubs, Diamonds, Hearts, Spades };
        public enum Ranks { Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

        public Dictionary<Ranks, int> rankValue = new Dictionary<Ranks, int>();

        public class Card
        {
            private Suits Suit { get; }
            private Ranks Rank { get; }

            public Card (Suits suit, Ranks rank)
            {
                this.Suit = suit;
                this.Rank = rank;
            }

        }

        List<Card> deck;

        public Durak()
        {
            Suits[] suits = new Suits[] { Suits.Clubs, Suits.Diamonds, Suits.Hearts, Suits.Spades };
            Ranks[] ranks = new Ranks[] { Ranks.Six, Ranks.Seven, Ranks.Eight, Ranks.Nine, Ranks.Ten, Ranks.Jack, Ranks.Queen, Ranks.King, Ranks.Ace };

            deck = new List<Card>();
            foreach (var suit in suits)
            {
                foreach(var rank in ranks)
                {
                    deck.Add(new Card(suit, rank));
                }
            }
            
            Random random = new Random();
            deck = deck.OrderBy(x => random.Next()).ToList();
        }

        public Card TakeCard()
        {
            var card = deck[deck.Count-1];
            deck.RemoveAt(deck.Count - 1);
            return card;
        }

        public class Player
        {
            private List<Card> hand;

            public Player()
            {
                hand = new List<Card>();
            }

            public void TakeCard(Card card)
            {
                hand.Add(card); 
            }

            public int HowManyCards()
            {
                return hand.Count();
            }
        }

    }
}
