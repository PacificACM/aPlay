// 
//  Player.cs
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
using aPlay.Platform;
using aPlay.Platform.VLC;

namespace aPlay.Media
{
    public class Player: IDisposable
    {
        IntPtr instance, player;
        string _file;
        public Player(string filePath)
        {
			
            _file = filePath;
           
           
        }
        ~Player()
        {
            Dispose();
        }
        public void Play()
        {
            //Create exception struct to catch when crap happens...
           //libvlc_exception_t ex = new libvlc_exception_t();
          //  LibVLC.libvlc_exception_init(ref ex);

       
            //args taken from vlc sample code
            string[] args = new string[] {
                "-I","dummy",
                @"--plugin-path=vlc_plugins",
                "--ignore-config",
                
                "--vout","dummy",
                "--no-media-library" ,              /* don't want that */
                "--services-discovery", "",         /* nor that */
             
                 "--no-stats",                       /* no stats */
    
               "--no-sub-autodetect"              /* don't want subtitles */

            };               
            instance = LibVLC.libvlc_new(args.Length,args);

            RaiseEx();
         
            IntPtr media = LibVLC.libvlc_media_new_path(instance, _file);
            RaiseEx();

            player = LibVLC.libvlc_media_player_new_from_media(media);
            RaiseEx();

            LibVLC.libvlc_media_release(media);
            RaiseEx();

            LibVLC.libvlc_media_player_play(player);
            RaiseEx();



        }
        //Raises a vlc exception.
        void RaiseEx()
        {
            string a = LibVLC.libvlc_errmsg();
            if (!string.IsNullOrEmpty(a))
                throw new Exception(a);

        }

        //TODO Implement
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
        


    }
}
