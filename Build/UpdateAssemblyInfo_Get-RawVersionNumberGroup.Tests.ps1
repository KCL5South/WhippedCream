if(-Not (Test-Path ((Split-Path -Parent $PSCommandPath) + "\UpdateAssemblyInfo.ps1")))
{
	throw new-object System.Exception("Make sure that .\UpdateAssemblyInfo.ps1 exists in the same directory as this test file.")
}

. ((Split-Path -Parent $PSCommandPath) + "\UpdateAssemblyInfo.ps1")

Describe "Get-RawVersionNumberGroup Parameter Validation Tests" {
	Context "Must Supply An Input File (null)" {
		BeforeAll {
			$exceptionThrown = $false;

			try
			{
				Get-RawVersionNumberGroup $null "pattern"
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
	
	Context "Must Supply A Match Pattern (null)" {
		BeforeAll {
			$exceptionThrown = $false;

			try
			{
				Get-RawVersionNumberGroup "dummyfile.xml" $null
			}
			catch [System.Management.Automation.ParameterBindingException]
			{
				$exceptionThrown = $true
			}
		}

		It "Exception was Thrown" {
			$exceptionThrown | Should -Be $true
		}
	}

	Context "Must Supply An Input File (empty string)" {
		BeforeAll {
			$exceptionThrown = $false;
		
			try
			{
				Get-RawVersionNumberGroup "" "pattern"
			}
			catch [System.Management.Automation.ParameterBindingException]
			{
				$exceptionThrown = $true
			}
		}

		It "Exception was Thrown" {
			$exceptionThrown | Should -Be $true
		}
	}

	Context "Must Supply A Match Pattern (empty string)" {
		BeforeAll {
			$exceptionThrown = $false;
		
			try
			{
				Get-RawVersionNumberGroup "dummyfile.xml" ""
			}
			catch [System.Management.Automation.ParameterBindingException]
			{
				$exceptionThrown = $true
			}
		}

		It "Exception was Thrown" {
			$exceptionThrown | Should -Be $true
		}
	}
		
	Context "File Not Found" {
		BeforeAll {
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
		}

		It "Exception was thrown" {
			$exceptionThrown | Should -Be $true
		}
	}
	
	Context "GroupToReturn must not be less than zero" {
		BeforeAll {
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
		}

		It "Exception was thrown" {
			$exceptionThrown | Should -Be $true
		}
	}
	
	Context "GroupToReturn must not be less than zero (Negative Infinity)" {
		BeforeAll {
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
		}

		It "Exception was thrown" {
			$exceptionThrown | Should -Be $true
		}
	}
}

Describe "Get-RawVersionNumberGroup Results Tests" {
	Context "Returned object is [String]" {
		BeforeAll {
			$fileContent = @"
hello there
[AssemblyVersion("1.0.0.0")]
This is another line to help confuse the search...
[AssemblyVersion("2.0.0.0")]
"@
			Mock test-path { return $true }
			Mock get-content { return $fileContent } -verifiable
			
			$result = Get-RawVersionNumberGroup "DummyFile.txt" "AssemblyVersion\(""([0-9]+(\.([0-9]+|\*)){1,3})""\)"
		}
		
		It "Get-Content Mock was called" {
			Should -InvokeVerifiable
		}
		
		It "Returned Item should not be null." {
			$result -eq $null | Should -Be $false
		}
		
		It "Returned object is a [String]" {
			$result.GetType().ToString() | Should -Be "System.String"
		}
	}
	
	Context "Returns First matched String (Default Group)" {
		BeforeAll {
			$fileContent = @"
hello there
[AssemblyVersion("1.0.0.0")]
This is another line to help confuse the search...
[AssemblyVersion("2.0.0.0")]
"@
			Mock test-path { return $true }
			Mock get-content { return $fileContent } -verifiable
			
			$result = Get-RawVersionNumberGroup "DummyFile.txt" "AssemblyVersion\(""([0-9]+(\.([0-9]+|\*)){1,3})""\)"
		}
		
		It "Get-Content Mock was called" {
			Should -InvokeVerifiable
		}
		
		It "Returned Item should not be null." {
			$result -eq $null | Should -Be $false
		}
		
		It "Returned Item should be what was expected." {
			$result | Should -Be "AssemblyVersion(""1.0.0.0"")"
		}
	}
	
	Context "Returns First matched String (Second Group)" {
		BeforeAll {
			$fileContent = @"
hello there
[AssemblyVersion("1.0.0.0")]
This is another line to help confuse the search...
[AssemblyVersion("2.0.0.0")]
"@
			Mock test-path { return $true }
			Mock get-content { return $fileContent } -verifiable
			
			$result = Get-RawVersionNumberGroup "DummyFile.txt" "AssemblyVersion\(""([0-9]+(\.([0-9]+|\*)){1,3})""\)" 1
		}
		
		It "Get-Content Mock was called" {
			Should -InvokeVerifiable
		}
		
		It "Returned Item should not be null." {
			$result -eq $null | Should -Be $false
		}
		
		It "Returned Item should be what was expected." {
			$result | Should -Be "1.0.0.0"
		}
	}
	
	Context "Returns Null if there is no match" {
		BeforeAll {
			$fileContent = @"
hello there
This is another line to help confuse the search...
"@
			Mock test-path { return $true }
			Mock get-content { return $fileContent } -verifiable
			
			$result = Get-RawVersionNumberGroup "DummyFile.txt" "AssemblyVersion\(""([0-9]+(\.([0-9]+|\*)){1,3})""\)"
		}
		
		It "Get-Content Mock was called" {
			Should -InvokeVerifiable
		}
		
		It "Returned Item should be null." {
			$result -eq $null | Should -Be $true
		}
	}
	
	Context "Returns Null if content is empty" {
		BeforeAll {
			$fileContent = ""
			Mock test-path { return $true }
			Mock get-content { return $fileContent } -verifiable
			
			$result = Get-RawVersionNumberGroup "DummyFile.txt" "AssemblyVersion\(""([0-9]+(\.([0-9]+|\*)){1,3})""\)"
		}
		
		It "Get-Content Mock was called" {
			Should -InvokeVerifiable
		}
		
		It "Returned Item should be null." {
			$result -eq $null | Should -Be $true
		}
	}
}