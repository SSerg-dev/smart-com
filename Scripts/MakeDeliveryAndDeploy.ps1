#Settings
$ErrorActionPreference = "Stop"
$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath
$CurrentDate = Get-Date
$logfile = "$CurrentPath\log_deploy.txt"

Write-Host "Script starting at $CurrentDate" 
"Script starting at $CurrentDate" | Out-File $logfile -Append

if($args) {
	$key1 = $($args[0].ToString())
	$key2 = $($args[1].ToString())
	$key3 = $($args[2].ToString())
	$key4 = $($args[3].ToString())
	$key5 = $($args[4].ToString())
	$key6 = $($args[5].ToString())
	$key7 = $($args[6].ToString())
	$key8 = $($args[7].ToString())
}

if($key1 -eq "true") { $deploy = $true }
Else { $deploy = $false }

if($key2 -eq "Development") { 
	$destFolderMap = "\\192.168.10.92\D$\TPM_dev\DeployScript\Source"
	$deployScript = "D:\TPM_dev\DeployScript\DevDeploy.ps1"
	$Server = "192.168.10.92"
	$username = "smartcom\DeployUser"
	$pass = "Pass123$"
	$cred = New-Object System.Management.Automation.PsCredential($username, (ConvertTo-SecureString "Pass123$" -AsPlainText -Force))
	$isFTP = $false
	$notFTP = $true
	$osaka = $false

	$SourceBranch = "$/TPM/Development/DMP/Source" # 14.01.021
}

if($key2 -eq "Development2") { 
	$destFolderMap = "\\192.168.10.92\D$\TPM_dev2\DeployScript\Source"
	$deployScript = "D:\TPM_dev2\DeployScript\DevDeploy.ps1"
	$Server = "192.168.10.92"
	$username = "smartcom\DeployUser"
	$pass = "Pass123$"
	$cred = New-Object System.Management.Automation.PsCredential($username, (ConvertTo-SecureString "Pass123$" -AsPlainText -Force))
	$isFTP = $false
	$notFTP = $true
	$osaka = $false

	$SourceBranch = "$/TPM/Development/AddTI/Source"
}

if($key2 -eq "Test") { 
	$destFolderMap = "\\192.168.10.92\D$\TPM\DeployScript\Source"
	$deployScript = "D:\TPM\DeployScript\StageDeploy.ps1"
	$Server = "192.168.10.92"
	$username = "smartcom\DeployUser"
	$pass = "Pass123$"
	$cred = New-Object System.Management.Automation.PsCredential($username, (ConvertTo-SecureString "Pass123$" -AsPlainText -Force))
	$isFTP = $false
	$notFTP = $true
	$osaka = $false

	$SourceBranch = "$/TPM/ReleaseCandidate/Source"	# 3.07.019
}

if($key2 -eq "Demo") { 
	$destFolderMap = "\\192.168.10.92\D$\TPM_demo\DeployScript\Source"
	$deployScript = "D:\TPM_demo\DeployScript\DemoDeploy.ps1"
	$Server = "192.168.10.92"
	$username = "smartcom\DeployUser"
	$pass = "Pass123$"
	$cred = New-Object System.Management.Automation.PsCredential($username, (ConvertTo-SecureString "Pass123$" -AsPlainText -Force))
	$isFTP = $false
	$notFTP = $true
	$osaka = $false

	$SourceBranch = "$/TPM/Development/Demo/Source"
}

<#13.05.019
if($key2 -eq "Stage") { 
	$destFolderMap = "\\192.168.10.92\C$\TPM\DeployScript\Source"
	$deployScript = "C:\TPM\DeployScript\StageDeploy.ps1"
	$Server = "192.168.10.92"
	$username = "smartcom\DeployUser"
	$pass = "Pass123$"
	$cred = New-Object System.Management.Automation.PsCredential($username, (ConvertTo-SecureString "Pass123$" -AsPlainText -Force))
	$isFTP = $false
	$notFTP = $true
	$osaka = $false
}
#>

