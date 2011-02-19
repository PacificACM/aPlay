using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xcom.aPlay.Lib;
using System.IO;
using System.Collections.ObjectModel;

namespace Xcom.aPlay.FileIO
{
    class MediaFinder
    {
        public delegate void ScanProgress(float _progress, string status);
        ILibrary _lib;
        DirectoryInfo[] _musicDir;
        FileSystemWatcher[] _watchers;
        public MediaFinder(ILibrary lib, DirectoryInfo[] music)
        {
            _lib = lib;
            _musicDir = music;
            _watchers = new FileSystemWatcher[music.Length];
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

            for (int i = 0; i < _musicDir.Length; i++)
            {
                Stack<FileInfo> files = new Stack<FileInfo>();
                ReadOnlyCollection<FileType> musicTypes = FileType.MusicFileTypes ;
                try
                {
                    if (prog != null)
                        prog(i*prog_folderSLength,"Finding files in: "+_musicDir[i].Name+".");
                    for (int a = 0; a < musicTypes.Count; a++)
                    {
                        foreach (FileInfo f in _musicDir[i].GetFiles("*." + musicTypes[a].FileExt, SearchOption.AllDirectories))
                            files.Push(f);
                    }

                    float prog_fileSLength = prog_folderSLength / (float)files.Count;
                    for (int a = 0; a < files.Count; a++)
                    {
                        FileInfo current = files.Pop();
                        float prog_current = i * prog_folderSLength + a * prog_fileSLength;
                        if (prog != null)
                            prog(prog_current, "Analyzing file: "+current.Name);
#if DEBUG
                        else
                            System.Console.WriteLine(prog_current+" Analyzing file: "+current.Name);
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

                                //_lib.UpdateItem(item);
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
                }catch(DirectoryNotFoundException)
                {
                    if(prog!=null)
                        prog(-2, "Could not find directory: " + _musicDir[i].FullName + "!");
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

        void MediaFinder_Music_Created(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
