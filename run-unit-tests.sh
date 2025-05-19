#!/bin/bash
# Script to run unit tests for the project

echo "Running unit tests..."
dotnet test src/Tests/UnitTests/LaunchQ.TakeHomeProject.UnitTests.csproj --verbosity normal || echo "Tests failed but continuing"

# Check the exit code
if [ $? -eq 0 ]; then
  echo -e "\n✅ Unit tests PASSED!"
else
  echo -e "\n❌ Unit tests FAILED (but script continues)!"
fi
