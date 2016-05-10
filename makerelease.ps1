cd 'Visual Studio Project Template C#'
& 'C:\Program Files\7-Zip\7z.exe' a -tzip NppPlugin.zip *
$path = [Environment]::GetFolderPath("MyDocuments")+'\Visual Studio 2015\Templates\ProjectTemplates\Visual C#\'
copy NppPlugin.zip $path
cd ..
& 'C:\Program Files\7-Zip\7z.exe' a -tzip c:\temp\nppDemoAndProjectTemplate.zip *
rm 'Visual Studio Project Template C#\NppPlugin.zip'