<# 3.07.019
if ($key3) {
	$SourceBranch = "$/TPM/Development/Phase1/Source"
	echo "yyyyyyy SourceBranch = $key3"
	#$SourceBranch = $key3
}
#>

if ($key6) {
		#09.01.2020 $Uri = "http://osaka:8080/tfs/dotnet_project_collection/TPM/_apis/build/builds/$key6/timeline?api-version=2.0"
		$Uri = "http://192.168.10.2:8080/tfs/dotnet_project_collection/TPM/_apis/build/builds/$key6/timeline?api-version=2.0"
		$releaseresponse = Invoke-RestMethod -Method Get -UseDefaultCredentials -ContentType application/json -Uri $Uri
		$date = [DateTime]$releaseresponse.records.startTime[6]
		$DateStart = $date.ToString("yyyy-MM-dd HH:mm:ss")
	} 


#Functions
Function CopyThroughFtp($FTPCredential, $Item) {
	$continueRelease = $false 
	$count = 0
	while ($count -lt 10 -and $continueRelease -ne $true) {
		try {
			Write-Host "Connecting to FTP" 
			"Connecting to FTP " | Out-File $logfile -Append 
			Set-FTPConnection -Credentials $FTPCredential -Server $ftp -Session MySession -UsePassive -UseBinary > $null 
			$ftpServer = Get-FTPConnection -Session MySession 
			Write-Host "Connecting successful"
			"Connecting successful" | Out-File $logfile -Append
			$PrevArchives = Get-FTPChildItem -Session $ftpServer -Path Source -Filter "*.zip"
			if($PrevArchives) {
				Foreach($PrevArchive in $PrevArchives){
					Write-Host "Moving previous archive"
					$oldPath = "Source\" + $PrevArchive.name
					$newPath = "old\" + $PrevArchive.name
					Rename-FTPItem -Session $ftpServer -Path $oldPath -NewName $newPath
				}
			} 
			Write-Host "Copying to FTP"
			$archives | Add-FTPItem -Session $ftpServer -Path Source -Overwrite > $null
			$continueRelease = $true 
		}
		catch {
			Write-Host "Retrying to connect to FTP Server" 
			$count++
		}
	}
	if($count -ge 10) {
		throw "Couldn't connect to FTP!!!"
	}
}

#Copy to server
if ($notFTP) {
	Write-Host "Connecting to Server" 
	"Connecting to Server" | Out-File $logfile -Append 
	$count = 0
	$connected = $false 
	while ($count -lt 10 -and $connected -ne $true) {
		try {
			$net = new-object -ComObject WScript.Network
			$net.MapNetworkDrive("W:", $destFolderMap, $false, $username, $pass)
			$connected = $true
		}
		catch {
			if (Test-Path "W:") { 
				net use W: /delete > $null
				Write-Host "Drive unmapped!" 
			}
			Write-Host "Retrying to map drive from Server"
			"Retrying to map drive from Server" | Out-File $logfile -Append 
			Write-Host "$_.Exception.Message"
			$_.Exception.Message | Out-File $logfile -Append 
			$count++
		}
	}
	if ($connected) {
		if (!(Test-Path W:\old)) {
			New-Item W:\old -ItemType Directory
		}
		Get-Item W:\*.zip | Move-Item -Destination W:\old -Force
		Write-Host "Copying to Server" 
		"Copying to Server" | Out-File $logfile -Append 
		Get-Item $CurrentPath\*.zip | Copy-Item -Destination "W:\"

		Write-Host "Disconnecting from Server" 
		"Disconnecting from Server" | Out-File $logfile -Append 
		net use /delete /y W: > $null
	}
	else { 
		Write-Host "Couldn't connect server! Archives Copying failed!" 
		"Couldn't connect server! Archives Copying failed!" | Out-File $logfile -Append 
		Write-Host "$_.Exception.Message"
		$_.Exception.Message | Out-File $logfile -Append 
		exit 1
	}
}

