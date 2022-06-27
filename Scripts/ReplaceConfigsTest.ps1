
$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath
$ErrorActionPreference = "Stop"
$currentDate = Get-Date

$webConfWinPath = "$CurrentPath\..\Frontend.TPM\Web.config"
$webConfAzurePath = "$CurrentPath\..\Frontend.TPM\Web.config.test"

$appConfWinPath = "$CurrentPath\..\ProcessingService.TPM\App.config"
$appConfAzurePath = "$CurrentPath\..\ProcessingService.TPM\App.config.test"

$appPersWinPath = "$CurrentPath\..\Module.Persist.TPM\app.config"
$appPersAzurePath = "$CurrentPath\..\Module.Persist.TPM\app.config.test"


function ReplaceConfig ($oldCongig, $newConfig) {	# ф-ция заменит конфиг


	if ( (Test-Path $oldCongig) -and (Test-Path $newConfig) ) {

		Write-Host "	Remove old config '$oldCongig'"
		Remove-Item $oldCongig -Force #-WhatIf   
	}
		
	if (Test-Path $newConfig) {

		Write-Host "	Replace config '$newConfig' to '$oldCongig'" 
		Copy-Item $newConfig -Destination $oldCongig -Force #-WhatIf
	}
}


try {

	Write-Host "Preparing configs ..."

	ReplaceConfig -oldCongig $webConfWinPath -newConfig $webConfAzurePath
	ReplaceConfig -oldCongig $appConfWinPath -newConfig $appConfAzurePath
	ReplaceConfig -oldCongig $appPersWinPath -newConfig $appPersAzurePath

	<#
	if (Test-Path "$CurrentPath\..\Module.Frontend.TPM\Templates") {

		Write-Host "Copying Templates ..."
		Copy-Item "$CurrentPath\..\Module.Frontend.TPM\Templates" -Destination "$CurrentPath\..\Frontend.TPM\" -Recurse -Force

	}
	#>
}	
catch {
	Write-Host $($_.Exception.Message)
	throw $($_.Exception.Message)
}	
finally {
	Write-Host "Done"
}	
	
	




