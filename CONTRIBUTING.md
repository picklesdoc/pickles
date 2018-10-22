# Guidelines for Contributors

Pickles is a volunteer effort. We encourage you to pitch in!

**Working on your first Pull Request?** You can learn how from this *free* series [How to Contribute to an Open Source Project on GitHub](https://egghead.io/series/how-to-contribute-to-an-open-source-project-on-github)

## To Contribute Code

- Fork the `Pickles` repository
- Create your code
- Send a pull request

### To Contribute to the Test Result Providers

If you find a bug in a test result provider and want to contribute towards fixing it, start of by adding a scenario or feature file that shows off the bug to the test harness solution in the `test-harness` directory. Add the scenario to the projects of each test result provider: it is quite likely that the same bug will occur across several test result providers. Don't forget the Cucumber and CucumberJS providers: it's easy to overlook them because they are not in the visual studio solution.

Implement the automation layer in .NET, Ruby and JS if you are able. If you create your scenarios so that they use only steps from the other feature files, then you will most likely not need this step.

Use the `CreateTestResults.ps1` script to create test result files and to deploy them to the source code of Pickles. Please remember to build the `TestHarness.sln` solution first!

Extend the set of unit tests for the unit test providers. Your best bet is to add test scenarios to the [Standard Test Suite](https://github.com/picklesdoc/pickles/blob/develop/src/Pickles/Pickles.TestFrameworks.UnitTests/StandardTestSuite.cs). Now comes the boring part: for each class that derives from `StandardTestSuite`, add methods in that class that call the test scenarios that you added in `StandardTestSuite`.

You will now have several failing tests. You can now go and fix them :-)

Once you're done, send a pull request.

#### Setting up the Test Harness

The `test-harness` directory contains the scenarios from which test results are derived to test the parsing of test results in Pickles.

##### .NET Code

The .NET code is found in `./TestHarness/TestHarness.sln` solution. You will need [Visual Studio 2017](https://visualstudio.microsoft.com/downloads/) in order to open the solution; the free Community version is fine. Remember to restore the NuGet packages before you compile.

##### Cucumber/Ruby Code

In order to generate the test output of the Cucumber features, you need to install the following software:

- [Ruby](http://rubyinstaller.org/downloads): use the stable 2.2.x version
- Install Cucumber by opening an admin-enabled command line window in the directory where you cloned this repository, and running these gem commands:
  - `gem install cucumber`
  - `gem install rspec`


##### CucumberJS/JS Code

In order to generate the test output of the CucumberJS features, you need to install the following software:

- [Node.js](https://nodejs.org/en/download/): the LTE version is good enough. You may have to restart your computer. You can optionally verify the installation of Node.js by running these two commands from a command line:
  - `node -v`
  - `npm -v`
- Install CucumberJS by opening an admin-enabled command line window in the directory where you cloned this repository, and running the `npm install --save-dev cucumber` command.


## To Contribute Documentation

- Fork the `doc` repository
- Write documentation
- If you add a new page, you need to add a reference to it in `mkdocs.yml`
- Send a pull request

## To Request a Feature

- Create an issue

## To Ask a Question

- Create an issue

## To Report a Bug

- Create an issue
- Describe steps to reproduce the bug
- If at all possible, attach feature file(s) that cause the bug. In most cases you can remove the steps from the scenarios in order to protect your intellectual property
