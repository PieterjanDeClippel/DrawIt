using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace DrawIt
{
	public static class Extensions
	{
		public static int Round(this float fl, bool omhoog)
		{
			if((fl % 1) == 0) return (int)fl;

			bool negatief = fl < 0;
			fl = Math.Abs(fl);

			fl = fl - fl % 1 + (omhoog ? 1 : 0);
			return negatief ? -(int)fl : (int)fl;
		}

		public static void DrawFillEllipse(this Graphics gr, Pen pen, Brush brush, float x, float y, float w, float h)
		{
			gr.FillEllipse(brush, x, y, w, h);
			gr.DrawEllipse(pen, x, y, w, h);
		}

        public static float MakeFloat(this string str)
        {
            char komma = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            return Convert.ToSingle(str.Replace(',', komma).Replace('.', komma));
        }

        public static IEnumerable<T> Combine<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            List<T> res = new List<T>();
            res.Clear();
            res.AddRange(first);
            res.AddRange(second);
            return res.ToArray();
        }
        public static IEnumerable<T> Combine<T>(this IEnumerable<T> first, T second)
        {
            List<T> res = new List<T>(first);
            res.Add(second);
            return res.ToArray();
        }

		// http://stackoverflow.com/questions/2556447/whats-an-efficient-way-to-tell-if-a-bitmap-is-entirely-black
		public static bool IsSolidColor(this Bitmap bmp, Color c)
		{
			// Lock the bitmap's bits.  
			Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

			// Get the address of the first line.
			IntPtr ptr = bmpData.Scan0;

			// Declare an array to hold the bytes of the bitmap.
			int bytes = bmpData.Stride * bmp.Height;
			byte[] rgbValues = new byte[bytes];

			// Copy the RGB values into the array.
			Marshal.Copy(ptr, rgbValues, 0, bytes);

			// Unlock the bits.
			bmp.UnlockBits(bmpData);

			// rgbvalues is in format BGRA - BGRA - ...
			byte[] bValues = rgbValues.Where((x, i) => i % 4 == 0).ToArray();
			byte[] gValues = rgbValues.Where((x, i) => i % 4 == 1).ToArray();
			byte[] rValues = rgbValues.Where((x, i) => i % 4 == 2).ToArray();
			byte[] aValues = rgbValues.Where((x, i) => i % 4 == 3).ToArray();

			if (!bValues.All(T => T == c.B)) return false;
			if (!gValues.All(T => T == c.G)) return false;
			if (!rValues.All(T => T == c.R)) return false;
			if (!aValues.All(T => T == c.A)) return false;
			return true;
		}

		public static bool[] ReadBits(this Stream stream, bool MSB_first)
		{
			List<bool> result = new List<bool>();

			int readByte;
			if (MSB_first)
				while ((readByte = stream.ReadByte()) >= 0)
					for (int i = 7; i >= 0; i--)
						result.Add(((readByte >> i) & 1) == 1);
			else
				while ((readByte = stream.ReadByte()) >= 0)
					for (int i = 0; i < 8; i++)
						result.Add(((readByte >> i) & 1) == 1);

			return result.ToArray();
		}

		public static string ToGBString(this float value)
		{
			return value.ToString(CultureInfo.GetCultureInfo("en-GB"));
		}
	}
}
