using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AdminServer.Models
{
    public class ClientConfig
    {

        public int RefreshTime { get; set; }
        public int ReportPeriod { get; set; }



        public static readonly string Filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SavedConfigs.json");
        public static ClientConfig Saved
        {
            get
            {
                if (!File.Exists(Filename)) return null;
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ClientConfig>(File.ReadAllText(Filename));
            }
            set
            {
                File.WriteAllText(Filename, Newtonsoft.Json.JsonConvert.SerializeObject(value));
            }
        }

    }
}