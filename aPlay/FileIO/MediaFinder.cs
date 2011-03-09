// 
//  MediaFinder.cs
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
using System.IO;
using System.Collections.ObjectModel;

namespace aPlay.FileIO
{
	/// <summary>
	/// Class used to for the searching and indexing of media files
	/// </summary>
    public class MediaFinder
    {

		/// <summary>
		/// Delegate used to call back the current status of media fetching
		/// </summary>
        public delegate void ScanProgress(float _progress, string status);
        ILibrary _lib;
        DirectoryInfo[] _musicDir;
        FileSystemWatcher[] _watchers;
        public MediaFinder(ILibrary lib, DirectoryInfo[] music)
        {
            _lib = lib;
            _musicDir = music;
            _watchers = new FileSystemWatcher[music.Length];
            //Create file watchers that monitor for new files.
            for (int i = 0; i < _watchers.Length; i++)
            {
                _watchers[i] = new FileSystemWatcher(music[i].FullName);
                _watchers[i].Created += new FileSystemEventHandler(MediaFinder_Music_Created);
                
            }
        }
        /// <summary>
        /// Scans the folders and adds/updates media.
        /// </summary>
        /// <param name="background">Set if it should be backgrounded or not</param>
        /// <param name="prog">Delegate to recieve scan progress events</param>
        public void ScanForNewFiles(bool background,ScanProgress prog)
        {
            
            //Calculate the size of the sub_progress for each folder.
            float prog_folderSLength = 100f/_musicDir.Length;
            //Scan all the music library directories
            for (int i = 0; i < _musicDir.Length; i++)
            {
                List<FileInfo> files = new List<FileInfo>();
                //Get all supported music format extensions
                ReadOnlyCollection<FileType> musicTypes = FileType.MusicFileTypes ;
                try
                {
#if DEBUG
                    if (prog != null)
                        prog(i * prog_folderSLength, "Finding files in: " + _musicDir[i].Name + ".");
#endif
                    //Get all the files with the supported extensions in the current folder
                    for (int a = 0; a < musicTypes.Count; a++)
                    {

                        files.AddRange(_musicDir[i].GetFiles("*." + musicTypes[a].FileExt, SearchOption.AllDirectories));

                    }
                    //Make sures the progress shows up in a logical order
                    files.Sort(delegate(FileInfo x, FileInfo y) { return x.FullName.CompareTo(y.FullName); });
                    float prog_fileSLength = prog_folderSLength / (float)files.Count;
                    for (int c = 0; c < files.Count; c++)
                    {
                        c++;
                        FileInfo current = files[c];
                        float prog_current = i * prog_folderSLength + c * prog_fileSLength;
                        if (prog != null)
                            prog(prog_current, "Analyzing file: " + current.Name);
#if DEBUG
                        else
                            System.Console.WriteLine(prog_current + " Analyzing file: " + current.Name);
#endif
                        try
                        {
                            //Get the database object for the file path.
                            MediaItem item = _lib.GetItemByFilePath(null, current.FullName);
                            //If file isn't already in database add it.
                            if (item == null)
                            {

                                item = AnayalizeFile(current, null);
                                _lib.AddItem(item);
                            }
                            //Check if the file has been modified since last time.
                            else if (((DateTime)item[CategoryField.DateModified]) != current.LastWriteTime)
                            {

                                item = AnayalizeFile(current, item);

                                _lib.UpdateItem(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (prog != null)
                                prog(prog_current, "Error: " + ex.Message);
                            Util.Log.LogMsg(ex.Message);
                        }
                    }
                    if (prog != null)
                        prog(100, "Finished.");
                }
                catch (DirectoryNotFoundException)
                {
                    if (prog != null)
                        prog(-2, "Could not find directory: " + _musicDir[i].FullName + "!");
                }
                catch (Exception ex)
                {
                    if (prog != null)
                        prog(-2, "Error in searching for files: "+ex.Message);
                }
            }
        }
        /// <summary>
        /// Anaylize a file and creates or updates the item
        /// </summary>
        /// <param name="file">The file</param>
        /// <param name="item">A mediaitem to update, set to null to create new.</param>
        /// <returns>the new/update mediaitem</returns>
        MediaItem AnayalizeFile(FileInfo file,MediaItem item)
        {
            
            Analyzer a = Analyzer.CreateFromFile(file);

            if (!a.IsValid)
            {
                throw new Exception("Unable to anayalize file: "+file.Name);
            }
            if (item == null)
                item = new MediaItem(a.Category);
            
            for(int i = 0; i < item.Category.Fields.Length;i++)
            {
                CategoryField field = item.Category.Fields[i];
                object v = a.GetFieldValue(field);
                if (v != null)
                    item[field] = v;
            }
            item[CategoryField.UID] = a.GetUID(_lib);
            item[CategoryField.Path] = file.FullName;
            return item;
        }
        //TODO Implement file created handler for media finder
        void MediaFinder_Music_Created(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
