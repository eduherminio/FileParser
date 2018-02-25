#!/bin/bash
clear
set -e
g++ -std=c++14 CppStandardIOApproachExample/CppStandardIOApproachExample.cpp -o linuxExec.out

./linuxExec.out < SampleFiles/SimpleInput.txt > CppGCCSimpleOutput.txt