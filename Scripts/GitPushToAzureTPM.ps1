$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath

if (! $args) {
	throw "Need arguments!"
}

$Source = $args[0]
$branchName = $args[1]

# for test:
# $Source = "D:\BDS01-A01\_work\608\s"
# $smartcomToolsSource = "$Source\..\a"
# $branchName = "stage"
# $branchName = "prod"

if (! $Source) {
	throw "Need source!"
}

if (! $branchName) {
	throw "Need branch name!"
}

$pathToBranch = "D:\Azure\TPM\$branchName"
$pathToRepo = "$pathToBranch\russia-petcare-jupiter"


try {

	git config --global http.postBuffer 524288000
	$value = git config --get http.postBuffer
	Write-host "Value of http.postBuffer is $value"

	$needClone = $false			# !!!!
	if ($needClone) {

		#If (Test-Path $pathToRepo) {
		#	Write-host "Cleaning the local repository"
		#	Remove-Item $pathToRepo -Force -Recurse
		#}

		if(!(Test-Path $pathToBranch)) {
			New-Item $pathToBranch -ItemType Directory > $null   
		}

		cd $pathToBranch
		
		# клонировать по-сути надо только первый раз!
		git clone https://prokoyur:6b643eunygn4nny5mngtulu2qiprflgazqg3mwnmheuqubb3exya@dev.azure.com/marsanalytics/RUSSIA%20PETCARE%20JUPITER/_git/russia-petcare-jupiter --depth 1 -b $branchName -q
		
		exit
	}

	$args


	if (Test-Path $pathToRepo) {
		cd $pathToRepo

		Write-host "Pull the depth 1 repository $(Get-Date -Format 'H:mm')"
		git reset --hard origin/$branchName #> $null # без этой строчки, следующая строчка сможет перезапись в "локальном", только если на "удалённом" файл редактировался после последнего Pull (!)
		git pull https://prokoyur:6b643eunygn4nny5mngtulu2qiprflgazqg3mwnmheuqubb3exya@dev.azure.com/marsanalytics/RUSSIA%20PETCARE%20JUPITER/_git/russia-petcare-jupiter --depth 1 $branchName -q --allow-unrelated-histories -s recursive -X theirs
		
		#Write-host "Cloning the remaining repository"
		# не открывать!	git fetch --unshallow
		
		Write-Host "Removing the previous version $(Get-Date -Format 'H:mm')"
		if ($pathToRepo -and (Test-Path "$pathToRepo\Source")) {
			Get-Childitem "$pathToRepo\Source" -Recurse | Remove-Item -Recurse -Force #-WhatIf
		}
		
		
		Write-Host "Copying the new version $(Get-Date -Format 'H:mm')"
		Copy-Item "$Source\*" -Exclude "bin", "packages", '$tf', "*.log", ".tfignore", "error.txt", "*.vssscc" -Destination "$pathToRepo\Source" -Recurse -Force
		if(!(Test-Path "$pathToRepo\Source\packages")) {
			New-Item "$pathToRepo\Source\packages" -ItemType Directory > $null   
		}
		Copy-Item "$Source\packages\RodMApp.Core.TPM.*" -Destination "$pathToRepo\Source\packages" -Recurse -Force
		
		git config user.name "prokoyur"
		git config user.email yuri.prokofiev@effem.com
		
		Write-Host "Adding the new version $(Get-Date -Format 'H:mm')"
		git add . > $null
		
		Write-Host "Committing the new version $(Get-Date -Format 'H:mm')"
		$commit_name = "Add new version of project from TFS ($(Get-Date -Format 'dd.MM.yyyy H:mm'))"
		git commit -m $commit_name > $null
		
		Write-Host "Pushing to git server.. $(Get-Date -Format 'H:mm')"
		git push origin $branchName -q
		
		Write-Host "Pushing to git server finished at $(Get-Date -Format 'H:mm')"
		
	}
}
catch {
	Logging -message "$_.Exception.Message"
	throw "$_.Exception.Message"
}
finally {
}


















