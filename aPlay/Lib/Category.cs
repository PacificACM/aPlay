// 
//  Category.cs
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

namespace aPlay.Lib
{
   /// <summary>
   /// Represets a category of media item types in a library
   /// </summary>
    public class Category
    {

        static Category()
        {
            Music = new Category("Music", new CategoryField[] { 
                CategoryField.Title,
                CategoryField.Album,
                CategoryField.Artist,
                CategoryField.Genre,
                CategoryField.Rating,
                CategoryField.Length,
                CategoryField.Path,
                CategoryField.ReleaseDate,
                CategoryField.Size,
                CategoryField.DiscNumber,
                CategoryField.TrackNumber,
                CategoryField.Composer,
                CategoryField.BitRate,
                CategoryField.SampleRate,
                CategoryField.BPM,
                CategoryField.Channels,
             
                CategoryField.CopyRight,
              
                CategoryField.DateAdded,
                CategoryField.DateModified,

                CategoryField.UID,
                CategoryField.Thumb

            },1);
         
        }

        public static Category Music{get; protected set;}

        string _name;
        CategoryField[] _fields;
         byte _id;
        public Category()
        {
            _name = "";
            _fields = new CategoryField[0];
            _id = 0;
        }
        public Category(string name,CategoryField[] fields, byte id)
        {
            _name = name;
            _fields = fields;
            _id = id;
        }
        public string Name
        {
            get
            {
                return _name;
            }
    
        }
        public CategoryField[] Fields
        {
            get
            {
                return _fields;
            }
      
        }
        public byte ID
        {
            get
            {
                return _id;
            }
        }

        public static bool operator ==(Category a, Category b)
        {
            if ((object)a == null || (object)b == null)
                return ((object)a == (object)b);
            return a.Name == b.Name;
        }
        public static bool operator !=(Category a, Category b)
        {
            return !(a == b);
        }
        public override string ToString()
        {
            return this._name;
        }
        public override int GetHashCode()
        {
            return _name.GetHashCode() ^ _id.GetHashCode();
        }
    }

    
}
