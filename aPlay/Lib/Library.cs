using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQL = System.Data.SQLite;
using System.IO;
namespace aPlay.Lib
{

    class Library:ILibrary
    {



        System.Data.SQLite.SQLiteCommand _selectFromCat,_selectCount,_selectFromFile,_selectFromID;
        SQL.SQLiteConnection _connection;
        Category[] _categories;
        /// <summary>
        /// Construct for Library class
        /// </summary>
        /// <param name="dataPath">The file path for the library's database file. If it dosn't exist it will be created.</param>
        public Library(string dataPath)
        {
            _categories = new Category[] { Category.Music };

            _connection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dataPath);
            _connection.Open();
            checkAndCreateDatabase();


            _selectFromCat = _connection.CreateCommand();
   
            _selectFromCat.CommandText = "SELECT * FROM @categoryV ORDER BY Title";


            _selectCount = _connection.CreateCommand();
            _selectCount.CommandText = "SELECT COUNT(*) FROM @categoryV ";

            _selectFromFile = _connection.CreateCommand();
            _selectFromFile.CommandText = "SELECT * FROM @categoryV WHERE " + CategoryField.Path.Name + " = @path";
            _selectFromFile.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@path"));

            _selectFromID = _connection.CreateCommand();
            _selectFromID.CommandText = "SELECT * FROM @categoryV WHERE " + CategoryField.UID.Name + " = @uid";
            _selectFromID.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@uid"));

        }
        /// <summary>
        /// Check and make sures all the needed table exist if not it creats them.
        /// </summary>
        void checkAndCreateDatabase()
        {
            SQL.SQLiteCommand command = _connection.CreateCommand();
            //Checks category tables
            command.CommandText = "SELECT name FROM sqlite_master WHERE name=@cname;";
            command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@cname"));
            for (int i = 0; i < _categories.Length; i++)
            {
               
                command.Parameters[0].Value = "C_"+ _categories[i].Name;
                //Check if table exists
                if (!command.ExecuteReader().HasRows)
                {
                    //Create table
                    SQL.SQLiteCommand createTableCommand = _connection.CreateCommand();
                    StringBuilder temp = new StringBuilder();
                    //Writing out sql command
                    temp.Append("CREATE TABLE C_");
                    temp.Append(_categories[i].Name);
                    temp.Append(" (");
                    CategoryField[] fields = _categories[i].Fields;
                    for (int a = 0; a < fields.Length; a++)
                    {
                        temp.Append(fields[a].Name);
                        temp.Append(' ');
                        if (fields[a].DataType == typeof(int)||fields[a].DataType==typeof(ulong)||fields[a].DataType==typeof(byte))
                        {
                            temp.Append("INTEGER");
                        }
                        else if (fields[a].DataType == typeof(string))
                        {
                            temp.Append("TEXT");
                        }
                        else if (fields[a].DataType == typeof(DateTime))
                        {
                            temp.Append("DATETIME");
                        }

                        if (fields[a] == CategoryField.UID)
                        {
                            temp.Append(" PRIMARY KEY");
                        }
                        else if (fields[a] == CategoryField.Path)
                        {
                            temp.Append(" UNIQUE");
                        }
                        if (a + 1 < fields.Length)
                            temp.Append(',');
                    }
                    temp.Append(");");
                    createTableCommand.CommandText = temp.ToString();
                    createTableCommand.ExecuteNonQuery();

                }
                

            }
        }


        public Category[] Categories
        {
            get
            {
                return _categories;
            }
        }


        /// <summary>
        /// Get library items by category.
        /// </summary>
        /// <param name="c">Category to return</param>
        /// <returns>A library reader to get the items in the category</returns>
        public ILibraryReader GetItemsByCat(Category c)
        {
            SQL.SQLiteCommand s = _selectFromCat.Clone()as SQL.SQLiteCommand;
             s.CommandText= s.CommandText.Replace("@categoryV","C_" + c.Name);
            SQL.SQLiteCommand countCommand = _selectCount.Clone()as SQL.SQLiteCommand;
            countCommand.CommandText = countCommand.CommandText.Replace("@categoryV", "C_" + c.Name);
            return new LibReader(s, countCommand, c);
        }
        public MediaItem GetItemByFilePath(string p)
        {
            return GetItemByFilePath(null, p);
        }
        public MediaItem GetItemByFilePath(Category c, string p)
        {
            //If not category specified, go through all categories
            if (c == null)
            {
                for (int i = 0; i < this.Categories.Length; i++)
                {
                    MediaItem m = GetItemByFilePath(this.Categories[i], p);
                    if (m != null)
                        return m;
                }
                return null;
            }
            SQL.SQLiteCommand s = _selectFromFile.Clone() as SQL.SQLiteCommand;
            s.CommandText = s.CommandText.Replace("@categoryV", "C_" + c.Name);
            s.Parameters["@path"].Value = p;
            SQL.SQLiteDataReader r = s.ExecuteReader();
            if (!r.HasRows)
                return null;
            return GetMediaItemByReader(r,c);

        }

        public void UpdateItem(MediaItem item)
        {
            throw new NotImplementedException();
        }

        MediaItem GetMediaItemByReader(SQL.SQLiteDataReader r,Category c)
        {
            r.Read();
            MediaItem m = new MediaItem(c);
            for (int i = 0; i < r.FieldCount; i++)
            {

                object v = r.GetValue(i);
                if (v is long)
                    v = (ulong)(long)v;
                m[r.GetName(i)] = v;
            }
            return m;
        }

