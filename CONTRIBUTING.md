# Guidelines for Contributors

Pickles is a volunteer effort. We encourage you to pitch in!

**Working on your first Pull Request?** You can learn how from this *free* series [How to Contribute to an Open Source Project on GitHub](https://egghead.io/series/how-to-contribute-to-an-open-source-project-on-github)

## To Contribute Code

- Fork the `Pickles` repository
- Create your code
- Send a pull request.

### To Contribute to the Test Results Providers

If you find a bug in a Test Result Provider and want to contribute towards fixing it, start of by adding a scenario or feature file that shows off the bug to the test harness solution in the [pickles-testresults repository](https://github.com/picklesdoc/pickles-testresults). Add the scenario to the projects of each test result provider: it is quite likely that the same bug will occur across several test result providers. Don't forget the Cucumber and CucumberJS providers: it's easy to overlook them because they are not in the visual studio solution.

Implement the automation layer in .NET, Ruby and JS if you are able. If you create your scenarios so that they use only steps from the other feature files, then you will most likely not need this step.

Use the `CreateTestresults.cmd` script to create test result files and to deploy them to the source code of Pickles. This step depends on you having checked out the `pickles` and `pickles-testresults` repositories in the same parent directory.

Extend the set of unit tests for the unit test providers. Your best bet is to add test scenarios to the [Standard Test Suite](https://github.com/picklesdoc/pickles/blob/develop/src/Pickles/Pickles.TestFrameworks.UnitTests/StandardTestSuite.cs). Now comes the boring part: for each class that derives from `StandardTestSuite`, add methods in that class that call the test scenarios that you added in `StandardTestSuite`.

You will now have several failing tests. You can now go and fix them :-) 

Once you're done, send a pull request.

## To Contribute Documentation

- Fork the `doc` repository
- Write documentation
- If you add a new page, you need to add a reference to it in `mkdocs.yml`
- Send a pull request

## To Request a Feature

- Create an issue.

## To Ask a Question

- Create an issue.

## To Report a Bug

- Create an issue
- Describe steps to reproduce the bug
- If at all possible, attach feature file(s) that cause the bug. In most cases you can remove the steps from the scenarios in order to protect your intellectual property.
