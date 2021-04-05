
# скрипт заменяет Win-конфиги на Azure-конфиги (PROD!!!)
# 12.01.021

$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath
$ErrorActionPreference = "Stop"
$currentDate = Get-Date

$webConfWinPath = "$CurrentPath\..\Frontend.TPM\Web.config"
$webConfAzurePath = "$CurrentPath\..\Frontend.TPM\Web.config.azure.prod"

$appInsightConfWinPath = "$CurrentPath\..\Frontend.TPM\ApplicationInsights.config"
$appInsightConfAzurePath = "$CurrentPath\..\Frontend.TPM\ApplicationInsights.config.azure.prod"

$appConfWinPath = "$CurrentPath\..\ProcessingService.TPM\App.config"
$appConfAzurePath = "$CurrentPath\..\ProcessingService.TPM\App.config.azure.prod"

$appProjWinPath = "$CurrentPath\..\ProcessingService.TPM\ProcessingService.TPM.csproj"
$appProjAzurePath = "$CurrentPath\..\ProcessingService.TPM\ProcessingService.TPM.csproj.azure.dev"		# ???????

$appPersWinPath = "$CurrentPath\..\Module.Persist.TPM\app.config"
$appPersAzurePath = "$CurrentPath\..\Module.Persist.TPM\app.config.azure.prod"

function ReplaceConfig ($oldCongig, $newConfig) {	# ф-ция заменит конфиг


	if ( (Test-Path $oldCongig) -and (Test-Path $newConfig) ) {

		Write-Host "Remove old config '$oldCongig'"
		Remove-Item $oldCongig -Force #-WhatIf   
	}
		
	if (Test-Path $newConfig) {

		Write-Host "Replace config '$newConfig' to '$oldCongig'" 
		Copy-Item $newConfig -Destination $oldCongig -Force #-WhatIf
	}
}

ReplaceConfig -oldCongig $webConfWinPath -newConfig $webConfAzurePath
ReplaceConfig -oldCongig $appInsightConfWinPath -newConfig $appInsightConfAzurePath
ReplaceConfig -oldCongig $appConfWinPath -newConfig $appConfAzurePath
ReplaceConfig -oldCongig $appProjWinPath -newConfig $appProjAzurePath
ReplaceConfig -oldCongig $appPersWinPath -newConfig $appPersAzurePath




