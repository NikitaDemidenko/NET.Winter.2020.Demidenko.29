using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Contract.Interfaces
{
    public interface IValidator<TSource>
    {
        public bool IsValid(TSource source);
    }
}
