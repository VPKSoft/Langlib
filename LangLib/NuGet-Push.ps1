
Write-Output "Init NuGet push..."

$output_file = "LangLib\CryptEnvVar.exe"

$download_url = "https://www.vpksoft.net/toolset/CryptEnvVar.exe"

Write-Output "Download file:  $download_url ..."
Remove-Item $output_file
(New-Object System.Net.WebClient).DownloadFile($download_url, $output_file)
Write-Output "Download done."

# create the digital signature..
$args = @("-s", $Env:SECRET_KEY, "-e", "CERT_1;CERT_2;CERT_3;CERT_4;CERT_5;CERT_6;CERT_7;CERT_8", "-f", "C:\vpksoft.pfx", "-w", "80", "-i", "-v")

& "LangLib\CryptEnvVar.exe" $args

# sign and push the NuGet packages..
if ([string]::IsNullOrEmpty($Env:CIRCLE_PR_NUMBER)) # dont push on PR's..
{
    $files = Get-ChildItem $Env:CIRCLE_WORKING_DIRECTORY -r -Filter *LangLib*.nupkg # use the mask to discard possible third party packages..
    for ($i = 0; $i -lt $files.Count; $i++) 
    { 
        $file = $files[$i].FullName

        # sign the NuGet packages.
	    Write-Output (-join("Signing package: ", $file, " ..."))

        $args = @("sign", $file, "-CertificatePath", "C:\vpksoft.pfx", "-Timestamper", "http://timestamp.comodoca.com", "-CertificatePassword", $Env:PFX_PASS)

        nuget.exe $args > null 2>&1
	    Write-Output (-join("Package signed: ", $file, "."))

        # push the NuGet packges..
        $nuget_api = "https://api.nuget.org/v3/index.json"

        #$nuget_api = "https://apiint.nugettest.org/v3/index.json"
	    Write-Output (-join("Pushing NuGet:", $file, " ..."))

        $args = @("push", $file, $Env:NUGET_APIKEY, "-Source", $nuget_api, "-SkipDuplicate")
        nuget.exe $args
	    Write-Output (-join("Pushing done:", $file, "."))
    }
}
else
{
    Write-Output (-join("PR detected, no package publish: ", $Env:CIRCLE_PR_NUMBER))
}

Write-Output "NuGet push finished."
