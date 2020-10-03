
Write-Output "Init NuGet push..."

$output_file = "LangLib\CryptEnvVar.exe"

$download_url = "https://www.vpksoft.net/toolset/CryptEnvVar.exe"

Write-Output "Download file:  $download_url ..."
Remove-Item $output_file
(New-Object System.Net.WebClient).DownloadFile($download_url, $output_file)
Write-Output "Download done."

# create the digital signature..
$args = @("-s", $Env:SECRET_KEY, "-e", "CERT_1;CERT_2;CERT_3", "-f", "C:\vpksoft.pfx", "-w", "80")

& "LangLib\CryptEnvVar.exe" $args

# sign and push the NuGet packages..
$files = Get-ChildItem $Env:CIRCLE_WORKING_DIRECTORY -r -Filter *LangLib*.nupkg # use the mask to discard possible third party packages..
for ($i = 0; $i -lt $files.Count; $i++) 
{ 
    $file = $files[$i].FullName

    # sign the NuGet packages.
	Write-Output (-join("Signing package: ", $file, " ..."))

    $args = @("sign", $file, "-CertificatePath", "C:\vpksoft.pfx", "-Timestamper", "http://timestamp.comodoca.com", "-CertificatePassword", $Env:PFX_PASS)

    nuget.exe $args # > null 2>&1
	Write-Output (-join("Package signed: ", $file, "."))

    # push the NuGet packges..
    #$nuget_api = "https://api.nuget.org/v3/index.json"

    $nuget_api = "https://apiint.nugettest.org/v3/index.json"
	Write-Output (-join("Pushing NuGet:", $file, " ..."))

    $args = @("push", $file, $Env:NUGET_TEST_APIKEY, "-Source", $nuget_api, "-SkipDuplicate")
    nuget.exe $args
	Write-Output (-join("Pushing done:", $file, "."))
}

Write-Output "NuGet push finished."
