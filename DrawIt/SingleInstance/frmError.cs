using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawIt.Properties;
using System.IO;
using System.Net;
using System.Web;

namespace DrawIt
{
	public partial class frmError : Form
	{
		public frmError()
		{
			InitializeComponent();
		}

		public DialogResult ShowDialog(Exception error)
		{
			lblTitle.Text = string.Format(Resources.ReportExceptionString,	error.GetType().Name, error.Message);
			txtCallStack.Text = error.StackTrace;
			DialogResult dr = base.ShowDialog();
			return dr;
		}
		public DialogResult ShowDialog(IWin32Window owner, Exception error)
		{
			lblTitle.Text = string.Format(Resources.ReportExceptionString, error.GetType().Name, error.Message);
			txtCallStack.Text = error.StackTrace;
			DialogResult dr = base.ShowDialog(owner);
			return dr;
		}
		public new DialogResult ShowDialog()
		{
			throw new ApplicationException("Error form cannot be shown without Exception parameter");
		}
		public new DialogResult ShowDialog(IWin32Window owner)
		{
			throw new ApplicationException("Error form cannot be shown without Exception parameter");
		}

		string sendFileName = "";
		private void btnSelectFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if (sendFileName != "") ofd.FileName = sendFileName;
			else ofd.InitialDirectory = Program.EnvironmentFolder;
			ofd.Filter = Resources.FilterOpenTekening;
			if(ofd.ShowDialog() == DialogResult.OK)
			{
				sendFileName = ofd.FileName;
				btnSelectFile.Text = Path.GetFileName(ofd.FileName);
			}
		}

		public static void upload(string host, string user, string pass, string remoteFile, string localFile)
		{
			FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + remoteFile);
			/* Log in to the FTP Server with the User Name and Password Provided */
			ftpRequest.Credentials = new NetworkCredential(user, pass);
			/* When in doubt, use these options */
			ftpRequest.UseBinary = true;
			ftpRequest.UsePassive = true;
			ftpRequest.KeepAlive = true;
			/* Specify the Type of FTP Request */
			ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
			/* Establish Return Communication with the FTP Server */
			Stream ftpStream = ftpRequest.GetRequestStream();
			/* Open a File Stream to Read the File for Upload */
			FileStream localFileStream = new FileStream(localFile, FileMode.Create);
			/* Buffer for the Downloaded Data */
			int bufferSize = 2048;
			byte[] byteBuffer = new byte[bufferSize];
			int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
			/* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
			try
			{
				while (bytesSent != 0)
				{
					ftpStream.Write(byteBuffer, 0, bytesSent);
					bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
				}
			}
			catch (Exception ex) { Console.WriteLine(ex.ToString()); }
			/* Resource Cleanup */
			localFileStream.Close();
			ftpStream.Close();
			ftpRequest = null;
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://pieterjan.pro/Projecten/csharp/DrawIt/UserExceptions/");
			string tekst = "callstack=";
			tekst += HttpUtility.UrlEncodeToBytes(txtCallStack.Text);
			tekst += "&message=";
			tekst += HttpUtility.UrlEncodeToBytes(txtPersonalMessage.Text);
			//tekst+= "&"
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
