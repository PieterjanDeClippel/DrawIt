using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using DrawIt.CoolPrintPreview;
using DrawIt.Tekenen;
using System.Threading;

namespace DrawIt
{
	public partial class Tekening : Control, IDocument
	{
		public Tekening()
		{
			InitializeComponent();
			DoubleBuffered = true;
			MouseWheel += Tekening_MouseWheel;
			vormen.Changed += vormen_Changed;
			
			pd.DefaultPageSettings.Margins = new Margins(39, 39, 39, 39);
			pd.DefaultPageSettings.PaperSize = new PaperSize("PaperA4", 826, 1169);
			
			txtSchaal.Location = new Point(Width - txtSchaal.Width - 3, Height - txtSchaal.Height - 3);
			Controls.Add(txtSchaal);
			layers.Add(Default);
			currentlayer = Default;
			
			SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
		}

		private void TmiNaarAchtergrond_Click(object sender, EventArgs e)
		{
			int min = vormen.Min(T => T.Niveau);
			Vorm[] v = vormen.Where(T => T.Geselecteerd).ToArray();
			foreach (Vorm vorm in v)
				vorm.Niveau = min - 1;
		}
		private void TmiNaarAchter_Click(object sender, EventArgs e)
		{
			Vorm[] v = vormen.Where(T => T.Geselecteerd).ToArray();
			foreach (Vorm vorm in v)
			{
				IEnumerable<Vorm> achter = vormen.Where(T => T.Niveau < vorm.Niveau);
				if (achter.Count() != 0)
					vorm.Niveau = achter.OrderBy(T => T.Niveau).Last().Niveau - 1;
				else
					vorm.Niveau--;
			}
		}
		private void TmiNaarVoor_Click(object sender, EventArgs e)
		{
			Vorm[] v = vormen.Where(T => T.Geselecteerd).ToArray();
			foreach (Vorm vorm in v)
			{
				IEnumerable<Vorm> voor = vormen.Where(T => T.Niveau > vorm.Niveau);
				if (voor.Count() != 0)
					vorm.Niveau = voor.OrderBy(T => T.Niveau).First().Niveau + 1;
				else
					vorm.Niveau++;
			}
		}
		private void TmiNaarVoorgrond_Click(object sender, EventArgs e)
		{
			int max = vormen.Max(T => T.Niveau);
			Vorm[] v = vormen.Where(T => T.Geselecteerd).ToArray();
			foreach (Vorm vorm in v)
				vorm.Niveau = max + 1;
		}

		private void Tekening_MouseLeave(object sender, EventArgs e)
		{
			is_hover = false;
			this.Invalidate();
		}

		private void TmiWissen_Click(object sender, EventArgs e)
		{
			RemoveSelection();
		}
		
		void txtSchaal_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				string sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				try { this.Schaal = txtSchaal.Text.MakeFloat(); }
				catch (Exception) { }
				finally { txtSchaal.Hide(); }
				//e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}


		void tmiEigenschappen_Click(object sender, EventArgs e)
		{
			frmVormProperties frm = new frmVormProperties();
			PropertyBundleChangedActie resultaat;
			DialogResult dr = frm.ShowDialog(vormen.Where(T => T.Geselecteerd).ToArray(), out resultaat);
			if (dr == DialogResult.OK)
			{
				this.Invalidate();
				if (resultaat != null) RegisterActie(resultaat);
			}
		}


		void vormen_Changed(object sender, Collectie<Vorm>.CollectieChangedEventArgs e)
		{
			foreach (Vorm vorm in e.ItemsVerwijderd)
				if (vorm != null)
					vorm.Veranderd -= vorm_Veranderd;
			foreach (Vorm vorm in e.ItemsToegevoegd)
				if (vorm != null)
					vorm.Veranderd += vorm_Veranderd;
			changed = true;
		}

		void vorm_Veranderd(object sender, VormVeranderdEventArgs e)
		{
			Invalidate();
		}
		
		void ShowCMS()
		{
			cms.Show(Control.MousePosition);
		}

		private Punt PlaatsNieuwPunt(Graphics gr)
		{
			Punt p1 = new Punt(punt_id++, currentlayer);
			p1.Coordinaat = pt_co(PointToClient(MousePosition), gr.DpiX, gr.DpiY);
			vormen.Add(p1);
			if (vraagCoordinaat)
			{
				frmPuntCoordinaat dlg1 = new frmPuntCoordinaat();
				dlg1.txtX.Text = p1.Coordinaat.X.ToString();
				dlg1.txtY.Text = p1.Coordinaat.Y.ToString();
				if (dlg1.ShowDialog(FindForm()) == DialogResult.OK)
				{
					p1.Coordinaat = new PointF(dlg1.txtX.Text.MakeFloat(), dlg1.txtY.Text.MakeFloat());
					p1.CanRaiseVeranderdEvent = true;
					RegisterActie(new VormenToegevoegdActie(new Vorm[] { p1 }, this, Properties.Resources.DrawPoint));
					return p1;
				}
				else
				{
					vormen.Remove(p1);
					return null;
				}
			}
			else
			{
				p1.CanRaiseVeranderdEvent = true;
				RegisterActie(new VormenToegevoegdActie(new Vorm[] { p1 }, this, Properties.Resources.DrawPoint));
				return p1;
			}
		}

		public delegate void BroadcastUndoRedoStackHandler(object sender, UndoRedoStackEventArgs e);
		public event BroadcastUndoRedoStackHandler BroadcastUndoRedoStack;
		private void RegisterActie(Actie actie)
		{
			if (actie == null) return;
			changed = true;
			UndoStack.Push(actie);
			RedoStack.Clear();
			RequestUndoRedoStack();
		}
		private void OnNieuweVormVoltooid()
		{
			NieuweVorm = null;
			Ref_Vormen.Clear();
			Actie = enActie.Selecteren;

			if (NieuweVormVoltooid != null)
				NieuweVormVoltooid(this, EventArgs.Empty);
		}

		private void Tekening_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			Focus();
			if((ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				IEnumerable<Vorm> vormen_rev = vormen.Reverse().Where(T => T.Layer.Zichtbaar);
				IEnumerable<int> niveaus = vormen.Select(T => T.Niveau).Distinct().OrderBy(T => -T);
				Graphics gr = this.CreateGraphics();
				foreach (int n in niveaus)
				{
					for (int soort = 0; soort < 4; soort++)
					{
						foreach (Vorm vorm in vormen_rev.Where(T => T.Vorm_Hoofdtype == (Vorm_hoofdtype)soort).Where(T => T.Niveau == n))
						{
							if (GetHitTest(vorm, PointToClient(MousePosition), gr.DpiX, gr.DpiY))
							{
								if ((ModifierKeys & Keys.Control) == Keys.Control)
									vorm.CopyFont();
								else
								{
									Actie res;
									vorm.PasteFont(out res);
									RegisterActie(res);
								}
								vorm.Geselecteerd = false;
								Invalidate();
								return;
							}
						}
					}
				}
			}
			else if ((e.Location.X > this.Width - 68) & (e.Location.Y > this.Height - 23))
			{
				txtSchaal.Text = schaal.ToString("0.0000000");
				txtSchaal.SelectAll();
				txtSchaal.Show();
				txtSchaal.Focus();
			}
			else if (txtSchaal.Visible)
			{
				try { this.Schaal = txtSchaal.Text.MakeFloat(); }
				catch (Exception) { }
				finally { txtSchaal.Hide(); }
			}
			else if(actie == enActie.Selecteren)
			{
				if(e.Button == MouseButtons.Left)
				{
					IEnumerable<Vorm> vormen_rev = vormen.Reverse().Where(T => T.Layer.Zichtbaar);
					Graphics gr = this.CreateGraphics();
					bool shift = (ModifierKeys & Keys.Shift) == Keys.Shift;
					bool ctrl = (ModifierKeys & Keys.Control) == Keys.Control;
					if (!shift & !ctrl)
						foreach (Vorm vorm in vormen)
							vorm.Geselecteerd = false;
					IEnumerable<int> niveaus = vormen.Select(T => T.Niveau).Distinct().OrderBy(T => -T);
					foreach (int n in niveaus)
					{
						for (int soort = 0; soort < 4; soort++)
						{
							foreach (Vorm vorm in vormen_rev.Where(T => T.Vorm_Hoofdtype == (Vorm_hoofdtype)soort).Where(T => T.Niveau == n))
							{
								if (GetHitTest(vorm, PointToClient(MousePosition), gr.DpiX, gr.DpiY))
								{
									vorm.Geselecteerd = ctrl ? !vorm.Geselecteerd : true;
									if ((ModifierKeys & Keys.Alt) == Keys.Alt)
									{
										List<Vorm> andere_dep_vormen = new List<Vorm>();
										foreach (Vorm v in vormen.Except(new Vorm[] { vorm }))
											andere_dep_vormen = andere_dep_vormen.Union(v.Dep_Vormen).ToList();
										foreach (Vorm v in vorm.Dep_Vormen.Except(andere_dep_vormen))
											v.Geselecteerd = vorm.Geselecteerd;
									}
									else
									{
										foreach (Vorm v in vorm.Dep_Vormen)
											v.Geselecteerd = vorm.Geselecteerd;
									}
									return;
								}
							}
						}
					}
				}
			}
		}

