
param (
    [string] $environment,	
    [string] $serverDB,	
    [string] $DB,	
    [string] $userDb,	
    [string] $userPassword	
)

$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath
$ErrorActionPreference = "Stop"

# logs
$logDate = Get-Date -format "yyyyMM"
$logDir = "$CurrentPath\Logs\$logDate"
If(!(Test-Path $logDir)) {
    New-Item $logDir -ItemType Directory > $null   
}
$logfile = "$logDir\ApplyDbScripts.log"

Function Logging($message) {
	Write-Host $message
	$message | Out-File $logfile -Append
}

# настройки
Logging -message "Script starting at $(Get-Date)"

$source = "$CurrentPath\.."
$dest = "$CurrentPath\..\..\b"

$MigrationFolderName = "Migrations_Persist"
$SQLInitializeFolderName = "SQLInitialize"


<# for test
if (!($serverDB)) {		# set default
	$serverDB = "STSVH-SQL01\MSSQLSERVER2017_yu,1435"
	$DB = "TPM_Jupiter2Azure_Main_Test_Current_yu"
	$userDb = "userdb_yu"
	# $userPassword =
}
#>

<#	отключаю /10.09.020
if (!($serverDB)) {		# set default
	$serverDB = "smartcomdevsqlsrv.database.windows.net"
	$DB = "jupiterdevsqldb"
	$userDb = "jupiter_user"
	$userPassword = "Kz3CL7XGXbCD"
    # <add name="DatabaseContext" connectionString="Server=smartcomdevsqlsrv.database.windows.net; Database=jupiterdevsqldb; Persist Security Info=True;User ID=jupiter_user;Password=Kz3CL7XGXbCD;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" />
}
#>

#keys
$doMigrations = $true
#06.10.020 $isScript = $true
$isScript = $false
$changeDB = $false	# отключаю смену строки подключения к БД - берём всё из конфигов /10.09.020


# на случай ошибки применения миграций и скриптов:	
$smtpServer = "stsvh-bak01.smartcom.local"
$smtpUser = "tfs-alerts@smartcom.support"
$cred = New-Object System.Management.Automation.PsCredential($smtpUser, (ConvertTo-SecureString "tGyDQnBYNt" -AsPlainText -Force))
$recipientAddress = "9dd79c81.smartcom.software@emea.teams.ms"


<#
if ($args) {

	foreach ($arg in $args) { 
	
		switch ($arg) {
			"" {
			}
		} # switch
	
	} # foreach
} # if ($args)
#>

