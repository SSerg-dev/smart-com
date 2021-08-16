
# скрипт добавляет, обновляет, удаляет KeyVault in Azure
# 06.08.020

param (

    [string] $configName,			# конфиг с переменными, в зависимости от среды (например: 'KeyVaultManager.smartcom.dev.json', 'KeyVaultManager.smartcom.test.json', 'KeyVaultManager.mars.dev1.json')
    [switch] $showAllSecretKeys		# показать ВСЕ секретные переменные перед заменой
)


$CurrentPath = Split-Path $MyInvocation.MyCommand.Path
Set-Location $CurrentPath
$ErrorActionPreference = "Stop"
$currentDate = Get-Date



# KeyVault
# KeyVaultManager.ps1

if ( -not $configName) {

	throw "The config with secret variables is not specified!" 
}

if (Test-Path "$CurrentPath\$configName.json") {

	$configFilePath = "$CurrentPath\$configName.json"
}
# или полное имя файла:
if (Test-Path "$CurrentPath\$configName") {

	$configFilePath = "$CurrentPath\$configName"
}
if ( -not $configFilePath) {

	throw "Config '$CurrentPath\$configName' does not exist!"
}


# подключим конфиг:
try {

	$tmpConfig = Get-Content -Path $configFilePath -raw 	# для версий PS ниже 5 нужно добавть "-raw"!!!, можно заместо "-raw" использовать " | Out-String"
	$config = $tmpConfig | ConvertFrom-Json
    #$Config = $Config.indexes.psobject.properties
}
catch { 
	throw "The config file is bad!" 
}

# $config


function SetKeySecret ($vaultName, $keyName, $keyValue, $keyContentType) {		# добавляет/обновляет секретную переменную

	$secretValue = ConvertTo-SecureString "$keyValue" -AsPlainText -Force
	$result = Set-AzKeyVaultSecret -VaultName "$vaultName" -Name "$keyName" -SecretValue $secretValue -ContentType "$keyContentType"
	
	return $result
}


