using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ConsoleApp1
{
    class Program
    {
        static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("1868392535:AAFCpKEYWvV4zs8kzjBYKrIIYX4K99vUNdM");

            Bot.OnMessage += Bot_OnMessageReceived;
            Bot.OnCallbackQuery += Bot_OnCallbackQueryReceived;
            var me = Bot.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);
            Bot.StartReceiving();
            Console.ReadLine();
        }

        private static void Bot_OnCallbackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static async void Bot_OnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;

            //if (message == null || message.Type != MessageType.Text)
            // return;
            Console.WriteLine(message.Text);
            await Bot.SendTextMessageAsync(message.From.Id, $"Welcome to the club buddy!)");
        }
    }
}
