using System.Globalization;
using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;
using Facebook;
using FacebookFeaturesLogic;

namespace C20_Ex03_Mor_316046093_Moshe_308459841
{
    public sealed partial class FormFaceBook : Form
    {
        private static readonly object sr_LockObj = new object();
        private static FormFaceBook s_Instance = null;
        private FormLogin m_FormLogin;
        private User m_LoggedInUser;
        private FacebookServicesFacade m_FacebookServicesFacade;
        private AppSettings m_AppSettings;
        private IIterator m_UserCheckins;

        private FormFaceBook()
        {
            InitializeComponent();
        }

        public static FormFaceBook Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (sr_LockObj)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new FormFaceBook();
                        }
                    }
                }

                return s_Instance;
            }
        }

        private void FormFaceBook_Load(object sender, EventArgs e)
        {
            m_AppSettings = AppSettings.LoadFromFile();

            if (m_AppSettings.RememberUser == true)
            {
                try
                {
                    connectToRememberdUser();
                }
                catch (Exception)
                {
                    loginScreenToFaceBook();
                }
            }
            else
            {
                loginScreenToFaceBook();
            }
        }

        private void connectToRememberdUser()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Size = m_AppSettings.LastWindowSize;
            this.Location = m_AppSettings.LastWindowLocation;
            m_LoggedInUser = FacebookService.Connect(m_AppSettings.LastAccessToken).LoggedInUser;
            this.Invoke(new Action(() => this.Show()));
            m_FacebookServicesFacade = new FacebookServicesFacade();

            loadUserLoggenInDetails();
        }

        private void loginScreenToFaceBook()
        {
            if (m_FormLogin == null)
            {
                m_FormLogin = FormCreator.CreateForm(FormCreator.eSupportedForms.Login) as FormLogin;
            }

            m_FormLogin.ShowDialog();
            if (m_FormLogin.DialogResult == DialogResult.OK)
            {
                clearAllData();
                m_LoggedInUser = m_FormLogin.m_LoggedInUser;
                m_FacebookServicesFacade = new FacebookServicesFacade();
                m_FacebookServicesFacade.DoWhenUserLoggedIn(m_LoggedInUser);
                this.Show();
                loadUserLoggenInDetails();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void loadUserLoggenInDetails()
        {
            new Thread(fetchUserInfo).Start();
            new Thread(fetchFriends).Start();
            new Thread(fetchMyCheckins).Start();
            new Thread(initDating).Start();
            new Thread(fetchYourLuck).Start();
            new Thread(fetchPages).Start();
            new Thread(fetchEvents).Start();
            new Thread(fetchPosts).Start();
            userBindingSource.DataSource = m_LoggedInUser;
        }

        private void clearUserInfo()
        {
            userBindingSource.Clear();
        }

        private void getAllDetails(out string o_Gender, out string o_City, out string o_Work, out string o_Education, out int o_FromAge, out int o_UntilAge, out string o_Connection)
        {
            o_Gender = whichRadioButtonGender();
            o_City = whichCitySelected();
            o_Work = whichRadioButtonWork();
            o_Education = whichRadioButtonEducation();
            o_Connection = whichConnection();
            rangeAge(out o_FromAge, out o_UntilAge);
        }

        private void initDating()
        {
            comboBoxConnection.Invoke(new Action(() => comboBoxConnection.Items.Add("RelationShip")));
            comboBoxConnection.Invoke(new Action(() => comboBoxConnection.Items.Add("new friend")));
            foreach (string adress in m_FacebookServicesFacade.AllAdress())
            {
                if (adress != null)
                {
                    comboBoxCity.Invoke(new Action(() => comboBoxCity.Items.Add(adress)));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            getAllDetails(out string io_Gender, out string io_City, out string io_work, out string io_Education, out int io_FromAge, out int io_UntilAge, out string io_Connection);
            populteMatchFriends(io_Gender, io_City, io_work, io_Education, io_FromAge, io_UntilAge, io_Connection);
        }

        private void textBoxFromAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            char inputNum = e.KeyChar;
            if (!char.IsDigit(inputNum) && inputNum != 8 && inputNum != 46)
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char inputNum = e.KeyChar;
            if (!char.IsDigit(inputNum) && inputNum != 8 && inputNum != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxCreatePost_Enter(object sender, EventArgs e)
        {
            this.textBoxCreatePost.GotFocus += new EventHandler(textBox1_Focus);
        }

        protected void textBox1_Focus(Object sender, EventArgs e)
        {
            textBoxCreatePost.Text = string.Empty;
            textBoxCreatePost.ForeColor = Color.Black;
            if (textBoxCreatePost.Text == string.Empty)
            {
                ButtonPost.Enabled = false;
            }

            if (textBoxCreatePost.Text != string.Empty)
            {
                ButtonPost.Enabled = true;
            }
        }

        private void textBoxCreatePost_Leave(object sender, EventArgs e)
        {
            if (textBoxCreatePost.Text == string.Empty)
            {
                textBoxCreatePost.Text = "What's on your mind ," + m_FacebookServicesFacade.NameOfTheLoggenInUser() + "?";
                textBoxCreatePost.ForeColor = Color.Silver;
            }
        }

        private void ButtonPost_Click(object sender, EventArgs e)
        {
            try
            {
                m_FacebookServicesFacade.SharePost(textBoxCreatePost.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("sorry, you cant share this post");
            }
        }

        private void textBoxCreatePost_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCreatePost.Text.Trim().Length > 0 && !textBoxCreatePost.Text.Equals("What's on your mind ," + m_FacebookServicesFacade.NameOfTheLoggenInUser() + "?"))
            {
                ButtonPost.Enabled = true;
            }
        }

        private void fetchPosts()
        {
            List<string> listOfPosts = new List<string>();
            listOfPosts = m_FacebookServicesFacade.GetAllUserPosts();
            if (listOfPosts.Count == 0)
            {
                labelNoPosts.Invoke(new Action(() => labelNoPosts.Text = "No posts to return"));
            }

            foreach (string post in listOfPosts)
            {
                listBoxPosts.Invoke(new Action(() => listBoxPosts.Items.Add(post)));
            }
        }

        private void listBoxFriends_SelectedValueChanged(object sender, EventArgs e)
        {
            User friend = listBoxFriends.SelectedItem as User;
            pictureBoxFriend.Load(friend.PictureSmallURL);
            pictureBoxFriend.SizeMode = PictureBoxSizeMode.StretchImage;
            this.labelName.Text = friend.Name;
            this.labelBirthday.Text = friend.Birthday;
            this.labelGender.Text = friend.Gender == User.eGender.female ? "Female" : "Male";
        }

        private void listBoxMyFriends_SelectedValueChanged(object sender, EventArgs e)
        {
            listViewMyFriendsCheckins.Clear();
            User friend = listBoxMyFriends.SelectedItem as User;
            List<string> allFriendCheckins = m_FacebookServicesFacade.GetALLFriendCheckins(friend);
            if (friend.Checkins.Count == 0)
            {
                listViewMyFriendsCheckins.Items.Add("No checkins for this friend");
            }

            foreach (string checkin in allFriendCheckins)
            {
                listViewMyFriendsCheckins.Items.Add(checkin);
            }
        }

        private void populteMatchFriends(string i_Gender, string i_City, string i_Work, string i_Education, int i_FromAge, int i_UntilAge, string i_Connection)
        {
            List<User> friends = m_FacebookServicesFacade.GetUserLoggedInMatch(i_Gender, i_City, i_Work, i_Education, i_FromAge, i_UntilAge, i_Connection);
            if (friends.Count == 0)
            {
                MessageBox.Show("Sorry, we have not a Match for you!");
            }

            int i = 0, col = 0;

            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(100, 100);
            foreach (User user in friends)
            {
                pictureBox2.Load(user.PictureNormalURL);
                imgs.Images.Add(pictureBox2.Image);
                listViewDating.SmallImageList = imgs;
                listViewDating.Items.Add(user.Name, i);
                listViewDating.Items[col].SubItems.Add(m_FacebookServicesFacade.HowManyLikes(user).ToString());
                listViewDating.Items[col].SubItems.Add(m_FacebookServicesFacade.AllFriendDetails(user));
                listViewDating.Items[col].SubItems.Add(m_FacebookServicesFacade.AllCheckins(user));
                listViewDating.Items[col].SubItems.Add(m_FacebookServicesFacade.AllcommonFriends(user));
                i++;
                col++;
            }
        }

        private void fetchPages()
        {
            listBoxPages.Invoke(new Action(() => listBoxPages.Items.Clear()));
            listBoxPages.Invoke(new Action(() => listBoxPages.DisplayMember = "Name"));
            List<Page> allLikedPagesOfUser = m_FacebookServicesFacade.GetAllUserPages();
            try
            {
                foreach (Page userLikedPage in allLikedPagesOfUser)
                {
                    listBoxPages.Invoke(new Action(() => listBoxPages.Items.Add(userLikedPage)));
                }

                if (allLikedPagesOfUser.Count == 0)
                {
                    labelPagesLoggedInUser.Invoke(new Action(() => labelPagesLoggedInUser.Text = "No liked pages to return."));
                }
            }
            catch (Exception)
            {
                labelPagesLoggedInUser.Invoke(new Action(() => labelPagesLoggedInUser.Text = "Cannot fetch liked pages for current user from Facebook."));
            }
        }

        private void fetchEvents()
        {
            listBoxEvents.Invoke(new Action(() => listBoxEvents.Items.Clear()));
            listBoxEvents.Invoke(new Action(() => listBoxEvents.DisplayMember = "Name"));
            List<Event> allEventsUserLoggedIn = m_FacebookServicesFacade.GetAllUserEvents();
            try
            {
                foreach (Event userEvent in allEventsUserLoggedIn)
                {
                    listBoxEvents.Invoke(new Action(() => listBoxEvents.Items.Add(userEvent)));
                }

                if (allEventsUserLoggedIn.Count == 0)
                {
                    labelNoEvents.Invoke(new Action(() => labelNoEvents.Text = "No events to Return"));
                }
            }
            catch (FacebookOAuthException)
            {
                labelNoEvents.Text = " Cannot fetch events for current user from Facebook.";
            }
        }

        private void fetchFriends()
        {
            List<User> allFriends = m_FacebookServicesFacade.GetAllUserFriends();

            listBoxFriends.Invoke(new Action(() => listBoxFriends.DisplayMember = "Name"));
            listBoxMyFriends.Invoke(new Action(() => listBoxMyFriends.DisplayMember = "Name"));
            if (allFriends.Count == 0)
            {
                listBoxFriends.Invoke(new Action(() => labelNoFriends.Text = "No friends to return"));
                listBoxFriends.Invoke(new Action(() => labelNoFriendsCheck.Text = "No Friends to return"));
            }

            foreach (User friend in allFriends)
            {
                listBoxFriends.Invoke(new Action(() => listBoxFriends.Items.Add(friend)));
                listBoxMyFriends.Invoke(new Action(() => listBoxMyFriends.Items.Add(friend)));
                friend.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }
        }

        private void fetchUserInfo()
        {
            textBoxCreatePost.Invoke(new Action(() => textBoxCreatePost.Text = "What's on your mind," + m_LoggedInUser.FirstName.ToString() + "?"));           
        }

        private void fetchMyCheckins()
        {
            m_FacebookServicesFacade.Alluserchckins();
            m_UserCheckins = m_FacebookServicesFacade.GetAllUserCheckins();

            while (m_UserCheckins.MoveNext())
            {
                var userCheckins = m_UserCheckins.Current as Checkin;
                listBoxMyCheckins.Invoke(new Action(() => listBoxMyCheckins.Items.Add(userCheckins.Place.Name.ToString())));
            }
        }

        private void buttonFamous_Click(object sender, EventArgs e)
        {
            {
                string nameOfTheFamous = string.Empty;
                DateTime date = m_FacebookServicesFacade.BirthdayOfUserDateTime(m_LoggedInUser);
                eZodiac getLuck = m_FacebookServicesFacade.GetZodiac(date);

                m_FacebookServicesFacade.CelebsData().TryGetValue(getLuck, out nameOfTheFamous);
                textBoxNameFamous.Text = nameOfTheFamous;

                switch (getLuck)
                {
                    case eZodiac.Aries:
                        pictureBoxCeleb.Image = Properties.Resources.sarahJessicaParker;
                        break;

                    case eZodiac.Libra:
                        pictureBoxCeleb.Image = Properties.Resources.gandhi;
                        break;

                    case eZodiac.Scorpio:
                        pictureBoxCeleb.Image = Properties.Resources.billGates;
                        break;

                    case eZodiac.Aquarius:
                        pictureBoxCeleb.Image = Properties.Resources.oprahWinfrey;
                        break;

                    case eZodiac.Cancer:
                        pictureBoxCeleb.Image = Properties.Resources.ladyDiana;
                        break;

                    case eZodiac.Capricorn:
                        pictureBoxCeleb.Image = Properties.Resources.elvisPresley;
                        break;

                    case eZodiac.Gemini:
                        pictureBoxCeleb.Image = Properties.Resources.marilynMonroe;
                        break;

                    case eZodiac.Leo:
                        pictureBoxCeleb.Image = Properties.Resources.Madonna;
                        break;

                    case eZodiac.Pisces:
                        pictureBoxCeleb.Image = Properties.Resources.yitzhakRabin;
                        break;

                    case eZodiac.Sagittarius:
                        pictureBoxCeleb.Image = Properties.Resources.bruceLee;
                        break;

                    case eZodiac.Taurus:
                        pictureBoxCeleb.Image = Properties.Resources.davidBeckham;
                        break;

                    case eZodiac.Virgo:
                        pictureBoxCeleb.Image = Properties.Resources.seanConnery;
                        break;
                }
            }
        }

        private void fetchYourLuck()
        {
            textBoxMyDate.Invoke(new Action(() => textBoxMyDate.Text = m_FacebookServicesFacade.BirthdayOfUser(m_LoggedInUser)));
            foreach (var item in Enum.GetValues(typeof(eZodiac)))
            {
                listBoxZodiacSignFriends.Invoke(new Action(() => listBoxZodiacSignFriends.Items.Add(item)));
            }

            List<User> allFriends = m_FacebookServicesFacade.GetAllUserFriends();
            foreach (User friends in allFriends)
            {
                listBoxFriendHaveSign.Invoke(new Action(() => listBoxFriendHaveSign.Items.Add(friends.Name)));
            }
        }

        private void buttonSignOfZodiac_Click(object sender, EventArgs e)
        {
            string value = string.Empty;
            DateTime date = m_FacebookServicesFacade.BirthdayOfUserDateTime(m_LoggedInUser);
            eZodiac getLuck = m_FacebookServicesFacade.GetZodiac(date);
            m_FacebookServicesFacade.YourLuckData().TryGetValue(getLuck, out value);
            textBoxZodiac.Text = value;

            switch (getLuck)
            {
                case eZodiac.Aries:
                    pictureBoxZodiac.Image = Properties.Resources.Aries;
                    break;

                case eZodiac.Libra:
                    pictureBoxZodiac.Image = Properties.Resources.Libra;
                    break;

                case eZodiac.Scorpio:
                    pictureBoxZodiac.Image = Properties.Resources.Scorpio;
                    break;

                case eZodiac.Aquarius:
                    pictureBoxZodiac.Image = Properties.Resources.Aquarius;
                    break;

                case eZodiac.Cancer:
                    pictureBoxZodiac.Image = Properties.Resources.Cancer;
                    break;

                case eZodiac.Capricorn:
                    pictureBoxZodiac.Image = Properties.Resources.Capricorn;
                    break;

                case eZodiac.Gemini:
                    pictureBoxZodiac.Image = Properties.Resources.Gemini;
                    break;

                case eZodiac.Leo:
                    pictureBoxZodiac.Image = Properties.Resources.Leo;
                    break;

                case eZodiac.Pisces:
                    pictureBoxZodiac.Image = Properties.Resources.Pisces;
                    break;

                case eZodiac.Sagittarius:
                    pictureBoxZodiac.Image = Properties.Resources.Sagittarius;
                    break;

                case eZodiac.Taurus:
                    pictureBoxZodiac.Image = Properties.Resources.Taurus;
                    break;

                case eZodiac.Virgo:
                    pictureBoxZodiac.Image = Properties.Resources.Virgo;
                    break;
            }
        }

        private string whichRadioButtonGender()
        {
            string femaleOrMale;
            if (radioButtonFemale.Checked == true)
            {
                femaleOrMale = "female";
            }
            else
            {
                femaleOrMale = "male";
            }

            return femaleOrMale;
        }

        private string whichRadioButtonEducation()
        {
            string educationYesOrNo;
            if (radioButtonEducationYES.Checked == true)
            {
                educationYesOrNo = "yes";
            }
            else
            {
                educationYesOrNo = "no";
            }

            return educationYesOrNo;
        }

        private string whichRadioButtonWork()
        {
            string workYesOrNo;
            if (radioButtonWorkYES.Checked == true)
            {
                workYesOrNo = "yes";
            }
            else
            {
                workYesOrNo = "no";
            }

            return workYesOrNo;
        }

        private string whichCitySelected()
        {
            return comboBoxCity.SelectedItem.ToString();
        }

        private void rangeAge(out int o_FromAge, out int o_UntilAge)
        {
            int.TryParse(textBoxFromAge.Text, out o_FromAge);
            int.TryParse(textBoxUntilAge.Text, out o_UntilAge);
        }

        private string whichConnection()
        {
            return comboBoxConnection.Text.ToString();
        }

        private void buttonLuckFriends_Click(object sender, EventArgs e)
        {
            ArrangeFriendsByZodiacSign();
        }

        private void listBoxAlbums_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBoxPhoto.Image = null;
            try
            {
                pictureBoxPhoto.Image = (listBoxAlbums.SelectedItem as Album).ImageAlbum;
            }
            catch (FacebookOAuthException)
            {
                MessageBox.Show(" Cannot create an Photo for this user on Facebook.");
            }
        }

        private void buttonRefreshAlbums_Click(object sender, EventArgs e)
        {
            updateAlbums();
        }

        private void buttonCreateAlbum_Click(object sender, EventArgs e)
        {
            createAlbum();
        }

        private void updateAlbums()
        {
            listBoxAlbums.Items.Clear();
            listBoxAlbums.DisplayMember = "Name";

            foreach (Album albums in m_FacebookServicesFacade.GetAllUserAlbums())
            {
                listBoxAlbums.Items.Add(albums);
            }
        }

        private void createAlbum()
        {
            string getAlbumName = textBoxAlbums.Text;
            try
            {
                m_FacebookServicesFacade.CreateAlbum(getAlbumName);
            }
            catch (FacebookOAuthException)
            {
                MessageBox.Show(" Cannot create an album for this user on Facebook.");
            }
        }

        private void FormFaceBook_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_LoggedInUser != null)
            {
                m_AppSettings = AppSettings.LoadFromFile();
                if (m_AppSettings.RememberUser == true)
                {
                    m_AppSettings.RememberUser = true;
                }

                m_AppSettings.LastWindowSize = this.Size;
                m_AppSettings.LastWindowLocation = this.Location;
                m_AppSettings.SaveToFile();
            }
        }

        public void ArrangeFriendsByZodiacSign()
        {
            List<User> allFriendLoggedIn = m_FacebookServicesFacade.GetAllUserFriends();

            int indexOfRowFriend = 0;
            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(50, 50);
            DateTime dataUser = m_FacebookServicesFacade.BirthdayOfUserDateTime(m_LoggedInUser);

            DateTime friendBirthday;
            eZodiac userZodiacSign = m_FacebookServicesFacade.GetZodiac(dataUser);
            eZodiac friendsZodiacSigns;

            foreach (User friend in allFriendLoggedIn)
            {
                friendBirthday = DateTime.ParseExact(friend.Birthday, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                friendsZodiacSigns = m_FacebookServicesFacade.GetZodiac(friendBirthday);

                if (userZodiacSign.Equals(friendsZodiacSigns))
                {
                    pictureBox4.Load(friend.PictureNormalURL);
                    imgs.Images.Add(pictureBox4.Image);
                    listViewFriendsLuck.SmallImageList = imgs;
                    listViewFriendsLuck.Items.Add(friend.Name, indexOfRowFriend);
                    indexOfRowFriend++;
                }
            }
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookService.Logout(null);
                this.Hide();
            }
            catch (Exception)
            {
                MessageBox.Show("Error, unable to logout");
                Environment.Exit(0);
            }

            m_AppSettings.BackToDefault();
            m_AppSettings.SaveToFile();
            loginScreenToFaceBook();
        }

        private void clearAllData()
        {
            comboBoxConnection.Items.Clear();
            textBoxCreatePost.Text = string.Empty;
            listBoxPosts.Items.Clear();
            listBoxFriends.Items.Clear();
            pictureBoxFriend.Image = null;
            labelName.Text = string.Empty;
            labelBirthday.Text = string.Empty;
            labelGender.Text = string.Empty;
            pictureBoxFriend.Image = null;
            listBoxMyCheckins.Items.Clear();
            listBoxMyFriends.Items.Clear();
            listViewMyFriendsCheckins.Items.Clear();
            listBoxAlbums.Items.Clear();
            textBoxAlbums.Text = null;
            pictureBoxPhoto.Image = null;
            listBoxPages.Items.Clear();
            listBoxPages.Items.Clear();
            listBoxEvents.Items.Clear();
            textBoxFromAge.Text = string.Empty;
            textBoxUntilAge.Text = string.Empty;
            textBoxMyDate.Text = string.Empty;
            pictureBoxZodiac.Image = null;
            textBoxZodiac.Text = null;
            textBoxNameFamous.Text = null;
            pictureBoxCeleb.Image = null;
            listViewFriendsLuck.Items.Clear();
            listViewDating.Items.Clear();
            m_LoggedInUser = null;
            m_FacebookServicesFacade = null;
            textBoxSoulmate.Text = null;
            pictureBoxSoulmate.Image = null;
            clearUserInfo();
        }

        private void buttonSoulmate_Click(object sender, EventArgs e)
        {
            string value = string.Empty;
            DateTime date = m_FacebookServicesFacade.BirthdayOfUserDateTime(m_LoggedInUser);
            eZodiac getLuck = m_FacebookServicesFacade.GetZodiac(date);
            m_FacebookServicesFacade.YourLuckData().TryGetValue(getLuck, out value);
            textBoxSoulmate.Text = value;

            switch (getLuck)
            {
                case eZodiac.Aries:
                    pictureBoxSoulmate.Image = Properties.Resources.Gemini;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Gemini, out value);
                    textBoxSoulmate.Text = value;
                    break;

                case eZodiac.Libra:
                    pictureBoxSoulmate.Image = Properties.Resources.Leo;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Leo, out value);
                    textBoxSoulmate.Text = value;
                    break;

                case eZodiac.Scorpio:
                    pictureBoxSoulmate.Image = Properties.Resources.Virgo;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Virgo, out value);
                    textBoxSoulmate.Text = value;
                    break;

                case eZodiac.Aquarius:
                    pictureBoxSoulmate.Image = Properties.Resources.Capricorn;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Capricorn, out value);
                    textBoxSoulmate.Text = value;
                    break;

                case eZodiac.Cancer:
                    pictureBoxSoulmate.Image = Properties.Resources.Pisces;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Pisces, out value);
                    textBoxSoulmate.Text = value;
                    break;

                case eZodiac.Capricorn:
                    pictureBoxSoulmate.Image = Properties.Resources.Aquarius;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Aquarius, out value);
                    textBoxSoulmate.Text = value;
                    break;

                case eZodiac.Gemini:
                    pictureBoxSoulmate.Image = Properties.Resources.Aries;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Aries, out value);
                    textBoxSoulmate.Text = value;
                    break;

                case eZodiac.Leo:
                    pictureBoxSoulmate.Image = Properties.Resources.Libra;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Libra, out value);
                    textBoxSoulmate.Text = value;
                    break;

                case eZodiac.Pisces:
                    pictureBoxSoulmate.Image = Properties.Resources.Cancer;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Cancer, out value);
                    textBoxSoulmate.Text = value;
                    break;

                case eZodiac.Sagittarius:
                    pictureBoxSoulmate.Image = Properties.Resources.Taurus;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Taurus, out value);
                    textBoxSoulmate.Text = value;
                    break;

                case eZodiac.Taurus:
                    pictureBoxSoulmate.Image = Properties.Resources.Sagittarius;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Sagittarius, out value);
                    textBoxSoulmate.Text = value;
                    break;

                case eZodiac.Virgo:
                    pictureBoxSoulmate.Image = Properties.Resources.Scorpio;
                    m_FacebookServicesFacade.YourLuckData().TryGetValue(eZodiac.Scorpio, out value);
                    textBoxSoulmate.Text = value;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listViewDating.Items.Clear();
        }
                  
        private void listBoxZodiacSignFriends_SelectedValueChanged(object sender, EventArgs e)
        {
            listBoxFriendHaveSign.Items.Clear();
            string zodiac = listBoxZodiacSignFriends.SelectedItem as string;
            IFriendsBy whichSign = null;
            switch (zodiac)
            {
                case "Aquarius":
                    whichSign = new FriendsByAquarius();
                    break;
                case "Aries":
                    whichSign = new FriendsByAries();
                    break;
                case "Cancer":
                    whichSign = new FriendsByCancer();
                    break;
                case "Capricorn":
                    whichSign = new FriendsByCapricorn();
                    break;
                case "Gemini":
                    whichSign = new FriendsByGemini();
                    break;
                case "Leo":
                    whichSign = new FriendsByLeo();
                    break;
                case "Libra":
                    whichSign = new FriendsByLibra();
                    break;
                case "Pisces":
                    whichSign = new FriendsByPisces();
                    break;
                case "Sagittarius":
                    whichSign = new FriendsBySagittarius();
                    break;
                case "Scorpio":
                    whichSign = new FriendsByScorpio();
                    break;
                case "Taurus":
                    whichSign = new FriendsByTaurus();
                    break;
                case "Virgo":
                    whichSign = new FriendsByVirgo();
                    break;
            }

            List<User> friendBy = m_FacebookServicesFacade.AllFriendByZodiacSign(whichSign);
            foreach(User friend in friendBy)
            {
                listBoxFriendHaveSign.Items.Add(friend.Name);
            }
        }
    }
}