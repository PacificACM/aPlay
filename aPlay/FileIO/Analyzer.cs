using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using aPlay.Lib;

namespace aPlay.FileIO
{
    /// <summary>
    /// Absract class that gets info for a files/stream or w/e
    /// Info includes, hash, thumb, media tags, ect.
    /// </summary>
    abstract class Analyzer
    {
        #region Abstract Methods
        public abstract bool IsValid { get; }
        public abstract string Name { get; }
        public abstract ulong Hash { get; }
        public abstract ulong FingerPrint { get; }
        public abstract int Length { get; }
        public abstract DateTime DateModified{ get; }
        public abstract Category Category { get; }
        public abstract object GetFieldValue(CategoryField field);
        #endregion

        #region Public Methods

        public ulong GetUID(ILibrary _lib)
        {
            byte[] b = BitConverter.GetBytes(this.Hash);
            b[0] = 0;
            b[1] = this.Category.ID;
            b[2] = _lib.ID;
            ulong v = BitConverter.ToUInt64(b, 0);
            if(_lib.GetItemByUID(v)!=null)
            {
                for (int i = 0; i < 255; i++)
                {
                    v++;
                    if (_lib.GetItemByUID(v) == null)
                        return v;
                }
                throw new Exception("Unable to get an UID!");
            }
            return v;
        }

        #endregion

        #region Static Methods
        /// <summary>
        /// Creat a Analyzer for a file.
        /// </summary>
        /// <param name="file">The FileInfo for the File</param>
        /// <returns>The Anaylzer</returns>
        public static Analyzer CreateFromFile(FileInfo file)
        {
            return new FileAnaylzer(file);
        }

        /// <summary>
        /// Computes a 64-bit hash for a stream.
        /// </summary>
        /// <param name="s">The stream.</param>
        /// <returns>A 64-bit hash.</returns>
        public static ulong GetHashByStream(Stream s)
        {
            aPlay.Util.Crc64 a = new aPlay.Util.Crc64();
            return BitConverter.ToUInt64(a.ComputeHash(s), 0);
        }
        protected static ulong GetPrintByStream(Stream s)
        {
            return 0;
        }
        /// <summary>
        /// Gets a dictionary indexed by category fields of the tags.
        /// </summary>
        /// <param name="s">The stream</param>
        /// <param name="name">The name of the file or null</param>
        /// <param name="mime">The mime type or null.</param>
        /// <returns></returns>
        public static Dictionary<CategoryField,object> GetTagByStream(Stream s,string name, string mime)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(new InnerTagStreamClass(name,s), mime, TagLib.ReadStyle.Average);

                Dictionary<CategoryField, object> _tags = new Dictionary<CategoryField, object>();

                //Maps TagLib.Tag to the Dictionary according to CategoryFields
                TagLib.Tag t = f.Tag;
                if (t.Title != null)
                    _tags.Add(CategoryField.Title, t.Title);
                if (t.Album != null)
                    _tags.Add(CategoryField.Album, t.Album);
                if (t.Artists != null&&t.Artists.Length>0)
                    _tags.Add(CategoryField.Artist, string.Join("/",t.Artists));
                if (t.BeatsPerMinute != 0)
                    _tags.Add(CategoryField.BPM, (int)t.BeatsPerMinute);
                if (t.Comment != null)
                    _tags.Add(CategoryField.Comment, t.Comment);
                if(t.Composers!=null&&t.Composers.Length>0)
                    _tags.Add(CategoryField.Composer,string.Join("/",t.Composers));
                if (t.Copyright != null)
                    _tags.Add(CategoryField.CopyRight, t.Copyright);
                if (t.Disc != 0)
                    _tags.Add(CategoryField.DiscNumber, (int)t.Disc);
                if (t.Genres != null && t.Genres.Length > 0)
                    _tags.Add(CategoryField.Genre, string.Join("/", t.Genres));
                if (t.Lyrics != null)
                    _tags.Add(CategoryField.Lyrics, t.Lyrics);
                if (t.Track!=0)
                    _tags.Add(CategoryField.TrackNumber, (int)t.Track);
                if (t.Year != 0)
                    _tags.Add(CategoryField.ReleaseDate, (int)t.Year);
                //Maps TagLib.Properties to the dictionary by CategoryFields
                TagLib.Properties p = f.Properties;
                if (p.AudioBitrate != 0)
                    _tags.Add(CategoryField.BitRate, p.AudioBitrate);
                if (p.AudioChannels != 0)
                    _tags.Add(CategoryField.Channels, p.AudioChannels);
                if (p.AudioSampleRate != 0)
                    _tags.Add(CategoryField.SampleRate, p.AudioSampleRate);
                if (p.Duration != TimeSpan.Zero)
                    _tags.Add(CategoryField.Length, p.Duration);



                return _tags;
            }
            catch(Exception e)
            {
                Util.Log.LogDebugMessage("Error getting tag info: " +e.Message);
                return null;
            }
        }

#endregion 

        class InnerTagStreamClass:TagLib.File.IFileAbstraction
        {
            Stream _s;
            String _name;
            #region IFileAbstraction Members

            public InnerTagStreamClass(String name, Stream s)
            {
                _s = s;
                _name = name;
            }

            public string Name
            {
                get { return _name; }
            }

            public Stream ReadStream
            {
                get { return _s; }
            }

            public Stream WriteStream
            {
                get { return _s; }
            }

            public void CloseStream(Stream stream)
            {
                //Do nothing, as we want to keep the stream open.
            }

            #endregion
        }

    }

    class FileAnaylzer : Analyzer
    {
        FileInfo _file;
        ulong _hash, _print;
        Category _cat;
        bool _isValid = true;
        Dictionary<CategoryField, object> _tags;
        public FileAnaylzer(FileInfo file)
        {
         
            if (file == null)
                throw new ArgumentNullException("file");
            _file = file;
            if (!file.Exists)
                throw new FileNotFoundException(file.FullName);

            FileType ftype = FileType.GetFileTypeByExt(file.Extension);
            if (ftype == null)
            {
                _isValid = false;
                return;
            }
            _cat = ftype.Category;
            if (_cat == null)
            {
                _isValid = false;
                return;
            }
            try
            {
                FileStream stream = file.OpenRead();
                _hash = GetHashByStream(stream);
                stream.Seek(0, SeekOrigin.Begin);
                _print = GetPrintByStream(stream );
                stream.Seek(0, SeekOrigin.Begin);
                _tags = GetTagByStream(stream, file.Name, ftype.Mime);
              
                _tags.Add(CategoryField.Thumb, _print);

                stream.Close();
            }
            catch(Exception ex)
            {
                Util.Log.LogDebugMessage("Error @ File Anaylzer: "+ex.Message);
                _isValid = false;
            }
          

        }
        public override ulong Hash
        {
            get { return _hash; }
        }

        public override ulong FingerPrint
        {
            get { return _print; }
        }

        public override int Length
        {
            get { return (int)_file.Length; }
        }

        public override DateTime DateModified
        {
            get { return _file.LastWriteTime; }
        }

        public override Category Category
        {
            get { return _cat; }
        }

        public override object GetFieldValue(CategoryField field)
        {
            if (!_tags.ContainsKey(field))
                return null;
            return _tags[field];
        }

        public override bool IsValid
        {
            get { return _isValid; }
        }

        public override string Name
        {
            get { return _file.Name; }
        }
    }
}
