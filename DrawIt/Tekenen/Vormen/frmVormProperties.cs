using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrawIt.Tekenen;

namespace DrawIt
{
	public partial class frmVormProperties : Form
	{
		public frmVormProperties()
		{
			InitializeComponent();
			//punt
			cmbPunt_PuntStijl.Items.AddRange(Enum.GetNames(typeof(Punt.enPuntStijl)));

			//lijn
			cmbLijn_LijnStijl.Items.AddRange(Enum.GetNames(typeof(DashStyle)));

			//vlak
			cmbVlak_LijnStijl.Items.AddRange(Enum.GetNames(typeof(DashStyle)));
			List<int> waarden = new List<int>();
			waarden.AddRange((int[])Enum.GetValues(typeof(HatchStyle)));
			foreach(int i in waarden.Distinct())
				cmbVlak_VulStijl.Items.Add(Enum.GetName(typeof(HatchStyle), i));
		}

		private void pnlVlak_LijnKleur_Click(object sender, EventArgs e)
		{
			ColorDialog cld = new ColorDialog();
			cld.Color = lijnVoorbeeldVlak.Kleur;

			Properties.Settings sett = new Properties.Settings();
			if(sett.kleuren != "")
				cld.CustomColors = sett.kleuren.Split(',').Select(T => Convert.ToInt32(T)).ToArray();

			if(cld.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				lijnVoorbeeldVlak.Kleur = cld.Color;
				pnlVlak_LijnKleur.BackColor = cld.Color;
				lblVlak_Omtrekmeerkleur.Visible = false;
				lblVlak_omkv = false;

				sett.kleuren = string.Join(",", cld.CustomColors.Select(T => T.ToString()).ToArray());
				sett.Save();
			}
		}
		private void cmbVlak_LijnStijl_SelectedIndexChanged(object sender, EventArgs e)
		{
			lijnVoorbeeldVlak.Lijnstijl = (DashStyle)cmbVlak_LijnStijl.SelectedIndex;
		}
		private void nudVlak_LijnDikte_ValueChanged(object sender, EventArgs e)
		{
			lijnVoorbeeldVlak.LijnDikte = (float)nudVlak_LijnDikte.Value;
		}
		private void pnlVlak_VulKleur1_Click(object sender, EventArgs e)
		{
			ColorDialog cld = new ColorDialog();
			cld.Color = vlakVoorbeeld2.Kleur1;
			Properties.Settings sett = new Properties.Settings();
			if(sett.kleuren != "")
				cld.CustomColors = sett.kleuren.Split(',').Select(T => Convert.ToInt32(T)).ToArray();
			if(cld.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				vlakVoorbeeld2.Kleur1 = cld.Color;
				pnlVlak_VulKleur1.BackColor = cld.Color;
				lblVlak_Opvullingmeerkleur1.Visible = false;
				lblVlak_vmk1v = false;
				sett.kleuren = string.Join(",", cld.CustomColors.Select(T => T.ToString()).ToArray());
				sett.Save();
			}
		}
		private void pnlVlak_VulKleur2_Click(object sender, EventArgs e)
		{
			ColorDialog cld = new ColorDialog();
			cld.Color = vlakVoorbeeld2.Kleur2;
			Properties.Settings sett = new Properties.Settings();
			if(sett.kleuren != "")
				cld.CustomColors = sett.kleuren.Split(',').Select(T => Convert.ToInt32(T)).ToArray();
			if(cld.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				vlakVoorbeeld2.Kleur2 = cld.Color;
				pnlVlak_VulKleur2.BackColor = cld.Color;
				lblVlak_Opvullingmeerkleur2.Visible = false;
				lblVlak_vmk2v = false;
				sett.kleuren = string.Join(",", cld.CustomColors.Select(T => T.ToString()).ToArray());
				sett.Save();
			}
		}
		private void cmbVlak_VulStijl_SelectedIndexChanged(object sender, EventArgs e)
		{
			vlakVoorbeeld2.VulStijl = (HatchStyle)cmbVlak_VulStijl.SelectedIndex;
		}

		private void V_Veranderd(object sender, VormVeranderdEventArgs e)
		{
			if (e != VormVeranderdEventArgs.Empty)
			{
				bundle.Items.Add(e.Actie);
			}
		}

