using System;
using System.Collections.Generic;
using System.Text;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions.Models
{
    public class MetaInformation
    {
        public DateTime RegistrationDeadline { get; set; }
        public DateTime RegistrationStart { get; set; }
        public bool IsDeadlineReached { get; set; }
        public bool IsRegistrationStartReached { get; set; }
        public bool IsRegistrationPossible { get; set; }
    }
}
