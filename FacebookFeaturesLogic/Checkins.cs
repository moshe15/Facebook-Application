using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookFeaturesLogic
{
    internal class Checkins : IAggregate
    {
        private readonly List<Checkin> m_UserCheckins;

        public Checkins(FacebookBasicFeatures i_UserCheckins)
        {
            m_UserCheckins = i_UserCheckins.UserLoggedInCheckins;
        }

        public IIterator CreateIterator()
        {
            return new CheckinsIterator(this);
        }

        private class CheckinsIterator : IIterator
        {
            private Checkins m_Agregate;
            private int m_CurrentIdx = -1;
            private int m_Count = -1;

            public CheckinsIterator(Checkins i_Collection)
            {
                m_Agregate = i_Collection;
                m_Count = m_Agregate.m_UserCheckins.Count;
            }           

            public bool MoveNext()
            {
                if (m_Count != m_Agregate.m_UserCheckins.Count)
                {
                    throw new Exception("Collection can not be changed during iteration!");
                }

                if (m_CurrentIdx >= m_Count)
                {
                    throw new Exception("its the end of the collection");
                }

                return ++m_CurrentIdx < m_Agregate.m_UserCheckins.Count;
            }

            public object Current
            {
                get { return m_Agregate.m_UserCheckins[m_CurrentIdx]; }
            }

            public void Reset()
            {
                m_CurrentIdx = -1;
            }
        }
    }
}