$here = (Split-Path -Parent $MyInvocation.MyCommand.Path)

#Including things that are needed within this build.
include (join-path $here "UpdateAssemblyInfo.ps1")

Properties {

	get-content logo.txt | Write-Host -ForegroundColor Magenta

	if(-Not (Test-Path Variable:\Development))
	{
		New-Item Variable:\Development -Value $true | out-null
	}
	if(-Not (Test-Path Variable:\UpdateBuildNumber))
	{
		New-Item Variable:\UpdateBuildNumber -Value $false | out-null
	}
	if(-Not (Test-Path Variable:\DoxygenOutput))
	{
		New-Item Variable:\DoxygenOutput -Value "..\DoxygenOutput" | out-null
	}
	if(-Not (Test-Path Variable:\BuildNumber))
	{
		New-Item Variable:\BuildNumber -Value 0 | out-null
	}

	Assert($Development -is [System.Boolean]) "The value of `$Development should be a boolean."
	Assert($UpdateBuildNumber -is [System.Boolean]) "The value of `$UpdateBuildNumber should be a boolean."
	Assert($DoxygenOutput -is [System.String]) "The value of `$DoxygenOutput should be a string."
	Assert($BuildNumber -is [System.Int32]) "The value of `$BuildNumber should be an Int32."
}

Task default -depends 	Task-RunPesterTests, `
						Task-UpdateAssemblyInfo, `
						Task-ResolveDependencies, `
						Task-Build, `
						Task-RunTests, `
						Task-DeployUserDatabase, `
						Task-PopulateUserDatabase, `
						Task-RunAcceptanceTests, `
						Task-BuildDocumentation

#-------------------------------
#	Run Pester Tests
#-------------------------------
Task Task-RunPesterTests {
	Invoke-Pester -outputXML (Join-Path $env:TMP IULReportsTestOutput.xml)
	$testResults = [xml](get-content (Join-Path $env:TMP IULReportsTestOutput.xml))
	
	if(Test-Path (Join-Path $env:TMP IULReportsTestOutput.xml))
	{
		Remove-Item (Join-Path $env:TMP IULReportsTestOutput.xml)
	}
	
	if(@($testResults | select-xml "//*/test-suite[descendant::failure]").Length -gt 0)
	{
		throw "Pester Tests Failed"
	}
}

#-------------------------------
#	Dependency Resolution
#-------------------------------
Task Task-ResolveDependencies {
	msbuild NuGet.targets /T:ResolveDependencyPackages
}

#-------------------------------
#	Assembly Info Update.
#-------------------------------
Task Task-UpdateAssemblyInfo {
	Assert(Test-Path Variable:\BuildDescription) "You must define the build Description before executing this task."
	Assert(Test-Path Variable:\BuildNumber) "You must define the build number before executing this task."
	
	Set-AssemblyAttributes (join-path $here "..\WhippedCream\Properties\AssemblyInfo.cs") $buildDescription $buildNumber
} -PreCondition {
	$UpdateBuildNumber -eq $true
}

#-------------------------------
#	Build
#-------------------------------
Task Task-BuildClean {
	Exec { msbuild ..\WhippedCream.sln /T:Clean }
}
Task Task-BuildDebug {
	Exec { msbuild ..\WhippedCream.sln /P:Configuration=DEBUG }
} -PreCondition {
	$Development -eq $true
}

Task Task-BuildRelease {
	Exec { msbuild ..\WhippedCream.sln /P:Configuration=RELEASE }
} -PreCondition {
	$Development -eq $false
}
Task Task-Build -depends Task-BuildClean, Task-BuildDebug, Task-BuildRelease

#-------------------------------
#	Deploy Test Database
#-------------------------------
Task Task-DeployUserDatabase {
	Invoke-PSake ..\UserDatabase\Build.ps1
}

#-------------------------------
#	Populate Test Database with dummy Data.
#-------------------------------
Task Task-PopulateUserDatabaseDebug {
	Exec {
		..\WhippedCream.Acceptance.Test\bin\Debug\WhippedCream.Acceptance.Test.exe
	}
} -Precondition { $Development -eq $true }
Task Task-PopulateUserDatabaseRelease {
	Exec {
		..\WhippedCream.Acceptance.Test\bin\Release\WhippedCream.Acceptance.Test.exe
	}
} -PreCondition { $Development -eq $false }
Task Task-PopulateUserDatabase -depends Task-PopulateUserDatabaseDebug, Task-PopulateUserDatabaseRelease

#-------------------------------
#	Run Tests
#-------------------------------
Task Task-RunTestsDebug {
	Exec {
		..\Packages\NUnit.Runners\tools\nunit-console ..\WhippedCream.Test\bin\debug\WhippedCream.Test.dll /xml=TestOutput.xml /nologo
	}
} -PreCondition { $Development -eq $true }
Task Task-RunTestsRelease {
	Exec {
		..\Packages\NUnit.Runners\tools\nunit-console ..\WhippedCream.Test\bin\release\WhippedCream.Test.dll /xml=TestOutput.xml /nologo
	}
} -PreCondition { $Development -eq $false }
Task Task-RunTests -depends Task-RunTestsDebug, Task-RunTestsRelease

#-------------------------------
#	Run Acceptance Tests
#-------------------------------
Task Task-RunAcceptanceTestsDebug {
	Exec {
		..\Packages\NUnit.Runners\tools\nunit-console ..\WhippedCream.Acceptance.Test\bin\Debug\WhippedCream.Acceptance.Test.exe /xml=AcceptanceTestOutput.xml /nologo
	}
} -PreCondition { $Development -eq $true }
Task Task-RunAcceptanceTestsRelease {
	Exec {
		..\Packages\NUnit.Runners\tools\nunit-console ..\WhippedCream.Acceptance.Test\bin\Release\WhippedCream.Acceptance.Test.exe /xml=AcceptanceTestOutput.xml /nologo
	}
} -PreCondition { $Development -eq $false }
Task Task-RunAcceptanceTests -depends Task-RunAcceptanceTestsDebug, Task-RunAcceptanceTestsRelease

#-------------------------------
#	Build Documentation
#-------------------------------
Task Task-BuildDocumentation {
	$doxyFile = @((gc Doxyfile), "OUTPUT_DIRECTORY = ""$DoxygenOutput""", "PROJECT_NUMBER = $BuildNumber", "HTML_OUTPUT = ""$DoxygenOutput""")

	Exec { 
		$doxyFile | Doxygen -
	}
}

#-------------------------------
#	NuGetPackage
#-------------------------------
Task Task-BuildNuGetPackage {
	$version = [Reflection.Assembly]::LoadFile((Resolve-Path ..\WhippedCream\Bin\Release\WhippedCream.dll)).GetName().version
	nuget pack .\WhippedCream.nuspec -Version $version.ToString()
}
Task Task-DeployNuGetPackage {
	Assert(Test-Path Variable:\NuGetApiKey) "The Variable `$NuGetApiKey must be defined"
	get-childItem *.nupkg | foreach { nuget push $_.FullName -s http://www.ikclife.com/KCLNuGetFeed/ $NuGetApiKey }
}
Task Task-CleanUpNuGetPackages {
	del *.nupkg
}
Task Task-NuGetPackage -depends Task-BuildNuGetPackage, Task-DeployNuGetPackage, Task-CleanUpNuGetPackages