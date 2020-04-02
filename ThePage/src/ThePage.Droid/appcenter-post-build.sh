#!/usr/bin/env bash

echo "Found Unit test projects"
find $APPCENTER_SOURCE_DIRECTORY -regex '*.Tests*\.csproj' -exec echo {} \;
echo
echo "Run Unit test projects"
find $APPCENTER_SOURCE_DIRECTORY -regex '*.Tests*\.csproj' | xargs dotnet test --logger "trx;LogFileName=testresult.trx";

#find file with results
echo "XUnit tests result:"
pathOfTestResults=$(find $APPCENTER_SOURCE_DIRECTORY -name 'testresult.trx')
echo  "Fetched results"

grep ' \[FAIL\]' $pathOfTestResults
failures=$(grep -o ' \[FAIL\]' $pathOfTestResults | wc -l)

echo "checking for failures"

if [[ $failures -eq 0 ]]
then 
echo "Unit Tests passed" 
else 
echo "Unit Tests failed"
exit 1
fi