#!/bin/bash

if [ -n "$COVERALLS_REPO_TOKEN" ]
then
  nuget install -OutputDirectory packages -Version 1.3.4 coveralls.io
  COVERALLSNET=$PWD/packages/coveralls.io.1.3.4/tools/coveralls.net.exe
  $COVERALLSNET \
    --opencover coverage/coverage.xml \
    -r $COVERALLS_REPO_TOKEN
fi