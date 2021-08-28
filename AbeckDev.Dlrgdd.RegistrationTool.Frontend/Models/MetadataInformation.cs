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

        [JsonProperty("isDeadlineReached")]
        public bool IsRegistrationDeadlineReached { get; set; }

        public bool IsLoaded { get; set; } = false;

    }
}
