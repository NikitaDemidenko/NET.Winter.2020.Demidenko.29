using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Contract.Interafces
{
    public interface ILoader<out TResult>
    {
        public TResult Load();
    }
}
