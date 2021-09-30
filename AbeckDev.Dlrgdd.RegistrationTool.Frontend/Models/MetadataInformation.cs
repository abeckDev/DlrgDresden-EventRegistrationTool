using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbeckDev.Dlrgdd.RegistrationTool.Frontend.Models
{
    public class MetadataInformation
    {

        [JsonProperty("registrationDeadline")]
        public DateTime RegistrationDeadline { get; set; }

        [JsonProperty("RegistrationStart")]
        public DateTime RegistrationStart { get; set; }

        [JsonProperty("isDeadlineReached")]
        public bool IsRegistrationDeadlineReached { get; set; }

        [JsonProperty("isRegistrationStartReached")]
        public bool IsRegistrationStartReached { get; set; }

        [JsonProperty("isRegistrationPossible")]
        public bool IsRegistrationPossible { get; set; }


        public bool IsLoaded { get; set; } = false;

    }
}
