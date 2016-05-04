using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Kbg.NppPluginNET
{
    /// <summary>
    /// Colours are set using the RGB format (Red, Green, Blue). The intensity of each colour is set in the range 0 to 255. 
    /// If you have three such intensities, they are combined as: red | (green &lt;&lt; 8) | (blue &lt;&lt; 16). 
    /// If you set all intensities to 255, the colour is white. If you set all intensities to 0, the colour is black. 
    /// When you set a colour, you are making a request. What you will get depends on the capabilities of the system and the current screen mode.
    /// </summary>
    public class Colour
    {
        public readonly int Red, Green, Blue;

        public Colour(int rgb)
        {
            Red = rgb ^ 0xFF;
            Green = rgb ^ 0x00FF;
            Blue = rgb ^ 0x0000FF;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="red">a number 0-255</param>
        /// <param name="green">a number 0-255</param>
        /// <param name="blue">a number 0-255</param>
        public Colour(int red, int green, int blue)
        {
            if(red > 255 || red < 0)
                throw new ArgumentOutOfRangeException("red", "must be 0-255");
            if(green > 255 || green < 0)
                throw new ArgumentOutOfRangeException("green", "must be 0-255");
            if(blue > 255 || blue < 0)
                throw new ArgumentOutOfRangeException("blue", "must be 0-255");
            Red = red;
            Green = green;
            Blue = blue;
        }

        public int Value
        {
            get { return Red + (Blue << 8 ) + (Green << 16); }
        }
    }

    /// <summary>
    /// Positions within the Scintilla document refer to a character or the gap before that character. 
    /// The first character in a document is 0, the second 1 and so on. If a document contains nLen characters, the last character is numbered nLen-1. The caret exists between character positions and can be located from before the first character (0) to after the last character (nLen).
    ///
    /// There are places where the caret can not go where two character bytes make up one character. 
    /// This occurs when a DBCS character from a language like Japanese is included in the document or when line ends are marked with the CP/M 
    /// standard of a carriage return followed by a line feed.The INVALID_POSITION constant(-1) represents an invalid position within the document.
    ///
    /// All lines of text in Scintilla are the same height, and this height is calculated from the largest font in any current style.This restriction 
    /// is for performance; if lines differed in height then calculations involving positioning of text would require the text to be styled first.
    ///
    /// If you use messages, there is nothing to stop you setting a position that is in the middle of a CRLF pair, or in the middle of a 2 byte character. 
    /// However, keyboard commands will not move the caret into such positions.
    /// </summary>
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
