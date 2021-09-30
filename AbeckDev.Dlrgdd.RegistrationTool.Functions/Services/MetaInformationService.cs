using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions.Services
{
    public class MetaInformationService
    {
        public DateTime GetEventRegistrationStartDate()
        {

            if (DateTime.TryParseExact(System.Environment.GetEnvironmentVariable("EventRegistrationStart"), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
            {
                return startDate;
            }

            throw new Exception("Could not determine StartDate from Configuration. Please Check settings!");
        }


        public DateTime GetEventRegistrationDeadline()
        {

            if (DateTime.TryParseExact(System.Environment.GetEnvironmentVariable("EventRegistrationDeadline"),"dd.MM.yyyy",CultureInfo.InvariantCulture,DateTimeStyles.None, out DateTime Deadline))
            {
                return Deadline;
            }

            throw new Exception("Could not determine Deadline from Configuration. Please Check settings!");
        }


        public bool IsRegistrationPossible()
        {
            if (IsRegistrationStartReached() &&
                !IsRegistrationDeadlineReached())
            {
                return true;
            }

            return false;
        }


        public bool IsRegistrationStartReached()
        {
            if (DateTime.Compare(GetEventRegistrationStartDate(), DateTime.Now) <= 0)
            {

                return true;
            }

            return false;
        }

        public bool IsRegistrationDeadlineReached()
        {
            if (DateTime.Compare(GetEventRegistrationDeadline(), DateTime.Now) < 0)
            {

                return true;
            }

            return false;
        }
    }
}
