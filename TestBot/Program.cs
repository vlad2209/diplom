using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ApiAiSDK;
using ApiAiSDK.Model;
using Telegram.Request;
using Telegram;



 

namespace TestBot
{
    class Program 
    {
        static TelegramBotClient Bot;
        static ApiAi apiAi;

        static void Main(string[] args)
        {

            Bot = new TelegramBotClient("792016498:AAEC2bDyfRLe2mdlK28Wc6GzccPg52oW_L0");
            AIConfiguration config = new AIConfiguration("890fc148d46046fe9ec0716db01c751e", SupportedLanguage.Russian);
            apiAi = new ApiAi(config);


            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallBackQueryReceived;



            var me = Bot.GetMeAsync().Result;

            Console.WriteLine(me.FirstName);

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StartReceiving();
        }

        private static async void BotOnCallBackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} нажал кнопку {buttonText}");

            try
            {

                if (buttonText == "Замены")
                {
                   Message x = await Bot.SendTextMessageAsync(e.CallbackQuery.Id, "http://kipt.sumdu.edu.ua/zaninu-na-zavtra.html");
                }
                else if (buttonText == "ИптКиСумДу")
                {
                   Message x = await Bot.SendTextMessageAsync(e.CallbackQuery.Id, "https://www.youtube.com/watch?v=mj4tAgzhE7E");
                }
                await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали конпку {buttonText}");
            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
            }


            

        }

        private static async void BotOnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;


            if (message == null || message.Type !=MessageType.Text)
                return; 
            string name = $"{message.From.FirstName} {message.From.LastName}";


            Console.WriteLine($"{name} отправил сообщение: '{message.Text}'");

            switch (message.Text)
            {
                case "/start":
                    string text =
                        @"Вот список команд которыми ты можешь пользоватся
/inline - вывод меню
/keyboard - вывод клавиатуры
/Напиши мне 'Привет' ";

                    await Bot.SendTextMessageAsync(message.From.Id, text);
                    break;

                case "/inline":
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Замены","http://kipt.sumdu.edu.ua/zaninu-na-zavtra.html"),
                            InlineKeyboardButton.WithUrl("VK","https://vk.com/id133155332"),
                            InlineKeyboardButton.WithUrl("Погода","https://sinoptik.ua/погода-конотоп-303011665 "),
                            InlineKeyboardButton.WithUrl("Рассписание Поездов ","http://poezdato.net/raspisanie-poezdov/bahmach--konotop/elektrichki/")

                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("731"),
                            InlineKeyboardButton.WithCallbackData("ИптКиСумДу")
                            
                        }

                    });
                    await Bot.SendTextMessageAsync(message.From.Id, "Выберите пункт меню",
                        replyMarkup: inlineKeyboard);
                    break;

                case "/keyboard":
                    var replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Привет"),
                            new KeyboardButton("Номери Телефонів"),
                            new KeyboardButton("Електронна пошта Вчителів")
                        },
                        new []
                        {
                            new KeyboardButton("Геолокация") {RequestLocation = true }

                        }
                    });
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сообщение",
                        replyMarkup: replyKeyboard);
                    break;

                default:
                    var response = apiAi.TextRequest(message.Text);
                    string answer = response.Result.Fulfillment.Speech;
                    if ( answer == "")

                       answer = "Прости, я тебя не понял";
                    await Bot.SendTextMessageAsync(message.From.Id, answer);
                    

                    break;
                   


            }
           

        }

    }
}