		bool textvalue_edit = false;
		PropertyBundleChangedActie bundle;
		public DialogResult ShowDialog(Vorm[] vormen, out PropertyBundleChangedActie actie)
		{
			#region vormen onderscheiden
			Punt[] punten = vormen.Where(T => (T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt)).Select(T => (Punt)T).ToArray();
			Lijn[] lijnen = vormen.Where(T => (T.Vorm_Hoofdtype == Vorm_hoofdtype.Lijn)).Select(T => (Lijn)T).ToArray();
			Vlak[] vlakken = vormen.Where(T => (T.Vorm_Hoofdtype == Vorm_hoofdtype.Vlak)).Select(T => (Vlak)T).ToArray();
			Tekst[] teksten = vormen.Where(T => (T.Vorm_Hoofdtype == Vorm_hoofdtype.Tekst)).Select(T => (Tekst)T).ToArray();
			#endregion
			tabControl1.TabPages.Clear();

			#region VormVeranderdCallback
			bundle = new PropertyBundleChangedActie(vormen, "Vormen opmaken");
			foreach (Vorm v in vormen)
				v.Veranderd += V_Veranderd;
			#endregion
			#region Punten
			if (punten.Count() != 0)
			{
				tabControl1.TabPages.Add(tabPunt);
				#region Puntstijlen
				Punt.enPuntStijl[] puntstijlen = punten.Select(T => T.PuntStijl).Distinct().ToArray();
				if(puntstijlen.Count() == 1)
				{
					cmbPunt_PuntStijl.SelectedIndex = (int)puntstijlen[0];
				}
				else
				{
					cmbPunt_PuntStijl.SelectedIndex = -1;
				}
				puntVoorbeeld.PuntStijl = puntstijlen[0];
				#endregion
				#region PuntKleuren
				Color[] puntkleuren = punten.Select(T => T.Kleur).Distinct().ToArray();
				if(puntkleuren.Count() == 1)
				{
					pnlPunt_PuntKleur.BackColor = puntkleuren[0];
					lblPunt_meerkleur.Visible = false;
					lblPunt_mkv = false;
				}
				else
				{
					pnlPunt_PuntKleur.BackColor = Color.Transparent;
					lblPunt_meerkleur.Visible = true;
					lblPunt_mkv = true;
				}
				puntVoorbeeld.Kleur = puntkleuren[0];
				#endregion
				#region Zichtbaarheid
				trbPuntZichtbaarheid.Value = punten.Max(T => T.Zichtbaarheid);
				lblPuntZichtbaarheid.Text = trbPuntZichtbaarheid.Value + " %";
				#endregion
			}
			#endregion
			#region Lijnen
			if(lijnen.Count() != 0)
			{
				tabControl1.TabPages.Add(tabLijn);
				#region Lijnstijlen
				DashStyle[] lijnstijlen = lijnen.Select(T => T.LijnStijl).Distinct().ToArray();
				if(lijnstijlen.Count() == 1)
				{
					cmbLijn_LijnStijl.SelectedIndex = (int)lijnstijlen[0];
				}
				else
				{
					cmbLijn_LijnStijl.SelectedIndex = -1;
				}
				lijnVoorbeeldLijn.Lijnstijl = lijnstijlen[0];
				#endregion
				#region LijnPatroon
				float[][] pat = lijnen.Select(T => T.DashPattern).Distinct().ToArray();
				List<bool> boolL = floatL_boolL(pat[0].ToList());
				customDash1.Dots.Clear();
				customDash1.Dots.AddRange(boolL.ToArray());
				customDash1.LijnStijl = lijnstijlen[0];
				customDash1.Changed = false;
				if(lijnstijlen[0] == DashStyle.Custom) lijnVoorbeeldLijn.DashPattern = boolL_floatL(boolL).ToArray();
				trackBar1.Value = boolL.Count;
				#endregion
				#region LijnKleur
				Color[] lijnkleuren = lijnen.Select(T => T.LijnKleur).Distinct().ToArray();
				if(lijnkleuren.Count() == 1)
				{
					pnlLijn_LijnKleur.BackColor = lijnkleuren[0];
					lblLijn_meerkleur.Visible = false;
					lblLijn_mkv = false;
				}
				else
				{
					pnlLijn_LijnKleur.BackColor = Color.Transparent;
					lblLijn_meerkleur.Visible = true;
					lblLijn_mkv = true;
				}
				lijnVoorbeeldLijn.Kleur = lijnkleuren[0];
				#endregion
				#region LijnDikte
				float[] lijndiktes = lijnen.Select(T => T.LijnDikte).Distinct().ToArray();
				if(lijndiktes.Length == 1)
				{
					nudLijn_LijnDikte.Value = (decimal)lijndiktes[0];
				}
				else
				{
					nudLijn_LijnDikte.ResetText();
				}
				lijnVoorbeeldLijn.LijnDikte = lijndiktes[0];
				#endregion
				#region DashCap
				DashCap[] caps = lijnen.Select(T => T.DashCap).Distinct().ToArray();
				if(caps.Length == 1)
				{
					rbnLijnCapFlat.Checked = caps[0] == DashCap.Flat;
					rbnLijnCapTriangle.Checked = caps[0] == DashCap.Triangle;
					rbnLijnCapRound.Checked = caps[0] == DashCap.Round;
				}
				else
				{
					rbnLijnCapFlat.Checked = false;
					rbnLijnCapTriangle.Checked = false;
					rbnLijnCapRound.Checked = false;
				}
				lijnVoorbeeldLijn.DashCap = caps[0];
				#endregion
				#region Zichtbaarheid
				trbLijnZichtbaarheid.Value = lijnen.Max(T => T.Zichtbaarheid);
				lblLijnZichtbaarheid.Text = trbLijnZichtbaarheid.Value + " %";
				#endregion
			}
			#endregion
			#region Vlakken
			if(vlakken.Count() != 0)
			{
				tabControl1.TabPages.Add(tabVlak);
				#region Lijnstijlen
				DashStyle[] lijnstijlen = vlakken.Select(T => T.LijnStijl).Distinct().ToArray();
				if(lijnstijlen.Count() == 1)
				{
					cmbVlak_LijnStijl.SelectedIndex = (int)lijnstijlen[0];
				}
				else
				{
					cmbVlak_LijnStijl.SelectedIndex = -1;
				}
				lijnVoorbeeldVlak.Lijnstijl = lijnstijlen[0];
				#endregion
				#region Lijnkleur
				Color[] lijnkleuren = vlakken.Select(T => T.LijnKleur).Distinct().ToArray();
				if(lijnkleuren.Count() == 1)
				{
					pnlVlak_LijnKleur.BackColor = lijnkleuren[0];
					lblVlak_Omtrekmeerkleur.Visible = false;
					lblVlak_omkv = false;
				}
				else
				{
					pnlVlak_LijnKleur.BackColor = Color.Transparent;
					lblVlak_Omtrekmeerkleur.Visible = true;
					lblVlak_omkv = true;
				}
				lijnVoorbeeldVlak.Kleur = lijnkleuren[0];
				#endregion
				#region LijnDikte
				float[] lijndiktes = vlakken.Select(T => T.LijnDikte).Distinct().ToArray();
				if(lijndiktes.Count() == 1)
				{
					nudVlak_LijnDikte.Value = (decimal)lijndiktes[0];
				}
				else
				{
					nudVlak_LijnDikte.ResetText();
				}
				lijnVoorbeeldVlak.LijnDikte = lijndiktes[0];
				#endregion
				#region DashCap
				DashCap[] caps = vlakken.Select(T => T.DashCap).Distinct().ToArray();
				if (caps.Length == 1)
				{
					rbnVlakCapFlat.Checked = caps[0] == DashCap.Flat;
					rbnVlakCapTriangle.Checked = caps[0] == DashCap.Triangle;
					rbnVlakCapRound.Checked = caps[0] == DashCap.Round;
				}
				else
				{
					rbnVlakCapFlat.Checked = false;
					rbnVlakCapTriangle.Checked = false;
					rbnVlakCapRound.Checked = false;
				}
				lijnVoorbeeldVlak.DashCap = caps[0];
				#endregion
				#region Soort Brush
				Vlak.OpvulSoort[] brushsoorten = vlakken.Select(T => T.OpvulType).Distinct().ToArray();
				vlakVoorbeeld2.OpvulType = brushsoorten[0];
				if(brushsoorten.Length == 1)
				{
					cmbVlakBrush.SelectedIndex = (int)brushsoorten[0];
					switch(brushsoorten[0])
					{
						case Vlak.OpvulSoort.Solid:
							tlpKleur1.Visible = true;
							tlpKleur2.Visible = false;
							tlpVulstijl.Visible = false;
							tlpVerloophoek.Visible = false;
							break;
						case Vlak.OpvulSoort.Hatch:
							tlpKleur1.Visible = true;
							tlpKleur2.Visible = true;
							tlpVulstijl.Visible = true;
							tlpVerloophoek.Visible = false;
							break;
						case Vlak.OpvulSoort.LinearGradient:
							tlpKleur1.Visible = true;
							tlpKleur2.Visible = true;
							tlpVulstijl.Visible = false;
							tlpVerloophoek.Visible = true;
							break;
						default:
							break;
					}
				}
				else
				{
					cmbVlakBrush.SelectedIndex = -1;
					tlpKleur1.Visible = false;
					tlpKleur2.Visible = false;
					tlpVulstijl.Visible = false;
					tlpVerloophoek.Visible = false;
				}
				#endregion
				#region Kleur1
				Color[] vulkleuren1 = vlakken.Select(T => T.VulKleur1).Distinct().ToArray();
				if(vulkleuren1.Count() == 1)
				{
					pnlVlak_VulKleur1.BackColor = vulkleuren1[0];
					lblVlak_Opvullingmeerkleur1.Visible = false;
					lblVlak_vmk1v = false;
				}
				else
				{
					pnlVlak_VulKleur1.BackColor = Color.Transparent;
					lblVlak_Opvullingmeerkleur1.Visible = true;
					lblVlak_vmk1v = true;
				}
				vlakVoorbeeld2.Kleur1 = vulkleuren1[0];
				#endregion
				#region Kleur2
				Color[] vulkleuren2 = vlakken.Select(T => T.VulKleur2).Distinct().ToArray();
				if(vulkleuren2.Count() == 1)
				{
					pnlVlak_VulKleur2.BackColor = vulkleuren2[0];
					lblVlak_Opvullingmeerkleur2.Visible = false;
					lblVlak_vmk2v = false;
				}
				else
				{
					pnlVlak_VulKleur2.BackColor = Color.Transparent;
					lblVlak_Opvullingmeerkleur2.Visible = true;
					lblVlak_vmk2v = true;
				}
				vlakVoorbeeld2.Kleur2 = vulkleuren2[0];
				#endregion
				#region VulStijl
				HatchStyle[] vulstijlen = vlakken.Select(T => T.VulStijl).Distinct().ToArray();
				if(vulstijlen.Count() == 1)
				{
					cmbVlak_VulStijl.SelectedIndex = (int)vulstijlen[0];
				}
				else
				{
					cmbVlak_VulStijl.SelectedIndex = -1;
				}
				vlakVoorbeeld2.VulStijl = vulstijlen[0];
				#endregion
				#region Verloophoek
				int[] hoeken = vlakken.Select(T => T.LoopHoek).Distinct().ToArray();
				if(hoeken.Count() == 1)
				{
					nudVlak_hoek.Value = hoeken[0];
				}
				else
				{
					nudVlak_hoek.ResetText();
				}
				vlakVoorbeeld2.Loophoek = hoeken[0];
				#endregion
				#region Zichtbaarheid
				trbVlakZichtbaarheid.Value = vlakken.Max(T => T.Zichtbaarheid);
				lblVlakZichtbaarheid.Text = trbVlakZichtbaarheid.Value + " %";
				#endregion
			}
			#endregion
			#region Teksten
			if(teksten.Length != 0)
			{
				tabControl1.TabPages.Add(tabTekst);
				#region Uitlijning
				IEnumerable<ContentAlignment> l = teksten.Select(T => T.Uitlijning).Distinct();
				if(l.Count() == 1)
				{
					switch(l.First())
					{
						case ContentAlignment.TopLeft:
							rbnTopLeft.Checked = true;
							break;
						case ContentAlignment.TopCenter:
							rbnTopCenter.Checked = true;
							break;
						case ContentAlignment.TopRight:
							rbnTopRight.Checked = true;
							break;
						case ContentAlignment.MiddleLeft:
							rbnMiddleLeft.Checked = true;
							break;
						case ContentAlignment.MiddleCenter:
							rbnMiddleCenter.Checked = true;
							break;
						case ContentAlignment.MiddleRight:
							rbnMiddleRight.Checked = true;
							break;
						case ContentAlignment.BottomLeft:
							rbnBottomLeft.Checked = true;
							break;
						case ContentAlignment.BottomCenter:
							rbnBottomCenter.Checked = true;
							break;
						case ContentAlignment.BottomRight:
							rbnBottomRight.Checked = true;
							break;
						default:
							break;
					}
				}
				else
					foreach(RadioButton rbn in groupBox6.Controls)
						rbn.Checked = false;
				#endregion
				#region Meeschalen
				IEnumerable<bool> s = teksten.Select(T => T.Meeschalen).Distinct();
				if(s.Count() == 1)
					chkMeeschalen.Checked = s.First();
				else
					chkMeeschalen.CheckState = CheckState.Indeterminate;
				#endregion
				#region Font
				IEnumerable<Font> fonts = teksten.Select(T => T.Font).Distinct();
				if(fonts.Count() == 1)
				{
					label21.Font = fonts.First();
					label21.Text = fonts.First().Name;
                }
				else
				{
					label21.Font = new Font(FontFamily.GenericMonospace, 14);
					label21.Text = "(meerdere)";
				}
				#endregion
				#region Tekstkleur
				IEnumerable<Color> kleuren = teksten.Select(T => T.Kleur).Distinct();
				if(kleuren.Count() == 1)
				{
					pnlTekstKleur.BackColor = kleuren.First();
					label20.Visible = false;
					lblTekst_mkv = false;
                }
				else
				{
					pnlTekstKleur.BackColor = Color.Transparent;
					label20.Visible = true;
					lblTekst_mkv = true;
                }
				#endregion
				#region Tekst
				List<string> txt = teksten.Select(T => T.Text).Distinct().ToList();
				if(txt.Count == 1)
				{
					btnWijzigTekst.Visible = false;
					txtTekstWaarde.Visible = true;
					txtTekstWaarde.Text = txt[0];
					textvalue_edit = true;
                }
				else
				{
					textvalue_edit = false;
					btnWijzigTekst.Visible = true;
					txtTekstWaarde.Visible = false;
				}
				#endregion
				#region Zichtbaarheid
				trbTekstZichtbaarheid.Value = teksten.Max(T => T.Zichtbaarheid);
				lblTekstZichtbaarheid.Text = trbTekstZichtbaarheid.Value + " %";
				#endregion
			}
			#endregion
			DialogResult dr = this.ShowDialog();

			if(dr == DialogResult.OK)
			{
				#region Punten
				foreach(Punt punt in punten)
				{
					#region Puntstijlen
					if(cmbPunt_PuntStijl.SelectedIndex != -1)
						punt.PuntStijl = (Punt.enPuntStijl)cmbPunt_PuntStijl.SelectedIndex;
					#endregion
					#region Puntkleuren
					if(!lblPunt_mkv)
						punt.Kleur = pnlPunt_PuntKleur.BackColor;
					#endregion
					#region Zichtbaarheid
					punt.Zichtbaarheid = trbPuntZichtbaarheid.Value;
					#endregion
				}
				#endregion
				#region Lijnen
				foreach(Lijn lijn in lijnen)
				{
					#region Lijnstijlen - LijnPatroon
					if(cmbLijn_LijnStijl.SelectedIndex != -1)
					{
						lijn.LijnStijl = (DashStyle)cmbLijn_LijnStijl.SelectedIndex;
						if((cmbLijn_LijnStijl.SelectedIndex == 5) & customDash1.Changed)
							lijn.DashPattern = boolL_floatL(customDash1.Dots.ToList()).ToArray();
					}
					#endregion
					#region DashCaps
					if (rbnLijnCapFlat.Checked)
						lijn.DashCap = DashCap.Flat;
					else if (rbnLijnCapTriangle.Checked)
						lijn.DashCap = DashCap.Triangle;
					else if (rbnLijnCapRound.Checked)
						lijn.DashCap = DashCap.Round;
					#endregion
					#region Lijnkleur
					if (!lblLijn_mkv)
						lijn.LijnKleur = pnlLijn_LijnKleur.BackColor;
					#endregion
					#region LijnDikte
					if(nudLijn_LijnDikte.Text != "")
						lijn.LijnDikte = (float)nudLijn_LijnDikte.Value;
					#endregion
					#region Zichtbaarheid
					lijn.Zichtbaarheid = trbLijnZichtbaarheid.Value;
					#endregion
				}
				#endregion
				#region Vlakken
				foreach(Vlak vlak in vlakken)
				{
					#region Lijnstijl
					if(cmbVlak_LijnStijl.SelectedIndex != -1)
						vlak.LijnStijl = (DashStyle)cmbVlak_LijnStijl.SelectedIndex;
					#endregion
					#region Lijnkleur
					if(!lblVlak_omkv)
						vlak.LijnKleur = pnlVlak_LijnKleur.BackColor;
					#endregion
					#region LijnDikte
					if(nudVlak_LijnDikte.Text != "")
						vlak.LijnDikte = (float)nudVlak_LijnDikte.Value;
					#endregion
					#region DashCaps
					if (rbnVlakCapFlat.Checked)
						vlak.DashCap = DashCap.Flat;
					else if (rbnVlakCapTriangle.Checked)
						vlak.DashCap = DashCap.Triangle;
					else if (rbnVlakCapRound.Checked)
						vlak.DashCap = DashCap.Round;
					#endregion
					#region Soort Brush
					if (cmbVlakBrush.SelectedIndex != -1)
						vlak.OpvulType = (Vlak.OpvulSoort)cmbVlakBrush.SelectedIndex;
					#endregion
					#region Kleur1
					if(!lblVlak_vmk1v)
						vlak.VulKleur1 = pnlVlak_VulKleur1.BackColor;
					#endregion
					#region Kleur2
					if(!lblVlak_vmk2v)
						vlak.VulKleur2 = pnlVlak_VulKleur2.BackColor;
					#endregion
					#region VulStijl
					if(cmbVlak_VulStijl.SelectedIndex != -1)
						vlak.VulStijl = (HatchStyle)cmbVlak_VulStijl.SelectedIndex;
					#endregion
					#region Verloophoek
					if(nudVlak_hoek.Text != "")
						vlak.LoopHoek = (int)nudVlak_hoek.Value;
					#endregion
					#region Zichtbaarheid
					vlak.Zichtbaarheid = trbVlakZichtbaarheid.Value;
					#endregion
				}
				#endregion
				#region Teksten
				foreach(Tekst tekst in teksten)
				{
					#region Uitlijning
					if(rbnTopLeft.Checked)
						tekst.Uitlijning = ContentAlignment.TopLeft;
					else if(rbnTopCenter.Checked)
						tekst.Uitlijning = ContentAlignment.TopCenter;
					else if(rbnTopRight.Checked)
						tekst.Uitlijning = ContentAlignment.TopRight;
					else if(rbnMiddleLeft.Checked)
						tekst.Uitlijning = ContentAlignment.MiddleLeft;
					else if(rbnMiddleCenter.Checked)
						tekst.Uitlijning = ContentAlignment.MiddleCenter;
					else if(rbnMiddleRight.Checked)
						tekst.Uitlijning = ContentAlignment.MiddleRight;
					else if(rbnBottomLeft.Checked)
						tekst.Uitlijning = ContentAlignment.BottomLeft;
					else if(rbnBottomCenter.Checked)
						tekst.Uitlijning = ContentAlignment.BottomCenter;
					else if(rbnBottomRight.Checked)
						tekst.Uitlijning = ContentAlignment.BottomRight;
					#endregion
					#region Meeschalen
					if(chkMeeschalen.CheckState != CheckState.Indeterminate)
						tekst.Meeschalen = chkMeeschalen.Checked;
					#endregion
					#region Font
					if(label21.Text != "(meerdere)")
					{
						tekst.Font = label21.Font;
					}
					#endregion
					#region Tekstkleur
					if(!lblTekst_mkv)
						tekst.Kleur = pnlTekstKleur.BackColor;
					#endregion
					#region Tekst
					if(textvalue_edit)
					{
						tekst.Text = txtTekstWaarde.Text;
					}
					#endregion
					#region Zichtbaarheid
					tekst.Zichtbaarheid = trbTekstZichtbaarheid.Value;
					#endregion
				}
				#endregion
				actie = bundle;
			}
			else
			{
				actie = null;
			}

			foreach (Vorm v in vormen)
				v.Veranderd -= V_Veranderd;

			return dr;
		}


