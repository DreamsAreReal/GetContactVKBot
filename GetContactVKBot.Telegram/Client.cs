using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GetContactVKBot.Core;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace GetContactVKBot.Telegram
{
    public class Client
    {
        
        private static TelegramBotClient _client;
        private string _token = "1814767350:AAGAVnHjYCgPMvxfRZoegCY4B8-1Apa_SGY";
        private VkApi.Client _vkClient;


        public void Startup()
        {
            _client = new TelegramBotClient(_token);
#pragma warning disable 618
            _client.OnMessage += BotOnMessageReceived;
            _client.StartReceiving();
#pragma warning restore 618
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            if (messageEventArgs == null)
            {
                return;
            }

            var message = messageEventArgs.Message;
            if (message?.Type == MessageType.Text)
            {
                if (new Regex(@"token .+").IsMatch(message.Text))
                {
                    try
                    {
                        var token = new Regex(@"token (.*)").Match(message.Text).Groups[1].Value;
                        if (string.IsNullOrEmpty(token))
                        {
                            await _client.SendTextMessageAsync(message.Chat.Id, "Не смогли спарсить токен");
                            return;
                        }

                        _vkClient = new VkApi.Client(token);
                        await _client.SendTextMessageAsync(message.Chat.Id, "Токен установлен");
                    }
                    catch (Exception e)
                    {
                        await _client.SendTextMessageAsync(message.Chat.Id, e.Message);
                        throw;
                    }
                }
                else
                {
                    if (!message.Text.Contains("https"))
                    {
                        await _client.SendTextMessageAsync(message.Chat.Id, "Неверная ссылка");
                        return;
                    }

                    var screenName = new Regex(@".+:\/\/.+\/(.*)").Match(message.Text).Groups[1].Value;
                    if (string.IsNullOrEmpty(screenName))
                    {
                        await _client.SendTextMessageAsync(message.Chat.Id, "Неверная ссылка");
                        return;
                    }

                    if (_vkClient == null)
                    {
                        await _client.SendTextMessageAsync(message.Chat.Id, "Установите токен");
                        return;
                    }

                    var groupId = await _vkClient.GetIdFromScreenName("club181495053");
                    var statusMessage = await _client.SendTextMessageAsync(message.Chat.Id, "Начинаю парсинг");
                    long offset = 0;
                    int? count = 0;
                    List<string> numbers = new List<string>();
                    try
                    {
                        await foreach (var member in _vkClient.GetMembers(groupId.Response))
                        {
                            count = member?.Response?.Count;
                            if (member?.Response?.Items?.Count > 0)
                            {
                                var memberModels = member.Response.Items.Where(x =>
                                        !string.IsNullOrEmpty(x.MobilePhone)
                                        && new Regex(@"[0-9]+").IsMatch(x.MobilePhone.FormatPhone()))
                                    .ToList();
                                memberModels.ForEach(x => x.MobilePhone = x.MobilePhone.FormatPhone());
                                numbers.AddRange(memberModels.Select(x => x.MobilePhone));
                            }

                            offset += 1000;
                            await _client.EditMessageTextAsync(statusMessage.Chat, statusMessage.MessageId,
                                $"Обработано {offset} из {member?.Response?.Count} участников");
                        }

                        await _client.EditMessageTextAsync(statusMessage.Chat, statusMessage.MessageId,
                            $"Готово, всего {count} участников");
                        
                        numbers.ForEach(x=> _client.SendTextMessageAsync(message.Chat.Id, x).Wait());
                    }
                    catch (Exception e)
                    {
                        await _client.SendTextMessageAsync(message.Chat.Id, e.Message);
                        throw;
                    }
                }
            }
        }
    }
}

