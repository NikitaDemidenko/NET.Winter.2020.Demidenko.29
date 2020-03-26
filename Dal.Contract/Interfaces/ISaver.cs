using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Contract.Interafces
{
    public interface ISaver<in TSource>
    {
        public void Save(TSource source);
    }
}
