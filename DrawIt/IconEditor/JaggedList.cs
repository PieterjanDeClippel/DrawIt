using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt
{
	public class JaggedList<T>
	{
		private List<List<object>> data = new List<List<object>>();

		#region Dimensie
		private Size dimension;
		public Size Dimension
		{
			get { return dimension; }
			set
			{
				dimension = value;
				while(data.Count != dimension.Width)
				{
					if(data.Count < dimension.Width)
					{
						List<object> nieuw = new List<object>();
						for (int i = 0; i < dimension.Height; i++)
							nieuw.Add(null);
						data.Add(nieuw);
					}
					else
					{
						data.RemoveAt(data.Count - 1);
					}
				}

				for (int i = 0; i < data.Count; i++)
				{
					while (data[i].Count != dimension.Height)
					{
						if (data[i].Count < dimension.Height)
						{
							data[i].Add(null);
						}
						else
						{
							data[i].RemoveAt(data[i].Count - 1);
						}
					}
				}
			}
		}
		#endregion
		#region Indexer
		public T this[int i, int j]
		{
			get
			{
				return (T)data[i][j];
			}
			set
			{
				data[i][j] = value;
			}
		}
		#endregion
	}
}
