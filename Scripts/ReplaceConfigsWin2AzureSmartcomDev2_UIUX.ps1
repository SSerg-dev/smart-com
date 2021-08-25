
# скрипт заменяет Win-конфиги на Azure-конфиги
# 31.07.020

$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath
$ErrorActionPreference = "Stop"
$currentDate = Get-Date

$webConfWinPath = "$CurrentPath\..\Frontend.TPM\Web.config"
$webConfAzurePath = "$CurrentPath\..\Frontend.TPM\Web.config.azure.uiux"

$appInsightConfWinPath = "$CurrentPath\..\Frontend.TPM\ApplicationInsights.config"
$appInsightConfAzurePath = "$CurrentPath\..\Frontend.TPM\ApplicationInsights.config.azure.uiux"

$appConfWinPath = "$CurrentPath\..\ProcessingService.TPM\App.config"
$appConfAzurePath = "$CurrentPath\..\ProcessingService.TPM\App.config.azure.uiux"

$appProjWinPath = "$CurrentPath\..\ProcessingService.TPM\ProcessingService.TPM.csproj"
$appProjAzurePath = "$CurrentPath\..\ProcessingService.TPM\ProcessingService.TPM.csproj.azure.uiux"

$appPersWinPath = "$CurrentPath\..\Module.Persist.TPM/app.config"
$appPersAzurePath = "$CurrentPath\..\Module.Persist.TPM/app.config.azure.uiux"


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
	ReplaceConfig -oldCongig $appInsightConfWinPath -newConfig $appInsightConfAzurePath
	ReplaceConfig -oldCongig $appConfWinPath -newConfig $appConfAzurePath
	ReplaceConfig -oldCongig $appProjWinPath -newConfig $appProjAzurePath
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
	
	




