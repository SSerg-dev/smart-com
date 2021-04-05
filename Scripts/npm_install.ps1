try {
	echo "smartcom npm install ..."
	$sourceDirectory = 'C:\Program Files\nodejs\node_modules_global\*'
	$destinationDirectory = $args[0]
	Copy-item -Force -Recurse $sourceDirectory -Destination $destinationDirectory
	echo "   ... done"
}
catch {
	throw $_.Exception.Message
}
	