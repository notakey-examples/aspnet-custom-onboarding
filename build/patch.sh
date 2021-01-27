#!/bin/bash

set -e

VERSION="$2"
BUILD="$3"
SRC_PATH="$1"

FULLVERSION="$VERSION.$BUILD"

cd "$SRC_PATH"

# Patching files mess
echo "  Patching files in $SRC_PATH with version $FULLVERSION"
sed -i -e "/<PropertyGroup>/,/<\/PropertyGroup>/ s|<AssemblyVersion>[0-9a-z.]\{1,\}</AssemblyVersion>|<AssemblyVersion>$FULLVERSION</AssemblyVersion>|g" ./src/LicensingManagerUI/LicensingManagerUI.csproj
rm -f ./src/LicensingManagerUI/LicensingManagerUI.csproj-e 2> /dev/null || true
sed -i -e "/<PropertyGroup>/,/<\/PropertyGroup>/ s|<FileVersion>[0-9a-z.]\{1,\}</FileVersion>|<FileVersion>$FULLVERSION</FileVersion>|g" ./src/LicensingManagerUI/LicensingManagerUI.csproj
rm -f ./src/LicensingManagerUI/LicensingManagerUI.csproj-e 2> /dev/null || true
sed -i -e "/<PropertyGroup>/,/<\/PropertyGroup>/ s|<Version>[0-9a-z.]\{1,\}</Version>|<Version>$VERSION</Version>|g" ./src/LicensingManagerUI/LicensingManagerUI.csproj
rm -f ./src/LicensingManagerUI/LicensingManagerUI.csproj-e 2> /dev/null || true

sed -i -e "/<PropertyGroup>/,/<\/PropertyGroup>/ s|<AssemblyVersion>[0-9a-z.]\{1,\}</AssemblyVersion>|<AssemblyVersion>$FULLVERSION</AssemblyVersion>|g" ./src/LicensingManagerShared/LicensingManagerShared.csproj
rm -f ./src/LicensingManagerShared/LicensingManagerShared.csproj-e 2> /dev/null || true
sed -i -e "/<PropertyGroup>/,/<\/PropertyGroup>/ s|<FileVersion>[0-9a-z.]\{1,\}</FileVersion>|<FileVersion>$FULLVERSION</FileVersion>|g" ./src/LicensingManagerShared/LicensingManagerShared.csproj
rm -f ./src/LicensingManagerShared/LicensingManagerShared.csproj-e 2> /dev/null || true
sed -i -e "/<PropertyGroup>/,/<\/PropertyGroup>/ s|<Version>[0-9a-z.]\{1,\}</Version>|<Version>$VERSION</Version>|g" ./src/LicensingManagerShared/LicensingManagerShared.csproj
rm -f ./src/LicensingManagerShared/LicensingManagerShared.csproj-e 2> /dev/null || true

sed -i -e "/<PropertyGroup>/,/<\/PropertyGroup>/ s|<AssemblyVersion>[0-9a-z.]\{1,\}</AssemblyVersion>|<AssemblyVersion>$FULLVERSION</AssemblyVersion>|g" ./src/LicensingManagerApi/LicensingManagerApi.csproj
rm -f ./src/LicensingManagerApi/LicensingManagerApi.csproj-e 2> /dev/null || true
sed -i -e "/<PropertyGroup>/,/<\/PropertyGroup>/ s|<FileVersion>[0-9a-z.]\{1,\}</FileVersion>|<FileVersion>$FULLVERSION</FileVersion>|g" ./src/LicensingManagerApi/LicensingManagerApi.csproj
rm -f ./src/LicensingManagerApi/LicensingManagerApi.csproj-e 2> /dev/null || true
sed -i -e "/<PropertyGroup>/,/<\/PropertyGroup>/ s|<Version>[0-9a-z.]\{1,\}</Version>|<Version>$VERSION</Version>|g" ./src/LicensingManagerApi/LicensingManagerApi.csproj
rm -f ./src/LicensingManagerApi/LicensingManagerApi.csproj-e 2> /dev/null || true