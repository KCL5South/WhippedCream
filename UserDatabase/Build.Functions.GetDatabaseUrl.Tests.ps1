BeforeAll {
	$here = (Split-Path -Parent $PSCommandPath)
	$sut = (Join-Path $here Build.Functions.ps1)

	. $sut
}

Describe -Tag "Build.Functions" "GetDatabaseUrl Parameter Tests" {
	Context "Must supply a Server Instance Name (null)" {
		BeforeAll {
			$exceptionThrown = $false;

			try
			{
				GetDatabaseUrl $null
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
				GetDatabaseUrl ""
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

Describe -Tag "Build.Functions" "GetDatabaseUrl Acceptance Tests" {
	Context "Is the ServerInstance being represented correctly?" {
		BeforeAll {
			$si = "TestServer"
			
			$result = GetDatabaseUrl $si
		}
		
		It "Result should not be null?" {
			$result -eq $null | Should -Be $false
		}
		
		It "Is the result correct?" {
			$result | Should -Be ("jdbc:sqlserver://{0};" -f $si)
		}
	}
	Context "Is the Database being represented correctly?" {
		BeforeAll {
			$si = "TestServer"
			$db = "TestDB"
			
			$result = GetDatabaseUrl $si -database $db
		}
		
		It "Result should not be null?" {
			$result -eq $null | Should -Be $false
		}
		
		It "Is the result correct?" {
			$result | Should -Be ("jdbc:sqlserver://{0};databaseName={1};" -f $si, $db)
		}
	}
	Context "Is the Username being represented correctly?" {
		BeforeAll {
			$si = "TestServer"
			$u = "TestRandomUser"
			
			$result = GetDatabaseUrl $si -user $u
		}
		
		It "Result should not be null?" {
			$result -eq $null | Should -Be $false
		}
		
		It "Is the result correct?" {
			$result | Should -Be ("jdbc:sqlserver://{0};Username={1};" -f $si, $u)
		}
	}
	Context "Is the Password being represented correctly?" {
		BeforeAll {
			$si = "TestServer"
			$p = "TestRandomPassword"
			
			$result = GetDatabaseUrl $si -pw $p
		}
		
		It "Result should not be null?" {
			$result -eq $null | Should -Be $false
		}
		
		It "Is the result correct?" {
			$result | Should -Be ("jdbc:sqlserver://{0};Password={1};" -f $si, $p)
		}
	}
	Context "Is Integrated Security being represented correctly?" {
		BeforeAll {
			$si = "TestServer"
			
			$result = GetDatabaseUrl $si -useIs:$true
		}
		
		It "Result should not be null?" {
			$result -eq $null | Should -Be $false
		}
		
		It "Is the result correct?" {
			$result | Should -Be ("jdbc:sqlserver://{0};integratedsecurity=true;" -f $si)
		}
	}
	Context "If I define Integrated Security, is the username and password being skipped?" {
		BeforeAll {
			$si = "TestServer"
			
			$result = GetDatabaseUrl $si -user "TestUser" -pw "TestPassword" -useIs:$true
		}
		
		It "Result should not be null?" {
			$result -eq $null | Should -Be $false
		}
		
		It "Is the result correct?" {
			$result | Should -Be ("jdbc:sqlserver://{0};integratedsecurity=true;" -f $si)
		}
	}
	Context "Is ServerInstance, Database, and integrated Security all being represented together?" {
		BeforeAll {
			$si = "TestServer"
			$db = "TestRoDB"
			
			$result = GetDatabaseUrl $si -database $db -useIs:$true
		}
		
		It "Result should not be null?" {
			$result -eq $null | Should -Be $false
		}
		
		It "Is the result correct?" {
			$result | Should -Be ("jdbc:sqlserver://{0};databaseName={1};integratedsecurity=true;" -f $si, $db)
		}
	}
	Context "Is ServerInstance Database, Username, and Password all being represented together?" {
		BeforeAll {
			$si = "TestServer"
			$db = "TestRoDB"
			$u = "TestUser"
			$p = "TestPassword"
			
			$result = GetDatabaseUrl $si -database $db -user $u -pw $p -useIs:$false
		}
		
		It "Result should not be null?" {
			$result -eq $null | Should -Be $false
		}
		
		It "Is the result correct?" {
			$result | Should -Be ("jdbc:sqlserver://{0};databaseName={1};Username={2};Password={3};" -f $si, $db, $u, $p)
		}
	}
}