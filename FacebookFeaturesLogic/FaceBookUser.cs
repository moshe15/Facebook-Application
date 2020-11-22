using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;
using Facebook;

namespace FacebookFeaturesLogic
{
    public class FaceBookUser
    {
        private readonly List<ILoggedInUserObservers> m_userObservers;

        public User LoggedInUser { get; set; }

        public FaceBookUser()
        {
            m_userObservers = new List<ILoggedInUserObservers>();
        }
     
        public void AttachObserver(ILoggedInUserObservers i_UserObservers)
        {
            m_userObservers.Add(i_UserObservers);
        }

        public void DetachObserver(ILoggedInUserObservers i_UserObservers)
        {
            m_userObservers.Remove(i_UserObservers);
        }

        public void DoWhenUserLoggedIn(User i_LoggedInUser)
        {
            LoggedInUser = i_LoggedInUser;
            notifyUserObservers();
        }

        private void notifyUserObservers()
        {
            foreach (ILoggedInUserObservers observer in m_userObservers)
            {
                observer.ReportUserLoggedIn(LoggedInUser);
            }
        }
    } 
}
