using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;

namespace FileMonitoringWindows
{
    public partial class FileMonitoringService : ServiceBase
    {
        private FileSystemWatcher _watcher;
        private string _sourcePath;
        private string _destinationPath;
        private string _logPath;

        public FileMonitoringService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _sourcePath = ConfigurationManager.AppSettings["SourceFolderPath"];
            _destinationPath = ConfigurationManager.AppSettings["DestinationFolderPath"];
            _logPath = ConfigurationManager.AppSettings["LogFilePath"];

            Directory.CreateDirectory(_sourcePath);
            Directory.CreateDirectory(_destinationPath);
            Directory.CreateDirectory(Path.GetDirectoryName(_logPath));

            _watcher = new FileSystemWatcher(_sourcePath);
            _watcher.Created += OnFileCreated;
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;

            WriteLog("Service started. Monitoring: " + _sourcePath);

            string[] existingFiles = Directory.GetFiles(_sourcePath);
            foreach (var file in Directory.GetFiles(_sourcePath))
            {
                ProcessFile(file, "(Startup) ");
            }

        }

        protected override void OnStop()
        {
            _watcher.Dispose();
            WriteLog("Service stopped.");
        }

        public void StartDebugMode() => OnStart(null);
        public void StopDebugMode() => OnStop();


        private void ProcessFile(string filePath, string source = "")
        {
            try
            {
                string fileName = Path.GetFileName(filePath);
                string extension = Path.GetExtension(filePath);
                string newName = Guid.NewGuid().ToString() + extension;
                string newPath = Path.Combine(_destinationPath, newName);

                WaitForFile(filePath);
                File.Copy(filePath, newPath, true);

                if (File.Exists(newPath))
                {
                    File.Delete(filePath);
                    WriteLog($"{source}File '{fileName}' copied to '{newName}' and deleted from source.");
                }
                else
                {
                    WriteLog($"{source}Error: Failed to verify file copy for '{fileName}'");
                }
            }
            catch (Exception ex)
            {
                WriteLog($"{source}Error: " + ex.Message);
            }
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            ProcessFile(e.FullPath);
        }

        private void WaitForFile(string path)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        if (fs.Length > 0)
                            break;
                    }
                }
                catch
                {
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        private void WriteLog(string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

            if (Environment.UserInteractive)
                Console.WriteLine(logEntry);

            try
            {
                File.AppendAllText(_logPath, logEntry + Environment.NewLine);
            }
            catch
            {
                // Fails silently if logging fails
            }
        }
    }
}
