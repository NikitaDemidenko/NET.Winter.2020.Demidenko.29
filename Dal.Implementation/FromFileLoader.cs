using System;
using System.IO;
using Dal.Contract.Interafces;
using System.Collections.Generic;

namespace Dal.Implementation
{
    public class FromFileLoader : ILoader<IEnumerable<string>>
    {
        private readonly string filePath;

        public FromFileLoader(string filePath)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            this.filePath = filePath;
        }

        public IEnumerable<string> Load()
        {
            using var reader = new StreamReader(this.filePath);

            while (!reader.EndOfStream)
            {
                yield return reader.ReadLine();
            }
        }
    }
}
