$here = (Split-Path -Parent $MyInvocation.MyCommand.Path)
$sut = (Join-Path $here Build.Functions.ps1)

. $sut

#------------------------Get_tSQLtTestClasses Tests------------------------#
Describe -Tag "Build.Functions" "Get_tSQLtTestClasses Parameter Tests" {
	Context "We must supply a ServerInstance (null)" {
		$exceptionThrown = $false;
		
		try
		{
			Get_tSQLtTestClasses $null "database"
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	Context "We must supply a ServerInstance (empty string)" {
		$exceptionThrown = $false;

		try
		{
			Get_tSQLtTestClasses "" "database"
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	Context "We must supply a DatabaseName (null)" {
		$exceptionThrown = $false;

		try
		{
			Get_tSQLtTestClasses "server" $null
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	Context "We must supply a DatabaseName (empty string)" {
		$exceptionThrown = $false;

		try
		{
			Get_tSQLtTestClasses "server" ""
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
