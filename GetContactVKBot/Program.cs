using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GetContactVKBot.Core;
using VkApi.Models;

namespace GetContactVKBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
        
            var s = new VkApi.Client(token);
            var c = await s.GetIdFromScreenName("club181495053");
            List<MemberModel> aas = new List<MemberModel>();
            await foreach (var member in s.GetMembers(c.Response))
            {
                if (member?.Response?.Items?.Count > 0)
                {
                    var memberModels = member.Response.Items.Where(x =>
                        !string.IsNullOrEmpty(x.MobilePhone) 
                        && new Regex(@"[0-9]+").IsMatch(x.MobilePhone.FormatPhone()))
                        .ToList();
                    memberModels.ForEach(x => x.MobilePhone = x.MobilePhone.FormatPhone());
                    aas.AddRange(memberModels);
                }
            }
            Console.WriteLine("Hello World!");
        }
    }
}