using System;
using System.Runtime.InteropServices;


namespace wpfMapChk
{
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    class Win32
    {
        [DllImport("user32.dll")]
        //[DllImport("user32", EntryPoint = "FindWindowA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        //[DllImport("user32", EntryPoint = "GetWindowRect", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetWindowRect(int hwnd, ref Rect lpRect);
        [DllImport("user32.dll")]
        //[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDC(int hwnd);
        [DllImport("user32.dll")]
        //[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ReleaseDC(int hWnd, int hDC);
        [DllImport("gdi32.dll")]
        //[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetPixel(int hdc, int x, int y);


        //不更動目前視窗位置
        const int SWP_NOMOVE = 0x2;
        //不更動目前視窗大小
        const int SWP_NOSIZE = 0x1;
        //設定為最上層
        const int HWND_TOPMOST = -1;
        //取消最上層設定
        const int HWND_NOTOPMOST = -2;
        const int FLAGS = SWP_NOMOVE | SWP_NOSIZE;
    }
}
