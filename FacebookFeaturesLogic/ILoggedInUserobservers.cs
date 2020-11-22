using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;
using Facebook;

namespace FacebookFeaturesLogic
{
    public interface ILoggedInUserObservers
    {
        void ReportUserLoggedIn(User i_LoggedInUser);
    }
}
