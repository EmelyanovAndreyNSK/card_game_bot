using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples.Bots
{ 
    public class BotJack : ActivityHandler
    {
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await SendWelcomeMessageAsync(turnContext, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        { 
            var text = turnContext.Activity.Text.ToLowerInvariant(); // Extract the text from the message
            var responseText = ProcessInput(text);

            await turnContext.SendActivityAsync(responseText, cancellationToken: cancellationToken);// Respond
            await SendSuggestedActionsAsync(turnContext, cancellationToken);
        }

        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(
                        $"Hi, {member.Name}! Wanna play some card games?",
                        cancellationToken: cancellationToken);
                    await SendSuggestedActionsAsync(turnContext, cancellationToken);
                }
            }
        }

        private static string ProcessInput(string text)
        {
            const string Text = "is the best choice, let's go!";
            switch (text)
            {
                case "/start":
                    {
                        return "Hi! Wanna play some card games?";
                    }
                case "Durak":
                    {
                        return $"Durak {Text}";
                    }

                case "Black Jack":
                    {
                        return $"Black Jack {Text}";
                    }

                default:
                    {
                        return "Please select a game from the suggested action choices";
                    }
            }
        }

        private static async Task SendSuggestedActionsAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var reply = MessageFactory.Text("Choose a game to play from menu: ");

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction() { Title = "Durak", Type = ActionTypes.ImBack, Value = "Durak", Image = "https://upload.wikimedia.org/wikipedia/en/thumb/0/0b/Card_heart.svg/866px-Card_heart.svg.png"},
                    new CardAction() { Title = "Black Jack", Type = ActionTypes.ImBack, Value = "Black Jack", Image = "https://upload.wikimedia.org/wikipedia/en/thumb/b/b8/Card_spade.svg/1200px-Card_spade.svg.png"},
                },
            };
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }
    }
}