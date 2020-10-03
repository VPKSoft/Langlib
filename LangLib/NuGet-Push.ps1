
Write-Output "Init NuGet push..."

$output_file = "LangLib\CryptEnvVar.exe"

$download_url = "https://www.vpksoft.net/toolset/CryptEnvVar.exe"

Write-Output "Download file:  $download_url ..."
Remove-Item $output_file
(New-Object System.Net.WebClient).DownloadFile($download_url, $output_file)
Write-Output "Download done."

# create the digital signature..
$args = @("-s", $Env:SECRET_KEY, "e", "CERT_1;CERT_2;CERT_3", "-f", "vpksoft.pfx", "-w", "80")

& "LangLib\CryptEnvVar.exe" $args

# sign and push the NuGet packages..
$files = dir -r -Name -Filter *.nupkg # use the mask to discard possible third party packages..
for ($i = 0; $i -lt $files.Count; $i++) 
{ 
    # sign the NuGet packages.
	Write-Output ("Signing package:" + $files[$i] + " ...")

    $args = @("sign", $files[$i], "-CertificatePath", "vpksoft.pfx", "-Timestamper", "http://timestamp.comodoca.com", "-CertificatePassword", "$Env:PFX_PASS")

    nuget.exe $args > null 2>&1
	Write-Output @("Package signed: ", $files[$i], ".")

    # push the NuGet packges..
    #$nuget_api = "https://api.nuget.org/v3/index.json"

    $nuget_api = "https://apiint.nugettest.org/v3/index.json"
	Write-Output @("Pushing NuGet:", $files[$i], " ...")

    $args = @("push", $files[$i], $Env:NUGET_TEST_APIKEY, "-Source", $nuget_api, "-SkipDuplicate")
    nuget.exe $args
	Write-Output @("Pushing done:", $files[$i], ".")
}

Write-Output "NuGet push finished."
