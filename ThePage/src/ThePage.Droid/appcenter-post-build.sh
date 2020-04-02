#!/usr/bin/env bash

directory = $APPCENTER_SOURCE_DIRECTORY -regex '/ThePage/src'
for entry in "$directory"/ThePage/src/*
do
  echo "$entry"
done

echo "Found Unit test projects"
echo $directory
find $directory -regex '.*Tests\*.csproj'

find $directory -regex '.*Tests\*.csproj' -exec echo {} \;

echo "Run Unit test projects"
find $directory -regex '.*Tests\*.csproj' | xargs dotnet test --logger "trx;LogFileName=testresult.trx";

#find file with results
echo "XUnit tests result:"
pathOfTestResults=$(find $directory -name 'testresult.trx')
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