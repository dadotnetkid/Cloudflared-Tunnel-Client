string strCmdText = "cloudflared access tcp --hostname ccc-rdp.interworx.app --url 127.0.0.1:3388";

var process = new System.Diagnostics.Process();
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

process.Start();

process.BeginErrorReadLine();
