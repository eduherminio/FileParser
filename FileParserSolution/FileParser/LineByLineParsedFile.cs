using System;
using System.Collections.Generic;
using System.Linq;

namespace FileParser
{
    public class LineByLineParsedFile : IDisposable
    {
        public Queue<Queue<string>> FileQueue { get; set; } = new Queue<Queue<string>>();

        public LineByLineParsedFile(ICollection<string> parsedFileAsAWhole, string lineSeparator)
        {
            int index = 0, nextIndex = 0;
            int n_items = parsedFileAsAWhole.Count;

            while (-1 != (
                nextIndex = parsedFileAsAWhole.ToList()
                    .GetRange(index, n_items - index)
                    .FindIndex(str => str.Equals(lineSeparator)))
                )
            {
                Queue<string> lineQueue = new Queue<string>(parsedFileAsAWhole.ToList()
                    .GetRange(index, n_items - index)
                    .GetRange(0, nextIndex));

                FileQueue.Enqueue(lineQueue);
                index += nextIndex + 1;
            }
        }

        #region IDisposable Implementation
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
