using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_B_11_RPG
{
    class DataMamager<T>
    {
        public static void Save(T item,string filePath)
        {
            string json = JsonConvert.SerializeObject(item);
            File.WriteAllText(filePath, json);
        }

        public static T Load(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
