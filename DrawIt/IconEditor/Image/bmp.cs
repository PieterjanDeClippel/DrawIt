using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Drawing.Imaging;

namespace DrawIt
{
	public static class bmp
	{
		public static JaggedList<Color> Parse(byte[] bmp_data, int bit_count, int color_count, Size dim)
		{
			JaggedList<Color> rasterdata = new JaggedList<Color>() { Dimension = dim };
			if (bit_count > 8)
			{
				int t = 40;
				for (int j = 0; j < dim.Height; j++)
					for (int i = 0; i < dim.Width; i++)
					{
						Color k = Color.Black;
						switch (bit_count)
						{
							case 32:
								k = Color.FromArgb(bmp_data[t + 3], bmp_data[t + 2], bmp_data[t + 1], bmp_data[t]);
								t += 4;
								break;
							case 24:
								k = Color.FromArgb(255, bmp_data[t + 2], bmp_data[t + 1], bmp_data[t]);
								t += 3;
								break;
						}
						rasterdata[i, dim.Height - j - 1] = k;
					}
			}
			else
			{
				Color[] colorTable = ReadColorTable(bmp_data, color_count);
				rasterdata = ReadRasterDataIndexed(bmp_data, colorTable, color_count, dim, bit_count);
				ApplyXORbitmap(bmp_data, ref rasterdata, color_count, bit_count);
			}
			return rasterdata;
		}

		private static Color[] ReadColorTable(byte[] data, int count)
		{
			Color[] kleuren = new Color[256];
			for (int i = 40, j = 0; i < 40 + 4 * count; i += 4)
			{
				int B = data[i];
				int G = data[i + 1];
				int R = data[i + 2];
				kleuren[j++] = Color.FromArgb(255, R, G, B);
			}
			return kleuren;
		}
		private static JaggedList<Color> ReadRasterDataIndexed(byte[] data, Color[] colorTable, int colorCount, Size size, int bitCount)
		{
			JaggedList<Color> result = new JaggedList<Color>() { Dimension = size };
			MemoryStream ms1 = new MemoryStream(data.Skip(40 + 4 * colorCount).ToArray());
			bool[] rasterdata_bits = ms1.ReadBits(true);

			for (int yi = 0, t = 0; yi < size.Height; yi++)
			{
				for (int xi = 0; xi < size.Width; xi++)
				{
					int kleur_index = conv_int(rasterdata_bits, (t++) * bitCount, bitCount);
					result[xi, size.Height - yi - 1] = colorTable[kleur_index];
				}
			}
			return result;
		}
		private static void ApplyXORbitmap(byte[] raw_data, ref JaggedList<Color> raster_data, int color_count, int bit_count)
		{
			int start_xor = 40 + 4 * color_count + raster_data.Dimension.Width * raster_data.Dimension.Height / 8 * bit_count;
			MemoryStream ms = new MemoryStream(raw_data.Skip(start_xor).ToArray());
			bool[] xor_bmp = ms.ReadBits(true);

			int b32 = RoundUp(raster_data.Dimension.Width, 32);
			List<bool> temp = new List<bool>();
			for (int j = 0; j < raster_data.Dimension.Height; j++)
				temp.AddRange(xor_bmp.Skip(j * b32).Take(raster_data.Dimension.Width));
			xor_bmp = temp.ToArray();


			int x = 0, y = raster_data.Dimension.Height - 1;
			foreach (bool bit in xor_bmp)
			{
				if (bit)
					raster_data[x, y] = Color.Transparent;

				if (++x == raster_data.Dimension.Width)
				{
					x = 0;
					y--;
				}
			}
		}
		public static int conv_int(bool[] b, int start, int count)
		{
			int res = 0;
			int factor = 128;
			for (int i = start; i < start + count; i++)
			{
				if (b[i]) res += factor;
				factor /= 2;
			}
			return res;
		}
		public static byte[] conv_bytes(int i, int count)
		{
			byte[] res = new byte[count];
			for (int t = 0; t < count; t++)
			{
				res[t] = Convert.ToByte(i % 256);
				i >>= 8;
			}
			return res;
		}
		private static int RoundUp(int input, int veelvoud)
		{
			int result = 0;
			while (result < input)
				result += veelvoud;
			return result;
		}

		static void a()
		{
			/*Bitmap bmp = new Bitmap(40,40);
			bmp.Save("", ImageCodecInfo.GetImageEncoders(), new EncoderParameters());
			EncoderParameters enc_p = new EncoderParameters(2);
			enc_p[0] = new EncoderParameter(Encoder.)*/
		}
	}
}
