using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aPlay.Lib
{
    public interface ILibraryReader: IEnumerator<MediaItem>, IEnumerable<MediaItem>,ICloneable
    {
        int Count { get; }
        void SeekTo(int index);
        MediaItem[] ToArray(int count);
        Category Category { get; }
    }
}
