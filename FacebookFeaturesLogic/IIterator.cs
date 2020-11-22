using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookFeaturesLogic
{
    public interface IIterator
    {
        bool MoveNext();

        object Current { get; }

        void Reset();
    }
}
