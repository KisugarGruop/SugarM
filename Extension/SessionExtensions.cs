using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SugarM.Extension {
    public static class SessionExtensions {
        public static void SetObjectAsJson (this ISession session, string key, object value) {
            session.SetString (key, JsonConvert.SerializeObject (value));
        }
        //ถ้าจะใช้ session สามาเรียกใช้ Oject ตัวนี้ได้เลย !
        public static T GetObjectFromJson<T> (this ISession session, string key) {
            var value = session.GetString (key);
            return value == null ? default (T) : JsonConvert.DeserializeObject<T> (value);
        }

    }
}