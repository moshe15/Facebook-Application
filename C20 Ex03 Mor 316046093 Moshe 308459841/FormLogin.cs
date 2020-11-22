using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace C20_Ex03_Mor_316046093_Moshe_308459841
{
    public partial class FormLogin : Form
    {
        private const string k_AppId = "992406207897451";

        public User m_LoggedInUser { get; private set; } = new User();

        private AppSettings m_AppSettings;

        public FormLogin()
        {
            InitializeComponent();
            m_AppSettings = new AppSettings();
        }

        private void formLogin_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent;

            checkBoxRememberMe.BackColor = Color.Transparent;
        }

        private void loginAndInit()
        {
            LoginResult result = FacebookService.Login(k_AppId,

               "public_profile",
               "publish_to_groups",
               "user_birthday",
               "user_age_range",
               "user_gender",
               "user_link",
               "user_tagged_places",
               "publish_to_groups",
               "groups_access_member_info",
               "user_friends",
               "user_events",
               "user_likes",
               "user_location",
               "user_photos",
               "user_hometown"
              );

            if (!string.IsNullOrEmpty(result.AccessToken))
            {
                m_LoggedInUser = result.LoggedInUser;
                if (checkBoxRememberMe.Checked)
                {
                    m_AppSettings.RememberUser = true;
                    m_AppSettings.LastAccessToken = result.AccessToken;
                    m_AppSettings.SaveToFile();
                }

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(result.ErrorMessage);
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            loginAndInit();
        }
    }
}