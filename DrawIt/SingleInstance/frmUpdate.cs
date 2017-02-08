using DrawIt.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace DrawIt
{
	public partial class frmUpdate : Form
	{
		public frmUpdate()
		{
			InitializeComponent();
			try
			{
				depl = ApplicationDeployment.CurrentDeployment;
				depl.CheckForUpdateProgressChanged += depl_CheckForUpdateProgressChanged;
				depl.CheckForUpdateCompleted += CurrentDeployment_CheckForUpdateCompleted;
				depl.UpdateProgressChanged += depl_UpdateProgressChanged;
				depl.UpdateCompleted += depl_UpdateCompleted;
			}
			catch(Exception) { }
		}
		
		/// <summary>
		/// True if v1 > v2
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns></returns>
		bool IsLaterVersion(string v1, string v2)
		{
			for(int i = 0; i < 4; i++)
				if(Convert.ToInt32(v1.Split('.')[i]) > Convert.ToInt32(v2.Split('.')[i]))
				{
					return true;
				}
				else if(Convert.ToInt32(v1.Split('.')[i]) < Convert.ToInt32(v2.Split('.')[i]))
				{
					return false;
				}
			return false;
		}

		ApplicationDeployment depl;

		private void frmUpdate_Load(object sender, EventArgs e)
		{
            if (depl != null)
				lblHuidigeVersie.Text = Resources.CurrentVersion + ":" + Environment.NewLine + depl.CurrentVersion.ToString();
			ZoekAsync();
		}

		private void btnOpnieuw_Click(object sender, EventArgs e)
		{
			ZoekAsync();
		}

		void ZoekAsync()
		{
			try
			{
				if(depl == null) return;
				btnUpdate.Enabled = false;
				btnOpnieuw.Enabled = false;
				prev_state = "";
				lblStatus.Text = Resources.Checking + "...";
				depl.CheckForUpdateAsync();
			}
			catch(Exception ex)
			{
				lblStatus.Text = Resources.SearchFailed + ": " + ex.Message;
			}
		}

		string prev_state = "";
		void depl_CheckForUpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
		{
			try
			{
				progressBar1.Value = e.ProgressPercentage;
				lblProgressPercent.Text = e.ProgressPercentage.ToString() + "%";
				lblProgressBytes.Text = e.BytesCompleted.ToString() + " / " + e.BytesTotal.ToString() + " bytes";
				if(e.State.ToString() != prev_state)
				{
					txtStatus.AppendText(Resources.Checking + ": " + e.State.ToString() + "\r\n");
					prev_state = e.State.ToString();
				}
			}
			catch(Exception ex)
			{
				txtStatus.AppendText(Resources.ErrorOccured + ": " + ex.Message);
			}
		}

		void CurrentDeployment_CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs e)
		{
			try
			{
				btnOpnieuw.Enabled = true;
				if(e.UpdateAvailable)
				{
					lblStatus.Text = Resources.UpdateAvailable;
					lblNieuweVersie.Text = Resources.NewVersion + Environment.NewLine + e.AvailableVersion.ToString();

					btnUpdate.Enabled = true;


					string url = "http://pieterjan.pro/Projecten/csharp/DrawIt/UpdInf/";
					HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
					HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
					StreamReader reader = new StreamReader(resp.GetResponseStream());
					string html = reader.ReadToEnd();
					Regex rgx = new Regex("<a href=\".*\">((?<file>.*)\\.txt)</a>");
					Match m = rgx.Match(html);

					string current = depl.CurrentVersion.ToString();
					while(m.Success)
					{
						string filename = m.Groups["file"].Value;
						if(IsLaterVersion(filename.Replace('_','.'), current))
						{
							HttpWebRequest req_info = (HttpWebRequest)WebRequest.Create(url + filename + ".txt");
							HttpWebResponse resp_info = (HttpWebResponse)req_info.GetResponse();
							StreamReader reader_info = new StreamReader(resp_info.GetResponseStream());
							string info = reader_info.ReadToEnd();

							if(info != "")
								textBox1.AppendText(Resources.version + " " + filename.Replace("_", ".") + ": " + info + Environment.NewLine);
						}
						m = m.NextMatch();
					}

				}
				else
				{
					lblStatus.Text = Resources.AppUpToDate;
				}
			}
			catch(Exception ex)
			{
				lblStatus.Text = Resources.SearchFailed + ex.InnerException.Message;
			}
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			btnUpdate.Enabled = false;
			btnOpnieuw.Enabled = false;
			prev_state = "";
			lblStatus.Text = Resources.updating + "...";
			depl.UpdateAsync();
		}

		void depl_UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
		{
			progressBar1.Value = e.ProgressPercentage;
			lblProgressPercent.Text = e.ProgressPercentage.ToString() + "%";
			lblProgressBytes.Text = e.BytesCompleted.ToString() + " / " + e.BytesTotal.ToString() + " bytes";
			if(e.State.ToString() != prev_state)
			{
				prev_state = e.State.ToString();
				txtStatus.AppendText(Resources.updating + ": " + e.State.ToString() + "\r\n");
			}
		}

		void depl_UpdateCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				lblStatus.Text = Resources.UpdateCancelled;
				txtStatus.AppendText(Resources.UpdateCancelled + "\r\n");
			}
			else if (e.Error != null)
			{
				lblStatus.Text = Resources.ErrorOccured;
				txtStatus.AppendText(Resources.ErrorOccured + ": " + e.Error.Message);
			}
			else
			{
				lblStatus.Text = Resources.AppUpToDate;
				// Add reference -> C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\WindowsBase.dll
				// OF Add reference -> Assemblies -> WindowsBase
				var disp = System.Windows.Threading.Dispatcher.CurrentDispatcher;
				disp.BeginInvoke((Action)delegate
				{ 
					if (MessageBox.Show(Resources.UpdateCompleteRestart, Resources.Restart, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						//Program.ReleaseMutex();
						//Thread.Sleep(50);
						Application.Restart();
						disp.BeginInvokeShutdown(System.Windows.Threading.DispatcherPriority.Normal);
					}
				});
			}
		}

		private void btnSluiten_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
