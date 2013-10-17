if(-Not (Test-Path ((Split-Path -Parent $MyInvocation.MyCommand.Path) + "\UpdateAssemblyInfo.ps1")))
{
	throw new-object System.Exception("Make sure that .\UpdateAssemblyInfo.ps1 exists in the same directory as this test file.")
}

. ((Split-Path -Parent $MyInvocation.MyCommand.Path) + "\UpdateAssemblyInfo.ps1")

Describe "Reset-BuildNumber Parameter Validation Tests" {
	Context "Must supply a version number string (null)" {
		
		$exceptionThrown = $false;

		try
		{
			Reset-BuildNumber $null 1
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	
	Context "Must supply a version number string (empty string)" {
		
		$exceptionThrown = $false;

		try
		{
			Reset-BuildNumber "" 1
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}
}

Describe "Reset-BuildNumber (Unknown Format) Tests" {
	
	Context "versionNumber does not contain periods (asdfasdfasdfasdfasdf)" {
		$result = Reset-BuildNumber "asdfasdfasdfasdfasdf" 1
		
		It "Result is not null" {
			$result -eq $null | should be $false
		}
		
		It "Result should be asdfasdfasdfasdfasdf.0.1" {
			$result | should be "asdfasdfasdfasdfasdf.0.1"
		}
	}
	
	Context "versionNumber does not contain 1 period (a.0)" {
		$result = Reset-BuildNumber "a.0" 1
		
		It "Result is not null" {
			$result -eq $null | should be $false
		}
		
		It "Result should be a.0.1" {
			$result | should be "a.0.1"
		}
	}
	
	Context "versionNumber does not contain 2 periods (a.0.aa)" {
		$result = Reset-BuildNumber "a.0.aa" 6
		
		It "Result is not null" {
			$result -eq $null | should be $false
		}
		
		It "Result should be a.0.6" {
			$result | should be "a.0.6"
		}
	}
	
	Context "versionNumber does not contain 3 periods (a.0.aa.Z)" {
		$result = Reset-BuildNumber "a.0.aa.Z" 99
		
		It "Result is not null" {
			$result -eq $null | should be $false
		}
		
		It "Result should be a.0.99.Z" {
			$result | should be "a.0.99.Z"
		}
	}
	
	Context "versionNumber does not contain 4 periods (a.0.aa.Z.%)" {
		$result = Reset-BuildNumber "a.0.aa.Z.%" 10
		
		It "Result is not null" {
			$result -eq $null | should be $false
		}
		
		It "Result should be a.0.10.Z.%" {
			$result | should be "a.0.10.Z.%"
		}
	}
}