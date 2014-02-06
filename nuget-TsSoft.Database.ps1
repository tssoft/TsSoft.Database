del TsSoft.Database.*.nupkg
del *.nuspec
del .\TsSoft.Database\bin\Release\*.nuspec

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

Remove-Item .\TsSoft.Database\bin -Recurse 
Remove-Item .\TsSoft.Database\obj -Recurse

$build = "c:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe ""TsSoft.Database\TsSoft.Database.csproj"" /p:Configuration=Release" 
Invoke-Expression $build

$Artifact = (resolve-path ".\TsSoft.Database\bin\Release\TsSoft.Database.dll").path

nuget spec -F -A $Artifact

Copy-Item .\TsSoft.Database.nuspec.xml .\TsSoft.Database\bin\Release\TsSoft.Database.nuspec

$GeneratedSpecification = (resolve-path ".\TsSoft.Database.nuspec").path
$TargetSpecification = (resolve-path ".\TsSoft.Database\bin\Release\TsSoft.Database.nuspec").path

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
del .\TsSoft.Database\bin\Release\*.nuspec

exit