        class LibReader : ILibraryReader
        {
            int _pos,_count;
            SQL.SQLiteCommand _command_select,_command_count;
            SQL.SQLiteDataReader _reader;
            Category _cat;
            MediaItem _current;
            public LibReader(SQL.SQLiteCommand select,SQL.SQLiteCommand count,Category c)
            {
                _current = null;
                try
                {
                    _pos = -1;
                    _command_select = select;
                    _command_count = count;

                    SQL.SQLiteDataReader r = _command_count.ExecuteReader();
                    r.Read();
                    _count = r.GetInt32(0);

                 

                    _command_select.CommandText = _command_select.CommandText.Replace("@start", "0");

                   

                    _reader = _command_select.ExecuteReader();
                    _cat = c;
                }
                catch(SQL.SQLiteException e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                
            }

            #region ILibraryReader Members

            public int Count
            {
                get { return _count; }
            }

            public void SeekTo(int index)
            {
                throw new NotImplementedException();
                _current = null;
                _reader.Dispose();
                _pos = index;
                _command_select.Parameters["@start"].Value = _pos;
                _reader = _command_select.ExecuteReader();
            }

            public MediaItem[] ToArray(int count)
            {

                ILibraryReader r = this.Clone() as ILibraryReader;
                List<MediaItem> items = new List<MediaItem>(count);
                while (r.MoveNext())
                {
                    items.Add(r.Current);
                }
                return items.ToArray();
            
            }

            #endregion

            #region IEnumerator<MediaItem> Members
            
            public MediaItem Current
            {
                get {
                    if (_current != null)
                        return _current;
                    MediaItem m = new MediaItem(_cat);
                    for (int i = 0; i < _reader.FieldCount; i++)
                    {
                        object v = _reader.GetValue(i);
                        if (v is long)
                            v = (ulong)(long)v;
                        m[_reader.GetName(i)] = v;
                    }
                    _current = m;
                    return m;
                
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                _current = null;
                _command_count.Dispose();
                _command_select.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

            public bool MoveNext()
            {
                _current = null;
                _pos++;

                return _reader.Read();
                
            }

            public void Reset()
            {
                _current = null;
                _reader.Dispose();
                _pos = -1; ;
                _reader = _command_select.ExecuteReader();
            }

            #endregion

            #region IEnumerable<MediaItem> Members

            IEnumerator<MediaItem> System.Collections.Generic.IEnumerable<MediaItem>.GetEnumerator()
            {
                return this;
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this;
            }

            #endregion



            #region ICloneable Members

            public object Clone()
            {
               
                return new LibReader(_command_select.Clone()as SQL.SQLiteCommand, _command_count.Clone() as SQL.SQLiteCommand, _cat);
            }

            #endregion



            #region ILibraryReader Members


            public Category Category
            {
                get { return _cat; }
            }

            #endregion
        }

        #region ILibrary Members


        public bool ReadOnly
        {
            get { return false; }
        }

        public void AddItem(MediaItem m)
        {
            if (Categories.Contains<Category>(m.Category))
            {
                StringBuilder ctext = new StringBuilder();
                ctext.Append("INSERT INTO C_");
                ctext.Append(m.Category.Name);
                ctext.Append(" (");
                for (int i = 0; i < m.Category.Fields.Length; i++)
                {
                    ctext.Append(m.Category.Fields[i].Name);
                    if (i + 1 < m.Category.Fields.Length)
                        ctext.Append(", ");
                }
                ctext.Append(") VALUES( ");
                for (int i = 0; i < m.Category.Fields.Length; i++)
                {
                    ctext.Append('@');
                    ctext.Append(m.Category.Fields[i].Name);
                    if (i + 1 < m.Category.Fields.Length)
                        ctext.Append(", ");
                }
                ctext.Append(");");
                SQL.SQLiteCommand insert = _connection.CreateCommand();
                insert.CommandText = ctext.ToString();
                for (int i = 0; i < m.Category.Fields.Length; i++)
                {
                    if(m.Category.Fields[i].DataType == typeof(ulong))
                        insert.Parameters.AddWithValue('@' + m.Category.Fields[i].Name, (long)(ulong)m[i]);
                    else
                        insert.Parameters.AddWithValue('@' + m.Category.Fields[i].Name, m[i]);

                    
     
                }
                insert.ExecuteNonQuery();
            }
            else
                throw new Exception("Non-compatiable item was added.");
        }

        #endregion

        #region ILibrary Members




        #endregion

        #region ILibrary Members

        //TODO set up ID
        public byte ID
        {
            get { return 0; }
        }

        public MediaItem GetItemByUID(ulong v)
        {
            SQL.SQLiteCommand s = _selectFromID.Clone() as SQL.SQLiteCommand;
            ulong catID = (v&0x000000000000FF00)>>8;
            Category cat = null;
            for(int i = 0; i < Categories.Length;i++)
            {
                if(Categories[i].ID == catID)
                {
                    cat = Categories[i];
                    break;
                }
            }
            if(cat == null)
                return null;
            s.CommandText = s.CommandText.Replace("@categoryV", "C_"+cat.Name);
            unchecked
            {
                s.Parameters["@uid"].Value =(long) v;
            }
            SQL.SQLiteDataReader r = s.ExecuteReader();
            if (!r.HasRows)
                return null;
            return GetMediaItemByReader(r, cat);
        }

        #endregion

        #region ILibrary Members


        public string Name
        {
            get { return "Your Library"; }
        }

        #endregion
    }
}
