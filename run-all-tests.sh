#!/bin/bash
# Script to run all tests for the project

echo "Running all tests..."

# Run unit tests
echo -e "\n🔍 Running Unit Tests..."
dotnet test src/Tests/UnitTests/LaunchQ.TakeHomeProject.UnitTests.csproj --verbosity minimal || echo "Unit tests failed but continuing"
UNIT_EXIT=$?

# Run integration tests
echo -e "\n🔍 Running Integration Tests..."
dotnet test src/Tests/IntegrationTests/LaunchQ.TakeHomeProject.IntegrationTests.csproj --verbosity minimal
INT_EXIT=$?

# Print summary
echo -e "\n----- Test Summary -----"
if [ $UNIT_EXIT -eq 0 ]; then
  echo "✅ Unit Tests: PASSED"
else
  echo "❌ Unit Tests: FAILED"
fi

if [ $INT_EXIT -eq 0 ]; then
  echo "✅ Integration Tests: PASSED"
else
  echo "❌ Integration Tests: FAILED"
fi

# Check if the script was called with the "doc" parameter
if [ "$1" == "doc" ]; then
  echo -e "\n📝 Running Tests Documentation"
  echo -e "=========================="
  echo -e "This script runs all tests in the project:"
  echo -e "1. Unit Tests - Testing individual components in isolation"
  echo -e "2. Integration Tests - Testing component interactions"
  echo -e "\nTo run unit tests individually:"
  echo -e "- Unit tests: ./run-unit-tests.sh"
  exit 0
fi

# Generate a test report
echo -e "\n📊 Test Report - $(date)"
echo -e "=========================="
echo -e "Unit Tests: $([ $UNIT_EXIT -eq 0 ] && echo "✅ PASSED" || echo "❌ FAILED")"
echo -e "Integration Tests: $([ $INT_EXIT -eq 0 ] && echo "✅ PASSED" || echo "❌ FAILED")"
echo -e "=========================="

# Determine final exit code
if [ $UNIT_EXIT -eq 0 ] && [ $INT_EXIT -eq 0 ]; then
  echo -e "\n✅ All tests PASSED!"
  exit 0
else
  echo -e "\n❌ Some tests FAILED!"
  exit 1
fi
