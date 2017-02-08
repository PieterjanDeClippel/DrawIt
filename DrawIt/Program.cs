using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Hosting;
using System.Reflection;
using DrawIt.Properties;
using System.Drawing;
using System.IO.Pipes;

namespace DrawIt
{
	static class Program
	{
		static Mutex mutex = new Mutex(true, guid());
		static string guid()
		{
			// http://stackoverflow.com/questions/502303/how-do-i-programmatically-get-the-guid-of-an-application-in-net2-0
			Assembly assembly = Assembly.GetExecutingAssembly();
			var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
			return attribute.Value;
		}
		
		public static string GetFileName()
		{
			ActivationArguments a = AppDomain.CurrentDomain.SetupInformation.ActivationArguments;
			string[] args = a == null ? null : a.ActivationData;
			return args == null ? "" : args[0];
		}
		public static string EnvironmentFolder
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DrawIt");
			}
		}

		public static PuntFont PuntFont = PuntFont.Default;
		public static Pen LijnFont = Pens.Black;
		public static VlakFont VlakFont = VlakFont.Default;
		public static TekstFont TekstFont = TekstFont.Default;


		static NamedPipeServerStream pipeServer;
		static IAsyncResult result;
		[STAThread]
		static void Main()
		{
			if (mutex.WaitOne(TimeSpan.Zero, true))
			{
				#region standaard
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				#endregion
				#region Culture instellen
				string cult = CultureInfo.CurrentCulture.Name;
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(cult);
				Thread.CurrentThread.CurrentCulture = new CultureInfo(cult);
				#endregion
				#region Fetch unhandled exceptions
				if (!Debugger.IsAttached)
				{
					Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
					Application.ThreadException += (sender, e) =>
					{
						frmError frm = new frmError();
						frm.ShowDialog(e.Exception);
					};
				}
				#endregion
				Hoofdscherm MainForm = new Hoofdscherm();
				#region PipeServer
				pipeServer = new NamedPipeServerStream("Drawit_pipeserver", PipeDirection.InOut, 15, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
				result = pipeServer.BeginWaitForConnection(new AsyncCallback(PipeConnectionCallBack), new object[] { pipeServer, MainForm });
				#endregion
				#region Cancel asynchronous operation when form closes
				ManualResetEvent signal = new ManualResetEvent(false);
				new Thread(new ParameterizedThreadStart(MonitorFormClose)).Start(signal);
				MainForm.FormClosed += delegate { signal.Set(); };
				#endregion
				#region Environment-folder
				if (!Directory.Exists(EnvironmentFolder))
					Directory.CreateDirectory(EnvironmentFolder);
				#endregion
				#region MainForm
				Application.Run(MainForm);
				mutex.ReleaseMutex();
				#endregion
			}
			else
			{
				try
				{
					Thread.Sleep(100);
					#region Send Filename
					NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "Drawit_pipeserver", PipeDirection.InOut, PipeOptions.None, System.Security.Principal.TokenImpersonationLevel.Impersonation);
					pipeClient.Connect(300);
					StreamString ss = new StreamString(pipeClient);
					ss.WriteString(GetFileName());
					pipeClient.Close();
					#endregion
				}
				catch (Exception)
				{ }
			}
		}

		static void MonitorFormClose(object sig)
		{
			ManualResetEvent signal = (ManualResetEvent)sig;
			signal.WaitOne();
			if (result.IsCompleted)
				pipeServer.EndWaitForConnection(result);
		}
		
		private static void PipeConnectionCallBack(IAsyncResult ar)
		{
			Hoofdscherm main_form = (Hoofdscherm)((object[])ar.AsyncState)[1];
			pipeServer = new NamedPipeServerStream("Drawit_pipeserver", PipeDirection.InOut, 15, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
			result = pipeServer.BeginWaitForConnection(new AsyncCallback(PipeConnectionCallBack), new object[] { pipeServer, main_form });
			NamedPipeServerStream pipe_server = (NamedPipeServerStream)((object[])ar.AsyncState)[0];
			pipe_server.EndWaitForConnection(ar);
			StreamString ss = new StreamString(pipe_server);
			string filename = ss.ReadString();
			main_form.Invoke((Action)delegate { main_form.OpenFile(filename); });
			pipe_server.Close();
			pipe_server.Dispose();
		}
	}
}
