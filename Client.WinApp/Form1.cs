using System.Diagnostics;

using Microsoft.Extensions.Configuration;

namespace Client.WinApp
{
    public partial class Form1 : Form, IDisposable
    {
        private Process process;

        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            var res = Program.Configuration
                             .GetSection("cloudflared:access")
                             .Get<List<string>>().ToArray();
            comboBox1.Items.AddRange(res);
        }

        private async void StartProcess_Click(object sender, EventArgs e)
        {
            string strCmdText = comboBox1.Text;

            if (string.IsNullOrEmpty(strCmdText))
            {
                MessageBox.Show("Cannot start service from cloudflared");
                return;
            }

            process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/C \"{strCmdText}\"", // Correctly pass the command
                RedirectStandardOutput = true, // Capture output (optional)
                RedirectStandardError = true,  // Capture errors (optional)
                UseShellExecute = false, // Required for output redirection
                CreateNoWindow = true // Ensures no visible window
            };

            process.StartInfo = startInfo;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;

            process.Start();

            process.BeginErrorReadLine();

            ((Button)sender).Enabled = false;

            btnStop.Enabled = true;

        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Invoke(() =>
            {
                textBox1.AppendText(e.Data);
                textBox1.AppendText(Environment.NewLine);
            });
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            KillProcesses();

            btnStop.Enabled = false;

            button1.Enabled = true;

            if (process is null)
            {
                return;
            }

            process.ErrorDataReceived -= Process_ErrorDataReceived;
        }

        private void KillProcesses()
        {
            foreach (var process in Process.GetProcessesByName("cloudflared"))
            {
                process.Kill();
            }
        }

       
    }

}
