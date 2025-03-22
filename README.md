# Swat Project
[![Build Pipeline](https://github.com/ostaubzug/SwatStockConsole/actions/workflows/buildpipeline.yml/badge.svg)](https://github.com/ostaubzug/SwatStockConsole/actions/workflows/buildpipeline.yml)
[![Code Coverage Pipeline](https://github.com/ostaubzug/SwatStockConsole/actions/workflows/codecoverage.yml/badge.svg)](https://github.com/ostaubzug/SwatStockConsole/actions/workflows/codecoverage.yml)
[![Code Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/ostaubzug/d3cd25b634cc40dd9ebd104ce7fffce7/raw/code-coverage.json)](https://github.com/ostaubzug/SwatStockConsole/actions/workflows/codecoverage.yml)
[![SonarCloud](https://sonarcloud.io/api/project_badges/measure?project=ostaubzug_SwatStockConsole&metric=alert_status)](https://sonarcloud.io/project/overview?id=ostaubzug_SwatStockConsole)

## Setup
In both Projects create a .env File with this content:
ALPHA_API_KEY=your-alpha-vantage-key

https://www.alphavantage.co/support/#api-key

Make sure the file is getting copied to the output directory.

## Git
Wir nutzen den main und develop branch.

## Pipelines

### Build Pipeline
The build Pipeline generates a summary about the tests that have been run.
![TestSummary](Images/TestSummary.png)

### Code Coverage Pipeline
The code coverage Pipeline generates a detailed report that can be downloaded.