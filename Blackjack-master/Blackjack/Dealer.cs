
namespace Blackjack
{
    public class Dealer : PlayerBase
    {
        public Dealer()
        {
            this.Hand = new Hand(isDealer: true);
        }
    }
}
