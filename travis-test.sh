#!/usr/bin/env bash

#exit if any command fails 
set -e 

dotnet test ./FileParserSolution/FileParserTest --framework=netcoreapp2.0

revision=${TRAVIS_JOB_ID:=1}
revision=$(printf "%04d" $revision)