		List<bool> floatL_boolL(List<float> floatL)
		{
			List<bool> result = new List<bool>();
			bool t = true;
			foreach(float fl in floatL)
			{
				for(int i = 0; i < fl; i++)
					result.Add(t);
				t = !t;
			}
			return result;
		}
		List<float> boolL_floatL(List<bool> boolL)
		{
			bool prev = boolL.Count == 0 ? true : boolL[0];
			List<float> result = new List<float>(new float[] { 1.0f });

			for(int i = 1; i < boolL.Count; i++)
			{
				if(boolL[i] == prev)
					result[result.Count - 1]++;
				else
				{
					result.Add(1.0f);
					prev = customDash1.Dots[i];
				}
			}
			return result;
		}

		private bool lblPunt_mkv, lblLijn_mkv, lblVlak_omkv, lblVlak_vmk1v, lblVlak_vmk2v, lblTekst_mkv;

		private void pnlLijn_LijnKleur_Click(object sender, EventArgs e)
		{
			ColorDialog cld = new ColorDialog();
			cld.Color = lijnVoorbeeldLijn.Kleur;
			Properties.Settings sett = new Properties.Settings();
			if(sett.kleuren != "")
				cld.CustomColors = sett.kleuren.Split(',').Select(T => Convert.ToInt32(T)).ToArray();
			if(cld.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				lijnVoorbeeldLijn.Kleur = cld.Color;
				pnlLijn_LijnKleur.BackColor = cld.Color;
				lblLijn_meerkleur.Visible = false;
				lblLijn_mkv = false;
				sett.kleuren = string.Join(",", cld.CustomColors.Select(T => T.ToString()).ToArray());
				sett.Save();
			}
		}

