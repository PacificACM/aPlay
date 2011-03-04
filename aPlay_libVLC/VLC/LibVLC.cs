using System;
using System.Runtime.InteropServices;

namespace aPlay.Platform.VLC
{

    // http://www.videolan.org/developers/vlc/doc/doxygen/html/group__libvlc.html
    //Structure seems to be obsolete 
    /*
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct libvlc_exception_t
    {
        public int b_raised;
        public int i_code;
        [MarshalAs(UnmanagedType.LPStr)]
        public string psz_message;
    }*/

    /// <summary>
    /// Callback delegate for event handling
    /// </summary>
    /// <param name="event_t">Ptr to the event type.</param>
    /// <param name="have_no_idea">User data???? maybe</param>
    public delegate void libvlc_callback_t(IntPtr event_t, IntPtr have_no_idea);
    /// <summary>
    /// Event tyes
    /// </summary>
    public enum libvlc_event_e : int
    {
        libvlc_MediaMetaChanged,
        libvlc_MediaSubItemAdded,
        libvlc_MediaDurationChanged,
        libvlc_MediaParsedChanged,
        libvlc_MediaFreed,
        libvlc_MediaStateChanged,
        libvlc_MediaPlayerMediaChanged,
        libvlc_MediaPlayerNothingSpecial,
        libvlc_MediaPlayerOpening,
        libvlc_MediaPlayerBuffering,
        libvlc_MediaPlayerPlaying,
        libvlc_MediaPlayerPaused,
        libvlc_MediaPlayerStopped,
        libvlc_MediaPlayerForward,
        libvlc_MediaPlayerBackward,
        libvlc_MediaPlayerEndReached,
        libvlc_MediaPlayerEncounteredError,
        libvlc_MediaPlayerTimeChanged,
        libvlc_MediaPlayerPositionChanged,
        libvlc_MediaPlayerSeekableChanged,
        libvlc_MediaPlayerPausableChanged,
        libvlc_MediaPlayerTitleChanged,
        libvlc_MediaPlayerSnapshotTaken,
        libvlc_MediaPlayerLengthChanged,
        libvlc_MediaListItemAdded,
        libvlc_MediaListWillAddItem,
        libvlc_MediaListItemDeleted,
        libvlc_MediaListWillDeleteItem,
        libvlc_MediaListViewItemAdded,
        libvlc_MediaListViewWillAddItem,
        libvlc_MediaListViewItemDeleted,
        libvlc_MediaListViewWillDeleteItem,
        libvlc_MediaListPlayerPlayed,
        libvlc_MediaListPlayerNextItemSet,
        libvlc_MediaListPlayerStopped,
        libvlc_MediaDiscovererStarted,
        libvlc_MediaDiscovererEnded,
        libvlc_VlmMediaAdded,
        libvlc_VlmMediaRemoved,
        libvlc_VlmMediaChanged,
        libvlc_VlmMediaInstanceStarted,
        libvlc_VlmMediaInstanceStopped,
        libvlc_VlmMediaInstanceStatusInit,
        libvlc_VlmMediaInstanceStatusOpening,
        libvlc_VlmMediaInstanceStatusPlaying,
        libvlc_VlmMediaInstanceStatusPause,
        libvlc_VlmMediaInstanceStatusEnd,
        libvlc_VlmMediaInstanceStatusError
    }

    /// <summary>
    /// LibVlc wrapper
    /// </summary>
    public static class LibVLC
    {
        #region core
        [DllImport("libvlc")]
        public static extern IntPtr libvlc_new(int argc, [MarshalAs(UnmanagedType.LPArray,
          ArraySubType = UnmanagedType.LPStr)] string[] argv);

        [DllImport("libvlc")]
        public static extern void libvlc_release(IntPtr instance);
        #endregion

        #region media
        [DllImport("libvlc")]
        public static extern IntPtr libvlc_media_new_path(IntPtr p_instance,
          [MarshalAs(UnmanagedType.LPStr)] string psz_mrl);

        [DllImport("libvlc")]
        public static extern void libvlc_media_release(IntPtr p_meta_desc);
        #endregion

        #region media player
        [DllImport("libvlc")]
        public static extern IntPtr libvlc_media_player_new_from_media(IntPtr media);

        [DllImport("libvlc")]
        public static extern void libvlc_media_player_release(IntPtr player);

        [DllImport("libvlc")]
        public static extern void libvlc_media_player_set_drawable(IntPtr player, IntPtr drawable);

        [DllImport("libvlc")]
        public static extern void libvlc_media_player_play(IntPtr player);

        [DllImport("libvlc")]
        public static extern void libvlc_media_player_pause(IntPtr player);

        [DllImport("libvlc")]
        public static extern void libvlc_media_player_stop(IntPtr player);
        #endregion

        /*
        #region exception
        [DllImport("libvlc")]
        public static extern void libvlc_exception_init(ref libvlc_exception_t p_exception);

        [DllImport("libvlc")]
        public static extern int libvlc_exception_raised(ref libvlc_exception_t p_exception);

        [DllImport("libvlc")]
        public static extern string libvlc_exception_get_message(ref libvlc_exception_t p_exception);
        #endregion

        */
        #region error handling
        [DllImport("libvlc")]
        public static extern string libvlc_errmsg();
        #endregion


        #region event handling



        [DllImport("libvlc")]
        public static extern int libvlc_event_attach(IntPtr p_event_manager, int i_event_type, libvlc_callback_t f_callback, IntPtr user_data);

        [DllImport("libvlc")]
        public static extern void libvlc_event_detach(IntPtr p_event_manager, int i_event_type, libvlc_callback_t f_callback, IntPtr p_user_data);
        #endregion
    }
    
}
