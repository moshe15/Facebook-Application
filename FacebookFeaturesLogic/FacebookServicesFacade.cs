using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookFeaturesLogic
{
    public class FacebookServicesFacade : ILoggedInUserObservers
    {
        private DatingFeature m_DatingFeature;
        private YourLuckFeature m_YourLuckFeature;
        private FacebookBasicFeatures m_FacebookBasicFeatures;
        private IAggregate m_UserCheckins;
        private FaceBookUser m_LoggedInUser;

        public FacebookServicesFacade()
        {
            m_LoggedInUser = new FaceBookUser();
            m_LoggedInUser.AttachObserver(this as ILoggedInUserObservers);
            m_DatingFeature = new DatingFeature(m_LoggedInUser);
            m_YourLuckFeature = new YourLuckFeature();
            m_FacebookBasicFeatures = new FacebookBasicFeatures(m_LoggedInUser);           
        }

        public void Alluserchckins()
        {
            m_FacebookBasicFeatures.AllUserCheckins();
        }

        public void DoWhenUserLoggedIn(User i_LoggedInUser)
        {
            m_LoggedInUser.DoWhenUserLoggedIn(i_LoggedInUser);
        }

        public void ReportUserLoggedIn(User i_LoggedInUser)
        {
            m_LoggedInUser.LoggedInUser = i_LoggedInUser;
        }

        public List<string> GetALLFriendCheckins(User i_User)
        {
            return m_FacebookBasicFeatures.GetALLFriendCheckins(i_User);
        }

        public List<User> AllFriendByZodiacSign(IFriendsBy i_FriendsBy)
        {
            List<User> allMyFriends = m_FacebookBasicFeatures.GetAllUserFriends();
            return m_YourLuckFeature.ZodiacFriends(i_FriendsBy, allMyFriends);
        }
       
        public List<Page> GetAllUserPages()
        {
            return m_FacebookBasicFeatures.GetAllUserPages();
        }

        public List<Event> GetAllUserEvents()
        {
            return m_FacebookBasicFeatures.GetAllUserEvents();
        }

        public IIterator GetAllUserCheckins()
        {
            m_UserCheckins = new Checkins(m_FacebookBasicFeatures);
            return m_UserCheckins.CreateIterator();
        }

        public List<User> GetAllUserFriends()
        {
            return m_FacebookBasicFeatures.GetAllUserFriends();
        }

        public List<string> GetAllUserPosts()
        {
            return m_FacebookBasicFeatures.GetAllUserPosts();
        }

        public void SharePost(string i_PostToShare)
        {
            m_FacebookBasicFeatures.SharePost(i_PostToShare);
        }

        public string NameOfTheLoggenInUser()
        {
            return m_FacebookBasicFeatures.NameOfTheLoggenInUser();
        }

        public void CreateAlbum(string i_albumName)
        {
            m_FacebookBasicFeatures.CreateAlbum(i_albumName);
        }

        public List<Album> GetAllUserAlbums()
        {
            return m_FacebookBasicFeatures.GetAllUserAlbums();
        }

        public DateTime BirthdayOfUserDateTime(User i_User)
        {
            return m_FacebookBasicFeatures.BirthdayOfUserDateTime(i_User);
        }

        public int HowManyLikes(User i_user)
        {
            return m_FacebookBasicFeatures.HowManyLikes(i_user);
        }

        public string BirthdayOfUser(User i_User)
        {
            return m_FacebookBasicFeatures.BirthdayOfUser(i_User);
        }

        public List<User> GetUserLoggedInMatch(string i_Gender, string i_City, string i_Work, string i_Education, int i_FromAge, int i_UntilAge, string i_Connection)
        {
            return m_DatingFeature.GetUserLoggedInMatch(i_Gender, i_City, i_Work, i_Education, i_FromAge, i_UntilAge, i_Connection);
        }

        public string AllCheckins(User i_User)
        {
            return m_DatingFeature.AllCheckins(i_User);
        }

        public HashSet<string> AllAdress()
        {
            return m_DatingFeature.AllAdress();
        }

        public string AllFriendDetails(User i_User)
        {
            return m_DatingFeature.AllFriendDetails(i_User);
        }

        public string AllcommonFriends(User i_User)
        {
            return m_DatingFeature.AllcommonFriends(i_User);
        }

        public eZodiac GetZodiac(DateTime i_DateTime)
        {
            return YourLuckFeature.GetZodiac(i_DateTime);
        }

        public Dictionary<eZodiac, string> CelebsData()
        {
            return m_YourLuckFeature.m_CelebsData;
        }
      
        public Dictionary<eZodiac, string> YourLuckData()
        {
            return m_YourLuckFeature.m_YourLuckData;
        }
    }
}