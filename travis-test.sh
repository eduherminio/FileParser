#!/usr/bin/env bash

#exit if any command fails
set -e

dotnet test ./FileParserSolution/FileParser.Test --framework=netcoreapp2.2

revision=${TRAVIS_JOB_ID:=1}
revision=$(printf "%04d" $revision)