		private void Tekening_MouseClick(object sender, MouseEventArgs e)
		{
			Focus();
			Graphics gr = this.CreateGraphics();
			IEnumerable<Vorm> vormen_rev = vormen.Reverse().Where(T => T.Layer.Zichtbaar);
			if ((e.Location.X > this.Width - 68) & (e.Location.Y > this.Height - 23))
			{
				float sch = this.schaal;
				txtSchaal.Text = sch.ToString("0.0000000");
				txtSchaal.SelectAll();
				txtSchaal.Show();
				txtSchaal.Focus();
			}
			else if (txtSchaal.Visible)
			{
				try { this.Schaal = txtSchaal.Text.MakeFloat(); }
				catch (Exception) { }
				finally { txtSchaal.Hide(); }
			}
			else
			{
				DialogResult dr;
				switch (actie)
				{
					case enActie.Selecteren:
						#region Selecteren
						if (e.Button == MouseButtons.Right)
						{
							//op selectie geklikt?
							Vorm v = GetVormBelowCursor();
							if (v == null) { }
							else if(v.Geselecteerd)
							{
								ShowCMS();
							}
							else
							{
								//niet rechtsgeklikt op selectie
								foreach (Vorm vorm in vormen)
									vorm.Geselecteerd = false;

								v.Geselecteerd = true;
								ShowCMS();
							}
						}
						else if ((ModifierKeys & Keys.Shift) == Keys.Shift)
						{
							Vorm v = GetVormBelowCursor();
							if (v != null)
								v.Geselecteerd = !v.Geselecteerd;
						}
						else
						{
							bool ctrl = (ModifierKeys & Keys.Control) == Keys.Control;
							if (!ctrl)
								foreach (Vorm vorm in vormen)
									vorm.Geselecteerd = false;

							Vorm v = GetVormBelowCursor();
							if(v != null)
								v.Geselecteerd = ctrl ? !v.Geselecteerd : true;
						}
						#endregion
						break;
					case enActie.Nieuw_punt:
						Punt nieuwpunt = (Punt)NieuweVorm;
						if (VraagCoordinaat)
						{
							frmPuntCoordinaat dialoog = new frmPuntCoordinaat();
							dialoog.txtX.Text = nieuwpunt.Coordinaat.X.ToString();
							dialoog.txtY.Text = nieuwpunt.Coordinaat.Y.ToString();
							dr = dialoog.ShowDialog(FindForm());
							if (dr == DialogResult.OK)
								nieuwpunt.Coordinaat = new PointF(dialoog.txtX.Text.MakeFloat(), dialoog.txtY.Text.MakeFloat());
						}
						else
							dr = DialogResult.OK;

						if (dr == DialogResult.OK)
						{
							RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this,Properties.Resources.DrawPoint));
							nieuwpunt.CanRaiseVeranderdEvent = true;
						}
						else
						{
							vormen.Remove(NieuweVorm);
						}
						OnNieuweVormVoltooid();
						break;
					case enActie.Nieuwe_Tekst:
						Punt p1 = GetPuntBelowCursor();
						if (p1 != null)
						{
							NieuweVorm.AddPunt(p1);
							p1.Cursor = Punt.SpecialCursor.None;
							RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.PlacedNewText));
							OnNieuweVormVoltooid();
						}
						else if ((p1 = PlaatsNieuwPunt(gr)) != null)
						{
							if (NieuweVorm.AddPunt(p1) == 1)
							{
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.PlacedNewText));
								OnNieuweVormVoltooid();
							}
						}
						break;
					case enActie.Nieuwe_rechte:
						Punt p2 = GetPuntBelowCursor();
						if(p2 != null)
						{
							if (NieuweVorm.AddPunt(p2) == 2)
							{
								p2.Cursor = Punt.SpecialCursor.None;
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawLine));
								OnNieuweVormVoltooid();
							}
						}
						else if ((p2 = PlaatsNieuwPunt(gr)) != null)
						{
							if (NieuweVorm.AddPunt(p2) == 2)
							{
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawLine));
								OnNieuweVormVoltooid();
							}
						}
						break;
					case enActie.Nieuwe_Maatlijn:
						Punt p3 = GetPuntBelowCursor();
						if(p3 != null)
						{
							if (NieuweVorm.AddPunt(p3) == 2)
							{
								p3.Cursor = Punt.SpecialCursor.None;
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawDimension));
								OnNieuweVormVoltooid();
							}
						}
						break;
					case enActie.Nieuwe_kromme:
						Punt p4 = GetPuntBelowCursor();
						if(p4 != null)
						{
							NieuweVorm.AddPunt(p4);
							p4.Cursor = Punt.SpecialCursor.None;
						}
						else if((p4 = PlaatsNieuwPunt(gr)) != null)
						{
							NieuweVorm.AddPunt(p4);
						}
						break;
					case enActie.Nieuwe_cirkelboog:
						Punt p5 = GetPuntBelowCursor();
						if(p5 != null)
						{
							if (NieuweVorm.AddPunt(p5) == 3)
							{
								p5.Cursor = Punt.SpecialCursor.None;
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawArc));
								OnNieuweVormVoltooid();
							}
						}
						else if((p5 = PlaatsNieuwPunt(gr)) != null)
						{
							if (NieuweVorm.AddPunt(p5) == 3)
							{
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawArc));
								OnNieuweVormVoltooid();
							}
						}
						break;
					case enActie.Nieuwe_raakboog:
						Punt p6 = GetPuntBelowCursor();
						if (p6 != null)
						{
							RaakBoog NieuweBoog = (RaakBoog)NieuweVorm;
							if (NieuweBoog.RichtPunt == null)
							{
								Rechte[] re = vormen.Where(T => T.Vorm_Type == Vorm_type.Rechte).Select(T => (Rechte)T).Where(T => ((T.Punt1 == p6) | (T.Punt2 == p6))).ToArray();
								if (re.Length == 0)
								{
									SystemSounds.Beep.Play();
								}
								else if (re.Length == 1)
								{
									NieuweBoog.RichtPunt = p6;
									NieuweBoog.RichtLijn = re[0];
								}
								else
								{
									ContextMenuStrip cms = new ContextMenuStrip();
									cms.Items.Clear();
									foreach (Rechte r in re)
									{
										ToolStripMenuItem tmi = new ToolStripMenuItem(Properties.Resources.Line + " " + r.ID);
										cms.Items.Add(tmi);
										tmi.MouseHover += delegate
										{
											r.Accent = true;
										};
										tmi.MouseLeave += delegate
										{
											r.Accent = false;
										};
										tmi.Click += delegate
										{
											NieuweBoog.RichtPunt = p6;
											NieuweBoog.RichtLijn = r;
										};
									}
									cms.Show(MousePosition);
								}
							}
							else
							{
								NieuweBoog.EindPunt = p6;
								p6.Cursor = Punt.SpecialCursor.None;
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawTangentArc));
								OnNieuweVormVoltooid();
							}
						}
						else if (((RaakBoog)NieuweVorm).RichtPunt != null)
						{
							if ((p6 = PlaatsNieuwPunt(gr)) != null)
							{
								((RaakBoog)NieuweVorm).EindPunt = p6;
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawTangentArc));
								OnNieuweVormVoltooid();
							}
						}
						break;
					case enActie.Nieuwe_cirkel2:
						Punt p7 = GetPuntBelowCursor();
						if(p7 != null)
						{
							if (NieuweVorm.AddPunt(p7) == 2)
							{
								p7.Cursor = Punt.SpecialCursor.None;
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawCircle2));
								OnNieuweVormVoltooid();
							}
						}
						else if ((p7 = PlaatsNieuwPunt(gr)) != null)
						{
							if (NieuweVorm.AddPunt(p7) == 2)
							{
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawCircle2));
								OnNieuweVormVoltooid();
							}
						}
						break;
					case enActie.Nieuwe_cirkel3:
						Punt p8 = GetPuntBelowCursor();
						if(p8 != null)
						{
							if (NieuweVorm.AddPunt(p8) == 3)
							{
								p8.Cursor = Punt.SpecialCursor.None;
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawCircle3));
								OnNieuweVormVoltooid();
							}
						}
						else if((p8 = PlaatsNieuwPunt(gr)) != null)
						{
							if (NieuweVorm.AddPunt(p8) == 3)
							{
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawCircle3));
								OnNieuweVormVoltooid();
							}
						}
						break;
					case enActie.Nieuwe_cirkelsector:
						Punt p9 = GetPuntBelowCursor();
						if(p9 != null)
						{
							if (NieuweVorm.AddPunt(p9) == 3)
							{
								p9.Cursor = Punt.SpecialCursor.None;
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawCircleSector));
								OnNieuweVormVoltooid();
							}
						}
						else if((p9 = PlaatsNieuwPunt(gr)) != null)
						{
							if (NieuweVorm.AddPunt(p9) == 3)
							{
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawCircleSector));
								OnNieuweVormVoltooid();
							}
						}
						break;
					case enActie.Nieuw_cirkelsegment:
						Punt p10 = GetPuntBelowCursor();
						if (p10 != null)
						{
							if (NieuweVorm.AddPunt(p10) == 3)
							{
								p10.Cursor = Punt.SpecialCursor.None;
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawCircleSegment));
								OnNieuweVormVoltooid();
							}
						}
						else if ((p10 = PlaatsNieuwPunt(gr)) != null)
						{
							if (NieuweVorm.AddPunt(p10) == 3)
							{
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawCircleSegment));
								OnNieuweVormVoltooid();
							}
						}
						break;
					case enActie.Nieuwe_ellips:
						Punt p11 = GetPuntBelowCursor();
						if (p11 != null)
						{
							if (NieuweVorm.AddPunt(p11) == 3)
							{
								p11.Cursor = Punt.SpecialCursor.None;
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawEllips));
								OnNieuweVormVoltooid();
							}
						}
						else if ((p11 = PlaatsNieuwPunt(gr)) != null)
						{
							if (NieuweVorm.AddPunt(p11) == 3)
							{
								RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, Properties.Resources.DrawEllips));
								OnNieuweVormVoltooid();
							}
						}
						break;
					case enActie.Nieuwe_gesloten_kromme:
						Punt p12 = GetPuntBelowCursor();
						if (p12 != null)
						{
							NieuweVorm.AddPunt(p12);
							p12.Cursor = Punt.SpecialCursor.None;
						}
						else if ((p12 = PlaatsNieuwPunt(gr)) != null)
						{
							NieuweVorm.AddPunt(p12);
						}
						break;
					case enActie.Nieuwe_veelhoek:
						Punt p13 = GetPuntBelowCursor();
						if (p13 != null)
						{
							NieuweVorm.AddPunt(p13);
							p13.Cursor = Punt.SpecialCursor.None;
						}
						else if ((p13 = PlaatsNieuwPunt(gr)) != null)
						{
							NieuweVorm.AddPunt(p13);
						}
						break;
					case enActie.Nieuwe_parallelle:
						if(Ref_Vormen.Count == 0)
						{
							Type[] types = new Type[] { typeof(Cirkelboog), typeof(RaakBoog), typeof(Rechte), typeof(Cirkel), typeof(Ellips), typeof(Veelhoek) };
							foreach (Vorm v in vormen_rev.Where(T => types.Contains(T.GetType())))
							{
								if (GetHitTest(v, PointToClient(MousePosition), gr.DpiX, gr.DpiY))
								{
									Ref_Vormen.Add(v);
									switch (v.Vorm_Type)
									{
										case Vorm_type.Rechte:
											NieuweVorm = new Rechte(rechte_id++, currentlayer);
											Ref_Vormen.Add(NieuweVorm);
											vormen.Add(NieuweVorm);
											break;
										case Vorm_type.Cirkel:
											NieuweVorm = new Cirkel(currentlayer);
											Ref_Vormen.Add(NieuweVorm);
											vormen.Add(NieuweVorm);
											break;
										case Vorm_type.CirkelBoog:
											NieuweVorm = new Cirkelboog(currentlayer);
											Ref_Vormen.Add(NieuweVorm);
											vormen.Add(NieuweVorm);
											break;
										case Vorm_type.Ellips:
											NieuweVorm = new Ellips(currentlayer);
											Ref_Vormen.Add(NieuweVorm);
											vormen.Add(NieuweVorm);
											break;
										case Vorm_type.Veelhoek:
											NieuweVorm = new Veelhoek(currentlayer);
											Ref_Vormen.Add(NieuweVorm);
											vormen.Add(NieuweVorm);
											break;
										case Vorm_type.RaakBoog:
											NieuweVorm = new RaakBoog(currentlayer);
											Ref_Vormen.Add(NieuweVorm);
											vormen.Add(NieuweVorm);
											break;
										default:
											break;
									}
								}
							}
						}
						else if(Ref_Vormen.Count(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt) == 0)
						{
							Vorm hoofdvorm = Ref_Vormen.First();
							PointF p = pt_co(MousePosition, gr.DpiX, gr.DpiY);
							switch (hoofdvorm.Vorm_Type)
							{
								case Vorm_type.Rechte:
									Rechte r = (Rechte)hoofdvorm;

									float a, b, c;
									RaakBoog.Calc_Rechte(r.Punt1.Coordinaat, r.Punt2.Coordinaat, out a, out b, out c);

									float a_l, b_l, c_l;
									RaakBoog.Calc_Loodlijn(r.Punt1.Coordinaat, r.Punt2.Coordinaat, p, out a_l, out b_l, out c_l);

									PointF S = RaakBoog.Calc_Snijpunt(a, b, c, a_l, b_l, c_l);
									double afstand = Ellips.Pyt(p, S);
									//afstand = Convert.ToDouble(InputBox.Toon("Afstand:", "Offset", afstand.ToString()));

									double hoek = Math.Atan2(r.Punt1.Coordinaat.X - r.Punt2.Coordinaat.X, r.Punt1.Coordinaat.Y - r.Punt2.Coordinaat.Y);

									if (hoek < 0) hoek += 2 * Math.PI;
									float dx = (float)(afstand * Math.Cos(hoek));
									float dy = (float)(afstand * Math.Sin(hoek));
									
									Punt punt1 = new Punt(r.Punt1.X + dx, r.Punt1.Y - dy, currentlayer);
									Punt punt2 = new Punt(r.Punt2.X + dx, r.Punt2.Y - dy, currentlayer);
									Rechte nieuwe_r = (Rechte)NieuweVorm;
									nieuwe_r.Punt1 = punt1;
									nieuwe_r.Punt2 = punt2;
									vormen.AddRange(new Vorm[] { punt1, punt2 });
									RegisterActie(new VormenToegevoegdActie(new Vorm[] { punt1, punt2, nieuwe_r }, this, Properties.Resources.DrawParallel));
									OnNieuweVormVoltooid();
									break;
							}
						}
						else
						{
							Vorm hoofdvorm = Ref_Vormen.First();
							Punt p = (Punt)Ref_Vormen.First(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt);
							switch (hoofdvorm.Vorm_Type)
							{
								case Vorm_type.Rechte:
									Rechte r = (Rechte)hoofdvorm;
									// Rechte berekenen
									float a, b, c;
									RaakBoog.Calc_Rechte(r.Punt1.Coordinaat, r.Punt2.Coordinaat, out a, out b, out c);

									// Loodlijn 1 berekenen
									float a_l1, b_l1, c_l1;
									RaakBoog.Calc_Loodlijn(r.Punt1.Coordinaat, r.Punt2.Coordinaat, r.Punt1.Coordinaat, out a_l1, out b_l1, out c_l1);

									// Loodlijn 2 berekenen
									float a_l2, b_l2, c_l2;
									RaakBoog.Calc_Loodlijn(r.Punt1.Coordinaat, r.Punt2.Coordinaat, r.Punt2.Coordinaat, out a_l2, out b_l2, out c_l2);

									// Evenwijdige berekenen
									float a_p, b_p, c_p;
									RaakBoog.Calc_Loodlijn(a_l1, b_l1, c_l1, p.Coordinaat, out a_p, out b_p, out c_p);

									PointF s1 = RaakBoog.Calc_Snijpunt(a_l1, b_l1, c_l1, a_p, b_p, c_p);
									PointF s2 = RaakBoog.Calc_Snijpunt(a_l2, b_l2, c_l2, a_p, b_p, c_p);

									Punt punt1 = new Punt(s1, currentlayer);
									Punt punt2 = new Punt(s2, currentlayer);
									Rechte nieuwe_r = (Rechte)NieuweVorm;
									nieuwe_r.Punt1 = punt1;
									nieuwe_r.Punt2 = punt2;
									vormen.AddRange(new Vorm[] { punt1, punt2 });
									RegisterActie(new VormenToegevoegdActie(new Vorm[] { punt1, punt2, nieuwe_r }, this, Properties.Resources.DrawParallel));
									OnNieuweVormVoltooid();
									p.Cursor = Punt.SpecialCursor.None;
									break;
								case Vorm_type.Cirkel:
									break;
								case Vorm_type.CirkelBoog:
									break;
								case Vorm_type.Ellips:
									break;
								case Vorm_type.Veelhoek:
									break;
								case Vorm_type.RaakBoog:
									break;
								default:
									break;
							}
						}
						break;
				}
			}
		}

		public event EventHandler EndMove;
		public bool DoMove()
		{
			if (actie == enActie.Move_Selectie)
			{
				Actie = enActie.Selecteren;
				foreach (Punt p in vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Select(T => (Punt)T).Where(T => T.Cursor == Punt.SpecialCursor.Move))
					p.Cursor = Punt.SpecialCursor.None;
				this.Invalidate();
				return false;
			}
			else
			{
				if (actie == enActie.Pan)
				{
					Cursor = Cursors.Default;
					if (EndPan != null) EndPan(this, EventArgs.Empty);
				}
				Actie = enActie.Move_Selectie;
				return true;
			}
		}
		private bool panning = false;
		public event EventHandler EndPan;
		public bool TogglePan()
		{
			if (actie == enActie.Pan)
			{
				Actie = enActie.Selecteren;
				Cursor = Cursors.Default;
			}
			else
			{
				if (actie == enActie.Move_Selectie)
					if (EndMove != null) EndMove(this, EventArgs.Empty);
				MemoryStream ms = new MemoryStream(Properties.Resources.grab);
				Cursor = new Cursor(ms);
				Actie = enActie.Pan;
			}
			return actie == enActie.Pan;
		}
		public void ToggleMeet()
		{
			if (actie == enActie.Meet)
			{
				Actie = enActie.Selecteren;
				Cursor = Cursors.Default;
			}
			else
			{
				//if(actie == enActie.Meet)
				Actie = enActie.Meet;
			}
		}


		private string filename = "";





		#region Broadcast Layers
		public delegate void ReportLayersEventHandler(object sender, LayersReportedEventArgs e);
		public event ReportLayersEventHandler ReportLayers;
		protected void OnReportLayers(LayersReportedEventArgs e)
		{
			if (ReportLayers != null)
				ReportLayers(this, e);
		}
		#endregion
		#region IDocument
		Rijndael GetRijndael()
		{
			Rijndael res = Rijndael.Create();
			res.Key = new byte[] { 88, 187, 246, 11, 61, 52, 255, 41, 121, 30, 19, 98, 20, 66, 106, 42, 247, 80, 138, 150, 120, 2, 132, 23, 236, 63, 153, 175, 205, 164, 5, 17 };
			res.IV = new byte[] { 188, 100, 182, 89, 55, 219, 48, 248, 164, 224, 157, 230, 106, 100, 85, 174 };
			return res;
		}

		public void OpenFile(string filename)
		{
			new Thread(new ThreadStart(delegate
			{
				this.filename = filename;
				vormen.Clear();
				layers.Clear();

				#region Prepare StreamReader
				FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
				CryptoStream cs = new CryptoStream(fs, GetRijndael().CreateDecryptor(), CryptoStreamMode.Read);
				StreamReader reader = new StreamReader(cs);
				#endregion
				#region ID's
				punt_id = Convert.ToInt32(reader.ReadLine());
				rechte_id = Convert.ToInt32(reader.ReadLine());
				#endregion
				#region Layers
				string lijn;
				while ((lijn = reader.ReadLine()) != "")
				{
					layers.Add(Layer.FromString(lijn));
				}
				currentlayer = layers.Where(T => T.Naam == "(default)").First();
				Invoke((MethodInvoker)delegate {
					OnReportLayers(new LayersReportedEventArgs(layers.Select(T => T.Naam).ToArray(), layers.IndexOf(currentlayer)));
				});
				#endregion
				#region Vormen
				while (!reader.EndOfStream)
				{
					string txt = reader.ReadLine();
					if (txt == "") continue;
					Vorm v = Vorm.FromString(txt, vormen.ToList(), layers);
					Invoke((MethodInvoker)delegate { vormen.Add(v); });
				}
				#endregion
				#region Close Stream
				reader.Close();
				reader = null;
				cs.Close();
				cs = null;
				fs.Close();
				fs = null;
				changed = false;
				#endregion
				Invalidate();
			})).Start();
		}
		public void Save(out DialogResult dr)
		{
			if (changed)
			{
				if (filename == "")
				{
					dr = SaveAs();
				}
				else
				{
					Write(filename);
					dr = DialogResult.None;
				}
			}
			else
			{
				dr = DialogResult.None;
			}
		}
		public DialogResult SaveAs()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = Properties.Resources.FilterSaveTekening;
			sfd.FileName = FindForm().Text + ".tek";
			sfd.InitialDirectory = Program.EnvironmentFolder;
			DialogResult dr = sfd.ShowDialog();
			if (dr == DialogResult.OK)
			{
				Write(sfd.FileName);
			}
			return dr;
		}
		private bool changed = false;
		public bool Changed
		{
			get { return changed; }
		}
		public void SaveBackup(string map)
		{
			Write(Path.Combine(map, Text + ".bkp.tek"));
		}

		public bool CanPrint
		{
			get
			{
				return vormen.Count != 0;
			}
		}
		public void PrintDirect()
		{
			if (CanPrint)
				pd.Print();
		}
		public void PrintWithDialog()
		{
			if (CanPrint)
			{
				PrintDialog pdl = new PrintDialog();
				pdl.Document = pd;
				pdl.UseEXDialog = true;
				if (pdl.ShowDialog() == DialogResult.OK)
					pd.Print();
			}
		}
		public void ShowPrintPreview()
		{
			NieuweVormCancel();
			if (CanPrint)
			{
				CoolPrintPreviewDialog dlg = new CoolPrintPreviewDialog();
				dlg.Document = pd;
				dlg.ShowDialog();
			}
		}
		public void ShowPageSetup()
		{
			PageSetupDialog dlg = new PageSetupDialog();
			dlg.EnableMetric = true; // problemen met margins oplossen !!
			dlg.Document = pd;
			dlg.PageSettings = pd.DefaultPageSettings;
			dlg.PrinterSettings = pd.PrinterSettings;
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				pd.DefaultPageSettings = dlg.PageSettings;
			}
		}

		Stack<Actie> UndoStack = new Stack<Actie>();
		Stack<Actie> RedoStack = new Stack<Actie>();
		public void Undo()
		{
			try
			{
				Actie a = UndoStack.Pop();
				RedoStack.Push(a);
				a.Undo();
				RequestUndoRedoStack();
			}
			catch (Exception)
			{ }
			this.Invalidate();
		}
		public void Redo()
		{
			try
			{
				Actie a = RedoStack.Pop();
				UndoStack.Push(a);
				a.Redo();
				RequestUndoRedoStack();
			}
			catch (Exception)
			{ }
			this.Invalidate();
		}
		public void RequestUndoRedoStack()
		{
			if (BroadcastUndoRedoStack != null)
				BroadcastUndoRedoStack(this, new UndoRedoStackEventArgs(UndoStack.ToArray(), RedoStack.ToArray()));
		}
		public bool CanUndo
		{
			get { return UndoStack.Count != 0; }
		}
		public bool CanRedo
		{
			get { return RedoStack.Count != 0; }
		}

		public void Cut()
		{
			if (CanCopy)
			{
				IEnumerable<Vorm> geselecteerd = vormen.Where(T => T.Geselecteerd);

				List<Vorm> ptn = new List<Vorm>();
				foreach (Punt[] p in geselecteerd.Select(T => T.Dep_Vormen))
					ptn.AddRange(p);
				ptn = ptn.Distinct().ToList();

				string txt = string.Join(Environment.NewLine, ptn.Union(geselecteerd).Select(T => T.ToString()).ToArray());
				Clipboard.SetData("Vorm_Array", txt);

				IEnumerable<Vorm> delete = geselecteerd.ToList();
				foreach (Vorm v in vormen.Except(geselecteerd).Where(T => T.Vorm_Hoofdtype != Vorm_hoofdtype.Punt))
					delete = delete.Except(v.Dep_Vormen);

				foreach (Vorm v in delete)
					vormen.Remove(v);

				RegisterActie(new VormenVerwijderdActie(delete.ToArray(), this, "Vormen knippen naar klembord"));
				Invalidate();
			}
		}
		public void Copy()
		{
			if (CanCopy)
			{
				IEnumerable<Vorm> geselecteerd = vormen.Where(T => T.Geselecteerd);

				List<Vorm> dep_pts = new List<Vorm>();
				foreach (Vorm[] v in geselecteerd.Select(T => T.Dep_Vormen))
					dep_pts.AddRange(v);
				dep_pts = dep_pts.Distinct().ToList();

				string txt = string.Join(Environment.NewLine, dep_pts.Union(geselecteerd).Select(T => T.ToString()).ToArray());
				Clipboard.SetData("Vorm_Array", txt);
			}
		}
		public void Paste()
		{
			if (CanPaste)
			{
				string[] v = (Clipboard.GetData("Vorm_Array") as string).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
				List<Vorm> vorm_list = new List<Vorm>();
				foreach (string str in v)
				{
					Vorm n = Vorm.FromString(str, vorm_list, layers);
					vorm_list.Add(n);
				}

				foreach (Punt p in vorm_list.Where(T => T.Vorm_Type == Vorm_type.Punt).Select(T => (Punt)T))
					p.set_id(punt_id++);
				foreach (Rechte r in vorm_list.Where(T => T.Vorm_Type == Vorm_type.Rechte).Select(T => (Rechte)T))
					r.set_id(rechte_id++);

				vormen.AddRange(vorm_list.ToArray());

				RegisterActie(new VormenToegevoegdActie(vorm_list.ToArray(), this, "Vormen van klembord geplakt"));

				foreach (Vorm v2 in vorm_list)
					v2.Geselecteerd = true;
				foreach (Vorm v2 in vormen.Except(vorm_list))
					v2.Geselecteerd = false;

				this.Invalidate();
			}
		}
		public bool CanCopy
		{
			get
			{
				return vormen.Where(T => T.Geselecteerd).ToArray().Length != 0;
			}
		}
		public bool CanPaste
		{
			get
			{
				return Clipboard.ContainsData("Vorm_Array");
			}
		}

		public void SelectAll()
		{
			foreach (Vorm v in vormen)
				v.Geselecteerd = v.Layer.Zichtbaar;
		}
		public void RemoveSelection()
		{
			IEnumerable<Vorm> Selectie = vormen.Where(T => T.Geselecteerd);
			IEnumerable<Punt> ptn = Selectie.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Select(T => (Punt)T);

			IEnumerable<Vorm> dep = vormen.Where(T => ptn.ToArray().Intersect(T.Dep_Vormen).Count() != 0);
			Vorm[] to_remove = Selectie.Union(dep).ToArray();
			if (to_remove.Length == 0) return;
			foreach (Vorm v in to_remove)
				vormen.Remove(v);

			RegisterActie(new VormenVerwijderdActie(to_remove, this, "Selectie verwijderen"));
			this.Invalidate();
		}
		
		public string CurrentFileName
		{
			get { return filename; }
		}
		public string PreferredFileName
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion
		#region Write
		private void Write(string filename)
		{
			string result = punt_id.ToString() + Environment.NewLine + rechte_id.ToString() + Environment.NewLine;

			result += string.Join(Environment.NewLine, layers.Select(T => T.ToString()).ToArray());
			result += Environment.NewLine + Environment.NewLine;

			IEnumerable<Punt> punten = vormen.Where(T => T.Vorm_Type == Vorm_type.Punt).Select(T => (Punt)T).Where(T => T.Ref_Punt == null);
			result += string.Join(Environment.NewLine, punten.Select(T => T.ToString()).ToArray());
			result += Environment.NewLine;

			IEnumerable<Rechte> rechten = vormen.Where(T => T.Vorm_Type == Vorm_type.Rechte).Select(T => (Rechte)T);
			result += string.Join(Environment.NewLine, rechten.Select(T => T.ToString()).ToArray());
			result += Environment.NewLine;
			
			result += string.Join(Environment.NewLine, vormen.Where(T => T.Vorm_Type != Vorm_type.Punt).Where(T => T.Vorm_Type != Vorm_type.Rechte).Select(T => T.ToString()).ToArray());

			if (!Directory.GetParent(filename).Exists) Directory.GetParent(filename).Create();
			
			FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
			CryptoStream cs = new CryptoStream(fs, GetRijndael().CreateEncryptor(), CryptoStreamMode.Write);
			StreamWriter writer = new StreamWriter(cs);
			writer.Write(result);

			#region Close Stream
			writer.Close();
			writer = null;
			cs.Close();
			cs = null;
			fs.Close();
			fs = null;
			#endregion

			changed = false;
		}
		#endregion


		public event EventHandler NieuweVormVoltooid;
		#region Zoom
		void Tekening_MouseWheel(object sender, MouseEventArgs e)
		{
			PointF muis = PointToClient(MousePosition);
			if (e.Delta > 0)
			{
				PointF to_shift = new PointF(muis.X - offset.X, muis.Y - offset.Y);
				offset = new PointF(offset.X - to_shift.X, offset.Y - to_shift.Y);
				Schaal *= 2;
			}
			else
			{
				offset = new PointF((offset.X + muis.X) / 2, (offset.Y + muis.Y) / 2);
				Schaal /= 2;
			}
		}
		#endregion
		void Tekening_Resize(object sender, EventArgs e)
		{
			this.Invalidate();
		}
		#region Vormen
		private Collectie<Vorm> vormen = new Collectie<Vorm>();
		public Collectie<Vorm> Vormen
		{
			get { return vormen; }
		}

		private List<Layer> layers = new List<Layer>();
		public List<Layer> Layers
		{
			get { return layers; }
		}
		private Layer Default = new Layer(true) { Naam = "(default)", Zichtbaar = true };
		private Layer currentlayer;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Layer CurrentLayer
		{
			get { return currentlayer; }
			set { currentlayer = value; }
		}

		public void RemoveLayer(Layer l)
		{
			if (ReferenceEquals(l, Default))
			{
				throw new InvalidOperationException("Kan standaardlayer niet verwijderen");
			}
			else
			{
				foreach (Vorm v in vormen.Where(T => ReferenceEquals(T.Layer, l)))
					v.Layer = Default;
				layers.Remove(l);

				if (ReferenceEquals(l, currentlayer))
					currentlayer = Default;
			}
		}
		#endregion
		#region Schaal
		private float schaal = 1.0f;
		public float Schaal
		{
			get { return schaal; }
			set
			{
				schaal = Convert.ToSingle(Math.Max(value, 1e-4));
				this.Invalidate();
			}
		}
		#endregion
		#region co_pt
		public Point co_pt(PointF co, float dpiX, float dpiY)
		{
			return new Point((int)((co.X * schaal) / 2.54 * dpiX + offset.X), (int)((co.Y * schaal) / 2.54 * dpiY + offset.Y));
		}
		public PointF pt_co(Point pt, float dpiX, float dpiY)
		{
			// pt =  co.X * schaal + offset.X
			// co = (pt.X - offset.X) / schaal

			// pt = -co.Y * schaal + offset.Y
			// co = (pt.Y - offset.Y) / -schaal
			return new PointF((float)(((pt.X - offset.X) * 2.54 / dpiX) / schaal), (float)(((pt.Y - offset.Y) * 2.54 / dpiY) / schaal));
		}
		#endregion
		#region offset
		private PointF offset = new PointF();
		public PointF Offset
		{
			get { return offset; }
			set { offset = value; }
		}
		#endregion

		#region Afdruk-commando
		int huidige_pagina_x, huidige_pagina_y;
		
		void pd_BeginPrint(object sender, PrintEventArgs e)
		{
			huidige_pagina_x = 0;
			huidige_pagina_y = 0;
		}
		void pd_PrintPage(object sender, PrintPageEventArgs e)
		{
			int schaal = 1;
			e.Graphics.PageUnit = GraphicsUnit.Millimeter;
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			// e.MarginBounds = honderdsten van een inch
			float p_w = pd.DefaultPageSettings.Bounds.Width - pd.DefaultPageSettings.Margins.Left - pd.DefaultPageSettings.Margins.Right; // 1/100 inch
			float p_h = pd.DefaultPageSettings.Bounds.Height - pd.DefaultPageSettings.Margins.Top - pd.DefaultPageSettings.Margins.Bottom;

			e.Graphics.Transform.Translate(
				-huidige_pagina_x * p_w,
				-huidige_pagina_y * p_h);

			p_w = p_w / 100 * 2.54f; // cm
			p_h = p_h / 100 * 2.54f; // cm

			IEnumerable<Punt> punten = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Select(T => (Punt)T);

			float xmin = vormen.Min(T => T.Bounds(e.Graphics).Left);
			float xmax = vormen.Max(T => T.Bounds(e.Graphics).Right);
			float ymin = vormen.Min(T => T.Bounds(e.Graphics).Top);
			float ymax = vormen.Max(T => T.Bounds(e.Graphics).Bottom);

			int aantal_x = ((xmax - xmin) * schaal / p_w).Round(true);
			int aantal_y = ((ymax - ymin) * schaal / p_h).Round(true);

			RectangleF voll_tek = new RectangleF(xmin, ymin, xmax - xmin, ymax - ymin);

			IEnumerable<int> niveaus = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Vlak).Where(T => T.Layer.Zichtbaar).Select(T => T.Niveau).Distinct().OrderBy(T => T);
			foreach (int n in niveaus)
				foreach (Vorm vorm in vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Vlak).Where(T => T.Layer.Zichtbaar).Where(T => T.Niveau == n))
					vorm.Draw(e.Graphics, 1, new RectangleF(
						voll_tek.Left + huidige_pagina_x * p_w,
						voll_tek.Top + huidige_pagina_y * p_h,
						p_w,
						p_h
						));
			niveaus = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Lijn).Where(T => T.Layer.Zichtbaar).Select(T => T.Niveau).Distinct().OrderBy(T => T);
			foreach (int n in niveaus)
				foreach (Vorm vorm in vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Lijn).Where(T => T.Layer.Zichtbaar).Where(T => T.Niveau == n))
					vorm.Draw(e.Graphics, 1, new RectangleF(
						voll_tek.Left + huidige_pagina_x * p_w,
						voll_tek.Top + huidige_pagina_y * p_h,
						p_w,
						p_h
						));
			niveaus = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Tekst).Where(T => T.Layer.Zichtbaar).Select(T => T.Niveau).Distinct().OrderBy(T => T);
			foreach (int n in niveaus)
				foreach (Vorm vorm in vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Tekst))
					vorm.Draw(e.Graphics, 1, new RectangleF(
						voll_tek.Left + huidige_pagina_x * p_w,
						voll_tek.Top + huidige_pagina_y * p_h,
						p_w,
						p_h
						));
			niveaus = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Where(T => T.Layer.Zichtbaar).Select(T => T.Niveau).Distinct().OrderBy(T => T);
			foreach (int n in niveaus)
				foreach (Vorm vorm in vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt))
					vorm.Draw(e.Graphics, 1, new RectangleF(
						voll_tek.Left + huidige_pagina_x * p_w,
						voll_tek.Top + huidige_pagina_y * p_h,
						p_w,
						p_h
						));

			float l = pd.DefaultPageSettings.Margins.Left / 100f * 25.4f; // mm
			float r = pd.DefaultPageSettings.Margins.Right / 100f * 25.4f; // mm
			float t = pd.DefaultPageSettings.Margins.Top / 100f * 25.4f; // mm
			float b = pd.DefaultPageSettings.Margins.Bottom / 100f * 25.4f; // mm
			p_w = 10 * p_w; // mm
			p_h = 10 * p_h; // mm
			float hm_left = pd.DefaultPageSettings.HardMarginX / 100f * 25.4f;
			float hm_top = pd.DefaultPageSettings.HardMarginY / 100f * 25.4f;

			e.Graphics.FillRectangle(Brushes.Red, new RectangleF(-l, -t, l, pd.DefaultPageSettings.Bounds.Height)); // links
			e.Graphics.FillRectangle(Brushes.Green, new RectangleF(pd.DefaultPageSettings.Bounds.Width - l, -t, r, pd.DefaultPageSettings.Bounds.Height));//rechts
			e.Graphics.FillRectangle(Brushes.Yellow, new RectangleF(-l, -t, pd.DefaultPageSettings.Bounds.Width, t)); //bovenaan
			e.Graphics.FillRectangle(Brushes.Blue, new RectangleF(-l, pd.DefaultPageSettings.Bounds.Height - t, pd.DefaultPageSettings.Bounds.Width, b)); //onderaan
																																						  /**/
																																						  /*e.Graphics.FillRectangle(Brushes.Red, new RectangleF(- hmx-l, -hmy-20, hmx+l, 297)); // links
																																						  e.Graphics.FillRectangle(Brushes.Green, new RectangleF(210-2*(20+hmx),-hmy -20, 20 + hmx, 297));//rechts
																																						  e.Graphics.FillRectangle(Brushes.Yellow, new RectangleF(-hmx-20, -hmy-20, 210, 20 + hmy)); //bovenaan
																																						  e.Graphics.FillRectangle(Brushes.Blue, new RectangleF(-hmy-20, 297-20-hmy, 210, 20 + hmy)); //onderaan*/

			e.Graphics.FillRectangle(Brushes.Green, -23, -23, 15, 15);

			e.HasMorePages = true;
			if (++huidige_pagina_x == aantal_x)
			{
				huidige_pagina_x = 0;
				if (++huidige_pagina_y == aantal_y)
				{
					e.HasMorePages = false;
				}
			}
		}

		#endregion

		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Up:
				case Keys.Down:
				case Keys.Left:
				case Keys.Right:
					return true;
				default:
					return base.IsInputKey(keyData);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics gr = e.Graphics;
			gr.SmoothingMode = SmoothingMode.AntiAlias;
			gr.Clear(Color.White);

			IEnumerable<Vorm> Vlakken = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Vlak);
			IEnumerable<Vorm> Lijnen = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Lijn);
			IEnumerable<Vorm> Punten = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt);
			IEnumerable<Vorm> Teksten = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Tekst);

			#region Bepalen muispositie
			PointF p = new PointF();
			if (!DesignMode)
			{
				p = pt_co(PointToClient(MousePosition), gr.DpiX, gr.DpiY);
				if (NieuweVorm != null)
				{
					Punt pt = GetPuntBelowCursor();
					if (pt != null)
						p = pt.Coordinaat;
					else if (((ModifierKeys & Keys.Control) == Keys.Control) & actie == enActie.Nieuw_punt)
						p = new PointF(Convert.ToSingle(Math.Round(p.X / 0.5f)) * 0.5f,
									   Convert.ToSingle(Math.Round(p.Y / 0.5f)) * 0.5f); // afronden tot op 0.5
				}
			}
			#endregion

			IEnumerable<int> niveaus = vormen.Select(T => T.Niveau).Distinct().OrderBy(T => T);

			foreach (int n in niveaus)
				foreach (Vorm vorm in Vlakken.Where(T => T.Niveau == n).Where(T => T.Layer.Zichtbaar))
					if (ReferenceEquals(vorm, NieuweVorm))
						vorm.Draw(this, gr, p, Ref_Vormen.ToArray());
					else
						vorm.Draw(this, gr, false, true);

			if (selectiekader)
				DrawSelectieKader(gr);

			foreach (int n in niveaus)
			{
				foreach (Vorm vorm in Vlakken.Where(T => T.Niveau == n).Where(T => T.Layer.Zichtbaar))
					if (ReferenceEquals(vorm, NieuweVorm))
						vorm.Draw(this, gr, p, Ref_Vormen.ToArray());
					else
						vorm.Draw(this, gr, false, false);
				foreach (Vorm vorm in Lijnen.Where(T => T.Niveau == n).Where(T => T.Layer.Zichtbaar))
					if (ReferenceEquals(vorm, NieuweVorm))
						vorm.Draw(this, gr, p, Ref_Vormen.ToArray());
					else
						vorm.Draw(this, gr, false, true);
				foreach (Vorm vorm in Teksten.Where(T => T.Niveau == n).Where(T => T.Layer.Zichtbaar))
					if (ReferenceEquals(vorm, NieuweVorm))
						vorm.Draw(this, gr, p, Ref_Vormen.ToArray());
					else
						vorm.Draw(this, gr, false, true);
				foreach (Vorm vorm in Punten.Where(T => T.Niveau == n).Where(T => T.Layer.Zichtbaar))
					if (ReferenceEquals(vorm, NieuweVorm))
						vorm.Draw(this, gr, p, Ref_Vormen.ToArray());
					else
						vorm.Draw(this, gr, false, true);
			}

			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Far;
			sf.LineAlignment = StringAlignment.Far;
			gr.DrawString(schaal.ToString("0.0000000"), this.Font, Brushes.Black, new Rectangle(0, 0, txtSchaal.Right, txtSchaal.Bottom), sf);

			if (is_hover) gr.DrawString("{" + p.X.ToString("0.000") + " , " + p.Y.ToString("0.000") + "}", this.Font, Brushes.Black, new PointF(3, Height - this.Font.Height));
		}

		private List<Vorm> Ref_Vormen = new List<Vorm>();

		private void DrawSelectieKader(Graphics gr)
		{
			PointF s = SelectieStart_pt;
			PointF e = SelectieEind_pt;

			float x = Math.Min(s.X, e.X);
			float y = Math.Min(s.Y, e.Y);
			float w = Math.Abs(s.X - e.X);
			float h = Math.Abs(s.Y - e.Y);

			gr.FillRectangle(SystemBrushes.Highlight, x, y, w, h);
			gr.DrawRectangle(SystemPens.Highlight, x, y, w, h);
		}

		public bool GetHitTest(Vorm vorm, Point pt, float dpiX, float dpiY)
		{
			if (vorm.Vorm_Type == Vorm_type.Punt)
			{
				Punt punt = (Punt)vorm;
				Point p = co_pt(punt.Coordinaat, dpiX, dpiY);
				return (((p.X - 3 <= pt.X) & (pt.X <= p.X + 3)) &
						((p.Y - 3 <= pt.Y) & (pt.Y <= p.Y + 3)));
			}
			else
			{
				Bitmap bmp = new Bitmap(this.Width, this.Height);
				Graphics gr = Graphics.FromImage(bmp);
				vorm.Draw(this, gr, true, true);
				Color kleur = bmp.GetPixel(pt.X, pt.Y);
				return (kleur.A != 0);
			}
		}

		#region Actie
		private enActie actie = enActie.Selecteren;
		public enActie Actie
		{
			get { return actie; }
			set
			{
				actie = value;
				switch (actie)
				{
					case enActie.Selecteren:
						Cursor = Cursors.Default;
						break;
					case enActie.Nieuw_punt:
						MemoryStream ms = new MemoryStream(Properties.Resources.CursorNone);
						Cursor c = new Cursor(ms);
						this.Cursor = c;
						break;
				}
			}
		}

		#endregion
		#region Muishandelingen
		private bool is_mousedown = false;
		private PointF MouseDownCo;
		private PointF MouseDownPt;
		private void Tekening_MouseUp(object sender, MouseEventArgs e)
		{
			Graphics gr = CreateGraphics();
			is_mousedown = false;
			if (actie == enActie.Move_Selectie)
			{
				if (GrabbingPunt != null)
				{
					if (EndMove != null)
						EndMove(this, EventArgs.Empty);
					actie = enActie.Selecteren;
					foreach (Punt p in vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Select(T => (Punt)T).Where(T => T.Cursor == Punt.SpecialCursor.Grab))
						p.Cursor = Punt.SpecialCursor.None;
					GrabbingPunt.Cursor = Punt.SpecialCursor.None;

					if (HoverPunt != null)
					{
						HoverPunt.Cursor = Punt.SpecialCursor.None;
						GrabbingPunt.Ref_Punt = HoverPunt;
						vormen.Remove(GrabbingPunt);
					}
					foreach (Punt p2 in vormen.Where(T => T.Geselecteerd).Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Select(T => (Punt)T))
						p2.Veranderd -= Moving_PointMoved;
					RegisterActie(move_actie);
					move_actie = null;
				}
			}
			else if (actie == enActie.Pan)
			{
				MemoryStream ms = new MemoryStream(Properties.Resources.grab);
				Cursor = new Cursor(ms);
			}
			else if (actie == enActie.Move_Maatlijn)
			{
				down_maatlijn = null;
				actie = enActie.Selecteren;
			}
			else if (actie == enActie.Selecteren)
			{
				selectiekader = false;
				RectangleF sel = new RectangleF(
					Math.Min(SelectieStart_pt.X, SelectieEind_pt.X),
					Math.Min(SelectieStart_pt.Y, SelectieEind_pt.Y),
					Math.Abs(SelectieStart_pt.X - SelectieEind_pt.X),
					Math.Abs(SelectieStart_pt.Y - SelectieEind_pt.Y));
				if (Math.Abs(sel.Width) < 2 & Math.Abs(sel.Height) < 2) return;

				Region r_sel = new Region(sel);
				foreach (Vorm v in vormen)
				{
					Region blob = v.GetRegion(this);
					if (blob.GetRegionScans(new Matrix()).Length != 0)
						v.Geselecteerd = true;
					else if (!((ModifierKeys & Keys.Shift) == Keys.Shift) & !((ModifierKeys & Keys.Control) == Keys.Control))
						v.Geselecteerd = false;
				}
			}
			Invalidate();
		}
		PropertyBundleChangedActie move_actie;
		void Tekening_MouseDown(object sender, MouseEventArgs e)
		{
			Graphics gr = CreateGraphics();
			MouseDownCo = pt_co(e.Location, gr.DpiX, gr.DpiY);
			MouseDownPt = e.Location;
			is_mousedown = true;
			if (actie == enActie.Pan)
			{
				MemoryStream ms = new MemoryStream(Properties.Resources.grabbing);
				Cursor = new Cursor(ms);
			}
			else if (actie == enActie.Move_Selectie)
			{
				if ((GrabbingPunt = GetPuntBelowCursor()) != null)
				{
					Vorm[] ptn = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Where(T => T.Geselecteerd).ToArray();
					move_actie = new PropertyBundleChangedActie(ptn, string.Format(Properties.Resources.MovePoints_fm, ptn.Length));
					foreach (Punt p2 in vormen.Where(T => T.Geselecteerd).Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Select(T => (Punt)T))
						p2.Veranderd += Moving_PointMoved;
				}
			}
			else if (actie == enActie.Selecteren)
			{
				#region Cursor op maatlijn
				foreach (Maatlijn ml in vormen.Where(T => T.Vorm_Type == Vorm_type.Maatlijn).Select(T => (Maatlijn)T))
				{
					if (ml.LigtOp(MouseDownCo, this.CreateGraphics(), schaal))
					{
						down_maatlijn = ml;
						this.Cursor = ml.getCursor();
						actie = enActie.Move_Maatlijn;
						return;
					}
				}
				#endregion
				Cursor = Cursors.Default;
				SelectieStart_pt = e.Location;
				SelectieEind_pt = SelectieStart_pt;
			}
		}

		private void Moving_PointMoved(object sender, VormVeranderdEventArgs e)
		{
			if (e != VormVeranderdEventArgs.Empty)
			{
				move_actie.Items.Add(e.Actie);
			}
		}

		Punt GrabbingPunt, HoverPunt;
		Maatlijn down_maatlijn;
		bool is_hover = false;
		bool moving = false;
		void Tekening_MouseMove(object sender, MouseEventArgs e)
		{
			is_hover = true;
			Graphics gr2 = this.CreateGraphics();
			PointF p = pt_co(e.Location, gr2.DpiX, gr2.DpiY);
			switch (actie)
			{
				case enActie.Selecteren:
					if (is_mousedown)
					{
						SelectieEind_pt = e.Location;
						if (!selectiekader)
							if (Math.Abs(MouseDownPt.X - e.Location.X) > 10 | Math.Abs(MouseDownPt.Y - e.Location.Y) > 10)
								selectiekader = true;
					}
					else
					{
						#region Hover maatlijn ?
						foreach (Maatlijn ml in vormen.Where(T => T.Vorm_Type == Vorm_type.Maatlijn).Select(T => (Maatlijn)T))
						{
							if (ml.LigtOp(p, gr2, schaal))
							{
								this.Cursor = ml.getCursor();
								return;
							}
						}
						#endregion
						Cursor = Cursors.Default;
					}
					break;
				case enActie.Move_Maatlijn:
					float a, b, c;
					RaakBoog.Calc_Rechte(down_maatlijn.Punt1.Coordinaat, down_maatlijn.Punt2.Coordinaat, out a, out b, out c);
					float a_l, b_l, c_l;
					RaakBoog.Calc_Loodlijn(down_maatlijn.Punt1.Coordinaat, down_maatlijn.Punt2.Coordinaat, p, out a_l, out b_l, out c_l);
					PointF s = RaakBoog.Calc_Snijpunt(a, b, c, a_l, b_l, c_l);
					double d = Ellips.Pyt(p, s);
					d = d / 2.54f * gr2.DpiX;
					down_maatlijn.Offset = Convert.ToSingle(d);
					break;
				case enActie.Nieuw_punt:
					if ((ModifierKeys & Keys.Control) == Keys.Control)
					{
						float nwk = 0.5f;
						((Punt)NieuweVorm).Coordinaat = new PointF((float)Math.Round(p.X / nwk) * nwk, (float)Math.Round(p.Y / nwk) * nwk);
					}
					else
						((Punt)NieuweVorm).Coordinaat = pt_co(e.Location, gr2.DpiX, gr2.DpiY);
					break;
				case enActie.Move_Selectie:
					if (is_mousedown)
					{
						if (!moving & (GrabbingPunt != null))
						{
							moving = true;
							PointF newco = pt_co(e.Location, gr2.DpiX, gr2.DpiY);
							foreach (Punt pt in vormen.Where(T => !T.Geselecteerd).Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Select(T => (Punt)T))
							{
								if (GetHitTest(pt, e.Location, gr2.DpiX, gr2.DpiY))
								{
									HoverPunt = pt;
									pt.Cursor = Punt.SpecialCursor.Grab;
									newco = pt.Coordinaat;
									goto next;
								}
								else
								{
									pt.Cursor = Punt.SpecialCursor.None;
								}
							}
							HoverPunt = null;
						next:
							foreach (Punt pt in vormen.Where(T => T.Geselecteerd).Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Select(T => (Punt)T))
							{
								pt.Coordinaat = new PointF(pt.Coordinaat.X + newco.X - MouseDownCo.X, pt.Coordinaat.Y + newco.Y - MouseDownCo.Y);
							}
							MouseDownCo = newco;
							moving = false;
						}
					}
					else
					{
						foreach (Vorm vorm in vormen)
							if (vorm.Vorm_Hoofdtype == Vorm_hoofdtype.Punt)
								((Punt)vorm).Cursor = Punt.SpecialCursor.None;

						foreach (Vorm vorm in vormen)
							if (vorm.Vorm_Hoofdtype == Vorm_hoofdtype.Punt & vorm.Geselecteerd)
								if (GetHitTest(vorm, this.PointToClient(Control.MousePosition), gr2.DpiX, gr2.DpiY))
								{
									((Punt)vorm).Cursor = Punt.SpecialCursor.Move;
									MemoryStream ms = new MemoryStream(Properties.Resources.CursorNone);
									Cursor = new Cursor(ms);
									this.Invalidate();
									return;
								}
						Cursor = Cursors.Default;
					}
					break;
				case enActie.Pan:
					if (is_mousedown)
					{
						if (!panning)
						{
							panning = true;
							PointF newco = e.Location;
							Offset = new PointF(Offset.X + newco.X - MouseDownPt.X, Offset.Y + newco.Y - MouseDownPt.Y);
							MouseDownPt = newco;
							panning = false;
						}
					}
					break;
				case enActie.Nieuwe_rechte:
				case enActie.Nieuwe_kromme:
				case enActie.Nieuwe_cirkelboog:
				case enActie.Nieuwe_raakboog:
				case enActie.Nieuwe_cirkel2:
				case enActie.Nieuwe_cirkel3:
				case enActie.Nieuwe_cirkelsector:
				case enActie.Nieuw_cirkelsegment:
				case enActie.Nieuwe_ellips:
				case enActie.Nieuwe_gesloten_kromme:
				case enActie.Nieuwe_veelhoek:
				case enActie.Nieuwe_Tekst:
				case enActie.Nieuwe_Maatlijn:
					foreach (Vorm vorm in vormen)
						if (vorm.Vorm_Hoofdtype == Vorm_hoofdtype.Punt)
							((Punt)vorm).Cursor = Punt.SpecialCursor.None;

					Punt p1 = GetPuntBelowCursor();
					if (p1 != null)
					{
						p1.Cursor = Punt.SpecialCursor.Grab;
						MemoryStream ms = new MemoryStream(Properties.Resources.CursorNone);
						Cursor = new Cursor(ms);
						Invalidate();
						return;
					}
					else
					{
						Cursor = Cursors.Default;
					}
					break;
				case enActie.Nieuwe_parallelle:
					if (Ref_Vormen.Count(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Lijn | T.Vorm_Hoofdtype == Vorm_hoofdtype.Vlak) != 0)
					{
						foreach (Vorm vorm in vormen)
							if (vorm.Vorm_Hoofdtype == Vorm_hoofdtype.Punt)
								((Punt)vorm).Cursor = Punt.SpecialCursor.None;
						Ref_Vormen.RemoveAll(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt);

						foreach (Vorm vorm in vormen)
							if (vorm.Vorm_Hoofdtype == Vorm_hoofdtype.Punt)
								if (GetHitTest(vorm, PointToClient(MousePosition), gr2.DpiX, gr2.DpiY))
								{
									((Punt)vorm).Cursor = Punt.SpecialCursor.Grab;
									Invalidate();
									Ref_Vormen.Add(vorm);
									return;
								}
					}
					break;
			}
			Invalidate();
		}
		#endregion
		#region VormBelowCursor
		Vorm GetVormBelowCursor()
		{
			Graphics gr = this.CreateGraphics();
			IEnumerable<Vorm> vormen_rev = vormen.Reverse().Where(T => T.Layer.Zichtbaar);
			IEnumerable<int> niveaus = vormen.Select(T => T.Niveau).Distinct().OrderBy(T => -T);
			foreach (int n in niveaus)
				for (int soort = 0; soort < 4; soort++)
					foreach (Vorm vorm in vormen_rev.Where(T => T.Vorm_Hoofdtype == (Vorm_hoofdtype)soort).Where(T => T.Niveau == n))
						if (GetHitTest(vorm, PointToClient(MousePosition), gr.DpiX, gr.DpiY))
							return vorm;
			return null;
		}
		Punt GetPuntBelowCursor()
		{
			Graphics gr = this.CreateGraphics();
			IEnumerable<Punt> punten_rev = vormen.Reverse().Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Select(T => (Punt)T).Where(T => T.Layer.Zichtbaar);
			IEnumerable<int> niveaus = vormen.Where(T=>T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Select(T => T.Niveau).Distinct().OrderBy(T => -T);
			foreach (int n in niveaus)
				foreach (Punt punt in punten_rev.Where(T => T.Niveau == n))
					if (GetHitTest(punt, PointToClient(MousePosition), gr.DpiX, gr.DpiY))
						return punt;
			return null;
		}
		#endregion
		#region Nieuwe Vormen
		Vorm NieuweVorm;
		int punt_id = 0, rechte_id = 0;
		public void NieuwPunt()
		{
			Actie = enActie.Nieuw_punt;
			Punt p = new Punt(punt_id++, currentlayer);
			NieuweVorm = p;
			p.CanRaiseVeranderdEvent = false;
			Vormen.Add(p);
		}
		public void NieuweTekst()
		{
			Actie = enActie.Nieuwe_Tekst;
			Tekst t = new Tekst(currentlayer);
			NieuweVorm = t;
			vormen.Add(t);
			//t.CanRaiseVeranderdEvent = false;
		}
		public void NieuweRechte()
		{
			Actie = enActie.Nieuwe_rechte;
			Rechte r = new Rechte(rechte_id++, currentlayer);
			NieuweVorm = r;
			vormen.Add(r);
		}
		public void NieuweMaatlijn()
		{
			Actie = enActie.Nieuwe_Maatlijn;
			Maatlijn ml = new Maatlijn(currentlayer);
			NieuweVorm = ml;
			vormen.Add(ml);
		}
		public void NieuweKromme()
		{
			Actie = enActie.Nieuwe_kromme;
			Kromme k = new Kromme(currentlayer);
			NieuweVorm = k;
			vormen.Add(k);
		}
		public void NieuweCirkelBoog()
		{
			Actie = enActie.Nieuwe_cirkelboog;
			Cirkelboog b = new Cirkelboog(currentlayer);
			NieuweVorm = b;
			vormen.Add(b);
		}
		public void NieuweRaakBoog()
		{
			Actie = enActie.Nieuwe_raakboog;
			RaakBoog b = new RaakBoog(currentlayer);
			NieuweVorm = b;
			vormen.Add(b);
		}
		public void NieuweCirkel2()
		{
			Actie = enActie.Nieuwe_cirkel2;
			Cirkel c = new Cirkel(currentlayer);
			NieuweVorm = c;
			vormen.Add(c);
		}
		public void NieuweCirkel3()
		{
			Actie = enActie.Nieuwe_cirkel3;
			Cirkel c = new Cirkel(currentlayer);
			NieuweVorm = c;
			vormen.Add(c);
		}
		public void NieuweCirkelSector()
		{
			Actie = enActie.Nieuwe_cirkelsector;
			CirkelSector cs = new CirkelSector(currentlayer);
			NieuweVorm = cs;
			vormen.Add(cs);
		}
		public void NieuwCirkelSegment()
		{
			Actie = enActie.Nieuw_cirkelsegment;
			CirkelSegment csg = new CirkelSegment(currentlayer);
			NieuweVorm = csg;
			vormen.Add(csg);
		}
		public void NieuweEllips()
		{
			Actie = enActie.Nieuwe_ellips;
			Ellips el = new Ellips(currentlayer);
			NieuweVorm = el;
			vormen.Add(el);
		}
		public void NieuweGeslotenKromme()
		{
			Actie = enActie.Nieuwe_gesloten_kromme;
			GeslotenKromme gk = new GeslotenKromme(currentlayer);
			NieuweVorm = gk;
			vormen.Add(gk);
		}
		public void NieuweVeelhoek()
		{
			Actie = enActie.Nieuwe_veelhoek;
			Veelhoek v = new Veelhoek(currentlayer);
			NieuweVorm = v;
			vormen.Add(v);
		}
		public void NieuweEvenwijdige()
		{
			Actie = enActie.Nieuwe_parallelle;
			MemoryStream ms = new MemoryStream(Properties.Resources.CurParallel);
			Cursor = new Cursor(ms); 
		}
		public void NieuweVormSubmit()
		{
			Actie = enActie.Selecteren;
			string vorm_naam = NieuweVorm.GetType().Name.ToLower();
			RegisterActie(new VormenToegevoegdActie(new Vorm[] { NieuweVorm }, this, string.Format(Properties.Resources.DrawNewShape, vorm_naam)));
			NieuweVorm.CanRaiseVeranderdEvent = true;
			NieuweVorm = null;
			this.Invalidate();
		}
		public void NieuweVormCancel()
		{
			if (NieuweVorm != null)
			{
				vormen.Remove(NieuweVorm);
				NieuweVorm = null;
			}
			Actie = enActie.Selecteren;
			this.Invalidate();
		}
		private void Tekening_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.Modifiers & Keys.Control) == Keys.Control)
			{
				if (e.KeyCode == Keys.Z & CanUndo) Undo();
				if (e.KeyCode == Keys.Y & CanRedo) Redo();
			}
			else if (e.KeyCode == Keys.Escape | e.KeyCode == Keys.Space)
			{
				switch (actie)
				{
					case enActie.Nieuw_punt:
					case enActie.Nieuwe_rechte:
					case enActie.Nieuwe_cirkelboog:
					case enActie.Nieuwe_raakboog:
					case enActie.Nieuwe_cirkel2:
					case enActie.Nieuwe_cirkel3:
					case enActie.Nieuwe_cirkelsector:
					case enActie.Nieuw_cirkelsegment:
					case enActie.Nieuwe_ellips:
					case enActie.Nieuwe_Maatlijn:
						NieuweVormCancel();
						OnNieuweVormVoltooid();
						break;
					case enActie.Nieuwe_gesloten_kromme:
					case enActie.Nieuwe_kromme:
					case enActie.Nieuwe_veelhoek:
						NieuweVormSubmit();
						OnNieuweVormVoltooid();
						break;
					default:
						Actie = enActie.Selecteren;
						break;
				}
			}
			else if (e.KeyCode == Keys.Left)
				Offset = new PointF(offset.X + 50, offset.Y);
			else if (e.KeyCode == Keys.Right)
				Offset = new PointF(offset.X - 50, offset.Y);
			else if (e.KeyCode == Keys.Up)
				Offset = new PointF(offset.X, offset.Y + 50);
			else if (e.KeyCode == Keys.Down)
				Offset = new PointF(offset.X, offset.Y - 50);
			this.Invalidate();
		}
		#endregion
		#region VraagCoordinaat
		private bool vraagCoordinaat = true;
		public bool VraagCoordinaat
		{
			get { return vraagCoordinaat; }
			set { vraagCoordinaat = value; }
		}
		#endregion

		public void Activate()
		{
			throw new NotImplementedException();
		}

		public void Close()
		{
			throw new NotImplementedException();
		}

		public void CopyFont()
		{
			var ptn = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Punt).Select(T => (Punt)T).ToArray();
			if (ptn.Length != 0) ptn.First().CopyFont();

			var ln = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Lijn).Select(T => (Lijn)T).ToArray();
			if (ln.Length != 0) ln.First().CopyFont();

			var vln = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Vlak).Select(T => (Vlak)T).ToArray();
			if (vln.Length != 0) vln.First().CopyFont();

			var txn = vormen.Where(T => T.Vorm_Hoofdtype == Vorm_hoofdtype.Tekst).Select(T => (Tekst)T).ToArray();
			if (txn.Length != 0) txn.First().CopyFont();
		}

		public void PasteFont()
		{
			var lijst = vormen.Where(T => T.Geselecteerd).ToArray();
			if (lijst.Length == 0) return;

			PropertyBundleChangedActie result = new PropertyBundleChangedActie(lijst, "Opmaak plakken");
			Actie tmp;
			foreach (Vorm v in vormen.Where(T => T.Geselecteerd))
			{
				v.PasteFont(out tmp);
				result.Items.Add(tmp);
			}
			RegisterActie(result);
			Invalidate();
		}


		#region Selectiekader
		private bool selectiekader = false;
		private PointF SelectieStart_pt = new PointF();
		private PointF SelectieEind_pt = new PointF();
		#endregion
	}
	public enum enActie
	{
		Selecteren,
		Move_Selectie,
		Pan,
		Meet,
		Nieuw_punt,
		Nieuwe_rechte,
		Nieuwe_kromme,
		Nieuwe_cirkelboog,
		Nieuwe_cirkel2,
		Nieuwe_cirkel3,
		Nieuwe_cirkelsector,
		Nieuwe_ellips,
		Nieuwe_gesloten_kromme,
		Nieuwe_veelhoek,
		Nieuwe_Tekst,
		Nieuwe_raakboog,
		Nieuw_cirkelsegment,
		Nieuwe_Maatlijn,
		Move_Maatlijn,
		Nieuwe_parallelle
	}
}
