using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace server.Handler
{
    public class VKSide
    {
        private static ulong ts;
        private static ulong? pts;
        private static VkApi api = new VkApi();
        private static Random rnd = new Random();
        public static void Init()
        {
            api.Authorize(new ApiAuthParams
            {
                AccessToken = Config.Data.cfg.vkToken
            });
            Console.WriteLine($"Acces token '{api.Token}' is true. / {api.Groups.GetById(null, null, null)[0].Name}");
            var longPoll = api.Messages.GetLongPollServer(needPts: true);
            pts = longPoll.Pts;
            ts = Convert.ToUInt64(longPoll.Ts);
            // Пытался оптимизировать код ниже, но при любых попытках руиниться
            while (true)
            {
                LongPollHistoryResponse longPollResponse = api.Messages.GetLongPollHistory(new MessagesGetLongPollHistoryParams() { Pts = pts, Ts = ts });
                pts = longPollResponse.NewPts;
                for (int i = 0; i < longPollResponse.History.Count; i++)
                {
                    switch (longPollResponse.History[i][0])
                    {
                        case 4:
                            if (longPollResponse.Messages.Count > i)
                                HandlerMessage(longPollResponse.Messages[i]);
                            break;
                    }
                }


            }


        }
        
        public static void HandlerMessage(Message mes)
        {
            var user = Tools.query.GetDataTable($"SELECT * FROM users WHERE id = {mes.FromId}").Rows;
            if (user.Count == 0)
            {
                var num = GenerateNumber();
                sms(mes.FromId, $"Ваш код: {num} \nПожалуйста не теряйте, чтобы узнать какая у вас скидка 'Скидка'(без кавычек)");
                Tools.query.Send($"INSERT INTO users (id, code, moneys, level) VALUES ({mes.FromId}, '{num}', 0, 0)");
            }
        }

        private static string GenerateNumber()
        {
            string number;
            do
            {
                number = "";
                number += (char)rnd.Next(0x0041, 0x005A);
                for (int i = 0; i < 3; i++)
                    number += (char)rnd.Next(0x0030, 0x0039);
                number += (char)rnd.Next(0x0041, 0x005A);

            } while (Tools.query.GetDataTable($"SELECT * FROM users WHERE 'code' = '{number}'").Rows.Count != 0);
            return number;
        }

        public int GetSale(int moneys)
        {
            if (moneys < 0)
                return 0;
            if (moneys < 2000)
                return Convert.ToInt32(moneys * 0.005);
            if (moneys < 20000)
                return Convert.ToInt32(moneys * 0.0001 < 10 ? 12 : moneys * 0.001);
            if (moneys < 200000)
                return Convert.ToInt32(moneys * 0.0002 < 20 ? 22 : moneys * 0.0002);
            if (moneys > 300000)
                return 65;
            return 0;
        }

        public static void sms(long? id, string message)
        {
            if (id > 2000000000 || id < 0)
                api.Messages.Send(new MessagesSendParams
                {
                    RandomId = rnd.Next(1, 1000 * 255), // уникальный
                    PeerId = id,
                    Message = message
                });
            else
                api.Messages.Send(new MessagesSendParams
                {
                    RandomId = rnd.Next(1, 1000 * 255), // уникальный
                    UserId = id,
                    Message = message
                });
        }
    }
}
