$here = (Split-Path -Parent $MyInvocation.MyCommand.Path)
$sut = (Join-Path $here Build.Functions.ps1)

. $sut

Describe -Tag "Build.Functions" "DeleteDatabase Parameter Tests" {
	Context "Must supply a Server Instance Name (null)" {
		$exceptionThrown = $false;

		try
		{
			DeleteDatabase $null "a"
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	Context "Must supply a Server Instance Name (empty string)" {
		$exceptionThrown = $false;

		try
		{
			DeleteDatabase "" "a"
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	Context "Must supply a Database Name (null)" {
		$exceptionThrown = $false;

		try
		{
			DeleteDatabase "a" $null
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	Context "Must supply a Database Name (empty string)" {
		$exceptionThrown = $false;

		try
		{
			DeleteDatabase "a" ""
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
}

Describe -Tag "Build.Functions" "DeleteDatabase Acceptance Tests" {
	Context "Is the correct database being deleted?" {
		
		$databaseName = "Test Database"
		$q = @"
ALTER DATABASE [$databaseName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
DROP DATABASE [$databaseName]
GO	
"@
		Mock CallSql
		Mock CallSql -verifiable -parameterFilter { $query -eq $q }
		
		DeleteDatabase "TestServer" $databaseName
		
		It "Was the Mock called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Is the correct server instance being used?" {
		
		$si = "Test Instance"
		Mock CallSql
		Mock CallSql -verifiable -parameterFilter { $server -eq $si }
		
		DeleteDatabase $si "Test Database"
		
		It "Was the Mock called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Is the correct database is being used?" {
		
		Mock CallSql
		Mock CallSql -verifiable -parameterFilter { $databaseName -eq "master" }
		
		DeleteDatabase "Test" "Test Database"
		
		It "Was the Mock called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Are Username and Password null along with UseIntegratedSecurity equal to true?" {
		Mock CallSql
		Mock CallSql -verifiable -parameterFilter { $username -eq "" -and $password -eq "" -and $UseIntegratedSecurity }
		
		DeleteDatabase "Test" "Test Database"
		
		It "Was the Mock called?" {
			Assert-VerifiableMocks
		}
	}
}