try {

	# for test:
	# $AzUser = "6143a5e4-4eed-4597-a86b-107c13e4527e"
	# $AzPassword = "i2hrp5-HjD0EZ25EP_~W8v2uOG9P-5OPr8" | ConvertTo-SecureString -AsPlainText -Force
	# $AzAdCredential = New-Object PSCredential($AzUser, $AzPassword)
	# Connect-AzAccount -Credential $AzAdCredential -Tenant "d43cf2af-b6de-47fe-afe3-9bbaff83dcee" -ServicePrincipal
	# :for test
	
	
	Write-Host ""
	Write-Host "Key vault name is '$($config.vaultName)'"
	Write-Host ""

	# $keyVault = Get-AzKeyVault -VaultName "jupitertestakv" -ResourceGroupName "JUPITER-TEST-RG"
	# $keyVault = Get-AzKeyVault -ResourceGroupName "JUPITER-TEST-RG"
	$keyVault = Get-AzKeyVault -VaultName $config.vaultName -ResourceGroupName $config.resourceGroupName

	
	# вывод секретных переменных:
	# (вывод сделан для "бэкапирования" предыдущих значений переменных, что-бы сохранить историчность в логах сборок)
	if ($showAllSecretKeys) {	# показать ВСЕ переменные
	
		Write-Host "List of values of ALL variables:"
		# Write-Host ""
		Write-Host "====================================="
		Write-Host ""
		
		$secretKeys = $keyVault | Get-AzKeyVaultSecret		# получить все секреты (сами значения не забирает( )
		foreach ($secretKey in $secretKeys) { 
		
			Write-Host "	Key name:	$($secretKey.Name)"
			Write-Host "	Key value:	$(Get-AzKeyVaultSecret -vaultName $keyVault.VaultName -Name $secretKey.Name -AsPlainText)"
			Write-Host ""
		}
		Write-Host "====================================="
		Write-Host ""
	}
	else {	# показать только те, которые создаём/меняем
	
		Write-Host "List of values of editable variables:"
		# Write-Host ""
		Write-Host "====================================="
		Write-Host ""
		
		foreach ($secretKey in $config.secretKeys) {
		
			Write-Host "	Key name:	$($secretKey.Name)"
			Write-Host "	Key value:	$(Get-AzKeyVaultSecret -vaultName $keyVault.VaultName -Name $secretKey.Name -AsPlainText)"
			Write-Host ""
		}
	
		Write-Host "====================================="
		Write-Host ""
	}
	
	
	Write-Host "Installing secret variables to the Key vault for '$($config.systemName)'..."
	Write-Host ""
	
	

	foreach ($secretKey in $config.secretKeys) {

		Write-Host "	Processing key '$($secretKey.name)'"
		
		if ($secretKey.enabled -eq $false) {
		
			Write-Host "		... processing is disabled. Miss ..."
			continue
		}
		
		#for test: Get-AzKeyVaultSecret -VaultName "jupitertestakv" -Name "TestKeyYu" -AsPlainText
		$secretKeyCurrentValue = Get-AzKeyVaultSecret -VaultName $keyVault.VaultName -Name $secretKey.name -AsPlainText
		$secretKeyCurrentValueInRemovedState = Get-AzKeyVaultSecret -VaultName $keyVault.VaultName -Name $secretKey.name -InRemovedState	# в статусе "удалена" (вернёт объект, но не значение переменной!)
		

	    switch ($secretKey.action) {
			
			"add" {
			
				
				if ( -not ($secretKeyCurrentValue -or $secretKeyCurrentValueInRemovedState) ) {		# если переменных не существует
				
					Write-Host "		Variable '$($secretKey.name)' does not exist. Adding it ..."
					
					if ( -not $secretKey.value ) {
					
						Write-Host "			variable '$($secretKey.name)' is null. Empty variables are not allowed! Miss ..."
						continue
					}
					
					# $result = Set-AzKeyVaultSecret -VaultName "jupitertestakv" -Name "TestKeyYu" -SecretValue $secretvalue -ContentType "test"
					
					try {
						
						# добавляем новую секретную переменную:
						$result = SetKeySecret -vaultName "$($keyVault.VaultName)" -keyName "$($secretKey.name)" -keyValue "$($secretKey.value)" -keyContentType "$($secretKey.contentType)"

						if ($result.name -eq $secretKey.name) {		# в ответе вернулось имя переменной
						
							Write-Host "			result: ok"
						}
						else {
						
							Write-Host "The problem of creating the secret variable '$($secretKey.name)'!"
							throw "The problem of creating the secret variable '$($secretKey.name)'!"
						}
					}
					catch {
						Write-Host "The problem of creating the secret variable '$($secretKey.name)'!"
						throw $($_.Exception.Message)
					}	

				}
				else {
				
					if ( $secretKeyCurrentValueInRemovedState ) {	# если переменная существовала ранее, а сейчас в статусе "удалена"
					
						# сначала восстановим переменную:
						try {
						
							Write-Host "		Variable '$($secretKey.name)' has been deleted. Restoring it ..."
							
							$result = Undo-AzKeyVaultSecretRemoval -VaultName "$($keyVault.VaultName)" -Name "$($secretKey.name)" 	#-WhatIf
							
							if ($result.name -eq $secretKey.name) {		# в ответе вернулось имя переменной
							
								Write-Host "			result: ok"
								
								# переменной $secretKeyCurrentValue ничего не будем присваивать, т.к. она уже пустая
							}
							else {
							
								Write-Host "The problem of restoring the deleted secret variable '$($secretKey.name)'!"
								throw "The problem of restoring the deleted secret variable '$($secretKey.name)'!"
							}
							
						}
						catch {
							Write-Host "The problem of restoring the deleted secret variable '$($secretKey.name)'!"
							throw $($_.Exception.Message)
						}	
					}


					if ( $secretKey.value -and ($secretKeyCurrentValue -ne $secretKey.value) ) {
					
						Write-Host "		Variable '$($secretKey.name)' already exists. Updating its value ..."
					
						try {
						
							# обновляем секретную переменную:
							$result = SetKeySecret -vaultName "$($keyVault.VaultName)" -keyName "$($secretKey.name)" -keyValue "$($secretKey.value)" -keyContentType "$($secretKey.contentType)"

							if ($result.name -eq $secretKey.name) {		# в ответе вернулось имя переменной
							
								Write-Host "			result: ok"
							}
							else {
							
								Write-Host "The problem of updating the secret variable '$($secretKey.name)'!"
								throw "The problem of updating the secret variable '$($secretKey.name)'!"
							}
						}
						catch {
							Write-Host "The problem of updating the secret variable '$($secretKey.name)'!"
							throw $($_.Exception.Message)
						}	
						
					}
					else {
					
						Write-Host "		Variable '$($secretKey.name)' already exists. But its new value is empty or has not changed. Miss ..."
					}
				}
			}
	
		
			"delete" {
			
				if ( -not $secretKeyCurrentValue) {
				
					Write-Host "		Variable '$($secretKey.name)' does not exist. Skipping the deletion ..."
				}
				else {
				
					try {
						
						Write-Host "		Deleting the secret variable '$($secretKey.name)' ..."
						
						#for test: $result = Remove-AzKeyVaultSecret -VaultName "jupitertestakv" -Name "TestKeyYu" -PassThru -Force -WhatIf				# удаление
						#for test: $result = Undo-AzKeyVaultSecretRemoval -VaultName "jupitertestakv" -Name "TestKeyYu" -WhatIf							# восстановление
						
						# так прав не хватает совсем удалить: 
						# $result = Remove-AzKeyVaultSecret -VaultName "$($keyVault.VaultName)" -Name "$($secretKey.name)" -InRemovedState -PassThru -Force #-WhatIf
						
						$result = Remove-AzKeyVaultSecret -VaultName "$($keyVault.VaultName)" -Name "$($secretKey.name)" -PassThru -Force #-WhatIf
						
						if ($result.name -eq $secretKey.name) {		# в ответе вернулось имя переменной
						
							Write-Host "			result: ok"
						}
						else {
						
							Write-Host "The problem of deleting the secret variable '$($secretKey.name)'!"
							throw "The problem of deleting the secret variable '$($secretKey.name)'!"
						}
					}
					catch {
						Write-Host "The problem of deleting the secret variable '$($secretKey.name)'!"
						throw $($_.Exception.Message)
					}
				}
			}

			
			default {
			
				Write-Host "		The 'action' property of variable '$($secretKey.name)' has an invalid value!"
				throw "		The 'action' property of variable '$($secretKey.name)' has an invalid value!"
			}
			
		}	# switch

	
	}

}	
catch {
	Write-Host $($_.Exception.Message)
	throw $($_.Exception.Message)
}	
finally {

	Write-Host ""
	
	# for test:
	# Disconnect-AzAccount -Scope Process -ErrorAction Stop
	# Clear-AzContext -Scope Process -ErrorAction Stop
	# :for test
	
	Write-Host "Done"
}	


	
	




