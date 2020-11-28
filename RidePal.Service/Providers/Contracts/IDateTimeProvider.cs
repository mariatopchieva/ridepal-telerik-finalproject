using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Service.Providers.Contracts
{
    public interface IDateTimeProvider
    {
        DateTime GetDateTime();
    }
}
