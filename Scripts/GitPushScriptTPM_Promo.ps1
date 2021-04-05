$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath

$path_to_repo = "D:\Git\TPM_Promo"

# это нужно только для клонирования:
#If (Test-Path "D:\Git\DAP") {
#	Write-host "Cleaning the local repository"
#	Remove-Item DAP -Force -Recurse
#}

git config --global http.postBuffer 524288000
$value = git config --get http.postBuffer
Write-host "Value of http.postBuffer is $value"

#git gc --auto


# клонировать по-сути надо только первый раз!
#Write-host "Cloning the depth 1 repository $(Get-Date -Format 'H:mm')"
#git clone http://root:Passw0rd@10.32.95.77/Smartcom/TPM_Promo.git #--depth 1 -b stage -q

if (Test-Path $path_to_repo) {
	cd $path_to_repo

    Write-host "Pull the depth 1 repository $(Get-Date -Format 'H:mm')"
	git reset --hard origin/stage > $null # без этой строчки, следующая строчка сможет перезапись в "локальном", только если на "удалённом" файл редактировался после последнего Pull (!)
    git pull http://root:Passw0rd@10.32.95.77/Smartcom/TPM_Promo.git --depth 1 stage -q --allow-unrelated-histories -s recursive -X theirs > $null 
	
	#Write-host "Cloning the remaining repository"
	# не открывать!	git fetch --unshallow
	
	Write-Host "Removing the previous version $(Get-Date -Format 'H:mm')"

	Copy-Item "Frontend.TPM\InstallGulpPackagesScript.ps1" # скопирует файл в текущую папку(!)

	# удалим все полученные из удалённого репозитория папки и файлы, кроме перечисленных ниже:
	Get-Item * -Exclude ".git", ".tfignore", ".gitlab-ci.yml", "MakePackages.ps1", "Release.ps1", "InstallGulpPackagesScript.ps1" | Remove-Item -Recurse -Force

	Write-Host "Copying the new version $(Get-Date -Format 'H:mm')"
	Get-Childitem $args[0] -Exclude "*.log", ".tfignore" | Copy-Item -Recurse

	Copy-Item InstallGulpPackagesScript.ps1 Frontend.TPM
	Remove-Item InstallGulpPackagesScript.ps1
	
	git config user.name "Administrator"
	git config user.email vadim.kosarev@smartcom.software
	
	Write-Host "Adding the new version $(Get-Date -Format 'H:mm')"
	git add . > $null
	
	Write-Host "Committing the new version $(Get-Date -Format 'H:mm')"
    $d = Get-Date -Format 'dd.MM.yyyy H:mm'
    if ( $args[1]) {
        $commit_name = $args[1] + " ($d)" # берём из параметра
        Write-Host "Commit name: $commit_name"
    }
    else {
        $commit_name = "Add new version of project from TFS ($d)"
    }
	git commit -m $commit_name > $null
	
	Write-Host "Pushing to git server.. $(Get-Date -Format 'H:mm')"
	git push origin stage -q
	
	Write-Host "Pushing to git server finished at $(Get-Date -Format 'H:mm')"
} 