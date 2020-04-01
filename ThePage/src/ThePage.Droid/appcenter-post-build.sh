{\rtf1\ansi\ansicpg1252\cocoartf2511
\cocoatextscaling0\cocoaplatform0{\fonttbl\f0\fnil\fcharset0 Menlo-Regular;}
{\colortbl;\red255\green255\blue255;\red27\green29\blue31;}
{\*\expandedcolortbl;;\cssrgb\c14118\c15294\c16078;}
\paperw11900\paperh16840\margl1440\margr1440\vieww10800\viewh8400\viewkind0
\deftab720
\pard\pardeftab720\sl300\partightenfactor0

\f0\fs26 \cf2 \expnd0\expndtw0\kerning0
\outl0\strokewidth0 \strokec2 #!/usr/bin/env bash\
\
echo "Found Unit test projects"\
find $APPCENTER_SOURCE_DIRECTORY -regex '*.Tests*\\.csproj' -exec echo \{\} \\;\
echo\
echo "Run Unit test projects"\
find $APPCENTER_SOURCE_DIRECTORY -regex '*.Tests*\\.csproj' | xargs dotnet test --logger "trx;LogFileName=testresult.trx";\
\
#find file with results\
echo "XUnit tests result:"\
pathOfTestResults=$(find $APPCENTER_SOURCE_DIRECTORY -name 'testresult.trx')\
echo\
\
grep ' \\[FAIL\\]' $pathOfTestResults\
failures=$(grep -o ' \\[FAIL\\]' $pathOfTestResults | wc -l)\
\
if [[ $failures -eq 0 ]]\
then \
echo "Unit Tests passed" \
else \
echo "Unit Tests failed"\
exit 1\
fi\
}