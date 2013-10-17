function CallSql
{
	param(	[Parameter(Mandatory = $true)][string]$query,
			[Parameter(Mandatory = $true)][string]$server,
			[Parameter(Mandatory = $true)][string]$databaseName,
			[string]$username,
			[string]$password,
			[int]$errorLevel,
			[bool]$UseIntegratedSecurity = $true)
			
	
			
	if($UseIntegratedSecurity)
	{
		return Invoke-SqlCmd -Query $query -ServerInstance $server -Database $databaseName -ErrorLevel $errorLevel -AbortOnError
	}
	else
	{
		return Invoke-SqlCmd -Query $query -ServerInstance $server -Database $databaseName -Username $username -Password $password -ErrorLevel $errorLevel -AbortOnError
	}
}