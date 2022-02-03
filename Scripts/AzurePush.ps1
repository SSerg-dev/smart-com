
param (

	[string] $pathToRepo = "\\192.168.10.177\Devops\TFS_share\Azure",
	[string] $source,
	[string] $branchName,				# stage, prod
	[string] $project = "SmartCom",
	[string] $repoName,					# �������� Mercury
    [string] $token,					# ����� smartcom �  Azure
	[switch] $useSSH,					# ������ �� SSH
	[switch] $noDropOnError	= $false	# �� ������������� ��� ������

)


$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath

if (-not $noDropOnError) {	# ��-��������� ������� ������ � ������ ������

	$ErrorActionPreference = "Stop"
}


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

if ( -not $repoName) {

	throw "Repository name not defined!"
}

if ( -not $token) {

	throw "Token not defined!"
}



# Functions:

# �-��� ��������� ����������� ����������� ������ (�������� ������ ��������), ��������� ��������� �� ����������� �������� �� ���������� ����� (� �� ������ ��� �� ��������) � ����� �� �����
function Power-Copy-Item ($pathSource, $pathDest, $excludedFolders, [string[]]$excludedFiles, [switch]$PassThru)
{

	try {
		if ($PassThru) {
			Write-Host "Creating directory '$pathDest'"
		}
		New-Item -Path $pathDest -ItemType Directory -Force > $null

		# �������� ������ ����� �������� ��������:
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


# �-��� ��������� ����������� �������� ������ (�������� ������ ��������), ��������� ��������� �� �������� �������� �� ���������� ����� (� �� ������ ��� �� ��������) � ����� �� �����
function Power-Remove-Item ($pathDest, $excludedFolders, [string[]]$excludedFiles, [switch]$PassThru)
{

	try {

		# ������� ������ ����� �������� ��������:
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

	# �������� ���������:
	try {

		$configPath = "$CurrentPath\AzurePush.json"
		$tmpConfig = Get-Content -encoding UTF8 -Path $configPath -raw	# ��� ������ PS ���� 5 ����� ������� "-raw"!!!, ����� ������� "-raw" ������������ " | Out-String"
		
		# ��������� ��������������:
		$tmpConfig = $tmpConfig.Replace("{source}", "$source")			# ������� ����� ���������� �� ��������
		$tmpConfig = $tmpConfig -replace '\\',"\\"						# ���������� �����, ����� Json ��������� ���� ���������
		
		$config = $tmpConfig | ConvertFrom-Json
	}
	catch { 
		Write-Host "Problem connecting the configuration file!" 
		throw $($_.Exception.Message)
	}


	git config --global http.postBuffer 524288000
	$value = git config --get http.postBuffer
	Write-host "Git config: Value of http.postBuffer is $value"

	Write-host "Git config: Setting property core.autocrlf to false"
	git config --global core.autocrlf false


	$pathToRepo = "$pathToRepo\$project\$branchName"	# ��������� � ���� �� ����������� ��� ������� � �������� �����

	if ( -not (Test-Path "$pathToRepo\$repoName")) {		# ���� ����������� ��� �� ������

		# Write-host "Project '$project' has not yet been cloned from branch '$branchName'"
		Write-host "Repository '$repoName' of project '$project' has not yet been cloned from branch '$branchName'"
		Write-host "	cloning the depth 1 repository $(Get-Date -Format 'H:mm')"
		
        New-Item "$pathToRepo" -ItemType Directory -Force > $null

		cd "$pathToRepo"
		git clone https://unused:$token@dev.azure.com/MarsDevTeam/$project/_git/$repoName --depth 1 -b $branchName -q

		Write-host "	cloning completed $(Get-Date -Format 'H:mm')"

		cd "$pathToRepo\$repoName"	# �� ������� ����
	}
	else {

		cd "$pathToRepo\$repoName"	# �� ������� ����
		
		Write-host "Pull the depth 1 repository $(Get-Date -Format 'H:mm')"
		git reset --hard origin/$branchName > $null 	# ��� ���� �������, ��������� ������� ������ ���������� � "���������", ������ ���� �� "��������" ���� �������������� ����� ���������� Pull (!)

		git pull https://unused:$token@dev.azure.com/MarsDevTeam/$project/_git/$repoName --depth 1 $branchName -q --allow-unrelated-histories -s recursive -X theirs
		
		#Write-host "Cloning the remaining repository"
		# �� ���������!	git fetch --unshallow
	}


	Write-Host "Removing the previous version $(Get-Date -Format 'H:mm')"
	Power-Remove-Item -pathDest "$pathToRepo\$repoName" -excludedFolders $config.savedFolders -excludedFiles $config.savedFiles

	Write-Host "Copying the new version $(Get-Date -Format 'H:mm')"
	Power-Copy-Item -pathSource $source -pathDest "$pathToRepo\$repoName" -excludedFolders $config.excludedFolders -excludedFiles $config.excludedFiles
	
	# ������� PS:
	New-Item "$pathToRepo\$repoName\Scripts" -ItemType Directory -Force > $null
	Copy-Item "$source\Scripts\MakePackageAzure.ps1" -Destination "$pathToRepo\$repoName\Scripts" -Force -ErrorAction SilentlyContinue
	Copy-Item "$source\Scripts\MakeDeliveryAndDeployFromAzure.ps1" -Destination "$pathToRepo\$repoName\Scripts" -Force -ErrorAction SilentlyContinue
	Copy-Item "$source\Scripts\Modules" -Destination "$pathToRepo\$repoName\Scripts" -Recurse -Force -ErrorAction SilentlyContinue
	#14.10.021 ��-�� TPM ����� ��������� �� ������ � ������� � ������ ���������� ����������� �������� ( -ErrorAction SilentlyContinue)
	
	git config user.name "prokoyur"
	git config user.email yuri.prokofiev@effem.com
	
	Write-Host "Adding the new version $(Get-Date -Format 'H:mm')"
	git add . > $null
	
	Write-Host "Committing the new version $(Get-Date -Format 'H:mm')"
	$commitName = "Add new version of project from TFS ($(Get-Date -Format 'dd.MM.yyyy H:mm'))"
	git commit -m $commitName > $null

	if ($useSSH) {		# ������ �� SSH /21.09.021
		
		Write-Host "Change Url for 'origin' $(Get-Date -Format 'H:mm')"
		git remote set-url origin git@ssh.dev.azure.com:v3/MarsDevTeam/$project/$repoName	# ��������� �� SSH /21.09.021
	}
	
	Write-Host "Pushing to git server .. $(Get-Date -Format 'H:mm')"
	git push origin $branchName -q
	
	Write-Host "Pushing to git server finished at $(Get-Date -Format 'H:mm')"



	<# 28.06.021 ��� Azure �������� ����� ���������
	# ��������� ���� ��� ��������� ������� � GitLab � ��������� ������ � ��������:
    if ($token){
	
        Write-Host "Looking for GitLab pipeline"
        Start-Sleep 30
        $headers = @{
            'PRIVATE-TOKEN' = $token
        }
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
        $url = "http://10.32.95.77/api/v4/projects/$id/pipelines/$pipelineId"
        do{
            $result  = Invoke-RestMethod -Method Get -Headers $headers -Uri $Url
            $status = $result.status
            write-host "Actual status: $status"
            Start-Sleep 10
        }
        while($status -ne "success" -and $status -ne "canceled" -and $status -ne "failed")
        Write-host "The end of GitLab pipeline with status $status"
        if($status -eq "canceled" -or $status -eq "failed"){
            throw "GitLab pipeline ended with status: $status"
        }
    }
	#>
	
}
catch {		
	Write-Host "$($_.Exception.Message)"
	throw "$($_.Exception.Message)"		
}
finally {
	Set-Location $CurrentPath
	Write-Host ""
}
	
	
	
	
	
	
	
	