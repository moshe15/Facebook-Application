using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookFeaturesLogic
{
    internal class DatingFeature : ILoggedInUserObservers
    {
        private FaceBookUser m_LoggedInUser { get; }

        public DatingFeature(FaceBookUser i_User)
        {
            m_LoggedInUser = i_User;
            m_LoggedInUser.AttachObserver(this as ILoggedInUserObservers);
        }

        public void ReportUserLoggedIn(User i_LoggedInUser)
        {
            m_LoggedInUser.LoggedInUser = i_LoggedInUser;
        }

        public List<User> GetUserLoggedInMatch(string i_Gender, string i_City, string i_Work, string i_Education, int i_FromAge, int i_UntilAge, string i_Connection)
        {
            List<User> friendListMatch = new List<User>();

            foreach (User user in m_LoggedInUser.LoggedInUser.Friends)
            {
                if (user.Gender.ToString().Equals(i_Gender) && ((user.Location == null) || user.Location.Name.ToString().Equals(i_City)) && user.WorkExperiences == null && user.Educations == null && AgeOfTheFriend(user) >= i_FromAge && AgeOfTheFriend(user) <= i_UntilAge)
                {
                    friendListMatch.Add(user);
                }
            }

            return friendListMatch;
        }

        public string AllCheckins(User i_User)
        {
            string allUserCheckins = string.Empty;
            List<string> listOfCommonCheckins = ListOfCommonCheckins(i_User.Checkins);
            foreach (string CommonCheckin in listOfCommonCheckins)
            {
                allUserCheckins += CommonCheckin;
                allUserCheckins += Environment.NewLine;
            }

            return allUserCheckins;
        }

        public HashSet<string> AllAdress()
        {
            HashSet<string> allAdressesOfTheFriends = new HashSet<string>();
            foreach (User friend in m_LoggedInUser.LoggedInUser.Friends)
            {
                allAdressesOfTheFriends.Add(AdressOfUser(friend));
            }

            return allAdressesOfTheFriends;
        }

        public string AllFriendDetails(User i_User)
        {
            string info = string.Empty;
            info = string.Format("the age of the friend: {0} " + Environment.NewLine +
                "the status: {1}" + Environment.NewLine +
                "the addres: {2}",
            AgeOfTheFriend(i_User), RelationShipStatus(i_User), AdressOfUser(i_User));
            return info;
        }

        public int AgeOfTheFriend(User i_User)
        {
            int currentYear = DateTime.Now.Year;

            int yearBirth = int.Parse(i_User.Birthday.Substring(6, 4));

            return currentYear - yearBirth;
        }

        public string RelationShipStatus(User i_User)
        {
            string relationShipStatus = null;
            try
            {
                if (i_User.RelationshipStatus == null)
                {
                    relationShipStatus = "single";
                }
            }
            catch
            {
                relationShipStatus = "no relationship";
            }

            return relationShipStatus;
        }

        public string AllcommonFriends(User i_User)
        {
            string allCommonFriends = string.Empty;
            List<string> commonFriends = CommonFriends(i_User);

            foreach (string CommonFriend in commonFriends)
            {
                allCommonFriends += CommonFriend;

                allCommonFriends += Environment.NewLine;
            }

            return allCommonFriends;
        }

        public List<string> CommonFriends(User i_User)
        {
            List<string> listOfCommonFriends = new List<string>();

            try
            {
                foreach (User friend in m_LoggedInUser.LoggedInUser.Friends)
                {
                    foreach (User userFriend in i_User.Friends)
                    {
                        if (friend.Name.ToString().Equals(userFriend.Name.ToString()))
                        {
                            listOfCommonFriends.Add(friend.Name.ToString());
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                listOfCommonFriends = null;
            }

            return listOfCommonFriends;
        }

        public string AdressOfUser(User i_User)
        {
            string addressOfUser = null;

            if (i_User.Location != null)
            {
                if (i_User.Location.Name != null)
                {
                    addressOfUser = i_User.Location.Name.ToString();
                }
            }

            return addressOfUser;
        }

        public List<string> ListOfCommonCheckins(FacebookObjectCollection<Checkin> i_CheckinsOfFriend)
        {
            List<string> commonCheckins = new List<string>();
            try
            {
                foreach (Checkin loggenInUserCheckins in m_LoggedInUser.LoggedInUser.Checkins)
                {
                    foreach (Checkin friendCheckins in i_CheckinsOfFriend)

                    {                 
                        if (loggenInUserCheckins.Place.Name.Equals(friendCheckins.Place.Name))
                        {
                            commonCheckins.Add(loggenInUserCheckins.Place.Name.ToString());
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                commonCheckins.Add("sorry, cant get checkins");
            }

            if (commonCheckins.Count == 0)
            {
                commonCheckins.Add("no common checkins");
            }

            return commonCheckins;
        }
    }
}
