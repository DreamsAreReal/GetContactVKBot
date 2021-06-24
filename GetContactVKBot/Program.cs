using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GetContactVKBot.Core;
using Telegram.Bot;
using VkApi.Models;

namespace GetContactVKBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            new GetContactVKBot.Telegram.Client().Startup();
            
            Console.WriteLine("Enter \"exit\" for exit");
            while (true)
            {
                if (Console.ReadLine() == "exit")
                {
                    return;
                }
            }
        }
    }
}