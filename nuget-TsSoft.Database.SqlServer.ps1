del TsSoft.Database.SqlServer.*.nupkg
del *.nuspec
del .\TsSoft.Database.SqlServer\bin\Release\*.nuspec

function GetNodeValue([xml]$xml, [string]$xpath)
{
	return $xml.SelectSingleNode($xpath).'#text'
}

function SetNodeValue([xml]$xml, [string]$xpath, [string]$value)
{
	$node = $xml.SelectSingleNode($xpath)
	if ($node) {
		$node.'#text' = $value
	}
}

Remove-Item .\TsSoft.Database.SqlServer\bin -Recurse 
Remove-Item .\TsSoft.Database.SqlServer\obj -Recurse

$build = "c:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe ""TsSoft.Database.SqlServer\TsSoft.Database.SqlServer.csproj"" /p:Configuration=Release" 
Invoke-Expression $build

$Artifact = (resolve-path ".\TsSoft.Database.SqlServer\bin\Release\TsSoft.Database.SqlServer.dll").path

nuget spec -F -A $Artifact

Copy-Item .\TsSoft.Database.SqlServer.nuspec.xml .\TsSoft.Database.SqlServer\bin\Release\TsSoft.Database.SqlServer.nuspec

$GeneratedSpecification = (resolve-path ".\TsSoft.Database.SqlServer.nuspec").path
$TargetSpecification = (resolve-path ".\TsSoft.Database.SqlServer\bin\Release\TsSoft.Database.SqlServer.nuspec").path

[xml]$srcxml = Get-Content $GeneratedSpecification
[xml]$destxml = Get-Content $TargetSpecification
$value = GetNodeValue $srcxml "//version"
SetNodeValue $destxml "//version" $value;
$value = GetNodeValue $srcxml "//description"
SetNodeValue $destxml "//description" $value;
$value = GetNodeValue $srcxml "//copyright"
SetNodeValue $destxml "//copyright" $value;
$destxml.Save($TargetSpecification)

nuget pack $TargetSpecification

del *.nuspec
del .\TsSoft.Database.SqlServer\bin\Release\*.nuspec

exit