		private void cmbLijn_LijnStijl_SelectedIndexChanged(object sender, EventArgs e)
		{
			lijnVoorbeeldLijn.Lijnstijl = (DashStyle)cmbLijn_LijnStijl.SelectedIndex;
			customDash1.LijnStijl = lijnVoorbeeldLijn.Lijnstijl;
			trackBar1.Value = customDash1.Lengte;
			trackBar1.Enabled = cmbLijn_LijnStijl.SelectedIndex == 5;
			customDash1.Enabled = cmbLijn_LijnStijl.SelectedIndex == 5;
		}

		private void trbPuntZichtbaarheid_Scroll(object sender, EventArgs e)
		{
			lblPuntZichtbaarheid.Text = trbPuntZichtbaarheid.Value + " %";
		}

		private void trbLijnZichtbaarheid_Scroll(object sender, EventArgs e)
		{
			lblLijnZichtbaarheid.Text = trbLijnZichtbaarheid.Value + " %";
		}

		private void trbVlakZichtbaarheid_Scroll(object sender, EventArgs e)
		{
			lblVlakZichtbaarheid.Text = trbVlakZichtbaarheid.Value + " %";
		}

		private void btnKleur1naarKleur2_Click(object sender, EventArgs e)
		{
			if(!lblVlak_vmk1v)
			{
				vlakVoorbeeld2.Kleur2 = pnlVlak_VulKleur1.BackColor;
				pnlVlak_VulKleur2.BackColor = pnlVlak_VulKleur1.BackColor;
				lblVlak_Opvullingmeerkleur2.Visible = false;
				lblVlak_vmk2v = false;
			}
		}

