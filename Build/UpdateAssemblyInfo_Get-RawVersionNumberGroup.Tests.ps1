if(-Not (Test-Path ((Split-Path -Parent $MyInvocation.MyCommand.Path) + "\UpdateAssemblyInfo.ps1")))
{
	throw new-object System.Exception("Make sure that .\UpdateAssemblyInfo.ps1 exists in the same directory as this test file.")
}

. ((Split-Path -Parent $MyInvocation.MyCommand.Path) + "\UpdateAssemblyInfo.ps1")

Describe "Get-RawVersionNumberGroup Parameter Validation Tests" {
	Context "Must Supply An Input File (null)" {
		$exceptionThrown = $false;

		try
		{
			Get-RawVersionNumberGroup $null "pattern"
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception Was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	
	Context "Must Supply A Match Pattern (null)" {
		$exceptionThrown = $false;

		try
		{
			Get-RawVersionNumberGroup "dummyfile.xml" $null
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}

	Context "Must Supply An Input File (empty string)" {
		$exceptionThrown = $false;
		try
		{
			Get-RawVersionNumberGroup "" "pattern"
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}

	Context "Must Supply A Match Pattern (empty string)" {
		$exceptionThrown = $false;
		try
		{
			Get-RawVersionNumberGroup "dummyfile.xml" ""
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}
		
	Context "File Not Found" {
		$exceptionThrown = $false;
		Mock Test-Path {return $false } -verifiable -parameterfilter { $Path -eq ".\DummyFile.txt" }
		
		try
		{
			Get-RawVersionNumberGroup ".\DummyFile.txt" "Dummy Pattern"
		}
		catch [System.IO.FileNotFoundException]
		{
			$exceptionThrown = $true
		}

		It "Exception was thrown" {
			$exceptionThrown | should be $true
		}
	}
	
	Context "GroupToReturn must not be less than zero" {
		$exceptionThrown = $false;
		Mock Test-Path {return $true }
		
		try
		{
			Get-RawVersionNumberGroup ".\DummyFile.txt" "Dummy Pattern" -1
		}
		catch [System.ArgumentException]
		{
			$exceptionThrown = $true
		}

		It "Exception was thrown" {
			$exceptionThrown | should be $true
		}
	}
	
	Context "GroupToReturn must not be less than zero (Negative Infinity)" {
		$exceptionThrown = $false;
		Mock Test-Path {return $true }
		
		try
		{
			Get-RawVersionNumberGroup ".\DummyFile.txt" "Dummy Pattern" ([System.Int32]::MinValue)
		}
		catch [System.ArgumentException]
		{
			$exceptionThrown = $true
		}

		It "Exception was thrown" {
			$exceptionThrown | should be $true
		}
	}
}

Describe "Get-RawVersionNumberGroup Results Tests" {
	Context "Returned object is [String]" {
		$fileContent = @"
hello there
[AssemblyVersion("1.0.0.0")]
This is another line to help confuse the search...
[AssemblyVersion("2.0.0.0")]
"@
		Mock test-path { return $true }
		Mock get-content { return $fileContent } -verifyable
		
		$result = Get-RawVersionNumberGroup "DummyFile.txt" "AssemblyVersion\(""([0-9]+(\.([0-9]+|\*)){1,3})""\)"
		
		It "Get-Content Mock was called" {
			Assert-MockCalled get-content
		}
		
		It "Returned Item should not be null." {
			$result -eq $null | Should Be $false
		}
		
		It "Returned object is a [String]" {
			$result.GetType().ToString() | should be "System.String"
		}
	}
	
	Context "Returns First matched String (Default Group)" {
		$fileContent = @"
hello there
[AssemblyVersion("1.0.0.0")]
This is another line to help confuse the search...
[AssemblyVersion("2.0.0.0")]
"@
		Mock test-path { return $true }
		Mock get-content { return $fileContent } -verifyable
		
		$result = Get-RawVersionNumberGroup "DummyFile.txt" "AssemblyVersion\(""([0-9]+(\.([0-9]+|\*)){1,3})""\)"
		
		It "Get-Content Mock was called" {
			Assert-MockCalled get-content
		}
		
		It "Returned Item should not be null." {
			$result -eq $null | Should Be $false
		}
		
		It "Returned Item should be what was expected." {
			$result | should be "AssemblyVersion(""1.0.0.0"")"
		}
	}
	
	Context "Returns First matched String (Second Group)" {
		$fileContent = @"
hello there
[AssemblyVersion("1.0.0.0")]
This is another line to help confuse the search...
[AssemblyVersion("2.0.0.0")]
"@
		Mock test-path { return $true }
		Mock get-content { return $fileContent } -verifyable
		
		$result = Get-RawVersionNumberGroup "DummyFile.txt" "AssemblyVersion\(""([0-9]+(\.([0-9]+|\*)){1,3})""\)" 1
		
		It "Get-Content Mock was called" {
			Assert-MockCalled get-content
		}
		
		It "Returned Item should not be null." {
			$result -eq $null | Should Be $false
		}
		
		It "Returned Item should be what was expected." {
			$result | should be "1.0.0.0"
		}
	}
	
	Context "Returns Null if there is no match" {
		$fileContent = @"
hello there
This is another line to help confuse the search...
"@
		Mock test-path { return $true }
		Mock get-content { return $fileContent } -verifyable
		
		$result = Get-RawVersionNumberGroup "DummyFile.txt" "AssemblyVersion\(""([0-9]+(\.([0-9]+|\*)){1,3})""\)"
		
		It "Get-Content Mock was called" {
			Assert-MockCalled get-content
		}
		
		It "Returned Item should be null." {
			$result -eq $null | should be $true
		}
	}
	
	Context "Returns Null if content is empty" {
		$fileContent = ""
		Mock test-path { return $true }
		Mock get-content { return $fileContent } -verifyable
		
		$result = Get-RawVersionNumberGroup "DummyFile.txt" "AssemblyVersion\(""([0-9]+(\.([0-9]+|\*)){1,3})""\)"
		
		It "Get-Content Mock was called" {
			Assert-MockCalled get-content
		}
		
		It "Returned Item should be null." {
			$result -eq $null | should be $true
		}
	}
}