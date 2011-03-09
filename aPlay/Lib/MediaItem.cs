// 
//  MediaItem.cs
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
using System.Collections;

namespace aPlay.Lib
{
	/// <summary>
	/// Represents a media item, like a song
	/// </summary>
    public class MediaItem:Dictionary<CategoryField,object>
    {
        Category _c;

        public MediaItem(Category c)
        {
            this._c = c;
            for (int i = 0; i < c.Fields.Length; i++)
            {
                this.Add(c.Fields[i], c.Fields[i].DefaultValue);
            }
        
        }

        public Category Category
        {
            get
            {
                return _c;
            }
        }

        public string Title
        {
            get
            {
               

                return base[CategoryField.Title]as String;
            }
        }
        public string Path
        {
            get
            {
                return base[CategoryField.Path] as String;
            }
        }

        public ulong UID
        {
            get
            {
                return (ulong)base[CategoryField.UID];
            }
        }
        public ulong ThumbPrint
        {
            get
            {
                return (ulong)base[CategoryField.Thumb];
            }
        }
     
        /// <summary>
        /// Gets or sets the item with the column index.
        /// </summary>
        /// <param name="i">column index</param>
        /// <returns>value</returns>
        public  object this[int i]
        {
            get
            {
                return this[_c.Fields[i]];
            }
            set
            {
                this[_c.Fields[i]] = value;
            }
        }
        /// <summary>
        /// Gets or sets the item value with column name
        /// </summary>
        /// <param name="name">column name</param>
        /// <returns>value</returns>
        public object this[string name]
        {
            get
            {
                for (int i = 0; i < _c.Fields.Length; i++)
                {
                    if (_c.Fields[i].Name == name)
                    {
                        return this[i];
                    }
                }
                return null;
            }
            set
            {
                for (int i = 0; i < _c.Fields.Length; i++)
                {
                    if (_c.Fields[i].Name == name)
                    {
                        this[i] = value;
                        return;
                    }
                }
                
            }
        }


    }
}
