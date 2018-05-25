#!/usr/bin/env bash
cd VolunteeringSystem || exit
cp app.yaml bin/Release/netcoreapp2.0/publish/
dotnet publish -c Release
gcloud app deploy bin/Release/netcoreapp2.0/publish/app.yaml