#Copy to MARS server through ftp
if ($isFTP) {
	Import-Module PSFTP
	$archives = Get-Item $CurrentPath\*.zip
	CopyThroughFtp -FTPCredential $FTPCred -Item $archives
}


#remove archives from temp folder on JavaBuildServer
#Remove-Item "$CurrentPath\..\..\..\..\..\TPM\*.zip" -Recurse -Force

if($deploy) {
	try {
		Write-Host "Deploy" 
		"Deploy" | Out-File $logfile -Append 
		Invoke-Command $Server -Scriptblock { param ( [string] $file ) & $file } -ArgumentList $deployScript -Credential $cred | Out-File $logfile -Append
		$continueRelease = $true
	}
	catch {
		Write-Host "Deploy failed!" 
		"Deploy failed!" | Out-File $logfile -Append 
		Write-Host "$_.Exception.Message"
		$_.Exception.Message | Out-File $logfile -Append 
		exit 1 
	}
}

if($continueRelease -and $deploy) {
	Write-Host "Generating Test Plan"
	Write-Host "$DateStart"
	
	<#05.04.021
	if ($osaka) {
		D:\TestPlanGenerator\TestPlanGenerator.exe  $SourceBranch $key4 $DateStart $key7 $key5 $key2
		if($LASTEXITCODE -ne 0) { 
			$continueRelease = $false
			Write-Host "TestPlanGenerator failed!"
			exit 1
		}
	}
	else { 
		if(Test-Path "D:\TestPlanGenerator\") {

			# BuildSourcesDirectory передаём через файл (другого решения пока не нашёл) и скармливаем Тестплану
			if (Test-Path "$CurrentPath\BuildSourcesDirectory.dat") {	# нужно для проектов из Git-репозитория
				$buildSourcesDirectory = Get-Content "$CurrentPath\BuildSourcesDirectory.dat"
			}
			else {
				Write-Host "BuildSourcesDirectory.dat does not exist!"
			}

			#27.11.020 D:\TestPlanGenerator\TestPlanGenerator.exe $SourceBranch $key4 $DateStart $key7 $key5 $key2 $key8 $buildSourcesDirectory  
			D:\TestPlanGenerator\TestPlanGenerator.exe $SourceBranch $key4 $DateStart $key7 $key5 $key2 $key8 # $buildSourcesDirectory  
		}
		else {
			C:\temp\TestPlanGenerator\TestPlanGenerator.exe  $SourceBranch $key4 $DateStart $key7 $key5 $key2 $key8
		}
		if($LASTEXITCODE -ne 0) { 
			$continueRelease = $false
			Write-Host "TestPlanGenerator failed!"
			exit 1
		}
	}
	05.04.021#>
	
	If(Test-Path "D:\TestPlanGeneratorGit_without_features\") {		#05.04.021

		Logging "INFO: Used TestPlanGeneratorGit_without_features"

		# BuildSourcesDirectory передаём через файл (другого решения пока не нашёл) и скармливаем Тестплану
		if (Test-Path "$CurrentPath\BuildSourcesDirectory.dat") {
			$buildSourcesDirectory = Get-Content "$CurrentPath\BuildSourcesDirectory.dat"
		}
		else {
			Logging "BuildSourcesDirectory.dat does not exist!"
		}
		D:\TestPlanGeneratorGit_without_features\TestPlanGenerator.exe  $SourceBranch $key4 $DateStart $key7 $key5 $key2 $key8 $buildSourcesDirectory 
	}
	else {

		Logging "INFO: Used TestPlanGeneratorGit"

		C:\temp\TestPlanGeneratorGit\TestPlanGenerator.exe  $SourceBranch $key4 $DateStart $key7 $key5 $key2 $key8 $key6
	}
	If($LASTEXITCODE -ne 0) { 
		Logging "TestPlanGenerator failed!"
		exit 1
	}
	
} 

$FinishDate = Get-Date
Write-Host "Script finished at $FinishDate" 
"Script finished at $FinishDate" | Out-File $logfile -Append