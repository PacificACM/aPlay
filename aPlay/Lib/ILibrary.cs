using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xcom.aPlay.Lib
{
    public interface ILibrary
    {
        Category[] Categories { get; }
        bool ReadOnly { get; }
        void AddItem(MediaItem m);
        ILibraryReader GetItemsByCat(Category c);
        MediaItem GetItemByFilePath(string p);
        MediaItem GetItemByFilePath(Category c, string p);

        void UpdateItem(MediaItem item);

        byte ID { get; }

        MediaItem GetItemByUID(ulong v);

        string Name { get; }
    }
}
