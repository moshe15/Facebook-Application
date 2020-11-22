using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookFeaturesLogic
{
    public class FriendsByCapricorn : IFriendsBy
    {
        public bool FriendsBy(string i_zodiacSign)
        {
            bool ifFriendByZodiacSign = false;
            if (i_zodiacSign.Equals("Capricorn"))
            {
                ifFriendByZodiacSign = true;
            }

            return ifFriendByZodiacSign;
        }
    }
}
