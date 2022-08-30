
param (

	[string] $pathToRepo = "\\192.168.10.177\Devops\TFS_share\Git",
	[string] $source,
	[string] $branchName,
	[string] $project,
	[string] $projectID,
    [string] $token,
	[string] $mapRepoFolderAsDisk	# для длинных путей /16.08.022

)


$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath


if ( -not $pathToRepo) {

	throw "Path to repo not defined!"
}

if ( -not $source) {

	throw "Path to source not defined!"
}

if ( -not (Test-Path "$source\*")) {

	throw "Source '$source' is empty!"
}

if ( -not $branchName) {

	throw "Branch name not defined!"
}

if ( -not $project) {

	throw "Project not defined!"
}



# Functions:

# ф-ция выполняет рекурсивное копирование файлов (занимает меньше ресурсов), позволяет исключать из копирования каталоги по абсолютным путям (а не только бля по названию) и файлы по маске
function Power-Copy-Item ($pathSource, $pathDest, $excludedFolders, [string[]]$excludedFiles, [switch]$PassThru)
{

	try {
		if ($PassThru) {
			Write-Host "Creating directory '$pathDest'"
		}
		New-Item -Path $pathDest -ItemType Directory -Force > $null

		# копируем только файлы текущего каталога:
		if ($PassThru) {
			Write-Host "Copying files from '$pathSource' to '$pathDest'"
		}
		Get-ChildItem "$pathSource\*" -File -Exclude $excludedFiles | Copy-Item -Destination $pathDest -Force 	#-WhatIf
	   
		$childsDir = Get-ChildItem $pathSource -Directory

		foreach($childDir in $childsDir)
		{
			# Write-Host "Processing dir '$($childDir.FullName)'"
		   
			if ($childDir.FullName -notin $excludedFolders) {
		   
				Power-Copy-Item -pathSource $childDir.FullName -pathDest "$pathDest\$($childDir.Name)" -excludedFolders $excludedFolders -excludedFiles $excludedFiles
			}
			else {
			
				Write-Host "Copy folder '$($childDir.FullName)' is disabled. Miss .."
			}
		}
	}
	catch {
		throw "$($_.Exception.Message)"
	}
}


# ф-ция выполняет рекурсивное удаление файлов (занимает меньше ресурсов), позволяет исключать из удаления каталоги по абсолютным путям (а не только бля по названию) и файлы по маске
function Power-Remove-Item ($pathDest, $excludedFolders, [string[]]$excludedFiles, [switch]$PassThru)
{

	try {

		# удаляем только файлы текущего каталога:
		if ($PassThru) {
			Write-Host "Remove files from '$pathDest'"
		}
		
		Get-ChildItem "$pathDest\*" -File -Exclude $excludedFiles | Remove-Item -Force 	#-WhatIf
	   
		$childsDir = Get-ChildItem $pathDest -Directory

		foreach($childDir in $childsDir)
		{
			# Write-Host "Processing dir '$($childDir.FullName)'"
		   
			if ($childDir.FullName -notin $excludedFolders) {

				Power-Remove-Item -pathDest $childDir.FullName -excludedFolders $excludedFolders -excludedFiles $excludedFiles
		   
				if ( -not ($childDir.EnumerateFileSystemInfos() | select -First 1) ) {
					
					if ($PassThru) {
						Write-Host "$($childDir.FullName) empty. Remove"
					}
					Remove-Item $childDir.FullName -Force #-WhatIf
				}
			}
			else {
			
				Write-Host "Remove folder '$($childDir.FullName)' is disabled. Miss .."
			}
		}
	}
	catch {
		throw "$($_.Exception.Message)"
	}
}



# Main:

