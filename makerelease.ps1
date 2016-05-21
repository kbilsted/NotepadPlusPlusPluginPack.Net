$version = "0.90"


function replaceVersionInfo($version)
{
	$versionInfoRegex = new-object Text.RegularExpressions.Regex "// NPP plugin platform for .Net v(\d+\.\d+) by Kasper B. Graversen etc.", "None"

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

# zip the projectTemplate
cd 'Visual Studio Project Template C#'
$filename = "NppPlugin" + $version + ".zip"
& 'C:\Program Files\7-Zip\7z.exe' a -tzip $filename *

# copy projectTemplate to local VS
$vsTemplatepath = [Environment]::GetFolderPath("MyDocuments")+'\Visual Studio 2015\Templates\ProjectTemplates\Visual C#\'
copy $filename $($vsTemplatepath)

# Zip template and all source files
cd ..
replaceVersionInfo($version)
& 'C:\Program Files\7-Zip\7z.exe' a -tzip c:\temp\nppDemoAndProjectTemplate$($version).zip *

# remove temp files
rm 'Visual Studio Project Template C#\NppPlugin*.zip'

