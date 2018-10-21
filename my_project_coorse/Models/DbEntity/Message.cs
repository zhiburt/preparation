using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using preparation.Models.Account;

namespace preparation.Models.DbEntity
{
    public enum MessageLevel
    {
        Spam = -2,
        Read,
        Normal,
        Important,
    }

    public class Message
    {
        [Key]
        public string Id { get; set; }
        public MessageLevel Level { get; set; }
        public string Test { get; set; }
        public string From { get; set; }

        public IDictionary<string, dynamic> GetDictionary()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(this.Test);
        }


        public IDictionary<string, dynamic> ParseDynamicFromJSON(string field)
        {
            if (field[0] != '{')
                return null;
            return JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(field);
        }

    }
}
