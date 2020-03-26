using System;
using System.Collections.Generic;
using System.Text;
using Bll.Contract.Interfaces;
using Dal.Contract.Interafces;

namespace Bll.Implementation.ServiceImplementation
{
    public class TradeService<TSource, TResult> : IService
    {
        private readonly ILoader<TSource> loader;
        private readonly ISaver<TResult> saver;
        private readonly IConverter<TSource, TResult> converter;

        public TradeService(ILoader<TSource> loader, ISaver<TResult> saver, IConverter<TSource, TResult> converter)
        {
            this.loader = loader ?? throw new ArgumentNullException(nameof(loader));
            this.saver = saver ?? throw new ArgumentNullException(nameof(saver));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public void Run()
        {
            var input = this.loader.Load();
            var result = this.converter.Convert(input);
            this.saver.Save(result);
        }
    }
}
