BeforeAll {
	$here = (Split-Path -Parent $PSCommandPath)
	$sut = (Join-Path $here Build.Functions.ps1)

	. $sut
}

Describe -Tag "Build.Functions" "DeleteDatabase Parameter Tests" {
	Context "Must supply a Server Instance Name (null)" {
		BeforeAll {
			$exceptionThrown = $false;

			try
			{
				DeleteDatabase $null "a"
			}
			catch [System.Management.Automation.ParameterBindingException]
			{
				$exceptionThrown = $true
			}
		}

		It "Exception Was Thrown" {
			$exceptionThrown | Should -Be $true
		}
	}
	Context "Must supply a Server Instance Name (empty string)" {
		BeforeAll {
			$exceptionThrown = $false;

			try
			{
				DeleteDatabase "" "a"
			}
			catch [System.Management.Automation.ParameterBindingException]
			{
				$exceptionThrown = $true
			}
		}

		It "Exception Was Thrown" {
			$exceptionThrown | Should -Be $true
		}
	}
	Context "Must supply a Database Name (null)" {
		BeforeAll {
			$exceptionThrown = $false;

			try
			{
				DeleteDatabase "a" $null
			}
			catch [System.Management.Automation.ParameterBindingException]
			{
				$exceptionThrown = $true
			}
		}

		It "Exception Was Thrown" {
			$exceptionThrown | Should -Be $true
		}
	}
	Context "Must supply a Database Name (empty string)" {
		BeforeAll {
			$exceptionThrown = $false;

			try
			{
				DeleteDatabase "a" ""
			}
			catch [System.Management.Automation.ParameterBindingException]
			{
				$exceptionThrown = $true
			}
		}

		It "Exception Was Thrown" {
			$exceptionThrown | Should -Be $true
		}
	}
}

Describe -Tag "Build.Functions" "DeleteDatabase Acceptance Tests" {
	Context "Is the correct database being deleted?" {
		BeforeAll {
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
		}
		
		It "Was the Mock called?" {
			Should -InvokeVerifiable
		}
	}
	Context "Is the correct server instance being used?" {
		BeforeAll {
			$si = "Test Instance"
			Mock CallSql
			Mock CallSql -verifiable -parameterFilter { $server -eq $si }
			
			DeleteDatabase $si "Test Database"
		}
		
		It "Was the Mock called?" {
			Should -InvokeVerifiable
		}
	}
	Context "Is the correct database is being used?" {
		BeforeAll {
			Mock CallSql
			Mock CallSql -verifiable -parameterFilter { $databaseName -eq "master" }
			
			DeleteDatabase "Test" "Test Database"
		}
		
		It "Was the Mock called?" {
			Should -InvokeVerifiable
		}
	}
	Context "Are Username and Password null along with UseIntegratedSecurity equal to true?" {
		BeforeAll {
			Mock CallSql
			Mock CallSql -verifiable -parameterFilter { $username -eq "" -and $password -eq "" -and $UseIntegratedSecurity }
			
			DeleteDatabase "Test" "Test Database"
		}
		
		It "Was the Mock called?" {
			Should -InvokeVerifiable
		}
	}
}