try {

	# достанем настройки:
	try {

		$configPath = "$CurrentPath\GitLabPush.json"
		$tmpConfig = Get-Content -encoding UTF8 -Path $configPath -raw	# для версий PS ниже 5 нужно добавть "-raw"!!!, можно заместо "-raw" использовать " | Out-String"
		
		# некоторые преобразования:
		$tmpConfig = $tmpConfig.Replace("{source}", "$source")			# подмена имени переменной на значение
		$tmpConfig = $tmpConfig -replace '\\',"\\"						# экранируем слэши, чтобы Json корректно пути обработал
		
		$config = $tmpConfig | ConvertFrom-Json
	}
	catch { 
		Write-Host "Problem connecting the configuration file!" 
		throw $($_.Exception.Message)
	}


	git config --global http.postBuffer 524288000
	$value = git config --get http.postBuffer
	Write-host "Value of http.postBuffer is $value"


	$pathToRepo = "$pathToRepo\$project\$branchName"	# добавляем к пути до репозитория имя проекта и название ветки

	if ( -not (Test-Path "$pathToRepo\$project")) {		# клон репозитория ещё не создан

		Write-host "Project '$project' has not yet been cloned from branch '$branchName'"
		Write-host "	cloning the depth 1 repository $(Get-Date -Format 'H:mm')"
		
        New-Item "$pathToRepo" -ItemType Directory -Force > $null

		cd "$pathToRepo"
		git clone "http://root:Passw0rd@10.32.95.77/Smartcom/$project`.git" --depth 1 -b $branchName -q
		
		Write-host "	cloning completed $(Get-Date -Format 'H:mm')"

		cd "$pathToRepo\$project"	# на уровень ниже
	}
	else {

		cd "$pathToRepo\$project"	# на уровень ниже
		
		Write-host "Pull the depth 1 repository $(Get-Date -Format 'H:mm')"
		git reset --hard origin/$branchName > $null 	# без этой строчки, следующая строчка сможет перезапись в "локальном", только если на "удалённом" файл редактировался после последнего Pull (!)
		git pull "http://root:Passw0rd@10.32.95.77/Smartcom/$project`.git" --depth 1 $branchName -q --allow-unrelated-histories -s recursive -X theirs > $null 
		
		#Write-host "Cloning the remaining repository"
		# не открывать!	git fetch --unshallow
	}


	if ($mapRepoFolderAsDisk) {		# путь к репо подмапим как диск /16.08.022
		
		# Subst X: $pathToRepo						# подмапливаем диск
		Subst $mapRepoFolderAsDisk $pathToRepo		# подмапливаем диск
		
		$pathToRepo = "$mapRepoFolderAsDisk"		# переукажем путь к репо на созданный диск
	}


	Write-Host "Removing the previous version $(Get-Date -Format 'H:mm')"
	Power-Remove-Item -pathDest "$pathToRepo\$project" -excludedFolders $config.savedFolders -excludedFiles $config.savedFiles

	Write-Host "Copying the new version $(Get-Date -Format 'H:mm')"
	Power-Copy-Item -pathSource $source -pathDest "$pathToRepo\$project" -excludedFolders $config.excludedFolders -excludedFiles $config.excludedFiles
	
	git config user.name "Administrator"
	git config user.email vadim.kosarev@smartcom.software

	# git config --system core.longpaths true
	git config core.longpaths true		# разрешить длинные имена путей /16.08.022
	
	Write-Host "Adding the new version $(Get-Date -Format 'H:mm')"
	git add . > $null
	
	Write-Host "Committing the new version $(Get-Date -Format 'H:mm')"
	$commitName = "Add new version of project from TFS ($(Get-Date -Format 'dd.MM.yyyy H:mm'))"
	git commit -m $commitName > $null
	
	Write-Host "Pushing to git server .. $(Get-Date -Format 'H:mm')"
	git push origin $branchName -q
	
	Write-Host "Pushing to git server finished at $(Get-Date -Format 'H:mm')"


	# доработка Коли для получения статуса с GitLab о состоянии сборки и доставки:
    if ($token){
	
        Write-host ""
        Write-host ""

        Write-Host "Looking for GitLab pipeline ..."
        Start-Sleep 30
        $headers = @{
            'PRIVATE-TOKEN' = $token
        }
		
		<#bad 26.01.022 (Коля дятел)
        $url = "http://10.32.95.77/api/v4/projects"
        $result  = Invoke-RestMethod -Method Get -Headers $headers -Uri $Url
        foreach($res in $result){
            if($res.name -eq $project){
                $id = $res.id
            }
        }

        $url = "http://10.32.95.77/api/v4/projects/$id/pipelines"
        $result  = Invoke-RestMethod -Method Get -Headers $headers -Uri $Url
        foreach($res in $result){
            if($res.ref -eq $branch){
                $pipelineId = $res.id
                break
            }
        }
		#>

		
        $url = "http://10.32.95.77/api/v4/projects/$projectID/pipelines"		# в MarsUslugi $projectID=31
        $result  = Invoke-RestMethod -Method Get -Headers $headers -Uri $Url
		$pipelineId = $($result | Sort-Object id -Descending | Select-Object -First 1).id

		
        # $url = "http://10.32.95.77/api/v4/projects/$id/pipelines/$pipelineId"
        $url = "http://10.32.95.77/api/v4/projects/$projectID/pipelines/$pipelineId"
        do {
		
            $result  = Invoke-RestMethod -Method Get -Headers $headers -Uri $Url
            $status = $result.status
            write-host "Actual status: $status"
            Start-Sleep 10
        }
        while($status -ne "success" -and $status -ne "canceled" -and $status -ne "failed")
		
        Write-host "The end of GitLab pipeline with status '$status'"
		
        if($status -eq "canceled" -or $status -eq "failed"){
            throw "GitLab pipeline ended with status: $status"
        }
    }
	
}
catch {		
	Write-Host "$($_.Exception.Message)"
	throw "$($_.Exception.Message)"		
}
finally {

	if ($mapRepoFolderAsDisk) {		# путь к репо отмапим как диск /16.08.022
	
		# Subst X: /d
		Subst $mapRepoFolderAsDisk /d
	}
	
	Set-Location $CurrentPath
	Write-Host ""
}
	
	
	
	
	
	
	
	