#Declare input variables

param(
	[string]$env, 
	[string]$dest
)

#Override 
#$env  = "Staging"
#$dest = "C:\Users\chrisb\Source\Repos\HL7Broker\HL7Broker\Library"

if($env -eq "")
{
    Write-Output "NO ENV"
    exit 1;
}

if($dest -eq "")
{
    Write-Output "NO DEST"
    exit 1;
}

#Update WSShieldsApps service reference.
$developmentLocation	= "https://shcapp2.shc.shcnet.pri/Test1/WSShieldsApps/ShieldsApps.svc"
$stagingLocation		= "https://shcapp2.shc.shcnet.pri/Staging/WSShieldsApps/ShieldsApps.svc"
$productionLocation		= "https://shcappprod.shc.shcnet.pri/WSShieldsApps/ShieldsApps.svc"

$location = ""

if($env -eq "Release")
{
	$location = $productionLocation 
}

elseif($env -eq "Debug")
{
	$location = $developmentLocation
}

elseif($env -eq "Staging")
{
	$location = $stagingLocation
}

elseif($env -eq "Development")
{
	$location = $developmentLocation 
}


elseif($env -eq "Production")
{
	$location = $productionLocation 
}

& cd $dest
& 'C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\svcutil.exe'  $location /ct:System.Collections.Generic.List``1
& 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe'  /target:library ShieldsApps.cs
#& rm *.config
& rm *.cs