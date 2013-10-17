function Get-RawVersionNumberGroup
{
<#
.SYNOPSIS
    This method, simply retrieves the first regular expression match in a file, then 
	(given the $groupToReturn parameter) returns the requested group from that match.
.DESCRIPTION
    If the parameter $matchPattern finds anything within the file, and the parameter
	$groupToReturn is not greater than the number of groups within the match
	(see [System.Text.RegularExpressions.Match]), then the group sepecified by the parameter
	$groupToReturn is returned.  Otherwise $null is returned.
.EXAMPLE
    Get-RawVersionNumberGroup <file path> <regular expression pattern>
.NOTES
    If the file represented by $file doesn't exist, a [System.IO.FileNotFoundException] is thrown.
	
	If $groupToReturn is negative, a [System.ArgumentException] is thrown.
#>
	param(	[Parameter(Mandatory = $true, HelpMessage = "A path to a file to search the content of.")][string]$file,
			[Parameter(Mandatory = $true, HelpMessage = "A regular expression parameter used to search the given file.")][string]$matchPattern,
			[Parameter(HelpMessage = "The group to return from the System.Text.RegularExpressions.Match.Groups collection.")][int]$groupToReturn = 0)

	if(-Not (Test-Path $file))
	{
		throw new-object System.IO.FileNotFoundException($file)
	}
	if($groupToReturn -lt 0)
	{
		throw new-object System.ArgumentException("Must not be less than zero.", "groupToReturn")
	}
	
	$resultingMatches = (get-content $file | select-string -pattern $matchPattern | select -first 1 | % { $_.Matches })
	if($resultingMatches -ne $null)
	{
		if($resultingMatches.Groups.Count -ge $groupToReturn)
		{
			return $resultingMatches.Groups[$groupToReturn].Value
		}
	}
	
	return $null
}

function Reset-BuildNumber
{
<#
.SYNOPSIS
    This method replaces the third item in a period delimited array of strings with
	the value of $newBuildNumber.
.DESCRIPTION
    This method first splits $versioNumber around periods (.) within the string.  
	
	For example:
	"1.s.5.gg" -> @("1", "s", "5", "gg")
	
	Then, the third item in the array is replaced by $newBuildNumber.
	Then, The resulting array is joined again with periods as the delimiters.
	
	If the split array results in less than three items, then enough items to get to three items are added
	to the array, in order to allow the rest of the operations to continue.
.EXAMPLE
    Reset-BuildNumber "4.1.5.100" 6 - >	"4.1.6.100"
.EXAMPLE
	Reset-BuildNumber "asdfasdf" 10 -> "asdfasdf.0.10"
.EXAMPLE
	Reset-BuildNumber "a.b" 99 -> "a.b.99"
.NOTES
    None
#>
	param(	[Parameter(Mandatory = $true, HelpMessage = "A string representing an assembly version number.")][string]$versionNumber,
			[Parameter(Mandatory = $true, HelpMessage = "The build number to use to replace the third item in the version number.")][int]$newBuildNumber)
	
	$splits = $versionNumber.Split(".")
	
	switch($splits.Length)
	{
		1 {
			$splits = @($splits[0], "0", $newBuildNumber)
		}
		2 {
			$splits = @($splits[0], $splits[1], $newBuildNumber)
		}
		default {
			$splits[2] = $newBuildNumber
		}
	}
	
	return [System.String]::Join(".", $splits)
}

function Get-ValidFileAttributes
{
<#
.SYNOPSIS
    Only ReadOnly, Archive, System, and Hidden attributes are valid within powershell.  This 
	function simply filters $attributes to a combination of those four.
.EXAMPLE
	Get-ValidFileAttributes (ReadOnly | Archive | Temporary) -> (ReadOnly | Archive)
	
	Since Temporary isn't one of the four excepted attributes, it is filtered out.
#>
	param(	[Parameter(Mandatory = $true, HelpMessage = "The Attributes to filter")][System.IO.FileAttributes]$attributes)
	
	return ($attributes -band ([System.IO.FileAttributes]::Hidden -bor [System.IO.FileAttributes]::ReadOnly -bor [System.IO.FileAttributes]::Archive -bor [System.IO.FileAttributes]::System))
}

function Set-AssemblyAttributes
{
<#
.SYNOPSIS
	This function is used to update the AssemblyVersion, AssemblyFileVersion, and AssemblyDescription attributes of a
	given file ($filePath) if they exist.
.NOTE
	If the file represented by $filePath doesn't exist, a [System.IO.FileNotFoundException] is thrown.
	
	If the $buildNumber is negative, a [System.ArgumentException] is thrown.
	
	If the $buildNumber is greater than [System.Int16]::MaxValue - 1 a [System.ArgumentException] is thrown.

	There is an attempt to save the original file's attributes, however this is limited to the valid attributes.
	(See Get-ValidFileAttributes)
#>
	param(	[Parameter(Mandatory = $true, HelpMessage = "The File to update.")][string]$filePath,
			[Parameter(Mandatory = $true, HelpMessage = "The Description to add to the AssemblyDescription Attribute.")][string]$description,
			[Parameter(Mandatory = $true, HelpMessage = "The new Build number.")][int]$buildNumber)
			
	if(-Not (Test-Path $filePath))
	{
		throw new-object System.IO.FileNotFoundException("$filePath")
	}
	if($buildNumber -lt 0)
	{
		throw new-object System.ArgumentException("Build Number must be greater than or equal to zero.")
	}
	if($buildNumber -gt ([System.Int16]::MaxValue - 1))
	{
		throw new-object System.ArgumentException([System.String]::Format("Build Number should not be greater than {0}", ([System.Int16]::MaxValue - 1)))
	}
	
    $assemblyVersionPattern = 'AssemblyVersion\("([0-9]+(\.([0-9]+|\*)){1,3})"\)'
    $assemblyFileVersionPattern = 'AssemblyFileVersion\("([0-9]+(\.([0-9]+|\*)){1,3})"\)'
	$assemblyDescriptionPattern = 'AssemblyDescription\("[^"\r\n]*"\)'
	
	$assemblyVersion = Get-RawVersionNumberGroup $filePath $assemblyVersionPattern
	$assemblyFileVersion = Get-RawVersionNumberGroup $filePath $assemblyFileVersionPattern
	
	$newAssemblyVersion = ""
	$newAssemblyFileVersion = ""
	if($assemblyVersion -ne $null)
	{
		$newAssemblyVersion = Reset-BuildNumber $assemblyVersion $buildNumber
	}
	if($assemblyFileVersion -ne $null)
	{
		$newAssemblyFileVersion = Reset-BuildNumber $assemblyFileVersion $buildNumber
	}
	$newDescription = "AssemblyDescription(""" + $description + """)"
	
	$fileFullPath = (Get-ChildItem $filePath).FullName
	$tempFilePath = $fileFullPath + ".tmp"
	
	Get-Content $fileFullPath |  
        %{$_ -replace $assemblyVersionPattern, $newAssemblyVersion } |
        %{$_ -replace $assemblyFileVersionPattern, $newAssemblyFileVersion }  |
		%{$_ -replace $assemblyDescriptionPattern, $newDescription } |
		Set-Content $tempFilePath
				
	$currentFileAttributes = Get-ValidFileAttributes ((Get-ItemProperty -Path $fileFullPath -Name Attributes).Attributes)
	#Overwrite the file
	Move-Item $tempFilePath $fileFullPath -Force
	#Reset the attributes
	Set-ItemProperty -Path $fileFullPath -Name Attributes -Value $currentFileAttributes
}