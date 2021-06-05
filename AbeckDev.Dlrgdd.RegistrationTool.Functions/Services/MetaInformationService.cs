using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions.Services
{
    public class MetaInformationService
    {

        public DateTime GetEventRegistrationDeadline()
        {
            if (DateTime.TryParse(System.Environment.GetEnvironmentVariable("EventRegistrationDeadline"),out DateTime Deadline))
            {
                return Deadline;
            }

            throw new Exception("Could not determine Deadline from Configuration. Please Check settings!");
        }

        public bool IsRegistrationDeadlineReached()
        {

            var firstDate = GetEventRegistrationDeadline();
            var secondDate = DateTime.Now;
            var debug = DateTime.Compare(GetEventRegistrationDeadline(), DateTime.Now);

            if (DateTime.Compare(GetEventRegistrationDeadline(), DateTime.Now) < 0)
            {

                return true;
            }

            return false;
        }
    }
}
