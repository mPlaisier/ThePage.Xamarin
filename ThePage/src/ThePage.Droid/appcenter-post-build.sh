#!/usr/bin/env bash

for entry in "$APPCENTER_SOURCE_DIRECTORY"/ThePage/src/*
do
  echo "$entry"
done

echo "Found Unit test projects"
echo "$APPCENTER_SOURCE_DIRECTORY"ThePage/src*
find $APPCENTER_SOURCE_DIRECTORY -regex '/.*ThePage/src/*Tests/*.csproj'

find $APPCENTER_SOURCE_DIRECTORY -regex '/.*ThePage/src/*Tests/*.csproj' -exec echo {} \;

echo "Run Unit test projects"
find $APPCENTER_SOURCE_DIRECTORY -regex '\.*\ThePage\src\*Tests\*.csproj' | xargs dotnet test --logger "trx;LogFileName=testresult.trx";

#find file with results
echo "XUnit tests result:"
pathOfTestResults=$(find $APPCENTER_SOURCE_DIRECTORY\.*\ThePage\src\*Tests\*.csproj -name 'testresult.trx')
echo  "Fetched results"
echo  "Path:"
echo $pathOfTestResults
echo "end of Path"


grep ' \[FAIL\]' $pathOfTestResults
echo "Done grep"
failures=$(grep -o ' \[FAIL\]' $pathOfTestResults | wc -l)

echo "checking for failures"

if [[ $failures -eq 0 ]]
then 
echo "Unit Tests passed" 
else 
echo "Unit Tests failed"
exit 1
fi