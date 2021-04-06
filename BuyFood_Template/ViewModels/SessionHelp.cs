using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public static class SessionHelp
    {
        //以前 ASP.NET 可以將物件型別直接存放到 Session，現在 ASP.NET Core Session 不再自動序列化物件到 Sesson。
        //如果要存放物件型態到 Session 就要自己序列化了，這邊以 JSON 格式作為範例
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
