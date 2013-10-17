$here = (Split-Path -Parent $MyInvocation.MyCommand.Path)
$sut = (Join-Path $here SqlHelpers.ps1)

. $sut

Describe -Tag "SqlHelpers" "CallSql Parameter Tests." {
	Context "Must supply a query. (null)" {
		$exceptionThrown = $false;

		try
		{
			CallSql $null "a" "a"
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	Context "Must supply a query. (emkpty string)" {
		$exceptionThrown = $false;

		try
		{
			CallSql "" "a" "a"
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	Context "Must supply a server. (null)" {
		$exceptionThrown = $false;

		try
		{
			CallSql "a" $null "a"
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	Context "Must supply a server. (empty string)" {
		$exceptionThrown = $false;

		try
		{
			CallSql "a" "" "a"
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	Context "Must supply a database name. (null)" {
		$exceptionThrown = $false;

		try
		{
			CallSql "a" "a" $null
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	Context "Must supply a database name. (empty string)" {
		$exceptionThrown = $false;

		try
		{
			CallSql "a" "a" ""
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

Describe -Tag "SqlHelpers" "CallSql Acceptance Tests." {
	Context "Using Integrated Security Causes username and password to not be used." {
        Mock Invoke-SqlCmd -verifiable -parameterFilter { $UseIntegratedSecurity -eq $true -and $username -eq "" -and $password -eq "" }
		Mock Invoke-SqlCmd { }
		
		CallSql -query "a" -Server "a" -databasename "a" -Username "DummyUsername" -password "DummyPassword" -UseIntegratedSecurity $true
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Not using Integrated Security Causes username and password to be used." {
        Mock Invoke-SqlCmd -verifiable -parameterFilter { $UseIntegratedSecurity -eq $false -and $username -eq "DummyUsername" -and $password -eq "DummyPassword" }
		Mock Invoke-SqlCmd { }
		
		CallSql "a" "a" "a" "DummyUsername" "DummyPassword" -UseIntegratedSecurity $false
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Is the query being used the one sent?" {
		Mock Invoke-SqlCmd -verifiable -parameterFilter { $query -eq "a" }
		Mock Invoke-SqlCmd { }
		
		CallSql "a" "b" "c" "d" "e" -UseIntegratedSecurity $false
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Is the server name being used the one sent?" {
		Mock Invoke-SqlCmd -verifiable -parameterFilter { $server -eq "b" }
		Mock Invoke-SqlCmd { }
		
		CallSql "a" "b" "c" "d" "e" -UseIntegratedSecurity $false
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Is the database name being used the one sent?" {
		Mock Invoke-SqlCmd -verifiable -parameterFilter { $databaseName -eq "c" }
		Mock Invoke-SqlCmd { }
		
		CallSql "a" "b" "c" "d" "e" -UseIntegratedSecurity $false
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Is the username name being used the one sent?" {
		Mock Invoke-SqlCmd -verifiable -parameterFilter { $username -eq "d" }
		Mock Invoke-SqlCmd { }
		
		CallSql "a" "b" "c" "d" "e" -UseIntegratedSecurity $false
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Is the password name being used the one sent?" {
		Mock Invoke-SqlCmd -verifiable -parameterFilter { $password -eq "e" }
		Mock Invoke-SqlCmd { }
		
		CallSql "a" "b" "c" "d" "e" -UseIntegratedSecurity $false
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Is the query being used the one sent when we use integrated security?" {
		Mock Invoke-SqlCmd -verifiable -parameterFilter { $query -eq "a" }
		Mock Invoke-SqlCmd { }
		
		CallSql "a" "b" "c" "d" "e" -UseIntegratedSecurity $true
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Is the server name being used the one sent when we use integrated security?" {
		Mock Invoke-SqlCmd -verifiable -parameterFilter { $server -eq "b" }
		Mock Invoke-SqlCmd { }
		
		CallSql "a" "b" "c" "d" "e" -UseIntegratedSecurity $true
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Is the database name being used the one sent when we use integrated security?" {
		Mock Invoke-SqlCmd -verifiable -parameterFilter { $databaseName -eq "c" }
		Mock Invoke-SqlCmd { }
		
		CallSql "a" "b" "c" "d" "e" -UseIntegratedSecurity $true
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Are we passing the correct ErrorLevel?" {
		Mock Invoke-SqlCmd -verifiable -parameterFilter { $ErrorLevel -eq 10 }
		Mock Invoke-SqlCmd -verifiable -parameterFilter { $UseIntegratedSecurity -eq $false -and $ErrorLevel -eq 101 }
		Mock Invoke-SqlCmd { }
		
		CallSql "a" "b" "c" -ErrorLevel 10
		CallSql "a" "b" "c" "d" "e" -ErrorLevel 101 -UseIntegratedSecurity $false
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
	Context "Are we passing the correct AbortOnError value?" {
		Mock Invoke-SqlCmd -verifiable -parameterFilter { $AbortOnError -eq $true }
		Mock Invoke-SqlCmd { }
		
		CallSql "a" "b" "c"
		
		It "Were the mocks called?" {
			Assert-VerifiableMocks
		}
	}
}