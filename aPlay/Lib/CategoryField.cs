using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xcom.aPlay.Lib
{
    /// <summary>
    /// Represents a field in a library category.
    /// </summary>
    public class CategoryField
    {
        public enum DisplayTypes
        {
            Default, DataSize, Rating, BitRate, SampleRate, Hide
        }
        #region Defualt Fields
        static CategoryField _title, _artist, _album, _release, _length, _size, _rating, _path, _genre, _uid, _thumb;

        static CategoryField()
        {
            _title = new CategoryField("Title", typeof(string));
            _artist = new CategoryField("Artist", typeof(string));
            _album = new CategoryField("Album", typeof(string));
            _release = new CategoryField("Year", "Year", typeof(int));
            _size = new CategoryField("Size", "File Size", typeof(int), DisplayTypes.DataSize,0);
            _length = new CategoryField("Length", typeof(TimeSpan));
            _path = new CategoryField("Path", typeof(string));
            _genre = new CategoryField("Genre", typeof(string));
            _rating = new CategoryField("Rating", typeof(byte), DisplayTypes.Rating);
            _uid = new CategoryField("UID", typeof(ulong), DisplayTypes.Hide);
            _thumb = new CategoryField("Thumb", typeof(ulong), DisplayTypes.Hide);
 
            Composer =  new CategoryField("Composer", typeof(string));
            DateAdded = new CategoryField("DateAdded", "Date Added", typeof(DateTime));
            DateModified = new CategoryField("DateMod", "Date Modified", typeof(DateTime));
            BitRate = new CategoryField("BitRate", "Bit Rate", typeof(int), DisplayTypes.BitRate,0);
            SampleRate = new CategoryField("SampleRate", "Sample Rate", typeof(int), DisplayTypes.SampleRate,0);
            TrackNumber = new CategoryField("TrackNumber", "Track #", typeof(int));
            DiscNumber = new CategoryField("DiscNumber", "Disc #", typeof(int));
            BPM = new CategoryField("BPM", typeof(int));
            Comment = new CategoryField("Comment", "Comment", typeof(string));
            CopyRight = new CategoryField("CopyRight", "Copyright", typeof(string));
            Lyrics = new CategoryField("Lyrics", "Lyrics", typeof(string));
            Channels = new CategoryField("Channels", "Audio Channels", typeof(int));
            //Hash = new CategoryField("Hash", "Hash", typeof(ulong), DisplayTypes.Hide,0);
        }
        //public static CategoryField Hash { get; protected set; }
        public static CategoryField Channels { get; protected set; }
        public static CategoryField Lyrics { get; protected set; }
        public static CategoryField CopyRight { get; protected set; }
        public static CategoryField Comment { get; protected set; }
        public static CategoryField BPM { get; protected set; }
        public static CategoryField Composer{get; protected set;}
        public static CategoryField DateAdded { get; protected set; }
        public static CategoryField DateModified { get; protected set; }
        public static CategoryField BitRate { get; protected set; }
        public static CategoryField SampleRate { get; protected set; }
        public static CategoryField TrackNumber { get; protected set; }
        public static CategoryField DiscNumber { get; protected set; }

        public static CategoryField Title
        {
            get
            {
                return _title;
            }
        }

        public static CategoryField Artist
        {
            get
            {
                return _artist;
            }
        }

        public static CategoryField Album
        {
            get
            {
                return _album;
            }
        }

        public static CategoryField ReleaseDate
        {
            get
            {
               return _release;
            }
        }

        public static CategoryField Size
        {
            get
            {
                return _size;
            }
        }

        public static CategoryField Length
        {
            get
            {
                return _length;
            }
        }
        public static CategoryField Path
        {
            get
            {
                return _path;
            }
        }
        public static CategoryField Genre
        {
            get
            {
                return _genre;
            }
        }
        public static CategoryField Rating
        {
            get
            {
                return _rating;
            }
        }

        public static CategoryField UID
        {
            get
            {
                return _uid;
            }
        }
        public static CategoryField Thumb
        {
            get
            {
                return _thumb;
            }
        }
        #endregion

        string _name;
        string _displayName;
        Type _type;
        DisplayTypes _displayT;
        object _defualtV;

        /// <summary>
        /// Construct for CategoryField based on anthor CategoryField.
        /// </summary>
        /// <param name="b">The base CategoryField</param>
        /// <param name="displayName">The displayName</param>
        public CategoryField(CategoryField b, String displayName):this(b.Name,displayName,b.DataType,b.DisplayType,b.DefaultValue)
        {
           
        }

        private CategoryField(string name, Type type)
            : this(name, name, type)
        {
        }
        private CategoryField(string name, Type type, DisplayTypes displayType)
            : this(name, name, type, displayType, null)
        {
        }
        private CategoryField(string name, string displayName, Type type)
            : this(name, displayName, type, DisplayTypes.Default, null)
        {
        }
      
        private CategoryField(string name, string displayName, Type type, DisplayTypes displayType, object defaultV)
        {
            _name = name;
            _displayName = displayName;
            _type = type;
            _displayT = displayType;
            if (defaultV != null)
                _defualtV = defaultV;
            else
            {
                if (type == typeof(int))
                {
                    _defualtV = (int)0;
                }
                else if (type == typeof(ulong))
                {
                    _defualtV = (ulong)0;
                }
                else if (type == typeof(string))
                {
                    _defualtV = "";
                }
                else if (type == typeof(DateTime))
                {
                    _defualtV = DateTime.MinValue;
                }
                else if (type == typeof(TimeSpan))
                {
                    _defualtV = new TimeSpan(12, 0, 1);
                }
                else if (type == typeof(byte))
                {
                    _defualtV = (byte)0;
                }

            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }
        public Type DataType
        {
            get
            {
                return _type;
            }
        }
        public DisplayTypes DisplayType
        {
            get
            {
                return _displayT;
            }
        }
        public object DefaultValue
        {
            get
            {
                return _defualtV;
            }
        }
        public override int GetHashCode()
        {
            return _name.GetHashCode();// ^ _type.GetHashCode();

        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new NullReferenceException();
            return this == (obj as CategoryField);
        }
        public override string ToString()
        {
            return _displayName;
        }

        public static bool operator ==(CategoryField a, CategoryField b)
        {
            if ((object)a == null || (object)b == null)
                return (object)a == (object)b; ;
            return a._name == b._name;// && a._type == b._type;

        }

        public static bool operator !=(CategoryField a, CategoryField b)
        {
            return !(a == b);
        }

    }
}
