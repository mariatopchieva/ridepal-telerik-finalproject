using RidePal.Service.Providers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Service.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTime() => DateTime.Now;
    }
}
