REM original version https://github.com/risk-of-thunder/R2Wiki/wiki/Networking-with-Weaver:-The-Unity-Way
REM open this in vs it'll be so much nicer

set TargetFileName=Enforcer.dll
set TargetDir=bin\Debug\netstandard2.0

REM robocopy to our weaver folder. idk what the fuck robocopy does but we leave one there for storage
robocopy %TargetDir% Weaver %TargetFileName% > %TargetDir%\Robocopy

REM rename our original build to prepatch
IF EXIST %TargetDir%\%TargetFileName%.prepatch (
	DEL /F %TargetDir%\%TargetFileName%.prepatch
)
ren %TargetDir%\%TargetFileName% %TargetFileName%.prepatch

REM le epic networking patch
REM	Unity.UNetWeaver.exe	{path to Coremodule}			{Path  Networking}			   {output path} {Path to patching dll}  {Path to all needed references for the to-be-patched dll}
Weaver\Unity.UNetWeaver.exe libs\UnityEngine.CoreModule.dll libs\com.unity.multiplayer-hlapi.Runtime.dll %TargetDir%\ Weaver\%TargetFileName% libs
del Weaver\%TargetFileName%
del %TargetDir%\Robocopy

REM that's it. This is meant to pretend we just built a dll like any other time except this one is networked
REM add your postbuilds in vs like it's any other project