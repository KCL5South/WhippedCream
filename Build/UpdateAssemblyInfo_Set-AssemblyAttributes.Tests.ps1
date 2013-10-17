if(-Not (Test-Path ((Split-Path -Parent $MyInvocation.MyCommand.Path) + "\UpdateAssemblyInfo.ps1")))
{
	throw new-object System.Exception("Make sure that .\UpdateAssemblyInfo.ps1 exists in the same directory as this test file.")
}

. ((Split-Path -Parent $MyInvocation.MyCommand.Path) + "\UpdateAssemblyInfo.ps1")

Describe "Set-AssemblyAttributes Parameters Tests" {
	Context "Must supply a File Path (null)" {
		$exceptionThrown = $false;

		try
		{
			Set-AssemblyAttributes $null "a" 0
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	
	Context "Must supply a File Path (empty string)" {
		$exceptionThrown = $false;

		try
		{
			Set-AssemblyAttributes "" "a" 0
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	
	Context "Must supply a Description (null)" {
		$exceptionThrown = $false;

		try
		{
			Set-AssemblyAttributes "a" $null 0
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	
	Context "Must supply a Description (empty string)" {
		$exceptionThrown = $false;

		try
		{
			Set-AssemblyAttributes "a" "" 0
		}
		catch [System.Management.Automation.ParameterBindingException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	
	Context "Throw an error if file is not found." {
		$exceptionThrown = $false;
		Mock Test-Path { return $false }
		
		try
		{
			Set-AssemblyAttributes "a" "a" 1
		}
		catch [System.IO.FileNotFoundException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	
	Context "Must supply a Build Number Greater or Equal to 0. (Negative 1)" {
		$exceptionThrown = $false;
		Mock Test-Path { return $true }
		try
		{
			Set-AssemblyAttributes "a" "a" -1
		}
		catch [System.ArgumentException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	
	Context "Must supply a Build Number Greater or Equal to 0. (Negative Infinity)" {
		$exceptionThrown = $false;
		Mock Test-Path { return $true }

		try
		{
			Set-AssemblyAttributes "a" "a" ([System.Int16]::MinValue)
		}
		catch [System.ArgumentException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}
	
	Context "Make sure that the maximum build number + 1 throws an error." {
		$exceptionThrown = $false;
		Mock Test-Path { return $true }

		try
		{
			Set-AssemblyAttributes "a" "a" ([System.Int16]::MaxValue)
		}
		catch [System.ArgumentException]
		{
			$exceptionThrown = $true
		}

		It "Exception was Thrown" {
			$exceptionThrown | should be $true
		}
	}
}

Describe "Set-AssemblyAttributes Integration Tests" {
	
	Context "Make sure the AssemblyVersion attribute is updated." {
		#Arrange
		$searchRoot = "TestDrive:\"
		$buildNumber = 10
		$description = "Test Description"
		Set-Content -Path "TestDrive:\AssemblyInfo.cs" -Value @"
[AssemblyVersion("1.0.0.0")]
"@
		$expectedContent = @"
[AssemblyVersion("1.0.$buildNumber.0")]
"@

		#Act
		Set-AssemblyAttributes	"TestDrive:\AssemblyInfo.cs" $description $buildNumber
		
		#Assert
		It "The content of the new file should be updated." {
			(Get-Content "TestDrive:\AssemblyInfo.cs") -join ([System.Environment]::NewLine) | should be $expectedContent
		}
	}
	
	Context "Make sure the AssemblyFileVersion attribute is updated." {
		#Arrange
		$searchRoot = "TestDrive:\"
		$buildNumber = ([System.Int16]::MaxValue - 1)
		$description = "Test Description"
		Set-Content -Path "TestDrive:\AssemblyInfo.cs" -Value @"
[AssemblyFileVersion("1.0.0.0")]
"@
		$expectedContent = @"
[AssemblyFileVersion("1.0.$buildNumber.0")]
"@

		#Act
		Set-AssemblyAttributes	"TestDrive:\AssemblyInfo.cs" $description $buildNumber
		
		#Assert
		It "The content of the new file should be updated." {
			(Get-Content "TestDrive:\AssemblyInfo.cs") -join ([System.Environment]::NewLine) | should be $expectedContent
		}
	}
	
	Context "Make sure the AssemblyDescription Attribute is updated." {
		#Arrange
		$searchRoot = "TestDrive:\"
		$buildNumber = ([System.Int16]::MaxValue - 1)
		$description = "Test Description"
		Set-Content -Path "TestDrive:\AssemblyInfo.cs" -Value @"
[AssemblyDescription("")]
"@
		$expectedContent = @"
[AssemblyDescription("$description")]
"@

		#Act
		Set-AssemblyAttributes	"TestDrive:\AssemblyInfo.cs" $description $buildNumber
		
		#Assert
		It "The content of the new file should be updated." {
			(Get-Content "TestDrive:\AssemblyInfo.cs") -join ([System.Environment]::NewLine) | should be $expectedContent
		}
	}
	
	Context "Make sure the AssemblyDescription Attribute is updated (populated description)" {
		#Arrange
		$searchRoot = "TestDrive:\"
		$buildNumber = ([System.Int16]::MaxValue - 1)
		$description = "Test Description"
		Set-Content -Path "TestDrive:\AssemblyInfo.cs" -Value @"
[AssemblyDescription("This is a temporary description that should be overriden.")]
"@
		$expectedContent = @"
[AssemblyDescription("$description")]
"@

		#Act
		Set-AssemblyAttributes	"TestDrive:\AssemblyInfo.cs" $description $buildNumber
		
		#Assert
		It "The content of the new file should be updated." {
			(Get-Content "TestDrive:\AssemblyInfo.cs") -join ([System.Environment]::NewLine) | should be $expectedContent
		}
	}
	
	Context "Test that all attributes are updated." {
	#Arrange
		$searchRoot = "TestDrive:\"
		$buildNumber = ([System.Int16]::MaxValue - 1)
		$description = "Test Description"
		Set-Content -Path "TestDrive:\AssemblyInfo.cs" -Value @"
Text to confuse the replace actions...
[AssemblyVersion("99.98.97.96")]
Text to confuse the replace actions...
[AssemblyFileVersion("1.2.3.444")]
Text to confuse the replace actions...
[AssemblyDescription("")]
Text to confuse the replace actions...
"@
		$expectedContent = @"
Text to confuse the replace actions...
[AssemblyVersion("99.98.$buildNumber.96")]
Text to confuse the replace actions...
[AssemblyFileVersion("1.2.$buildNumber.444")]
Text to confuse the replace actions...
[AssemblyDescription("$description")]
Text to confuse the replace actions...
"@

		#Act
		Set-AssemblyAttributes	"TestDrive:\AssemblyInfo.cs" $description $buildNumber
		
		#Assert
		It "The content of the new file should be updated." {
			(Get-Content "TestDrive:\AssemblyInfo.cs") -join ([System.Environment]::NewLine) | should be $expectedContent
		}
	}
	
	Context "File Attributes are kept the same." {
		$searchRoot = "TestDrive:\"
		$buildNumber = ([System.Int16]::MaxValue - 1)
		$description = "Test Description"
		Set-Content -Path "TestDrive:\AssemblyInfo.cs" -Value @"
Text to confuse the replace actions...
[AssemblyVersion("99.98.97.96")]
Text to confuse the replace actions...
[AssemblyFileVersion("1.2.3.444")]
Text to confuse the replace actions...
[AssemblyDescription("")]
Text to confuse the replace actions...
"@
		$expectedContent = @"
Text to confuse the replace actions...
[AssemblyVersion("99.98.$buildNumber.96")]
Text to confuse the replace actions...
[AssemblyFileVersion("1.2.$buildNumber.444")]
Text to confuse the replace actions...
[AssemblyDescription("$description")]
Text to confuse the replace actions...
"@
	
		Set-ItemProperty -Path "TestDrive:\AssemblyInfo.cs" -Name Attributes -Value ([System.IO.FileAttributes]::ReadOnly)
		$currentAttributes = Get-ValidFileAttributes (Get-ItemProperty -Path "TestDrive:\AssemblyInfo.cs" -Name Attributes).Attributes

		#Act
		Set-AssemblyAttributes	"TestDrive:\AssemblyInfo.cs" $description $buildNumber
		
		#Assert
		It "The resulting file should still be read only" {
			(Get-ItemProperty -Path "TestDrive:\AssemblyInfo.cs" -Name Attributes).Attributes | should be $currentAttributes
		}
	}
}