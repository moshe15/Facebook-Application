using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookFeaturesLogic
{
    public class FriendsByLeo : IFriendsBy
    {
        public bool FriendsBy(string i_zodiacSign)
        {
            bool ifFriendByZodiacSign = false;
            if (i_zodiacSign.Equals("Leo"))
            {
                ifFriendByZodiacSign = true;
            }

            return ifFriendByZodiacSign;
        }
    }
}