# settings
$ErrorActionPreference = "Stop"
$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath

$source = "$CurrentPath\.."
$dest = "$CurrentPath\..\..\b"

$BuildConfiguration = $args[0].ToString()

if (!(Test-Path $dest)) {
    New-Item $dest -ItemType Directory    
}
else {
    Remove-Item $dest\* -Recurse -Force
}

$logfile = "$CurrentPath\MakePackage.log"

# functions
Function Logging ($message) {
    Write-Host $message
    $message | Out-File $logfile -Append
}

Function CopyFrontend {
	"	Copying Frontend . . ." | Out-File $logfile -Append
	$webTPM = "$dest\Frontend"
	md $webTPM > $null
	Copy-Item "$source\Frontend.TPM\bin" -Destination "$webTPM" -Recurse
	Copy-Item "$source\Frontend.TPM\Views" -Destination "$webTPM" -Recurse
	Copy-Item "$source\Frontend.TPM\Global.asax" -Destination "$webTPM" -Recurse
	Copy-Item "$source\Frontend.TPM\Bundles" -Destination "$webTPM" -Recurse
	Copy-Item "$source\Module.Frontend.TPM\Templates" -Destination "$webTPM\bin" -Recurse #12.04.019
}

Function CopyHost {
	Write-Host "	Copying Host . . ." 
	"	Copying Host . . ." | Out-File $logfile -Append
	$serviceTPM = "$dest\ProcessingService"
	md $serviceTPM > $null
	Copy-Item "$source\ProcessingService.TPM\bin\$BuildConfiguration\*.pdb" -Destination "$serviceTPM"
	Copy-Item "$source\ProcessingService.TPM\bin\$BuildConfiguration\*.dll" -Destination "$serviceTPM"
}

function cleaningOldArtifacts ($source, $count) {

	# очистка артефактов по заданному пути $source, кроме последних $count
	# в $source нужно указать маску
	#  -WhatIf опция имитации удаления

	if (!($source -and (Test-Path "$source"))) {
		Logging "Error: No files at the specified path $source"
        return 1 
	}

	echo ""
	Logging "Deleting old artifacts in folder $source except the last $count"
	
    # достанем все и отсортируем по-убыванию даты создания:
    $filesForDel = Get-Childitem $source -Directory | Select Fullname, LastWriteTime | Sort-Object -Property LastWriteTime -Descending 

	# удаляем, кроме нескольких последних:
	if ( $filesForDel.Count -gt $count ) { # >
		#$filesForDel | select -Last ($filesForDel.Count - $count) | %{Remove-Item $_.Fullname -Force -Recurse -WhatIf}
		$filesForDel | select -Last ($filesForDel.Count - $count) | %{Remove-Item $_.Fullname -Force -Recurse}
        
		#echo $filesForDel.Fullname
		Logging "	Done" 
	}
	else {
		Logging "	No files to clear"
	}
	echo ""
}


Logging -message "Script started at $(Get-Date)" 

# creating archives
$currDate = Get-Date -UFormat %Y%m%d_%H%M
$RelName = "TPM_" + $currDate

try {
	Logging -message  "	Creating '$RelName'"
	
	CopyFrontend
	CopyHost
	Logging -message "		Archiving" 
	
	# Write-Zip -Path "$dest\*" -OutputPath "$dest\$RelName.zip" -Quiet > $null
	& "C:\Program Files\7-Zip\7z.exe" a "$dest\$RelName.zip" "$dest\*" > $null
	
} catch {
	Logging -message "Can't continue Release. Release creating failed!" 
	Logging -message "$_.Exception.Message"
	exit 1
}


#creating Migrations archives
try {
	$folders = Get-Childitem $source -Directory
	foreach ($folder in $folders) {
		$path = $folder.FullName
		if (Test-path "$path\Migrations") {
			Logging -message "        Creating migrations archive for $folder"
			$MigrationFolderName = "Migrations_" + $folder.Name + "_" + $currDate
			md $dest\$MigrationFolderName > $null

			Copy-Item $path\Migrations -Recurse -Destination $dest\$MigrationFolderName
			Copy-Item $path\bin\Release\*.* -Exclude "*.config" -Destination $dest\$MigrationFolderName
			Copy-Item $source\packages\EntityFramework.6.1.3\tools\migrate.exe -Destination $dest\$MigrationFolderName

			# Write-Zip -Path "$dest\$MigrationFolderName\*" -OutputPath "$dest\$MigrationFolderName.zip" -Quiet > $null
			& "C:\Program Files\7-Zip\7z.exe" a "$dest\$MigrationFolderName.zip" "$dest\$MigrationFolderName\*" > $null
			Logging -message "        $MigrationFolderName.zip created successfully"
		}
	}
}
catch {
	Logging -message "$_.Exception.Message"
	exit 1
}
	
	
#creating SQLInitialize archives
try {
	$folders = Get-Childitem $source -Directory
	Logging -message "        Creating archive with SQLInitialize scripts"
	foreach ($folder in $folders) {
		$path = $folder.FullName
		if (Test-path "$path\SQLInitialize") {
			if (!(Test-Path "$dest\SQLInitialize_Release*")) {
				Logging -message "            Getting SQLInitialize scripts ($path)"
				$SQLFolderName = "SQLInitialize_Release" + "_" + $currDate
				md $dest\$SQLFolderName > $null
				Copy-Item $path\SQLInitialize\*.sql -Destination $dest\$SQLFolderName
			}
			else {
				Logging -message "            Getting SQLInitialize scripts (add + $path)"
				$NameFolder = (Get-Item "$dest\SQLInitialize_Release*").Name
				Copy-Item $path\SQLInitialize\*.sql "$dest\$NameFolder"
			}
		}
	}
		
	$NameFolder = (Get-Item "$dest\SQLInitialize_Release*").Name
    if ($(Get-Item "$dest\$NameFolder\*").Count -gt 0) {
	
		# Write-Zip -Path "$dest\$NameFolder\*" -OutputPath "$dest\$NameFolder.zip" -Quiet > $null
		& "C:\Program Files\7-Zip\7z.exe" a "$dest\$NameFolder.zip" "$dest\$NameFolder\*" > $null
		
		Logging -message "        Archive with SQLInitialize scripts created successfully"
    } else {
        Logging -message "        No new scripts to archive"
    }
}
catch {
	Logging -message "$_.Exception.Message"
	exit 1
}


if (!(Test-Path "$dest\..\a")) {	# какого-то хера не всегда по-умолчанию создаётся
    New-Item "$dest\..\a" -ItemType Directory    
}


Copy-Item "$dest\*.zip" -Destination "$dest\..\a" -Force
Copy-Item "$CurrentPath\MakeDeliveryAndDeploy.ps1" -Destination "$dest\..\a" -Force


if ($args[1]) {

	# запишем Build.SourcesDirectory в файл для передачи релизу:
	$args[1] | Set-Content -Path "$dest\..\a\BuildSourcesDirectory.dat"
}


if ($args[2]) {	# есть путь до артефактов
	if ($args[3]) {	# количество передаётся параметром
		$countRetain = $args[3]
	} 
	else {
		$countRetain = 2
	}
	
	# очистка старых артефактов:
	cleaningOldArtifacts -source "D:\TFS_share\$($args[2])\*" -count $countRetain
}


Logging -message "Script finished at $(Get-Date)" 