		private void btnKleur2naarKleur1_Click(object sender, EventArgs e)
		{
			if(!lblVlak_vmk2v)
			{
				vlakVoorbeeld2.Kleur1 = pnlVlak_VulKleur2.BackColor;
				pnlVlak_VulKleur1.BackColor = pnlVlak_VulKleur2.BackColor;
				lblVlak_Opvullingmeerkleur1.Visible = false;
				lblVlak_vmk1v = false;
			}
		}

		private void trbTekstZichtbaarheid_Scroll(object sender, EventArgs e)
		{
			lblTekstZichtbaarheid.Text = trbTekstZichtbaarheid.Value + " %";
		}

		private void label21_Click(object sender, EventArgs e)
		{
			FontDialog fd = new FontDialog();
			fd.Font = label21.Font;
			if(fd.ShowDialog() == DialogResult.OK)
			{
				label21.Font = fd.Font;
				label21.Text = fd.Font.Name;
			}
		}

		private void btnWijzigTekst_Click(object sender, EventArgs e)
		{
			txtTekstWaarde.Visible = true;
			btnWijzigTekst.Visible = false;
			textvalue_edit = true;
		}

		private void cmbVlakBrush_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch(cmbVlakBrush.SelectedIndex)
			{
				case 1:
					tlpKleur1.Visible = true;
					tlpKleur2.Visible = true;
					tlpVulstijl.Visible = true;
					tlpVerloophoek.Visible = false;
					vlakVoorbeeld2.OpvulType = Vlak.OpvulSoort.Hatch;
					break;
				case 2:
					tlpKleur1.Visible = true;
					tlpKleur2.Visible = true;
					tlpVulstijl.Visible = false;
					tlpVerloophoek.Visible = true;
					vlakVoorbeeld2.OpvulType = Vlak.OpvulSoort.LinearGradient;
					break;
				case 3:
					tlpKleur1.Visible = true;
					tlpKleur2.Visible = true;
					tlpVulstijl.Visible = false;
					tlpVerloophoek.Visible = false;
					vlakVoorbeeld2.OpvulType = Vlak.OpvulSoort.RadialGradient;
					break;
				case 4:
					tlpKleur1.Visible = false;
					tlpKleur2.Visible = false;
					tlpVulstijl.Visible = false;
					tlpVerloophoek.Visible = false;
					vlakVoorbeeld2.OpvulType = Vlak.OpvulSoort.Geen;
					break;
				default:
					tlpKleur1.Visible = true;
					tlpKleur2.Visible = false;
					tlpVulstijl.Visible = false;
					tlpVerloophoek.Visible = false;
					vlakVoorbeeld2.OpvulType = Vlak.OpvulSoort.Solid;
					break;
			}
		}

