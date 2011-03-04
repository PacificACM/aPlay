// 
//  FileType.cs
//  
//  Author:
//       Cameron Lucas <c_lucas3@u.pacific.edu>
// 
//  Copyright (c) 2011 Cameron
// 
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using aPlay.Lib;
using System.Collections.ObjectModel;

namespace aPlay.FileIO
{
    /// <summary>
    /// Represents a file type.
    /// </summary>
    class FileType
    {

        static FileType()
        {
            MP3 = new FileType("mp3", "audio/mpeg", Category.Music);
            OGG = new FileType("ogg", "application/ogg", Category.Music);
            FLAC = new FileType("flac", "audio/x-flac", Category.Music);
            WAV = new FileType("wav", "audio/wav", Category.Music);
            AAC = new FileType("acc", "audio/aac", Category.Music);
            WMA = new FileType("wma", "audio/x-ms-wma", Category.Music);
            M4A = new FileType("m4a", "audio/mp4", Category.Music);
            AIFF = new FileType("aiff", "audio/aiff", Category.Music);
            MusicFileTypes = new ReadOnlyCollection<FileType>(new FileType[]{
                MP3,OGG,FLAC,WAV,AAC,WMA,M4A,AIFF});
            FileTypes = new ReadOnlyCollection<FileType>(new FileType[]{
                MP3,OGG,FLAC,WAV,AAC,WMA,M4A,AIFF});

        }
        public static ReadOnlyCollection<FileType> MusicFileTypes { get; private set; }
        public static ReadOnlyCollection<FileType> FileTypes {get; private set;}
       
        /// <summary>
        /// Get a file type by the file extension.
        /// </summary>
        /// <param name="ext">The file extension</param>
        /// <returns>The file type or null if unkown</returns>
        public static FileType GetFileTypeByExt(string ext)
        {
            if(ext.StartsWith("."))
                ext = ext.Substring(1);
            ext = ext.ToLower();
            for(int i = 0; i < FileTypes.Count;i++)
            {
                if(FileTypes[i].FileExt==ext)
                {
                    return FileTypes[i];
                }
            }
            return null;
        }


        public static FileType MP3 { get; private set; }
        public static FileType OGG { get; private set; }
        public static FileType FLAC { get; private set; }
        public static FileType WAV { get; private set; }
        public static FileType AAC { get; private set; }
        public static FileType WMA { get; private set; }
        //public static FileType WMV { get; private set; }
        public static FileType M4A { get; private set; }
        //public static FileType M4V { get; private set; }
        //public static FileType MP4 { get; private set; }
        public static FileType AIFF { get; private set; }

       // public static FileType AIV { get; private set; }
       // public static FileType MPG { get; private set; }
       // public static FileType MPEG { get; private set; }






        string _fileExt;
        string _mime;
        Category _cat;
        public FileType(string ext,string mime, Category cat)
        {
            _fileExt = ext;
            _cat = cat;
        }
        public string FileExt
        {
            get
            {
                return _fileExt;
            }
        }
        public Category Category
        {
            get
            {
                return _cat;
            }
        }
        public string Mime
        {
            get
            {
                return _mime;
            }
        }
    }
}
