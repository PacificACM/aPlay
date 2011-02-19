using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Xcom.aPlay.Platform.Windows
{
    internal static class DWM
    {



        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea(
            IntPtr hWnd, MARGINS pMargins);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

     

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmGetColorizationColor(
            out int pcrColorization,
            [MarshalAs(UnmanagedType.Bool)]out bool pfOpaqueBlend);


        [StructLayout(LayoutKind.Sequential)]
        public class MARGINS
        {
            public int cxLeftWidth, cxRightWidth,
                       cyTopHeight, cyBottomHeight;

            public MARGINS(int left, int top, int right, int bottom)
            {
                cxLeftWidth = left; cyTopHeight = top;
                cxRightWidth = right; cyBottomHeight = bottom;
            }
        }


    }
}
