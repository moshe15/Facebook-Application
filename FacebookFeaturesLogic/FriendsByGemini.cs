using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookFeaturesLogic
{
    public class FriendsByGemini : IFriendsBy
    {
        public bool FriendsBy(string i_zodiacSign)
        {
            bool ifFriendByZodiacSign = false;
            if (i_zodiacSign.Equals("Gemini"))
            {
                ifFriendByZodiacSign = true;
            }

            return ifFriendByZodiacSign;
        }
    }
}