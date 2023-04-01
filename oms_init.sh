#!/bin/bash

git clone https://github.com/openmaptiles/openmaptiles.git
cd ./openmaptiles
git checkout -b v3.12.2 refs/tags/v3.14
docker-compose pull
./quickstart.sh