try {

	#Functions
	Function StopService($serviceName) {
	}

	Function StartService($serviceName) {
	}


	if (!($environment)) {	# если среда не определена
	
		throw "Need the parameter value 'environment'!"
	}
	

	#prepare Migrations files
	try {
		$folders = Get-Childitem $source -Directory
		foreach ($folder in $folders) {
			$path = $folder.FullName
			if (Test-path "$path\Migrations") {
				Logging -message "        Preparing migrations files for $folder"
				
				if (!(Test-path "$dest\$MigrationFolderName")) {
					md $dest\$MigrationFolderName > $null
				}

				Copy-Item $path\Migrations -Recurse -Destination $dest\$MigrationFolderName -Force
				Copy-Item $path\bin\Release\*.* -Destination $dest\$MigrationFolderName -Force
				Copy-Item $source\packages\EntityFramework.6.1.3\tools\migrate.exe -Destination $dest\$MigrationFolderName -Force
			}
		}
	}
	catch {
		Logging -message "$($_.Exception.Message)"
		exit 1
	}
	
	
	#prepare SQLInitialize files
	try {
		$folders = Get-Childitem $source -Directory
		Logging -message "        Preparing folder with SQLInitialize scripts"
		foreach ($folder in $folders) {
			$path = $folder.FullName
			if (Test-path "$path\SQLInitialize") {
			
				if (!(Test-Path "$dest\$SQLInitializeFolderName")) {
					Logging -message "            Getting SQLInitialize scripts ($path)"
					md $dest\$SQLInitializeFolderName > $null
				}
				else {
					Logging -message "            Getting SQLInitialize scripts (add + $path)"
				}
				Copy-Item $path\SQLInitialize\*.sql -Destination $dest\$SQLInitializeFolderName -Force
			}
		}
	}
	catch {
		Logging -message "$($_.Exception.Message)"
		throw "$($_.Exception.Message)"
	}


	try {
	
		if ( $changeDB ) { # смена строки подключения к БД
		
			Logging -message "The DB connection string is changing ..."
		
			$configDB_Server = "Server=$serverDB;"
			$configDB_DataSource = "Data Source=$serverDB;"
			$configDB_Database = "Database=$DB;"
			$configDB_InitialCatalog = "Initial Catalog=$DB;"
			$configDB_UserID = "User ID=$userDb;"
			$configDB_Password = "Password=$userPassword;"
		
			$pathToConfig = @()
			$pathToConfig += "$dest\$MigrationFolderName\Module.Persist.TPM.dll.config"
			$pathToConfig += "$CurrentPath\DbDelivery\config.xml"

			# меняем строку подключения к БД:
			foreach ( $file in $pathToConfig) {
			
				(Get-Content $file) -replace "Server=(.*?);",$configDB_Server | Set-Content $file -Encoding UTF8
				(Get-Content $file) -replace "Data Source=(.*?);",$configDB_DataSource | Set-Content $file -Encoding UTF8
				(Get-Content $file) -replace "Database=(.*?);",$configDB_Database | Set-Content $file -Encoding UTF8
				(Get-Content $file) -replace "Initial Catalog=(.*?);",$configDB_InitialCatalog | Set-Content $file -Encoding UTF8

				if ( $configDB_UserID ) {
					(Get-Content $file) -replace "User ID=(.*?);",$configDB_UserID | Set-Content $file -Encoding UTF8
				}
				if ( $configDB_Password ) {
					(Get-Content $file) -replace "Password=(.*?);",$configDB_Password | Set-Content $file -Encoding UTF8
				}
			}
		}
		
		
		# меняем путь к папке со скриптами (на сервере сборки он всегда разный):
		Logging "Changing the path to the scripts folder ..."
		
		$pathToConfig = @()
		$pathToConfig += "$CurrentPath\DbDelivery\config.xml"
		foreach ( $file in $pathToConfig) {
		
			(Get-Content $file) -replace "`"RootDirectory`" value=(.*?) />", "`"RootDirectory`" value=`"$($(Get-Item $dest).FullName)`" />" | Set-Content $file -Encoding UTF8
			(Get-Content $file) -replace "`"SourceDirectories`" value=(.*?) />", "`"SourceDirectories`" value=`"$SQLInitializeFolderName`" />" | Set-Content $file -Encoding UTF8
			
			# <CommandSettingModel name="RootDirectory" value="D:\TPM_dev\DeployScript\Source" />
			# <CommandSettingModel name="SourceDirectories" value="SQLInitialize_Release*" />
		}
	}
	catch {
		Logging -message "$($_.Exception.Message)"
		throw "Problem changing connection string to the database!"
	}


	#apply migrations
	If($doMigrations) {

		try {
			Logging -message "	Apply Migrations"
			
			$targetMigrationsPersist = "$dest\$MigrationFolderName"
			if ($targetMigrationsPersist -and (Test-Path "$targetMigrationsPersist")) {

				cd $targetMigrationsPersist
				& ./migrate.exe Module.Persist.TPM.dll /startupConfigurationFile=Module.Persist.TPM.dll.config > "$CurrentPath\apply_script_migrations_log.txt"
				
				If($LASTEXITCODE -eq 0){
					$result = Get-Content $CurrentPath\apply_script_migrations_log.txt
					Logging -message "			$result"
					Logging -message "		MigrationsPersist finished successfully"
				}
				else {
					$result = Get-Content $CurrentPath\apply_script_migrations_log.txt
					Logging -message $result
					
					$SubjectMail = "TPM Azure Dev Server. Problem with applying migrations"
					$BodyMail = "$result"
					
					try {
						if ($environment -eq "AzureDev") {
							Send-MailMessage -SmtpServer $smtpServer -Body $BodyMail -Subject $SubjectMail -To $recipientAddress -Port 25 -From $smtpUser -Credential $cred -Encoding UTF8
						}
					}
					catch {
						Logging "The problem of sending a notification!"
					}
					
					throw "$result"
				}
			}
			else {
				throw "$targetMigrationsPersist does not exist"
			}
		}
		catch {
			Logging -message "$_.Exception.Message"
			throw "$_.Exception.Message"
		}
	}


	#apply SQL scripts
	if($isScript) {
	
		try {
		
			Logging -message "	Starting SQLInitialize"
			
			if (Test-Path "$CurrentPath\DbDelivery") {
			
				cd "$CurrentPath\DbDelivery"
				# & ./DbDeliveryCmd.exe TPM dev > "$CurrentPath\apply_script_migrations_log.txt"
				& ./DbDeliveryCmd.exe TPM $environment > "$CurrentPath\apply_script_migrations_log.txt"
				
				
				if ($LASTEXITCODE -eq 0) {
					Logging -message "	SQLInitialize finished successfully"
				}
				else {
					$result = Get-Content $CurrentPath\apply_script_migrations_log.txt
					Logging -message "$result"
					
					$SubjectMail = "TPM Azure Dev Server. Problem with applying SQL-scripts"
					$BodyMail = "$result"
			
					try {
						if ($environment -eq "AzureDev") {
							Send-MailMessage -SmtpServer $smtpServer -Body $BodyMail -Subject $SubjectMail -To $recipientAddress -Port 25 -From $smtpUser -Credential $cred -Encoding UTF8
						}
					}
					catch {
						Logging "The problem of sending a notification!"
					}
			
					throw "$result"
				}
			}
			else {
				throw "$CurrentPath\DbDelivery does not exist"
			}
		}
		catch {
			Logging -message "$_.Exception.Message"
			throw "$_.Exception.Message"
		}
	}
	
	
}
finally {
}

Logging -message "Script finished at $(Get-Date)"
Logging -message ""
Logging -message ""






























