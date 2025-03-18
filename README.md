Pickles
=======

## Pickles End of life

**Tricentis has end-of-lifed SpecFlow. Since Pickles is built completely around SpecFlow, there is no point in continuing with Pickles. In fact, there has been little activity over the past several years. Let’s make official what we have all known (and presumably accepted) for a long time: Pickles is going to remain as it is.**

**All is not lost, however: the repositories will remain. More importantly, there is a good fork of SpecFlow: [ReqnRoll](https://reqnroll.net/). If you want a Reqnroll-based Living Documentation generator, head over to Reqnroll and ask how you can support that effort.**


Pickles is an open source living documentation generator that works on feature files written in the Gherkin language, popularized in tools like Cucumber and SpecFlow.

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)
[![Join the chat at https://gitter.im/picklesdoc/pickles](https://badges.gitter.im/picklesdoc/pickles.svg)](https://gitter.im/picklesdoc/pickles?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

|                             |Status                                                                                                                                   |
|-----------------------------|-----------------------------------------------------------------------------------------------------------------------------------------|
| Docs                        |[![Documentation Status](https://readthedocs.org/projects/pickles/badge/?version=stable)](http://docs.picklesdoc.com/en/latest/)         |
| GitHub Release              |[![GitHub release](https://img.shields.io/github/release/picklesdoc/pickles.svg)](https://github.com/picklesdoc/pickles/releases/latest) |
| NuGet (Pickles)             |[![NuGet](https://img.shields.io/nuget/v/Pickles.svg)](https://www.nuget.org/packages/Pickles)                                           |
| NuGet (Pickles.CommandLine) |[![NuGet](https://img.shields.io/nuget/v/Pickles.CommandLine.svg)](https://www.nuget.org/packages/Pickles.CommandLine)                   |
| NuGet (Pickles.MSBuild)     |[![NuGet](https://img.shields.io/nuget/v/Pickles.MSBuild.svg)](https://www.nuget.org/packages/Pickles.MSBuild)                           |



Pickles can be incorporated into your build process to produce living documentation in a format that is more accessible to your clients.  Gherkin language files are written in plain text and stored in your source folder.  This can make them inaccessible to clients who may not know how to work with source control or who are not interested in seeing all of the source code, just the features.

Why stop with just the features though?  Pickles can also read plain text files written in the Markdown format so you can add other files to your feature to add all sorts of context.  Well-written features are great to have but even the best written features can leave out some important context information.  Markdown is very simple to write and is designed to be easily read even in plain text files so they are a great way of adding additional context to your feature files to turn them into a real set of living documentation.

Supported Output Formats
------------------------

- HTML
- DHTML (javascript-enabled, search capabilities)
- JSON
- Word
- Excel

If there are other formats you would like to see feel free to create a [GitHub Issue](https://github.com/picklesdoc/pickles/issues).

Supported Test Runner Integrations
----------------------------------

- NUnit (versions 2.x and 3.x)
- xUnit (versions 1.x and 2.x)
- MSTest
- Cucumber JSON
- SpecFlow+ Runner (formerly known as SpecRun)

Contributing
------------

Pickles welcomes contributions from the community! Please refer to [Contributing.md](https://github.com/picklesdoc/pickles/blob/develop/CONTRIBUTING.md) for more information on how to contribute and how to set up your environment.

License
-------

Pickles is licensed with the Apache License, version 2.0.  You can find more information on the license here: http://www.apache.org/licenses/LICENSE-2.0.html
