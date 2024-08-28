# Define the files to be modified
$file1 = "C:\Program Files (x86)\Steam\steamapps\common\SteamVR\drivers\null\resources\settings\default.vrsettings"
$file2 = "C:\Program Files (x86)\Steam\config\steamvr.vrsettings"

# Define the search and replace strings
$search1 = '"enable": true,'
$replace1 = '"enable": false,'
$search2 = '"forcedDriver": "null",'
$replace2 = '"forcedDriver": "",'
$search3 = '"enableDashboard" : false,'
$replace3 = '"enableDashboard" : true,'

# Replace in file1
(Get-Content $file1) | ForEach-Object {
    $_ -replace $search1, $replace1
} | Set-Content $file1

# Replace in file2
(Get-Content $file2) | ForEach-Object {
    $_ -replace $search2, $replace2
} | Set-Content $file2

(Get-Content $file2) | ForEach-Object {
    $_ -replace $search3, $replace3
} | Set-Content $file2
