#!/bin/bash
csc -platform:x86 src/*.cs -out:build/HuneClient.exe && mono ./build/HuneClient.exe