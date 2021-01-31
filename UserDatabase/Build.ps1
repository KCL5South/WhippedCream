$here = (Split-Path -Parent $PSCommandPath)

#-------------------------------
#	Includes
#-------------------------------
Include (Join-Path $here Build.Functions.ps1)

Properties {
	. (join-path $here Build.DevelopmentParameters.ps1)
	if(-Not (Test-Path Variable:\x64Java))
	{
		$x64Java = $false
	}
}

Task default -Depends Deploy

Task Clean -Depends RunPesterTests, `
					EnvironmentVariablesSqljdbc_x86, `
					EnvironmentVariablesSqljdbc_x64, `
					EnvironmentVariablesLiquibase, `
					LoadUpSQLPS, `
					DeleteUserDatabaseTestDatabase
Task Build -Depends Clean, `
					CreateUserDatabaseTestDatabase, `
					DeployUserDatabase
Task Test -Depends 	Build
Task Deploy -Depends Test


#-------------------------------
#	Run Pester Tests
#-------------------------------
Task RunPesterTests {
	Invoke-Pester -Output Detailed -CI
	$testResults = [xml](get-content testResults.xml)
	
	if(Test-Path testResults.xml)
	{
		Remove-Item testResults.xml
	}
	
	if(@($testResults | select-xml "//*/test-suite[descendant::failure]").Length -gt 0)
	{
		throw "Pester Tests Failed"
	}
}

#-------------------------------
#	EnvironmentVariablesSqljdbc_x86

#	Make sure that the Sqljdbc_auth.dll is available in the system's path so that a 32-bit 
#	version of java can pick it up.
#-------------------------------
Task EnvironmentVariablesSqljdbc_x86 {
	$env:Path += ";" + (Resolve-Path $here\..\Dependencies\sqljdbc_4.0\enu\auth\x86)
} -PreCondition {
	(@($env:Path.Split(";") | Where { Test-Path (Join-Path $_ sqljdbc_auth.dll) }).Length -eq 0) -and (-Not $x64Java) 
}

#-------------------------------
#	EnvironmentVariablesSqljdbc_x64

#	Make sure that the Sqljdbc_auth.dll is available in the system's path so that a 64-bit 
#	version of java can pick it up.
#-------------------------------
Task EnvironmentVariablesSqljdbc_x64 {
	$env:Path += ";" + (Resolve-Path $here\..\Dependencies\sqljdbc_4.0\enu\auth\x64)
} -PreCondition {
	(@($env:Path.Split(";") | Where { Test-Path (Join-Path $_ sqljdbc_auth.dll) }).Length -eq 0) -and $x64Java 
}

#-------------------------------
#	EnvironmentVariablesLiquibase

#	Make sure that liquibase is available.
#-------------------------------
Task EnvironmentVariablesLiquibase {
	$env:Path += ";" + (Resolve-Path $here\..\Dependencies\liquibase)
} -PreCondition { -Not ($env:Path -match "\\Dependencies\\liquibase") }

#-------------------------------
#	LoadUpSQLPS

#	Make sure that SQLPS is loaded.
#-------------------------------
Task LoadUpSQLPS {
	$location = get-location
	import-module SQLPS 3> $null #Notice I'm supressing the warning message on import.
	set-location $location
} -PreCondition { @(Get-Module | ? { $_.Name.ToLower() -eq "sqlps" }).Length -eq 0 }

#-------------------------------
#	DeleteEquiFlexTestDatabase

#	If the EquiFlex database already exists locally, delete it.
#-------------------------------
Task DeleteUserDatabaseTestDatabase {
	Assert($ServerInstance -ne $null) @"
You must supply a `$ServerInstance parameter in order to execute this task.
"@
	DeleteDatabase $ServerInstance "UserDatabase"
} -PreCondition { 
	(Test-Path "SQLSERVER:\SQL\$ServerInstance\Default\databases\UserDatabase")
}

#-------------------------------
#	CreateEquiFlexTestDatabase

#	Creates the EquiFlex Test Database.
#-------------------------------
Task CreateUserDatabaseTestDatabase {
	Invoke-SqlCmd -query "CREATE DATABASE UserDatabase" -ServerInstance $ServerInstance
} -PreCondition {
	!(Test-Path "SQLSERVER:\SQL\$ServerInstance\Default\databases\UserDatabase")
}

#-------------------------------
#	DeployEquiFlex

#	Deploys the EquiFlex Database.
#-------------------------------
Task DeployUserDatabase {
	
	Assert(Test-Path Variable:\ServerInstance) "You must supply a `$ServerInstance parameter in order to execute this task."
	
	$changeLog = (Resolve-Path $here\UserDatabase.xml)
	#$changeLog = (Join-Path $here "UserDatabaseData.xml")
	$sqljbdc4Jar = (Resolve-Path $here\..\Dependencies\sqljdbc_4.0\enu\sqljdbc4.jar)
	$url = GetDatabaseUrl "localhost" "UserDatabase" $null $null -UseIS:$true
	
	$executable = "liquibase "
	$executable += "--changeLogFile=""$changeLog"" "
	$executable += "--classpath=""$sqljbdc4Jar"" "
	$executable += "--driver=com.microsoft.sqlserver.jdbc.SQLServerDriver "
	$executable += """--url=$url"" "
	$executable += "updateTestingRollback"
	#$executable += "--diffTypes=""data"" "
	#$executable += "generateChangeLog"

	Exec { 
		Cmd /c $executable
	}
} 