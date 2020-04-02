#!/usr/bin/env bash

echo "Search Unit test projects"
find "$APPCENTER_SOURCE_DIRECTORY"/.*/ThePage/src/*Tests/*.csproj -exec echo {} \;
echo
echo "Run Unit test projects"
find "$APPCENTER_SOURCE_DIRECTORY"/.*/ThePage/src/*Tests/*.csproj | xargs dotnet test --logger "trx;LogFileName=testresult.trx";

echo
#find file with results
echo "XUnit tests result:"
pathOfTestResults=$(find "$APPCENTER_SOURCE_DIRECTORY"/.*/ThePage/src/*Tests/TestResults/* -name 'testresult.trx')

grep ' \[FAIL\]' $pathOfTestResults
failures=$(grep -o ' \[FAIL\]' $pathOfTestResults | wc -l)

if [[ $failures -eq 0 ]]
then 
echo "Unit Tests passed" 
else 
echo "Unit Tests failed!"
exit 1
fi