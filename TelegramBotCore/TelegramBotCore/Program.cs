using BLL.Models;
using BLL.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotCore
{
    public static class Program
    {
        private static TelegramBotClient Bot;
        private static TransactionService Service { get; set; }

        public static async Task Main()
        {
            Bot = new TelegramBotClient(Configuration.BotToken);

            Service = new TransactionService();
            var me = await Bot.GetMeAsync();
            Console.Title = me.Username;
            
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");

            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            Service.AddUser(Convert.ToString(message.Chat.Id));
            if (message == null || message.Type != MessageType.Text)
                return;

            switch (message.Text.Split(' ').First())
            {
                // send custom keyboard
                case "/keyboard":
                    await SendReplyKeyboard(message);
                    break;
                case "/start":
                    await Start(message);
                    break;
                case "/exit":
                    await Exit(message);
                    break;
                case "/addincome":
                    await AddIncome(message);
                    break;
                case "AddIncome":
                    await AddIncome(message);
                    break;
                case "AddI":
                    await AddI(message);
                    break;
                case "/addspending":
                    await AddSpending(message);
                    break;
                case "AddSpending":
                    await AddSpending(message);
                    break;
                case "AddS":
                    await AddS(message);
                    break;
                case "/showstat":
                    await ShowStatistics(message);
                    break;
                case "ShowStatistic":
                    await ShowStatistics(message);
                    break;
                default:
                    await Usage(message);
                    break;
            }

            static async Task SendReplyKeyboard(Message message)
            {
                var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[][]
                    {
                        new KeyboardButton[] { "AddIncome", "AddSpending" },
                        new KeyboardButton[] { "Show Statistic", "Exit" },
                    },
                    resizeKeyboard: true
                );

                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: replyKeyboardMarkup

                );
            }

            static async Task Usage(Message message)
            {
                const string usage = "Usage:\n" +
                                        "/start - show existing commands " +
                                        "/keyboard - open keyboard with commands\n"
                                        +
                                        "/addincome - Adding new Income\n"
                                        +
                                        "/addspending - Adding new Spending\n"
                                        +
                                        "/showstat - Shows Income and Spending Statistics\n"
                                        +
                                        "/exit - Exit to Main Menu\n";
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: usage,
                    replyMarkup: new ReplyKeyboardRemove()
                );
            }
            
            static async Task Start(Message message)
            {
                const string start = "Existing commands:\n" +
                                        "/keyboard - open keyboard with commands\n"
                                        +
                                        "/addincome - Adding new Income\n"
                                        +
                                        "/addspending - Adding new Spending\n"
                                        +
                                        "/showstat - Shows Income and Spending Statistics\n"
                                        +
                                        "/exit - Exit to Main Menu\n";
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Welcome to the club buddy:)",
                    replyMarkup: new ReplyKeyboardRemove()
                );
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: start,
                    replyMarkup: new ReplyKeyboardRemove()
                );
            }

            static async Task Exit(Message message)
            {
                const string exit = "See you soon buddy:)\n";
                const string start = "To show commands press:\n"
                                        +
                                        "/keyboard - open keyboard with commands\n"
                                        +
                                        "/start - show existing commands\n"
                                        ;
                await Bot.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: exit,
                   replyMarkup: new ReplyKeyboardRemove()
               );
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: start,
                    replyMarkup: new ReplyKeyboardRemove()
                );
            }

            static async Task AddIncome(Message message)
            {
                const string f = "Write bellow:\n";
                const string start = "To add income:\n"
                                        +
                                        "write - AddI *<sum> *<appointment> \n"
                                        ;
                await Bot.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: f,
                   replyMarkup: new ReplyKeyboardRemove()
               );
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: start,
                    replyMarkup: new ReplyKeyboardRemove()
                );
            }

            static async Task AddI(Message message)
            {
                const string f = "Income succesfull added!";
                var transactionType = "Income";
                var chatId = Convert.ToString(message.Chat.Id);
                var att = message.Text.Split('*');
                
                var sum = Convert.ToDouble(att[1].Trim());
                var appointment = att[2];
                var transaction = new Transaction(sum, appointment, transactionType);
                Service.AddTransaction(chatId, transaction);
                string result = $"Sum: {transaction.Amount}\nAppointment: {transaction.Appointment}";

                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: f,
                    replyToMessageId: message.MessageId
                );
                   await Bot.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: result,
                   replyMarkup: new ReplyKeyboardRemove()
               );
            }

            static async Task AddSpending(Message message)
            {
                const string f = "Write bellow:\n";
                const string start = "To add spending:\n"
                                        +
                                        "write - AddS *<sum> *<appointment> \n"
                                        ;
                await Bot.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: f,
                   replyMarkup: new ReplyKeyboardRemove()
               );
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: start,
                    replyMarkup: new ReplyKeyboardRemove()
                );
            }

            static async Task AddS(Message message)
            {
                const string f = "Spending succesfull added!";

                var transactionType = "Spending";
                var chatId = Convert.ToString(message.Chat.Id);
                var att = message.Text.Split('*');

                var sum = Convert.ToDouble(att[1].Trim());
                var appointment = att[2];
                var transaction = new Transaction(sum, appointment, transactionType);
                Service.AddTransaction(chatId, transaction);
                string result = $"Sum: {transaction.Amount}\nAppointment: {transaction.Appointment}";

                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: f,
                    replyToMessageId: message.MessageId
                );
                await Bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: result,
                replyMarkup: new ReplyKeyboardRemove()
            );
            }
            static async Task ShowStatistics(Message message)
            {
                string income = "Incomes:";
                int count = 1;
                double incomeSum = 0;
                foreach(var t in Service.GetTransactions(Convert.ToString(message.Chat.Id),"Income"))
                {
                    if(t != null)
                    {
                        income += $"\n{count})\nSum: {t.Amount}\nAppointment: {t.Appointment}";
                        incomeSum += t.Amount;
                        count++;
                    }
                }

                string spendings = "Spendings:";
                count = 1;
                double spendingsSum = 0;
                foreach (var t in Service.GetTransactions(Convert.ToString(message.Chat.Id), "Spending"))
                {
                    if (t != null)
                    {
                        spendings += $"\n{count})\nSum: {t.Amount}\nAppointment: {t.Appointment}";
                        spendingsSum += t.Amount;
                        count++;
                    }
                }

                string left = $"Money left: {incomeSum-spendingsSum}";
                await Bot.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: income,
                   replyMarkup: new ReplyKeyboardRemove()
               );
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: spendings,
                    replyMarkup: new ReplyKeyboardRemove()
                );
                await Bot.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: left,
                   replyMarkup: new ReplyKeyboardRemove()
               );
            }
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }
    }
}
