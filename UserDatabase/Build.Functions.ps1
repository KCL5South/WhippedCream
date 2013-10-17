$here = (Split-Path -Parent $MyInvocation.MyCommand.Path)
$include = (Join-Path $here SqlHelpers.ps1)

. $include

function DeleteDatabase
{
	param(	[Parameter(Mandatory = $true)][string]$si,
			[Parameter(Mandatory = $true)][string]$dbn)
			
	$query = @"
ALTER DATABASE [$dbn] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
DROP DATABASE [$dbn]
GO	
"@

	CallSql $query $si "master" $null $null -UseIntegratedSecurity:$true
}

function GetDatabaseUrl
{
	param(	[Parameter(Mandatory = $true)][string]$server, 
			[string]$database, 
			[string]$user, 
			[string]$pw,
			[switch]$useIS)
			
	$result = "jdbc:sqlserver://{0};" -f $server
	if(![System.String]::IsNullOrEmpty($database))
	{
		$result += "databaseName={0};" -f $database
	}
	if($useIS)
	{
		$result += "integratedSecurity=true;"
	}
	else
	{
		if(![System.String]::IsNullOrEmpty($user))
		{
			$result += "Username={0};" -f $user
		}
		if(![System.String]::IsNullOrEmpty($pw))
		{
			$result += "Password={0};" -f $pw
		}
	}
	
	return $result
}

function Get_tSQLtTestClasses
{
	param(	[Parameter(Mandatory = $true)][string]$serverName,
			[Parameter(Mandatory = $true)][string]$databaseName,
			[string]$username,
			[string]$password,
			[switch]$useIntegratedSecurity)

	$query = "SELECT Name FROM [EquiFlex].[tSQLt].[TestClasses]"
	
	$testClassNames = CallSql	-server $serverName `
								-databaseName $databaseName `
								-username $username `
								-password $password `
								-useIntegratedSecurity $useIntegratedSecurity `
								-query $query

	return $testClassNames
}
