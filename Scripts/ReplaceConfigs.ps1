
#requires -version 4
<#
.SYNOPSIS
	SFM3 deployment script
  
.NOTES
	Version:        1.0
  	Author:         Pavel Sergeev, Smartcom LLC
  	Creation Date:  02.09.2022
  	Change Date:    02.09.2022

#>

#---------------------------------------------------------[ Script Parameters ]------------------------------------------------------

param (
	[Parameter(Mandatory)]
	[string] $SourcesDirectory,
	[Parameter(Mandatory)]
	[string] $Environment
)

#---------------------------------------------------------[ Initialization ]--------------------------------------------------------

$ErrorActionPreference = "Stop"

$Configurations = @(
	'Frontend.TPM\Web.config',
	'ProcessingService.TPM\App.config',
	'Module.Persist.TPM\app.config'
)

#-----------------------------------------------------------[ Execution ]------------------------------------------------------------

try {
	Write-Host "Preparing configuration files"
	foreach ($Configuration in $Configurations) {
		$ConfigFile = $Configuration
		$ConfigEnvFile = $Configuration + '.' + $Environment
		$ConfigPath = Join-Path -Path $SourcesDirectory -ChildPath $Configuration
		$ConfigEnvPath = Join-Path -Path $SourcesDirectory -ChildPath ($Configuration + '.' + $Environment)

		Write-Host "Replace config file '$ConfigEnvFile' to '$ConfigFile'"
		Copy-Item -Path $ConfigEnvPath -Destination $ConfigPath -Force
	}

	Write-Host "Done"
} catch {
	Write-Host $($_.Exception.Message)
	throw $($_.Exception.Message)
}
