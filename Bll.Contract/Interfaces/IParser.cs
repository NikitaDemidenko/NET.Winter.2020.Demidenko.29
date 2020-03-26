using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Contract.Interfaces
{
    public interface IParser<out TResult>
    {
        public TResult Parse(string source);
    }
}
