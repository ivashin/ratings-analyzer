%systemroot%\Microsoft.NET\Framework\v4.0.30319\MsBuild.exe ^
	build\build.proj ^
	/target:Main ^
	/property:root=%__CD__% ^
	/fileLogger /flp:logfile=build.log
