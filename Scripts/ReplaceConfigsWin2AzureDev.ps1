
# скрипт заменяет Win-конфиги на Azure-конфиги (Stage!!!)
# 31.07.020

$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath
$ErrorActionPreference = "Stop"
$currentDate = Get-Date

$webConfWinPath = "$CurrentPath\..\Frontend.TPM\Web.config"
$webConfAzurePath = "$CurrentPath\..\Frontend.TPM\Web.config.azure.dev"

$appInsightConfWinPath = "$CurrentPath\..\Frontend.TPM\ApplicationInsights.config"
$appInsightConfAzurePath = "$CurrentPath\..\Frontend.TPM\ApplicationInsights.config.azure.dev"

$appConfWinPath = "$CurrentPath\..\ProcessingService.TPM\App.config"
$appConfAzurePath = "$CurrentPath\..\ProcessingService.TPM\App.config.azure.dev"

$appProjWinPath = "$CurrentPath\..\ProcessingService.TPM\ProcessingService.TPM.csproj"
$appProjAzurePath = "$CurrentPath\..\ProcessingService.TPM\ProcessingService.TPM.csproj.azure.dev"

$appPersWinPath = "$CurrentPath\..\Module.Persist.TPM\app.config"
$appPersAzurePath = "$CurrentPath\..\Module.Persist.TPM\app.config.azure.dev"

$logoImageWinPath = "$CurrentPath\..\Module.Frontend.TPM/Content/images/logo.svg"
$logoImageAzurePath = "$CurrentPath\..\Module.Frontend.TPM/Content/images/logoTest.svg"

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
ReplaceConfig -oldCongig $logoImageWinPath -newConfig $logoImageAzurePath




