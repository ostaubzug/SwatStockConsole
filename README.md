# Swat Project

[![Build Pipeline](https://github.com/OliverStaub/SwatStockConsole/actions/workflows/buildpipeline.yml/badge.svg)](https://github.com/OliverStaub/SwatStockConsole/actions/workflows/buildpipeline.yml)
[![Code Coverage Pipeline](https://github.com/OliverStaub/SwatStockConsole/actions/workflows/codecoverage.yml/badge.svg)](https://github.com/OliverStaub/SwatStockConsole/actions/workflows/codecoverage.yml)
[![Code Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/OliverStaub/d3cd25b634cc40dd9ebd104ce7fffce7/raw/code-coverage.json)](https://github.com/OliverStaub/SwatStockConsole/actions/workflows/codecoverage.yml)
[![SonarCloud](https://sonarcloud.io/api/project_badges/measure?project=OliverStaub_SwatStockConsole&metric=alert_status)](https://sonarcloud.io/project/overview?id=OliverStaub_SwatStockConsole)
[![Dependabot](https://img.shields.io/badge/dependabot-enabled-025e8c?logo=dependabot)](https://docs.github.com/code-security/dependabot/dependabot-version-updates)

## Setup

In the StockConsole Project create a .env File with this content:

```
ALPHA_API_KEY=your-alpha-vantage-key
ALPHA_API_URL=https://www.alphavantage.co/query?function={function}&symbol={symbol}&apikey={apiKey}
```

You can get your API Key here: https://www.alphavantage.co/support/#api-key

Make sure the file is getting copied to the output directory.

## AI generated Documentation

The Github Pages are deploayed here:
https://OliverStaub.github.io/SwatStockConsole/c4_documentation.html

## Git

Wir nutzen den main und develop branch.

## Pipelines

### Build Pipeline

The build Pipeline generates a summary about the tests that have been run.
![TestSummary](Images/TestSummary.png)

### Code Coverage Pipeline

The code coverage Pipeline generates a detailed report that can be downloaded.
