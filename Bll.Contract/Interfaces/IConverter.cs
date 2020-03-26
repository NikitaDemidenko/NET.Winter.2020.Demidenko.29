using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Contract.Interfaces
{
    public interface IConverter<in TSource, out TResult>
    {
        public TResult Convert(TSource source);
    }
}
