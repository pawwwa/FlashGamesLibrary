using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibFinal.Infrastructure
{
    [Serializable]
    class Settings
    {
        public string Localization { get; set; }

        public static void Serialize(Settings instance)
        {
            var data = JsonConvert.SerializeObject(instance);

            File.WriteAllText(Environment.CurrentDirectory + @"\settings.json", data);
        }

        public static Settings Deserialize()
        {
            var data = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Environment.CurrentDirectory + @"\settings.json"));
            return data;
        }
    }
}
