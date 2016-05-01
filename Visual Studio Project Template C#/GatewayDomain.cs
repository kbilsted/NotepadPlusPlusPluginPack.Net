using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Kbg.NppPluginNET
{
    public class Colour
    {
        public readonly int R, G, B;

        // TODO KBG figure out
        public Colour(int rgb)
        {
            R = rgb;
        }

        public Colour(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        // TODO KBG figure out
        public int Value
        {
            get { return R + (B << 8 ) + (G << 16); }
        }
    }

    public class Position
    {
        private readonly int Pos;

        public Position(int pos)
        {
            Pos = pos;
        }

        public int Value
        {
            get { return Pos; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterRange
    {
        public CharacterRange(int cpmin, int cpmax) { cpMin = cpmin; cpMax = cpmax; }
        public int cpMin;
        public int cpMax;
    }


    public class TextRange : IDisposable
    {
        Sci_TextRange _sciTextRange;
        IntPtr _ptrSciTextRange;
        bool _disposed = false;

        public TextRange(CharacterRange chrRange, int stringCapacity)
        {
            _sciTextRange.chrg = chrRange;
            _sciTextRange.lpstrText = Marshal.AllocHGlobal(stringCapacity);
        }
        public TextRange(int cpmin, int cpmax, int stringCapacity)
        {
            _sciTextRange.chrg.cpMin = cpmin;
            _sciTextRange.chrg.cpMax = cpmax;
            _sciTextRange.lpstrText = Marshal.AllocHGlobal(stringCapacity);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Sci_TextRange
        {
            public CharacterRange chrg;
            public IntPtr lpstrText;
        }

        public IntPtr NativePointer { get { _initNativeStruct(); return _ptrSciTextRange; } }

        public string lpstrText { get { _readNativeStruct(); return Marshal.PtrToStringAnsi(_sciTextRange.lpstrText); } }

        public CharacterRange chrg { get { _readNativeStruct(); return _sciTextRange.chrg; } set { _sciTextRange.chrg = value; _initNativeStruct(); } }

        void _initNativeStruct()
        {
            if (_ptrSciTextRange == IntPtr.Zero)
                _ptrSciTextRange = Marshal.AllocHGlobal(Marshal.SizeOf(_sciTextRange));
            Marshal.StructureToPtr(_sciTextRange, _ptrSciTextRange, false);
        }

        void _readNativeStruct()
        {
            if (_ptrSciTextRange != IntPtr.Zero)
                _sciTextRange = (Sci_TextRange)Marshal.PtrToStructure(_ptrSciTextRange, typeof(Sci_TextRange));
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_sciTextRange.lpstrText != IntPtr.Zero) Marshal.FreeHGlobal(_sciTextRange.lpstrText);
                if (_ptrSciTextRange != IntPtr.Zero) Marshal.FreeHGlobal(_ptrSciTextRange);
                _disposed = true;
            }
        }

        ~TextRange()
        {
            Dispose();
        }
    }

}
