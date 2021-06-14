using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions.Services
{
    public class MetaInformationService
    {

        public DateTime GetEventRegistrationDeadline()
        {

            if (DateTime.TryParseExact(System.Environment.GetEnvironmentVariable("EventRegistrationDeadline"),"dd.MM.yyyy",CultureInfo.InvariantCulture,DateTimeStyles.None, out DateTime Deadline))
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
