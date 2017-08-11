#!/bin/bash

if [ -n "$COVERALLS_REPO_TOKEN" ]
then
  nuget install -OutputDirectory packages -Version 1.3.4 coveralls.io
  packages\coveralls.io.1.3.4\tools\coveralls.net.exe --opencover -i coverage/coverage.xml --useRelativePaths
fi