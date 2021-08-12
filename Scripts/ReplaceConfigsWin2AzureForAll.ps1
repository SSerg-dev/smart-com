
# скрипт заменяет Win-конфиги на Azure-конфиги
# 31.07.020


param (

    [string] $postfix	# постфикс в названии конфига, в зависимости от среды (например: '.azure', '.azure.uiux', '.azure.test')
)


$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath
$ErrorActionPreference = "Stop"
$currentDate = Get-Date


function ReplaceConfig ($oldCongig, $newConfig) {	# ф-ция заменит конфиг


	if ( (Test-Path $oldCongig) -and (Test-Path $newConfig) ) {

		Write-Host "	Remove old config '$oldCongig'"
		Remove-Item $oldCongig -Force #-WhatIf   
	}
		
	if (Test-Path $newConfig) {

		Write-Host "	Replace config '$newConfig' to '$oldCongig'" 
		Copy-Item $newConfig -Destination $oldCongig -Force #-WhatIf
	}
	else {
	
		throw "Config '$newConfig' not found!"
	}
}


try {


	if ( -not $postfix) {

		throw "The postfix is not specified! For example: '.azure', '.azure.uiux', '.azure.test'"
	}

	Write-Host "Preparing configs ..."


	$webConfWinPath = "$CurrentPath\..\Frontend.TPM\Web.config"
	$webConfAzurePath = "$webConfWinPath$postfix"

	$appInsightConfWinPath = "$CurrentPath\..\Frontend.TPM\ApplicationInsights.config"
	$appInsightConfAzurePath = "$appInsightConfWinPath$postfix"

	$appConfWinPath = "$CurrentPath\..\ProcessingService.TPM\App.config"
	$appConfAzurePath = "$appConfWinPath$postfix"

	$appProjWinPath = "$CurrentPath\..\ProcessingService.TPM\ProcessingService.TPM.csproj"
	$appProjAzurePath = "$appProjWinPath$postfix"

	$appPersWinPath = "$CurrentPath\..\Module.Persist.TPM/app.config"
	$appPersAzurePath = "$appPersWinPath$postfix"


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
	
	