		private void nudVlak_hoek_ValueChanged(object sender, EventArgs e)
		{
			vlakVoorbeeld2.Loophoek = (int)nudVlak_hoek.Value;
		}

		private void rbnDashCapLijn_changed(object sender, EventArgs e)
		{
			RadioButton rbn = (RadioButton)sender;
			if (rbn.Checked)
			{
				lijnVoorbeeldLijn.DashCap = (DashCap)Convert.ToInt32(rbn.Tag);
			}
		}

		private void rbnDashCapVlak_changed(object sender, EventArgs e)
		{
			RadioButton rbn = (RadioButton)sender;
			if(rbn.Checked)
			{
				lijnVoorbeeldVlak.DashCap = (DashCap)Convert.ToInt32(rbn.Tag);
			}
		}

		private void pnlTekstKleur_Click(object sender, EventArgs e)
		{
			ColorDialog cld = new ColorDialog();
			cld.Color = pnlTekstKleur.BackColor;
			Properties.Settings sett = new Properties.Settings();
			if(sett.kleuren != "")
				cld.CustomColors = sett.kleuren.Split(',').Select(T => Convert.ToInt32(T)).ToArray();
			if(cld.ShowDialog() == DialogResult.OK)
			{
				pnlTekstKleur.BackColor = cld.Color;
				label20.Visible = false;
				lblTekst_mkv = false;
                sett.kleuren = string.Join(",", cld.CustomColors.Select(T => T.ToString()).ToArray());
				sett.Save();
			}
		}

