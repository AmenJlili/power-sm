; PowerSM installer (NSIS 3)
; Build:  makensis installer\PowerSM.nsi        (Windows or Linux)
; Expects the Release build in PowerSM\PowerSM\bin\Release.

Unicode true
!include "MUI2.nsh"
!include "x64.nsh"
!include "LogicLib.nsh"

!ifndef VERSION
  !define VERSION "1.5"
!endif
!define PRODUCT      "PowerSM"
!define PUBLISHER    "Blue Byte Systems Inc."
!define URL          "https://github.com/AmenJlili/power-sm"
!define BINDIR       "..\PowerSM\PowerSM\bin\Release"
!define UNINSTKEY    "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT}"
!define REGASM       "$WINDIR\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe"

Name "${PRODUCT} ${VERSION}"
OutFile "output\setup_x64_${VERSION}.exe"
InstallDir "$PROGRAMFILES64\Blue Byte\${PRODUCT}"
InstallDirRegKey HKLM "${UNINSTKEY}" "InstallLocation"
RequestExecutionLevel admin
SetCompressor /SOLID lzma
BrandingText "${PUBLISHER}"

VIProductVersion "${VERSION}.0.0"
VIAddVersionKey "ProductName" "${PRODUCT}"
VIAddVersionKey "CompanyName" "${PUBLISHER}"
VIAddVersionKey "FileDescription" "${PRODUCT} add-in for SOLIDWORKS setup"
VIAddVersionKey "FileVersion" "${VERSION}"
VIAddVersionKey "ProductVersion" "${VERSION}"
VIAddVersionKey "LegalCopyright" "Copyright (c) 2015-2026 ${PUBLISHER}"

!define MUI_ICON   "..\PowerSM\PowerSM\psm.ico"
!define MUI_UNICON "..\PowerSM\PowerSM\psm.ico"
!define MUI_ABORTWARNING
!define MUI_FINISHPAGE_SHOWREADME "$INSTDIR\Readme.txt"
!define MUI_FINISHPAGE_SHOWREADME_TEXT "Show the Readme"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"

Function .onInit
  ${IfNot} ${RunningX64}
    MessageBox MB_ICONSTOP "${PRODUCT} requires 64-bit Windows and 64-bit SOLIDWORKS."
    Abort
  ${EndIf}
  SetRegView 64

  ; .NET Framework 4.5 or later (Release >= 378389)
  ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"
  ${If} $0 < 378389
    MessageBox MB_ICONSTOP ".NET Framework 4.5 or later is required.$\r$\nInstall it from Windows Update, then run this setup again."
    Abort
  ${EndIf}

  ; SOLIDWORKS must be closed so the add-in can be registered cleanly
  check_sw:
  nsExec::ExecToStack 'cmd.exe /c tasklist /NH /FI "IMAGENAME eq SLDWORKS.exe" | find /I "SLDWORKS.exe"'
  Pop $0
  Pop $1
  ${If} $0 == 0
    MessageBox MB_RETRYCANCEL|MB_ICONEXCLAMATION "SOLIDWORKS is running. Close it, then click Retry." IDRETRY check_sw
    Abort
  ${EndIf}
FunctionEnd

Section "Install"
  SetOutPath "$INSTDIR"
  SetOverwrite on

  File "${BINDIR}\PowerSM.dll"
  File "${BINDIR}\SolidWorks.Interop.sldworks.dll"
  File "${BINDIR}\SolidWorks.Interop.swconst.dll"
  File "${BINDIR}\SolidWorks.Interop.swpublished.dll"
  File "Readme.txt"
  File "Changes.txt"

  ; Register the add-in (COM + SOLIDWORKS add-in key, see PowerSM.RegisterAssembly)
  nsExec::ExecToLog '"${REGASM}" /codebase "$INSTDIR\PowerSM.dll"'
  Pop $0
  ${If} $0 != 0
    MessageBox MB_ICONEXCLAMATION "Registering PowerSM.dll failed (RegAsm exit code $0).$\r$\nMake sure SOLIDWORKS is closed and run the setup as administrator."
  ${EndIf}

  WriteUninstaller "$INSTDIR\uninstall.exe"
  WriteRegStr   HKLM "${UNINSTKEY}" "DisplayName"     "${PRODUCT} ${VERSION} (SOLIDWORKS add-in)"
  WriteRegStr   HKLM "${UNINSTKEY}" "DisplayVersion"  "${VERSION}"
  WriteRegStr   HKLM "${UNINSTKEY}" "Publisher"       "${PUBLISHER}"
  WriteRegStr   HKLM "${UNINSTKEY}" "URLInfoAbout"    "${URL}"
  WriteRegStr   HKLM "${UNINSTKEY}" "DisplayIcon"     "$INSTDIR\uninstall.exe"
  WriteRegStr   HKLM "${UNINSTKEY}" "InstallLocation" "$INSTDIR"
  WriteRegStr   HKLM "${UNINSTKEY}" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "${UNINSTKEY}" "NoModify" 1
  WriteRegDWORD HKLM "${UNINSTKEY}" "NoRepair" 1
SectionEnd

Function un.onInit
  SetRegView 64
FunctionEnd

Section "Uninstall"
  nsExec::ExecToLog '"${REGASM}" /unregister "$INSTDIR\PowerSM.dll"'
  Pop $0

  Delete "$INSTDIR\PowerSM.dll"
  Delete "$INSTDIR\SolidWorks.Interop.sldworks.dll"
  Delete "$INSTDIR\SolidWorks.Interop.swconst.dll"
  Delete "$INSTDIR\SolidWorks.Interop.swpublished.dll"
  Delete "$INSTDIR\add_icon.bmp"
  Delete "$INSTDIR\Readme.txt"
  Delete "$INSTDIR\Changes.txt"
  Delete "$INSTDIR\uninstall.exe"
  RMDir "$INSTDIR"
  RMDir "$PROGRAMFILES64\Blue Byte"

  DeleteRegKey HKLM "${UNINSTKEY}"
SectionEnd
