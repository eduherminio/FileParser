#!/usr/bin/env bash

#exit if any command fails 
set -e 

dotnet restore ./FileParserSolution 
dotnet build ./FileParserSolution/FileParser --configuration Release --force --no-incremental --framework=netstandard2.0

revision=${TRAVIS_JOB_ID:=1}
revision=$(printf "%04d" $revision)