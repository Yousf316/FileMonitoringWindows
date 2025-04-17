===========================
File Monitoring Windows Service
===========================
---------------------------
📌 Project Overview:
---------------------------
This Windows Service monitors a specific folder for new files. When a file is detected, it:
- Renames the file with a GUID
- Moves it to a destination folder
- Logs the actions taken
- Reads source/destination folders from App.config for flexibility

---------------------------
🛠 Build Instructions:
---------------------------
1. Open the solution in Visual Studio.
2. Set the build configuration to "Release".
3. Go to Build > Build Solution or press Ctrl + Shift + B.
4. Navigate to the output: /bin/Release/

---------------------------
🚀 Deployment Instructions:
---------------------------
1. Open "Developer Command Prompt for Visual Studio" as Administrator.
2. Navigate to the Release folder:

   cd path\to\your\bin\Release

3. To install the service:

   InstallUtil FileMonitoringWindows.exe

4. To start the service:

   net start FileMonitoringService

5. To stop the service:

   net stop FileMonitoringService

6. To uninstall the service:

   InstallUtil /u FileMonitoringWindows.exe

---------------------------
📁 Configuration:
---------------------------
The folders used by the service can be set in App.config:

<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<appSettings>
		<add key="SourceFolderPath" value="C:\WatchFolder" />
		<add key="DestinationFolderPath" value="C:\ProcessedFolder" />
		<add key="LogFilePath" value="C:\Logs\FileMonitorService.log" />
	</appSettings>
</configuration>

---------------------------
📄 Log Output Example:
---------------------------
[2025-04-17 12:00:01] Service started.
[2025-04-17 12:00:05] File detected: report.txt
[2025-04-17 12:00:06] File moved to archive folder as: 5a3e2b9d.txt


