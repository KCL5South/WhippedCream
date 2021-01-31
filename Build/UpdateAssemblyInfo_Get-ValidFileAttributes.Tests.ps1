if(-Not (Test-Path ((Split-Path -Parent $PSCommandPath) + "\UpdateAssemblyInfo.ps1")))
{
	throw new-object System.Exception("Make sure that .\UpdateAssemblyInfo.ps1 exists in the same directory as this test file.")
}

. ((Split-Path -Parent $PSCommandPath) + "\UpdateAssemblyInfo.ps1")

Describe "Get-ValidFileAttributes Acceptance Tests" {
	Context "Hidden is Valid" {
		BeforeAll{
			$file = "TestPath:\temp.tmp"
			
			$validAttributes = Get-ValidFileAttributes ([System.IO.FileAttributes]::Hidden -bor [System.IO.FileAttributes]::NotContentIndexed )
		}
		
		It "Result should not be null" {
			$validAttributes -eq $null | Should -Be $false
		}
		
		It "Hidden should be the only attribute that suvived." {
			$validAttributes | Should -Be ([System.IO.FileAttributes]::Hidden)
		}
	}
	Context "ReadOnly is Valid" {
		BeforeAll {
			$file = "TestPath:\temp.tmp"
			
			$validAttributes = Get-ValidFileAttributes ([System.IO.FileAttributes]::ReadOnly -bor [System.IO.FileAttributes]::Temporary -bor [System.IO.FileAttributes]::Offline)
		}
		
		It "Result should not be null" {
			$validAttributes -eq $null | Should -Be $false
		}
		
		It "ReadOnly should be the only attribute that suvived." {
			$validAttributes | Should -Be ([System.IO.FileAttributes]::ReadOnly)
		}
	}
	Context "System is Valid" {
		BeforeAll {
			$file = "TestPath:\temp.tmp"
			
			$validAttributes = Get-ValidFileAttributes ([System.IO.FileAttributes]::System -bor [System.IO.FileAttributes]::Normal)
		}
		
		It "Result should not be null" {
			$validAttributes -eq $null | Should -Be $false
		}
		
		It "System should be the only attribute that suvived." {
			$validAttributes | Should -Be ([System.IO.FileAttributes]::System)
		}
	}
	Context "Archive is Valid" {
		BeforeAll {
			$file = "TestPath:\temp.tmp"
			
			$validAttributes = Get-ValidFileAttributes ([System.IO.FileAttributes]::Archive -bor [System.IO.FileAttributes]::Device)
		}
		
		It "Result should not be null" {
			$validAttributes -eq $null | Should -Be $false
		}
		
		It "Archive should be the only attribute that suvived." {
			$validAttributes | Should -Be ([System.IO.FileAttributes]::Archive)
		}
	}
}