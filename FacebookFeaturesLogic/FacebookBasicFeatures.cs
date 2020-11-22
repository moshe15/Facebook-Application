using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;
using Facebook;

namespace FacebookFeaturesLogic
{
    internal class FacebookBasicFeatures : ILoggedInUserObservers
    {
        private FaceBookUser m_LoggedInUser { get; }

        public List<Checkin> UserLoggedInCheckins { get; private set; }
    
        public FacebookBasicFeatures(FaceBookUser i_UserLoggenIn)
        {
            m_LoggedInUser = i_UserLoggenIn;
            m_LoggedInUser.AttachObserver(this as ILoggedInUserObservers);         
        }

        public void AllUserCheckins()
        {
            UserLoggedInCheckins = new List<Checkin>();
            getAllUserCheckins();
        }

        public void ReportUserLoggedIn(User i_LoggedInUser)
        {
            m_LoggedInUser.LoggedInUser = i_LoggedInUser;
        }

        public List<string> GetAllUserPosts()
        {
            List<string> listPostLoggedInUser = new List<string>();

            if (m_LoggedInUser.LoggedInUser.Posts.Count > 0)
            {
                foreach (Post post in m_LoggedInUser.LoggedInUser.Posts)
                {
                    listPostLoggedInUser.Add(string.Format("The Post: {0}," + " posted time: {1} ," + " coments:{2}" + " Type:{3}", post.Message, post.UpdateTime, post.Comments.Count, post.Type.ToString()));
                }
            }

            return listPostLoggedInUser;
        }

        public List<Page> GetAllUserPages()
        {
            List<Page> listPagesLoggedInUser = new List<Page>();
            try
            {
                foreach (Page page in m_LoggedInUser.LoggedInUser.LikedPages)
                {
                    listPagesLoggedInUser.Add(page);
                }
            }
            catch (FacebookOAuthException)
            {
                listPagesLoggedInUser = null;
            }

            return listPagesLoggedInUser;
        }

        public List<Event> GetAllUserEvents()
        {
            List<Event> listEventsLoggedInUser = new List<Event>();

            foreach (Event Event in m_LoggedInUser.LoggedInUser.Events)
            {
                listEventsLoggedInUser.Add(Event);
            }

            return listEventsLoggedInUser;
        }
     
        public List<User> GetAllUserFriends()
        {
            List<User> listFriendsLoggedInUser = new List<User>();
            foreach (User friend in m_LoggedInUser.LoggedInUser.Friends)
            {
                listFriendsLoggedInUser.Add(friend);
            }

            return listFriendsLoggedInUser;
        }
        
        private void getAllUserCheckins()
        {          
          foreach (Checkin checkin in m_LoggedInUser.LoggedInUser.Checkins)
           {
             UserLoggedInCheckins.Add(checkin);
           }            
        }
      
        public void CreateAlbum(string i_albumName)
        {
            m_LoggedInUser.LoggedInUser.CreateAlbum(i_albumName);
        }

        public List<Album> GetAllUserAlbums()
        {
            List<Album> listAlbumsLoggedInUser = new List<Album>();
            {
                foreach (Album album in m_LoggedInUser.LoggedInUser.Albums)
                {
                    listAlbumsLoggedInUser.Add(album);
                }
            }

            return listAlbumsLoggedInUser;
        }

        public void SharePost(string i_PostToShare)
        {
            m_LoggedInUser.LoggedInUser.PostStatus(i_PostToShare);
        }

        public DateTime BirthdayOfUserDateTime(User i_User)
        {
            DateTime birthdayOfTheUser;
            string value = i_User.Birthday;
            string[] birthdaySplit = value.Split('/');
            int.TryParse(birthdaySplit[0], out int month);
            int.TryParse(birthdaySplit[1], out int day);
            int.TryParse(birthdaySplit[2], out int year);
            birthdayOfTheUser = new DateTime(year, month, day);
            return birthdayOfTheUser;
        }

        public string NameOfTheLoggenInUser()
        {
            return m_LoggedInUser.LoggedInUser.FirstName.ToString();
        }

        public List<string> GetALLFriendCheckins(User i_User)
        {
            List<string> allFriendCheckins = new List<string>();

            try
            {
                foreach (Checkin checkin in i_User.Checkins)
                {
                    allFriendCheckins.Add(checkin.Place.Name.ToString());
                }
            }
            catch (FacebookOAuthException)
            {
                allFriendCheckins = null;
            }

            return allFriendCheckins;
        }

        public int HowManyLikes(User i_user)
        {
            int counterOfLikes = 0;
            try
            {
                foreach (Album album in m_LoggedInUser.LoggedInUser.Albums)
                {
                    foreach (Photo photo in album.Photos)
                    {
                        if (photo.LikedBy.Contains(i_user))
                        {
                            counterOfLikes++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                counterOfLikes = 0;
            }

            return counterOfLikes;
        }
   
        public string BirthdayOfUser(User i_User)
        {
            return i_User.Birthday;
        }
    }
}