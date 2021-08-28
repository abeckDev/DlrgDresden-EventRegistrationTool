using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbeckDev.Dlrgdd.RegistrationTool.Frontend.Models
{
    public class UserRegistration
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [JsonProperty("birthday")]
        public string Birthday { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("zip")]
        public string zip { get; set; }
    }
}
