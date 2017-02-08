using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DrawIt
{
	public class Pictogram
	{
		public void Load(byte[] b)
		{
			Icons.Clear();

			// File-headers uitlezen
			if (conv_int(b, 0, 2) != 0)
				throw new FileLoadException("Ongeldige header");
			if (conv_int(b, 2, 2) != 1)
				throw new FileLoadException("Dit is geen icon-bestand");
			int count = conv_int(b, 4, 2);

			// Icon-headers uitlezen
			for (int i = 0; i < count; i++)
			{
				Subpictogram pic = new Subpictogram(b, i);
				Icons.Add(pic);
			}
				/*try
				{
				}
				catch (Exception ex)
				{
				}*/
		}

		public void Load(string filename)
		{
			byte[] data = File.ReadAllBytes(filename);
			this.Load(data);
		}

		public void Save(Stream s)
		{

		}

		int conv_int(byte[] b, int start, int count)
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
		public List<Subpictogram> Icons = new List<Subpictogram>();
	}
}
