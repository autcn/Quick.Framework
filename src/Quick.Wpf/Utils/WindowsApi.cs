using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace Quick
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemPoint
    {
        public int X;
        public int Y;
    }
    public static class WindowsApi
    {
        #region Win32 API定义
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, IntPtr lpParameters, IntPtr lpDirectory, UInt32 nShowCmd);

        [DllImport("User32.dll")]
        public static extern bool GetCursorPos(ref SystemPoint point);

        [DllImport("shell32.dll", ExactSpelling = true)]
        private static extern void ILFree(IntPtr pidlList);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern IntPtr ILCreateFromPathW(string pszPath);

        [DllImport("shell32.dll", ExactSpelling = true)]
        private static extern int SHOpenFolderAndSelectItems(IntPtr pidlList, uint cild, IntPtr children, uint dwFlags);

        #endregion

        public static void ShellOpenFile(string strFilePath)
        {
            ShellExecute(IntPtr.Zero, "open", strFilePath, IntPtr.Zero, IntPtr.Zero, 1);
        }

        public static void ShellEdit(string strFilePath)
        {
            ShellExecute(IntPtr.Zero, "edit", strFilePath, IntPtr.Zero, IntPtr.Zero, 1);
        }

        public static void OpenWithNotepad(string strFilePath)
        {
            Process.Start("notepad.exe", string.Format("\"{0}\"", strFilePath));
        }

        public static void SelectFileInExplorer(string filePath)
        {
            if (!File.Exists(filePath) && !Directory.Exists(filePath))
                return;

            if (Directory.Exists(filePath))
                Process.Start(@"explorer.exe", "/select,\"" + filePath + "\"");
            else
            {
                IntPtr pidlList = ILCreateFromPathW(filePath);
                if (pidlList != IntPtr.Zero)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0));
                    }
                    finally
                    {
                        ILFree(pidlList);
                    }
                }
            }
        }
    }
}
