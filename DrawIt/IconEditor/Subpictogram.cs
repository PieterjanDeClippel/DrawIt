using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DrawIt
{
	public class Subpictogram
    {
        byte[] data_bytes;
		public Subpictogram(byte[] b, int index)
		{
			ReadDirectory(b, index);
			ReadContent(b, index);
		}

		static int RoundUp(int input, int veelvoud)
		{
			int result = 0;
			while (result < input)
				result += veelvoud;
			return result;
		}

		//PngBitmapEncoder enc = new PngBitmapEncoder();

		static int conv_int(byte[] b, int start, int count)
		{
			int res = 0;
			int factor = 1;
			for (int i = start; i < start + count; i++)
			{
				res += b[i] * factor;
				factor *= 256;
			}
			return res;
		}
		private static int conv_int(bool[] b, int start, int count)
		{
			int res = 0;
			int factor = 128;
			for (int i = start; i < start + count; i++)
			{
				if(b[i]) res += factor;
				factor /= 2;
			}
			return res;
		}
		byte[] conv_bytes(int i, int count)
        {
            byte[] res = new byte[count];
            for(int t = 0; t< count;t++)
            {
                res[t] = Convert.ToByte(i % 256);
                i >>= 8;
            }
            return res;
        }
		
        #region Width
        private int width = 0;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        #endregion
        #region Height
        private int height = 0;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        #endregion
        #region ColorCount
        private int colorCount = 0;
        public int ColorCount
        {
            get { return colorCount; }
            set { colorCount = value; }
        }
        #endregion
        #region ColorPlanes
        private int colorPlanes;
        public int ColorPlanes
        {
            get { return colorPlanes; }
            set { colorPlanes = value; }
        }
        #endregion
        #region BitCount
        private int bitCount;
        public int BitCount
        {
            get { return bitCount; }
            set { bitCount = value; }
        }
        #endregion
        #region DataSize
        private int dataSize;
        public int DataSize
        {
            get { return dataSize; }
            set { dataSize = value; }
        }
        #endregion
        #region DataOffset
        private int dataOffset;
        public int DataOffset
        {
            get { return dataOffset; }
            set { dataOffset = value; }
        }
        #endregion
        #region Compression
        private int compression;
        public int Compression
        {
            get { return compression; }
        }
		#endregion
		#region RasterData
		private Bitmap bmp_local;
		private JaggedList<Color> rasterData = new JaggedList<Color>();
		public JaggedList<Color> RasterData
		{
			get { return rasterData; }
		}
		#endregion

		public void WriteDirectory(MemoryStream ms)
        {
            ms.Write(conv_bytes(width, 1), 0, 1);
            ms.Write(conv_bytes(height, 1), 0, 1);
            ms.Write(conv_bytes(colorCount, 1), 0, 1);
            ms.Write(conv_bytes(0, 1), 0, 1);
            ms.Write(conv_bytes(1, 1), 0, 1);
            ms.Write(conv_bytes(bitCount, 2), 0, 2);
            ms.Write(conv_bytes(dataSize, 4), 0, 4);
            ms.Write(conv_bytes(dataOffset, 4), 0, 4);
		}
		private void ReadDirectory(byte[] b, int index)
		{
			width = b[6 + index * 16 + 0]; if (width == 0) width = 256;
			height = b[6 + index * 16 + 1]; if (height == 0) height = 256;
			colorCount = b[6 + index * 16 + 2];
			if (b[6 + index * 16 + 3] != 0)
				throw new FileLoadException("Invalid Icon-directory");
			colorPlanes = conv_int(b, 6 + index * 16 + 4, 2);
			bitCount = conv_int(b, 6 + index * 16 + 6, 2);
			dataSize = conv_int(b, 6 + index * 16 + 8, 4);
			dataOffset = conv_int(b, 6 + index * 16 + 12, 4);
			compression = conv_int(b, dataOffset + 16, 4);
			if ((colorCount == 0) & (bitCount <= 8)) colorCount = 1 << bitCount;
		}
		private void ReadContent(byte[] b, int index)
		{
			int colors = (bitCount <= 8) ? (1 << bitCount) : 0;
			data_bytes = b.Skip(dataOffset).Take(dataSize).ToArray();
			if (conv_int(data_bytes, 0, 4) == 40)
			{
				rasterData = bmp.Parse(data_bytes, bitCount, colorCount, new Size(width, height));
				bmp_local = new Bitmap(width, height);
				for (int i = 0; i < width; i++)
					for (int j = 0; j < height; j++)
						bmp_local.SetPixel(i, j, rasterData[i, j]);
			}
			else if (data_bytes.Take(8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }))
			{
				MemoryStream ms = new MemoryStream(data_bytes);
				bmp_local = (Bitmap)Image.FromStream(ms);
			}
		}
		/*PixelFormat conv_pf(int bitcount)
		{
			switch(bitcount)
			{
				case 32:
					return PixelFormat.Format32bppArgb;
				case 24:
					return PixelFormat.Format24bppRgb;
				case 8:
					return PixelFormat.Format8bppIndexed;
				case 4:
					return PixelFormat.Format4bppIndexed;
				case 1:
					return PixelFormat.Format1bppIndexed;
				case 48:
					return PixelFormat.Format48bppRgb;
				case 64:
					return PixelFormat.Format64bppArgb;
				default:
					return PixelFormat.DontCare;
					break;
			}
		}*/

		Color Trans = Color.FromArgb(0, 0, 0, 0);
		public Bitmap GetBitmap()
		{
			return bmp_local;
		}
    }
}
