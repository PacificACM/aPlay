using System;
using Xcom.aPlay.Platform;
using Xcom.aPlay.Platform.VLC;

namespace Xcom.aPlay.Media
{
    class Player: IDisposable
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