		private void nudLijn_LijnDikte_ValueChanged(object sender, EventArgs e)
		{
			lijnVoorbeeldLijn.LijnDikte = (float)nudLijn_LijnDikte.Value;
		}

		private void pnlPunt_PuntKleur_Click(object sender, EventArgs e)
		{
			ColorDialog cld = new ColorDialog();
			cld.Color = puntVoorbeeld.Kleur;
			Properties.Settings sett = new Properties.Settings();
			if(sett.kleuren != "")
				cld.CustomColors = sett.kleuren.Split(',').Select(T => Convert.ToInt32(T)).ToArray();
			if(cld.ShowDialog() == DialogResult.OK)
			{
				puntVoorbeeld.Kleur = cld.Color;
				pnlPunt_PuntKleur.BackColor = cld.Color;
				lblPunt_meerkleur.Visible = false;
				lblPunt_mkv = false;
				sett.kleuren = string.Join(",", cld.CustomColors.Select(T => T.ToString()).ToArray());
				sett.Save();
			}
		}

		private void cmbPunt_PuntStijl_SelectedIndexChanged(object sender, EventArgs e)
		{
			puntVoorbeeld.PuntStijl = (Punt.enPuntStijl)cmbPunt_PuntStijl.SelectedIndex;
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			customDash1.Lengte = trackBar1.Value;

		}
	}
}
