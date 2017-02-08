using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace DrawIt.Tekenen
{
	// Dashcap moet nog opgeslagen en uitgelezen worden
	// version-tolerant serialization
	// https://msdn.microsoft.com/en-us/library/ms229752%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
	// https://msdn.microsoft.com/en-us/library/system.runtime.serialization.formatters.binary.binaryformatter.binder(v=vs.110).aspx
	public abstract class Vorm
	{
		public Vorm(Vorm_type type, Layer default_layer)
		{
			vorm_type = type;
			layer = default_layer;
			switch(type)
			{
				// nieuwe vorm
				case Vorm_type.Punt:
					vorm_hoofdtype = Vorm_hoofdtype.Punt;
					break;
				case Vorm_type.Rechte:
				case Vorm_type.Kromme:
				case Vorm_type.CirkelBoog:
				case Vorm_type.RaakBoog:
                case Vorm_type.Maatlijn:
					vorm_hoofdtype = Vorm_hoofdtype.Lijn;
					break;
				case Vorm_type.Cirkel:
				case Vorm_type.Ellips:
				case Vorm_type.Veelhoek:
				case Vorm_type.GeslotenKromme:
				case Vorm_type.CirkelSector:
				case Vorm_type.CirkelSegment:
					vorm_hoofdtype = Vorm_hoofdtype.Vlak;
					break;
				case Vorm_type.Tekst:
					vorm_hoofdtype = Vorm_hoofdtype.Tekst;
					break;
			}
		}

		#region HoofdType
		private Vorm_hoofdtype vorm_hoofdtype;
		public Vorm_hoofdtype Vorm_Hoofdtype
		{
			get { return vorm_hoofdtype; }
		}
		#endregion
		#region Type
		private Vorm_type vorm_type;
		public Vorm_type Vorm_Type
		{
			get { return vorm_type; }
		}
		#endregion
		#region Geselecteerd
		private bool geselecteerd = false;
		public bool Geselecteerd
		{
			get { return geselecteerd; }
			set
			{
				geselecteerd = value;
				OnVeranderd(VormVeranderdEventArgs.Empty);
			}
		}
		#endregion
		#region Zichtbaar
		private int zichtbaarheid = 100;
		public int Zichtbaarheid
		{
			get { return zichtbaarheid; }
			set
			{
				if(zichtbaarheid == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Zichtbaarheid", zichtbaarheid, value));
				zichtbaarheid = value;
				OnVeranderd(e);
			}
		}

		#endregion
		#region Niveau
		private int niveau;
		public int Niveau
		{
			get { return niveau; }
			set
			{
				if(niveau == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Layer", niveau, value));
				niveau = value;
				OnVeranderd(e);
			}
		}
		#endregion
		#region Veranderd
		public delegate void VeranderdEventHandler(object sender, VormVeranderdEventArgs e);
		public event VeranderdEventHandler Veranderd;
		protected void OnVeranderd(VormVeranderdEventArgs e)
		{
			if(this.Veranderd != null & canRaiseVeranderd)
				this.Veranderd(this, e);
		}
		private bool canRaiseVeranderd = true;
		public bool CanRaiseVeranderdEvent
		{
			get { return canRaiseVeranderd; }
			set { canRaiseVeranderd = value; }
		}
		#endregion
		#region Bounds
		public abstract RectangleF Bounds(Graphics gr);
		#endregion
		#region DependentPoints
		public abstract Vorm[] Dep_Vormen { get; }
		#endregion
		#region Layer
		private Layer layer;
		public Layer Layer
		{
			get { return layer; }
			set { layer = value; }
		}
		#endregion
		public abstract int AddPunt(Punt p);
        public abstract void Draw(Tekening tek, Graphics gr, bool widepen, bool fill);
        public abstract void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vorm);
        public abstract void Draw(Graphics gr, float schaal, RectangleF window);
		public abstract override string ToString();
		public static Vorm FromString(string tekst, List<Vorm> vorm_list, List<Layer> layers)
		{
			string[] parts = tekst.Split(';');
			if(parts[3] == "") parts[3] = "(default)";
			Layer L = layers.Where(T => T.Naam == parts[3]).Count() == 0 ? layers.Where(T => T.Naam == "(default)").First() : layers.Where(T => T.Naam == parts[3]).First();
            Vorm result;
			IEnumerable<Punt> punten = vorm_list.Where(T => T.vorm_type == Vorm_type.Punt).Select(T => (Punt)T);
			IEnumerable<Rechte> lijnen = vorm_list.Where(T => T.vorm_type == Vorm_type.Rechte).Select(T => (Rechte)T);
			switch(parts[0])
			{
				case "boog":
					Cirkelboog b = new Cirkelboog(L);
					b.Zichtbaarheid = Convert.ToInt32(parts[1]);
					b.Niveau = Convert.ToInt32(parts[2]);
					b.LijnKleur = ColorTranslator.FromOle(Convert.ToInt32(parts[4]));
					b.LijnDikte = Convert.ToInt32(parts[5]);
					b.LijnStijl = (DashStyle)Convert.ToInt32(parts[6]);
					if(b.LijnStijl == DashStyle.Custom)
						b.DashPattern = parts[7].Split('/').Select(T => Convert.ToSingle(T)).ToArray();
					for(int i = 8; i < parts.Length; i++)
						b.Punten.Add(punten.Where(T => T.ID == Convert.ToInt32(parts[i])).ElementAt(0));
					result = b;
					break;
				case "kromme":
					Kromme k = new Kromme(L);
					k.Zichtbaarheid = Convert.ToInt32(parts[1]);
					k.Niveau = Convert.ToInt32(parts[2]);
					k.LijnKleur = ColorTranslator.FromOle(Convert.ToInt32(parts[4]));
					k.LijnDikte = Convert.ToInt32(parts[5]);
					k.LijnStijl = (DashStyle)Convert.ToInt32(parts[6]);
					if(k.LijnStijl == DashStyle.Custom)
						k.DashPattern = parts[7].Split('/').Select(T => Convert.ToSingle(T)).ToArray();
					for(int i = 8; i < parts.Length; i++)
						k.Punten.Add(punten.Where(T => T.ID == Convert.ToInt32(parts[i])).ElementAt(0));
					result = k;
					break;
				case "rechte":
					Rechte r = new Rechte(Convert.ToInt32(parts[1]), L);
					r.Zichtbaarheid = Convert.ToInt32(parts[2]);
					r.Niveau = Convert.ToInt32(parts[4]);
					r.LijnKleur = ColorTranslator.FromOle(Convert.ToInt32(parts[5]));
					r.LijnDikte = Convert.ToInt32(parts[6]);
					r.LijnStijl = (DashStyle)Convert.ToInt32(parts[7]);
					if(r.LijnStijl == DashStyle.Custom)
						r.DashPattern = parts[8].Split('/').Select(T => Convert.ToSingle(T)).ToArray();
					r.Punt1 = punten.Where(T => T.ID == Convert.ToInt32(parts[9])).ElementAt(0);
					r.Punt2 = punten.Where(T => T.ID == Convert.ToInt32(parts[10])).ElementAt(0);
					result = r;
					break;
				case "maatlijn":
					Maatlijn ml = new Maatlijn(L);
					ml.Zichtbaarheid = Convert.ToInt32(parts[1]);
					ml.Niveau = Convert.ToInt32(parts[2]);
					ml.LijnKleur = ColorTranslator.FromOle(Convert.ToInt32(parts[4]));
					ml.LijnDikte = Convert.ToInt32(parts[5]);
					ml.LijnStijl = (DashStyle)Convert.ToInt32(parts[6]);
					ml.Punt1 = punten.Where(T => T.ID == Convert.ToInt32(parts[7])).ElementAt(0);
					ml.Punt2 = punten.Where(T => T.ID == Convert.ToInt32(parts[8])).ElementAt(0);
					ml.Offset = Convert.ToSingle(parts[9]);
					result = ml;
					break;
				case "punt":
					Punt p = new Punt(Convert.ToInt32(parts[4]), L);
					p.Zichtbaarheid = Convert.ToInt32(parts[1]);
					p.Niveau = Convert.ToInt32(parts[2]);
					p.X = parts[5].MakeFloat();
					p.Y = parts[6].MakeFloat();
					p.PuntStijl = (Punt.enPuntStijl)Convert.ToInt32(parts[7]);
					p.Kleur = ColorTranslator.FromOle(Convert.ToInt32(parts[8]));
					result = p;
					break;
				case "cirkel":
					Cirkel c = new Cirkel(L);
					c.Zichtbaarheid = Convert.ToInt32(parts[1]);
					c.Niveau = Convert.ToInt32(parts[2]);
					c.LijnKleur = ColorTranslator.FromOle(Convert.ToInt32(parts[4]));
					c.LijnDikte = Convert.ToInt32(parts[5]);
					c.LijnStijl = (DashStyle)Convert.ToInt32(parts[6]);
					c.VulKleur1 = ColorTranslator.FromOle(Convert.ToInt32(parts[7]));
					c.VulKleur2 = ColorTranslator.FromOle(Convert.ToInt32(parts[8]));
					c.OpvulType = (Vlak.OpvulSoort)Convert.ToInt32(parts[9]);
					c.VulStijl = (HatchStyle)Convert.ToInt32(parts[10]);
					c.LoopHoek = Convert.ToInt32(parts[11]);
					for(int i = 12; i < parts.Length; i++)
						c.Punten.Add(punten.Where(T => T.ID == Convert.ToInt32(parts[i])).ElementAt(0));
					result = c;
					break;
				case "cirkelsector":
					CirkelSector cs = new CirkelSector(L);
					cs.Zichtbaarheid = Convert.ToInt32(parts[1]);
					cs.Niveau = Convert.ToInt32(parts[2]);
					cs.LijnKleur = ColorTranslator.FromOle(Convert.ToInt32(parts[4]));
					cs.LijnDikte = Convert.ToInt32(parts[5]);
					cs.LijnStijl = (DashStyle)Convert.ToInt32(parts[6]);
					cs.VulKleur1 = ColorTranslator.FromOle(Convert.ToInt32(parts[7]));
					cs.VulKleur2 = ColorTranslator.FromOle(Convert.ToInt32(parts[8]));
					cs.OpvulType = (Vlak.OpvulSoort)Convert.ToInt32(parts[9]);
					cs.VulStijl = (HatchStyle)Convert.ToInt32(parts[10]);
					cs.LoopHoek = Convert.ToInt32(parts[11]);
					for(int i = 12; i < parts.Length; i++)
						cs.Punten.Add(punten.Where(T => T.ID == Convert.ToInt32(parts[i])).ElementAt(0));
					result = cs;
					break;
				case "cirkelsegment":
					CirkelSegment csg = new CirkelSegment(L);
					csg.Zichtbaarheid = Convert.ToInt32(parts[1]);
					csg.Niveau = Convert.ToInt32(parts[2]);
					csg.LijnKleur = ColorTranslator.FromOle(Convert.ToInt32(parts[4]));
					csg.LijnDikte = Convert.ToInt32(parts[5]);
					csg.LijnStijl = (DashStyle)Convert.ToInt32(parts[6]);
					csg.VulKleur1 = ColorTranslator.FromOle(Convert.ToInt32(parts[7]));
					csg.VulKleur2 = ColorTranslator.FromOle(Convert.ToInt32(parts[8]));
					csg.OpvulType = (Vlak.OpvulSoort)Convert.ToInt32(parts[9]);
					csg.VulStijl = (HatchStyle)Convert.ToInt32(parts[10]);
					csg.LoopHoek = Convert.ToInt32(parts[11]);
					for(int i = 12; i < parts.Length; i++)
						csg.Punten.Add(punten.Where(T => T.ID == Convert.ToInt32(parts[i])).ElementAt(0));
					result = csg;
					break;
                case "ellips":
					Ellips el = new Ellips(L);
					el.Zichtbaarheid = Convert.ToInt32(parts[1]);
					el.Niveau = Convert.ToInt32(parts[2]);
					el.LijnKleur = ColorTranslator.FromOle(Convert.ToInt32(parts[4]));
					el.LijnDikte = Convert.ToInt32(parts[5]);
					el.LijnStijl = (DashStyle)Convert.ToInt32(parts[6]);
					el.VulKleur1 = ColorTranslator.FromOle(Convert.ToInt32(parts[7]));
					el.VulKleur2 = ColorTranslator.FromOle(Convert.ToInt32(parts[8]));
					el.OpvulType = (Vlak.OpvulSoort)Convert.ToInt32(parts[9]);
					el.VulStijl = (HatchStyle)Convert.ToInt32(parts[10]);
					el.LoopHoek = Convert.ToInt32(parts[11]);
					el.F1 = punten.Where(T => T.ID == Convert.ToInt32(parts[12])).ElementAt(0);
					el.F2 = punten.Where(T => T.ID == Convert.ToInt32(parts[13])).ElementAt(0);
					el.P = punten.Where(T => T.ID == Convert.ToInt32(parts[14])).ElementAt(0);
					result = el;
					break;
				case "gkromme":
					GeslotenKromme g = new GeslotenKromme(L);
					g.Zichtbaarheid = Convert.ToInt32(parts[1]);
					g.Niveau = Convert.ToInt32(parts[2]);
					g.LijnKleur = ColorTranslator.FromOle(Convert.ToInt32(parts[4]));
					g.LijnDikte = Convert.ToInt32(parts[5]);
					g.LijnStijl = (DashStyle)Convert.ToInt32(parts[6]);
					g.VulKleur1 = ColorTranslator.FromOle(Convert.ToInt32(parts[7]));
					g.VulKleur2 = ColorTranslator.FromOle(Convert.ToInt32(parts[8]));
					g.OpvulType = (Vlak.OpvulSoort)Convert.ToInt32(parts[9]);
					g.VulStijl = (HatchStyle)Convert.ToInt32(parts[10]);
					g.LoopHoek = Convert.ToInt32(parts[11]);
					for(int i = 12; i < parts.Length; i++)
						g.Punten.Add(punten.Where(T => T.ID == Convert.ToInt32(parts[i])).ElementAt(0));
					result = g;
					break;
				case "veelhoek":
					Veelhoek v = new Veelhoek(L);
					v.Zichtbaarheid = Convert.ToInt32(parts[1]);
					v.Niveau = Convert.ToInt32(parts[2]);
					v.LijnKleur = ColorTranslator.FromOle(Convert.ToInt32(parts[4]));
					v.LijnDikte = Convert.ToInt32(parts[5]);
					v.LijnStijl = (DashStyle)Convert.ToInt32(parts[6]);
					v.VulKleur1 = ColorTranslator.FromOle(Convert.ToInt32(parts[7]));
					v.VulKleur2 = ColorTranslator.FromOle(Convert.ToInt32(parts[8]));
					v.OpvulType = (Vlak.OpvulSoort)Convert.ToInt32(parts[9]);
					v.VulStijl = (HatchStyle)Convert.ToInt32(parts[10]);
					v.LoopHoek = Convert.ToInt32(parts[11]);
					for(int i = 12; i < parts.Length; i++)
						v.Punten.Add(punten.Where(T => T.ID == Convert.ToInt32(parts[i])).ElementAt(0));
					result = v;
					break;
				case "tekst":
					Tekst t = new Tekst(L);
					t.Zichtbaarheid = Convert.ToInt32(parts[1]);
					t.niveau = Convert.ToInt32(parts[2]);
					t.Uitlijning = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), parts[4]);
					t.Text = parts[5].Replace("\\r\\n",Environment.NewLine);
					FontConverter conv = new FontConverter();
					t.Font = (Font)conv.ConvertFromString(parts[6].Replace('|',';'));
					t.Kleur = ColorTranslator.FromOle(Convert.ToInt32(parts[7]));
					t.Punt = punten.Where(T => T.ID == Convert.ToInt32(parts[8])).ElementAt(0);
					t.Meeschalen = parts[9] == "1";
					result = t;
					break;
				case "raak":
					RaakBoog rb = new RaakBoog(L);
					rb.Zichtbaarheid = Convert.ToInt32(parts[1]);
					rb.Niveau = Convert.ToInt32(parts[2]);
					rb.LijnKleur = ColorTranslator.FromOle(Convert.ToInt32(parts[4]));
					rb.LijnDikte = Convert.ToInt32(parts[5]);
					rb.LijnStijl = (DashStyle)Convert.ToInt32(parts[6]);
					if(rb.LijnStijl == DashStyle.Custom)
						rb.DashPattern = parts[7].Split('/').Select(T => Convert.ToSingle(T)).ToArray();
					rb.RichtPunt = punten.Where(T => T.ID == Convert.ToInt32(parts[8])).ElementAt(0);
					rb.EindPunt = punten.Where(T => T.ID == Convert.ToInt32(parts[9])).ElementAt(0);
					rb.RichtLijn = lijnen.Where(T => T.ID == Convert.ToInt32(parts[10])).ElementAt(0);
					result = rb;
					break;
				default:
					result = null;
					break;
            }
			return result;
		}

		public abstract void CopyFont();
		public abstract void PasteFont(out Actie result);

		public bool Intersect(Tekening tekening, RectangleF sel)
		{
			if (sel.Width == 0 | sel.Height == 0) return false;
			Bitmap bmp = new Bitmap((int)sel.Width, (int)sel.Height);
			Graphics gr = Graphics.FromImage(bmp);
			gr.TranslateTransform(-sel.Left, -sel.Top);
			Draw(tekening, gr, false, false);

			Color trans = Color.FromArgb(0, 0, 0, 0);
			return !bmp.IsSolidColor(trans);
		}

		public abstract Region GetRegion(Tekening tek);
	}

	public class VormVeranderdEventArgs : EventArgs
	{
		public VormVeranderdEventArgs(PropertyChangedActie Actie)
		{
			actie = Actie;
		}

		private PropertyChangedActie actie;
		public PropertyChangedActie Actie
		{
			get { return actie; }
		}

		public static new VormVeranderdEventArgs Empty = new VormVeranderdEventArgs(new PropertyChangedActie(null, "", null, null));
	}

	public enum Vorm_type
	{
		// nieuwe vorm
		Punt,
		Rechte,
		Kromme,
		Cirkel,
		CirkelBoog,
		Ellips,
		Veelhoek,
		GeslotenKromme,
		CirkelSector,
		Tekst,
		RaakBoog,
		CirkelSegment,
        Maatlijn
    }
	public enum Vorm_hoofdtype
	{
		Punt,
		Lijn,
		Tekst,
		Vlak
	}
}
