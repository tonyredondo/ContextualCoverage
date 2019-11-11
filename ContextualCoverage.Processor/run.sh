#!/bin/sh

cd ..
dotnet run -p ./ContextualCoverage.Processor/ContextualCoverage.Processor.csproj -- --apply
cd ContextualCoverage.Processor