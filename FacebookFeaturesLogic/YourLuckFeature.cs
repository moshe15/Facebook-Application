using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookFeaturesLogic
{
    internal class YourLuckFeature
    {
        public List<string> SameZodiacSign { get; private set; }

        public Dictionary<eZodiac, string> FriendsZodiacSigns { get; private set; }

        public List<string> ProfilePicture { get; private set; }

        public Dictionary<eZodiac, string> m_YourLuckData;

        private readonly string pisces = "Pisces is considered to have the greatest compassion and personal ability of all the zodiac signs." + " They are creative, connected to mysticism and have a strong imagination.";
        private readonly string capricorn = "Capricorns are ruled by Saturn, and are known for their perseverance and determination. They strive for greatness and are willing to go all the way, step by step, and keep going until they conquer their destination.";
        private readonly string aquarius = "The buckets are known as the great idealists, they believe in equality and justice and are willing to fight for them. They are liberal and have endless mental openness, they need freedom of expression.";
        private readonly string aries = "The Patches are ruled by a Mars star, fast, warm, impulsive people and love to be the center of attention.";
        private readonly string taurus = "Taurus is ruled by the star Venus. They are known for being stable, reliable and trustworthy, they are sensual and quiet and love beauty and comfort.";
        private readonly string gemini = "Gemini love to gather knowledge and share it, they are known for their good verbal ability, curious, multidisciplinary and lovers of diversity.";
        private readonly string cancer = "Crabs are known to be relatively introverted, emotional and very familial people. They are nostalgic and lovers of tradition and history, and they need great emotional security.";
        private readonly string leo = "Lions are warm and extroverted people, love to lead and are good at it. They are charismatic and need a stage to perform on and a band to lead it. They are familial, opinionated.";
        private readonly string virgo = "Virgos are ruled by Mercury and are known for their sense of justice and their need for wholeness. Therefore they may be critical and get caught up in small details. They are efficient, organized, know how to work hard and have a therapeutic tendency.";
        private readonly string libra = "Libra people are ruled by the star Venus and love aesthetics, harmony and beauty. They do not get along well with conflicts and prefer to maintain balance and coziness.";
        private readonly string scorpio = "The scorpions are ruled by the planet Pluto. " + "They are known for their magnetism, their ability for secrecy and deep and thorough mental processes." + " They are loyal, total and uncompromising.";
        private readonly string sagittarius = "The rainbows are ruled by the star Jupiter. They are fast, need thrills, lovers of knowledge and philosophy and avid seekers of truth. They are optimistic and have a natural sense of humor and joy, needing lightness, development and enjoyment.";

        public Dictionary<eZodiac, string> m_CelebsData;

        private readonly string yitzhakRabin = "The Name: Yitzhak Rabin." + Environment.NewLine + "The Age: 73." + Environment.NewLine + "profession: Israel's prime minister.";
        private readonly string elvisPresley = "The Name: Elvis Presley." + Environment.NewLine + "The Age: 42." + Environment.NewLine + "profession: Singer.";
        private readonly string oprahWinfrey = "The Name: Oprah Winfrey." + Environment.NewLine + " The Age: 66." + Environment.NewLine + "profession: American TV presenter.";
        private readonly string sarahJessicaParker = "The Name: Sarah Jessica Parker." + Environment.NewLine + "The Age: 55." + Environment.NewLine + "profession: American actress.";
        private readonly string davidBeckham = "The Name: David Beckham." + Environment.NewLine + "The Age: 45." + Environment.NewLine + "profession: English footballer.";
        private readonly string marilynMonroe = "The Name: Marilyn Monroe." + Environment.NewLine + "The Age: 36." + Environment.NewLine + "profession: American actress, model, and singer.";
        private readonly string ladyDiana = "The Name:  Lady Diana." + Environment.NewLine + "The Age: 36." + Environment.NewLine + "profession: Princess of Wales.";
        private readonly string madonna = "The Name: Madonna." + Environment.NewLine + "The Age: 61." + Environment.NewLine + "profession: singer\n";
        private readonly string seanConnery = "The Name: Sean Connery." + Environment.NewLine + "The Age: 89." + Environment.NewLine + "profession: Scottish retired actor and producer ";
        private readonly string gandhi = "The Name: Gandhi." + Environment.NewLine + "The Age: 78." + Environment.NewLine + "profession: Indian lawyer, anti-colonial nationalist, and political ethicist";
        private readonly string billGates = "The Name: Bill Gates." + Environment.NewLine + "The Age: 64." + Environment.NewLine + "profession: American business magnate, software developer, investor, and philanthropist. ";
        private readonly string bruceLee = "The Name: Bruce Lee." + Environment.NewLine + " The Age: 32." + Environment.NewLine + "profession:  Hong Kong American actor, director, martial artist, martial arts instructor .";

        public YourLuckFeature()
        {
            m_YourLuckData = new Dictionary<eZodiac, string>();
            m_CelebsData = new Dictionary<eZodiac, string>();
            SameZodiacSign = new List<string>();
            FriendsZodiacSigns = new Dictionary<eZodiac, string>();
            ProfilePicture = new List<string>();
            m_YourLuckData = DataInfo();
            m_CelebsData = DataCelebsInfo();
        }

        public static eZodiac GetZodiac(DateTime i_DateTime)
        {
            eZodiac zodiacSign = eZodiac.Null;
            eMonth month = (eMonth)i_DateTime.Month;
            int day = i_DateTime.Day;

            switch (month)
            {
                case eMonth.January:
                    zodiacSign = (day < 20) ? eZodiac.Capricorn : eZodiac.Aquarius;
                    break;

                case eMonth.February:
                    zodiacSign = (day < 19) ? eZodiac.Aquarius : eZodiac.Pisces;
                    break;

                case eMonth.March:
                    zodiacSign = (day < 21) ? eZodiac.Pisces : eZodiac.Aries;
                    break;

                case eMonth.April:
                    zodiacSign = (day < 20) ? eZodiac.Aries : eZodiac.Taurus;
                    break;

                case eMonth.May:
                    zodiacSign = (day < 21) ? eZodiac.Taurus : eZodiac.Gemini;
                    break;

                case eMonth.June:
                    zodiacSign = (day < 21) ? eZodiac.Gemini : eZodiac.Cancer;
                    break;

                case eMonth.July:
                    zodiacSign = (day < 23) ? eZodiac.Cancer : eZodiac.Leo;
                    break;

                case eMonth.August:
                    zodiacSign = (day < 23) ? eZodiac.Leo : eZodiac.Virgo;
                    break;

                case eMonth.September:
                    zodiacSign = (day < 23) ? eZodiac.Virgo : eZodiac.Libra;
                    break;

                case eMonth.October:
                    zodiacSign = (day < 23) ? eZodiac.Libra : eZodiac.Scorpio;
                    break;

                case eMonth.November:
                    zodiacSign = (day < 22) ? eZodiac.Scorpio : eZodiac.Sagittarius;
                    break;

                case eMonth.December:
                    zodiacSign = (day < 22) ? eZodiac.Sagittarius : eZodiac.Capricorn;
                    break;
            }

            return zodiacSign;
        }

        public List<User> ZodiacFriends(IFriendsBy i_FriendsBy, List<User> i_allFriends)
        {           
            List<User> friendsByZoaidc = new List<User>();
            foreach (User friend in i_allFriends)
            {
                eZodiac friendsZodiacSigns = GetZodiac(DateTime.ParseExact(friend.Birthday, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                if (i_FriendsBy.FriendsBy(friendsZodiacSigns.ToString()))
                {
                    friendsByZoaidc.Add(friend);
                }
            }

            return friendsByZoaidc;
        }

        public Dictionary<eZodiac, string> DataInfo()
        {
            m_YourLuckData.Add(eZodiac.Capricorn, capricorn);
            m_YourLuckData.Add(eZodiac.Aquarius, aquarius);
            m_YourLuckData.Add(eZodiac.Pisces, pisces);
            m_YourLuckData.Add(eZodiac.Aries, aries);
            m_YourLuckData.Add(eZodiac.Taurus, taurus);
            m_YourLuckData.Add(eZodiac.Gemini, gemini);
            m_YourLuckData.Add(eZodiac.Cancer, cancer);
            m_YourLuckData.Add(eZodiac.Leo, leo);
            m_YourLuckData.Add(eZodiac.Virgo, virgo);
            m_YourLuckData.Add(eZodiac.Libra, libra);
            m_YourLuckData.Add(eZodiac.Scorpio, scorpio);
            m_YourLuckData.Add(eZodiac.Sagittarius, sagittarius);
            return m_YourLuckData;
        }

        public Dictionary<eZodiac, string> DataCelebsInfo()
        {
            m_CelebsData.Add(eZodiac.Capricorn, elvisPresley);
            m_CelebsData.Add(eZodiac.Aquarius, oprahWinfrey);
            m_CelebsData.Add(eZodiac.Pisces, yitzhakRabin);
            m_CelebsData.Add(eZodiac.Aries, sarahJessicaParker);
            m_CelebsData.Add(eZodiac.Taurus, davidBeckham);
            m_CelebsData.Add(eZodiac.Gemini, marilynMonroe);
            m_CelebsData.Add(eZodiac.Cancer, ladyDiana);
            m_CelebsData.Add(eZodiac.Leo, madonna);
            m_CelebsData.Add(eZodiac.Virgo, seanConnery);
            m_CelebsData.Add(eZodiac.Libra, gandhi);
            m_CelebsData.Add(eZodiac.Scorpio, billGates);
            m_CelebsData.Add(eZodiac.Sagittarius, bruceLee);
            return m_CelebsData;
        }

        public void AddFriendsProfilePictures(string i_FriendPicture)
        {
            ProfilePicture.Add(i_FriendPicture);
        }
    }
}