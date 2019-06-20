#!/usr/bin/env bash

# To be invoked from outside ci folder

# exit if any command fails
set -e

dotnet test ./src/FileParser.Test --framework=netcoreapp2.2 --logger trx --collect "Code coverage"

revision=${TRAVIS_JOB_ID:=1}
revision=$(printf "%04d" $revision)