using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VkApi.Models;

namespace VkApi
{
    public class Client
    {
        private HttpClient _client;
        private string _token;
        
        public Client(string token)
        {
            _token = token;
            _client = new HttpClient();
        }
        
        public async IAsyncEnumerable<RootModel<ListModel<MemberModel>>> GetMembers(ObjectModel model)
        {
            if (model.Type != "group")
            {
                throw new Exception("GetMembers can parse from group only");
            }

            RootModel<ListModel<MemberModel>> list;
            long offset = 0;
            do
            {
                var answer = await (await _client.GetAsync($"{Routes.MainRoute}{Routes.GetMembersFromGroupRoute}?count=1000&offset={offset}&group_id={model.ObjectId}&fields=contacts&v={Consts.Version}&access_token={_token}")).Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<RootModel<ListModel<MemberModel>>>(answer);
                yield return list;
                offset += 1000;
                Console.WriteLine($"{offset}<{list?.Response?.Count}");
                await Task.Delay(2000);
            } while (list != null && list?.Response?.Count>offset);
            
        }

        public async Task<RootModel<ObjectModel>> GetIdFromScreenName(string screenName)
        {
            var answer = await (await _client.GetAsync($"{Routes.MainRoute}{Routes.GetScreenNameRoute}?screen_name={screenName}&v={Consts.Version}&access_token={_token}")).Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RootModel<ObjectModel>>(answer);
        }
    }
}