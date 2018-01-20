$version = "0.94.00"


function replaceVersionInfo($version)
{
	$versionInfoRegex = new-object Text.RegularExpressions.Regex "// NPP plugin platform for .Net v(\d+\.\d+(\.\d+)?) by Kasper B. Graversen etc.", "None"

	$files = get-ChildItem -Path . -Recurse -ErrorAction SilentlyContinue -Filter *.cs
	ForEach ($f in $files)
    {
		$name = $f.Fullname
		$content  = [IO.File]::ReadAllText($name)
		$original = $content
		
		$content = $versionInfoRegex.Replace($content, "// NPP plugin platform for .Net v"+ $version + " by Kasper B. Graversen etc.")
		If ($content -ne $original)
		{
			[IO.File]::WriteAllText("$name", $content, [Text.Encoding]::"UTF8")
		}
	}
}

cd 'Visual Studio Project Template C#'

replaceVersionInfo($version)

$filename = "NppPlugin" + $version + ".zip"
write-host "# zip the projectTemplate '$filename'" -foreground green
& 'C:\Program Files\7-Zip\7z.exe' a -tzip $filename * -xr!bin -xr!obj


$vsTemplatepath = [Environment]::GetFolderPath("MyDocuments")+'\Visual Studio 2017\Templates\ProjectTemplates\Visual C#\'
write-host "# Copy projectTemplate to VS: '$vsTemplatepath'" -foreground green
del "$($vsTemplatepath)\nppplugin*.zip"
copy $filename $($vsTemplatepath)
copy $filename c:\temp\

write-host "# remove temp files" -foreground green
rm $filename

cd ..
