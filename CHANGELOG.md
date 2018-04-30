# CHANGE LOG

All notable changes to this project will be documented in this file.

This project adheres to [Semantic Versioning](http://semver.org). We consider the following to be the API of Pickles for the purposes of Semantic Versioning:

- The arguments of the command line runner
- The arguments of the PowerShell runner
- The arguments of the MSBuild configuration
- The settings file of the GUI runner
- The JSON output of the JSON format

Features in Experimental are subject to change and removal without being considered breaking.

This document is formatted according to the principles of [Keep A CHANGELOG](http://keepachangelog.com).

## Unreleased

### Added

### Changed

### Deprecated

### Removed

### Fixed

### Security

## [2.18.2] - 2018-04-28

### Changed

- Updated several external libraries

### Fixed

- Problem with Chocolatey Deployment

## [2.18.1] - 2018-04-20

### Fixed

- More robust scenario outline example mapping by using code from SpecFlow ([519](https://github.com/picklesdoc/pickles/pull/519)) (by [@dirkrombauts](https://github.com/dirkrombauts), based on an idea by [@janosmagyar](https://github.com/janosmagyar))
- Only the table of the examples in a scenario outline should have test results ([515](https://github.com/picklesdoc/pickles/pull/515)) (by [@spacehole1](https://github.com/spacehole1))

## [2.18.0] - 2018-02-02

### Added

- Dhtml reports and Scenario outline support (includes extension of the JSON output) ([502](https://github.com/picklesdoc/pickles/pull/502)) (by [@DominikBaran](https://github.com/DominikBaran))

### Fixed

- Cache the NUnit feature scan results to improve performance on large solutions ([503](https://github.com/picklesdoc/pickles/pull/503)) (by [@tlecomte](https://github.com/tlecomte))
- Use lookups in JSONDocumentation Builder to improve performance on large solutions ([504](https://github.com/picklesdoc/pickles/pull/504)) (by [@tlecomte](https://github.com/tlecomte))
- Cache LanguageServices and GherkinDialectProvider in FeatureParser to improve performance on large solutions ([505](https://github.com/picklesdoc/pickles/pull/505)) (by [@tlecomte](https://github.com/tlecomte))

## [2.17.0] - 2017-11-06

### Added

- Added error handling for Markdown parsing exceptions ([497](https://github.com/picklesdoc/pickles/pull/497)) (by [@joebuschmann](https://github.com/joebuschmann))

### Changed

### Deprecated

### Removed

### Fixed

- PowerShell Runner Shows Only First Test Result File ([485](https://github.com/picklesdoc/pickles/pull/485)) (by [@dirkrombauts](https://github.com/dirkrombauts))
- Fixed ExcludeTags Typo In MSBuild targets ([478](https://github.com/picklesdoc/pickles/pull/478)) (by [@nachereshata](https://github.com/nachereshata))

### Security

## [2.16.2] - 2017-08-13

### Fixed

- Pickles is unable to deal with Danish characters ([477](https://github.com/picklesdoc/pickles/pull/477)) (by [@s991116](https://github.com/s991116))

## [2.16.1] - 2017-08-07

### Fixed

- Remove unnecessary backslash conversion in json feature tree ([469](https://github.com/picklesdoc/pickles/pull/469)) (by [@AntoineTheb](https://github.com/AntoineTheb))
- Pickles cannot deal with languages that have a hyphen in the name ([478](https://github.com/picklesdoc/pickles/pull/478)) (by [@dirkrombauts](https://github.com/dirkrombauts))


## [2.16.0] - 2017-06-06

### Added

- Add handling to guess some encodings ([457](https://github.com/picklesdoc/pickles/pull/457)) (by [@s991116](https://github.com/s991116) and [@dirkrombauts](https://github.com/dirkrombauts))

### Fixed

- Show inconclusive results in DHTML output ([463](https://github.com/picklesdoc/pickles/pull/463)) (by [@dirkrombauts](https://github.com/dirkrombauts))

### Changed

- Under the hood: Move NDesk.Options dependency to command line runner ([459](https://github.com/picklesdoc/pickles/pull/459)) (by [@dirkrombauts](https://github.com/dirkrombauts))
- Under the hood: update several third party packages ([462](https://github.com/picklesdoc/pickles/pull/462)) (by [@dirkrombauts](https://github.com/dirkrombauts))

## [2.15.0] - 2017-03-09

### Added

- Continue loading feature files after parsing errors ([445](https://github.com/picklesdoc/pickles/pull/445)) (by [@dirkrombauts](https://github.com/dirkrombauts), based on an idea by [@pleveill](https://github.com/pleveill))

### Fixed

- Fix localisation of "Examples" in Scenario Outlines ([440](https://github.com/picklesdoc/pickles/pull/440)) (by [@dirkrombauts](https://github.com/dirkrombauts))
- Make ExcludeTags available to MSBuild ([444](https://github.com/picklesdoc/pickles/pull/444)) (by [@dirkrombauts](https://github.com/dirkrombauts), based on an idea by [@pleveill](https://github.com/pleveill))

## [2.14.0] - 2017-02-24

### Added

- Add wildcard support for test result files ([435](https://github.com/picklesdoc/pickles/pull/435)) (by [@Autom8edChaos](https://github.com/Autom8edChaos))
- Exclude Feature or Scenario by Tag ([433](https://github.com/picklesdoc/pickles/pull/433)) (by [@Wil75](https://github.com/Wil75))

## [2.13.0] - 2017-02-10

### Added

- Improved Tag Support: Tags in Excel and Tags for Examples Blocks in Json, Dhtml, Html, and Word formats ([424](https://github.com/picklesdoc/pickles/pull/424)) (by [@pleveill](https://github.com/pleveill))

### Fixed

- Fix problem of NUnit 3.6 and truncation ([425](https://github.com/picklesdoc/pickles/pull/425)) (by [@dirkrombauts](https://github.com/dirkrombauts))

## [2.12.0] - 2017-01-26

### Added

- Adding Cucumber JSON Documentation Builder ([413](https://github.com/picklesdoc/pickles/pull/413)) (by [armsteadj1](https://github.com/armsteadj1), [jgrossrieder](https://github.com/jgrossrieder) and [dgrekov](https://github.com/dgrekov))

### Fixed

- Fixes hiding topnav when printing ([411](https://github.com/picklesdoc/pickles/pull/411)) (by [bliles](https://github.com/bliles))

## [2.11.1] - 2016-12-16

### Fixed

- Enable Pickles to deal with ignored scenario examples in VsTest Result Provider ([340](https://github.com/picklesdoc/pickles/pull/340)) (by [@dirkrombauts](https://github.com/dirkrombauts))
- Enable xUnit Test Result Provider to Deal with more than 255 Scenarios in a File ([405](https://github.com/picklesdoc/pickles/pull/405)) (by [@dirkrombauts](https://github.com/dirkrombauts))

## [2.11.0] - 2016-12-05

### Changed

- Enable xUnit Test Result Provider to Deal with more than 255 Scenarios in a File ([397](https://github.com/picklesdoc/pickles/pull/397)) (by [@eugene-sea](https://github.com/eugene-sea))
- Several external libraries were update to their newest versions ([394](https://github.com/picklesdoc/pickles/pull/394)) (by [@dirkrombauts](https://github.com/dirkrombauts))

### Fixed

- Null reference error when running with MsTest trx results file ([356](https://github.com/picklesdoc/pickles/issues/356)) (by [@dirkrombauts](https://github.com/dirkrombauts))
- Test result file is read as null in CucumberJson when elements is missing ([390](https://github.com/picklesdoc/pickles/issues/390)) (by [@dirkrombauts](https://github.com/dirkrombauts))
- DHTML version sorts features and folders in descending order ([383](https://github.com/picklesdoc/pickles/issues/383)) (by [@aquilanl](https://github.com/aquilanl))

## [2.10.0] - 2016-10-22

### Changed

- Enable Pickles to with cultures in the language setting ([308](https://github.com/picklesdoc/pickles/pull/308)) (by [@dirkrombauts](https://github.com/dirkrombauts))
- Improve Markdown Parsing to Reduce Unintended Block Quotes ([302](https://github.com/picklesdoc/pickles/pull/302)) (by [@dirkrombauts](https://github.com/dirkrombauts))

## [2.9.0] - 2016-10-07

### Changed

- Show parser failures and quit with an error ([379](https://github.com/picklesdoc/pickles/pull/379)) (by [@Sjaaky](https://github.com/Sjaaky)).

### Fixed

- Fix Importing Test Results Failure for MsTest for Ignored Scenarios ([378](https://github.com/picklesdoc/pickles/pull/378)) (by [@wbagit](https://github.com/wbagit)).
- Better Dealing with Special Characters in Scenario Examples ([375](https://github.com/picklesdoc/pickles/pull/375)) (by [@thopark](https://github.com/thopark)).

## [2.8.3] - 2016-09-28

### Fixed

- Compatibility with nunit.console 3.x and nunit.framework 2.x ([369](https://github.com/picklesdoc/pickles/pull/369)) (by [@lars-erik](https://github.com/lars-erik)).
- Correct sorting of features in output file (MS Word) ([357](https://github.com/picklesdoc/pickles/issues/357)) (by [@lars-erik](https://github.com/lars-erik) and [@dirkrombauts](https://github.com/dirkrombauts)).

## [2.8.2] - 2016-08-18

### Fixed

- Handle Encoding or Opposite Slash in Feature Paths ([358](https://github.com/picklesdoc/pickles/pull/362)) (by [@ocsurfnut](https://github.com/ocsurfnut)).

## [2.8.1] - 2016-07-29

### Fixed

- Blank Example Entries in a Scenario Outline cause Pickles not to Find the matching Scenario Outline ([358](https://github.com/picklesdoc/pickles/pull/358)) (by [@aaronjrich](https://github.com/aaronjrich)).

## [2.8.0] - 2016-06-29

### Added

- Hyperlink Feature #1: Automatic Hyperlink Generation for Scenario Titles ([320](https://github.com/picklesdoc/pickles/issues/320)) (by [@ocsurfnut](https://github.com/ocsurfnut)).

## [2.7.0] - 2016-06-14

### Added

- Make #-style comments configurable - Default to True ([346](https://github.com/picklesdoc/pickles/issues/346)) (by [@ocsurfnut](https://github.com/ocsurfnut)).

## [2.6.3] - 2016-05-24

### Fixed

- Fix path handling for Unix and Mac ([344](https://github.com/picklesdoc/pickles/issues/344)) (by [@jboffel](https://github.com/jboffel)).


## [2.6.2] - 2016-05-11

### Fixed

- Command line help does not list all possible test result formats ([340](https://github.com/picklesdoc/pickles/issues/340)) (by [@magicmonty](https://github.com/magicmonty)).
- Some examples were not recognized ([#343](https://github.com/picklesdoc/pickles/issues/343)) (by [@danielpullwitt](https://github.com/danielpullwitt)).


## [2.6.1] - 2016-05-10

### Changed

- Remove Dependency on AutoMapper ([#333](https://github.com/picklesdoc/pickles/issues/333)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- Update DHTML version to use Knockout version 3.4.0 ([#325](https://github.com/picklesdoc/pickles/issues/325)) (by [@dirkrombauts](https://github.com/dirkrombauts)).

### Fixed

- General handling of special characters in scenario outline inputs, and XUnit failed, if multiple TestResults.xml were used ([336](https://github.com/picklesdoc/pickles/issues/336)) (by [@magicmonty](https://github.com/magicmonty)).
- SpecFlow conformant name mapping in other test result providers ([#326](https://github.com/picklesdoc/pickles/issues/326)) (by [@danielpullwitt](https://github.com/danielpullwitt)).

## [2.6.0] - 2016-04-12

### Added

- Support for multiple tags in DHTML version ([#283](https://github.com/picklesdoc/pickles/issues/283)) (by [@aaronjrich](https://github.com/aaronjrich)).
- Output commented lines ([#271](https://github.com/picklesdoc/pickles/issues/271)) (by [@ludwigjossieaux](https://github.com/ludwigjossieaux)).

### Changed

- Use version 4 of the Gherkin parser ([#322](https://github.com/picklesdoc/pickles/issues/322)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- Use new logo as icon and in nuget packages ([#323](https://github.com/picklesdoc/pickles/issues/323)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- Update external libraries ([#310](https://github.com/picklesdoc/pickles/issues/310)) (by [@dirkrombauts](https://github.com/dirkrombauts)).

### Fixed

- SpecFlow conformant name mapping in nUnit test result provider ([#315](https://github.com/picklesdoc/pickles/issues/315)) (by [@danielpullwitt](https://github.com/danielpullwitt)).
- Prevent crash when no Description is provided ([#314](https://github.com/picklesdoc/pickles/issues/314)) (by [@danielpullwitt](https://github.com/danielpullwitt)).


## [2.5.0] - 2016-03-21

### Added

- Experimental features - see the [documentation](http://docs.picklesdoc.com/en/latest/ExperimentalFeatures/) (by [@dirkrombauts](https://github.com/dirkrombauts)).

### Experimental

- Using a different MarkDown component. Warning: it is not entirely compatible with the static HTML version. ([#269](https://github.com/picklesdoc/pickles/issues/269)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- Enabling mathematics in the description elements in the DHTML version. Warning: requires internet connectivity. ([#281](https://github.com/picklesdoc/pickles/issues/281)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- Overview Dashboard on DHTML Pickles Site with Summary Charts by Namespace and Tags ([#295](https://github.com/picklesdoc/pickles/pull/295)) (by [@ocsurfnut](https://github.com/ocsurfnut))

## [2.4.1] - 2016-03-01

### Fixed

- Scenario Outline with parenthesis parses correctly and reports the correct result ([#299](https://github.com/picklesdoc/pickles/pull/299)) (by [@ocsurfnut](https://github.com/ocsurfnut))

## [2.4.0] - 2016-02-26

### Added

- Support for the test result format of VsTest.Console.exe ([#280](https://github.com/picklesdoc/pickles/issues/280)) (by [@dirkrombauts](https://github.com/dirkrombauts)).

### Changed

- The MsTest test result provider is now able to give the result of individual examples in a scenario outline ([#285](https://github.com/picklesdoc/pickles/issues/285)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- The SpecFlow+ Runner (formerly SpecRun) test result provider is now able to give the result of individual examples in a scenario outline. See the [documentation](http://docs.picklesdoc.com/en/latest/IntegratingTestResultsFromSpecRun/) for an important caveat. ([#286](https://github.com/picklesdoc/pickles/issues/286)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- The Cucumber test result provider is now able to give the result of individual examples in a scenario outline ([#287](https://github.com/picklesdoc/pickles/issues/287)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- The GUI now uses a combobox to display the choice of test result formats ([#297](https://github.com/picklesdoc/pickles/issues/297)) (by [@dirkrombauts](https://github.com/dirkrombauts)).


### Fixed

- Word document is corrupt if a Feature has no description ([#261](https://github.com/picklesdoc/pickles/issues/261)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- The Cucumber JSON test result provider should deal with background steps correctly  ([#293](https://github.com/picklesdoc/pickles/issues/293)) (by [@dirkrombauts](https://github.com/dirkrombauts) based on [an idea by MikeThomas64](https://github.com/picklesdoc/pickles/pull/251)).


## [2.3.0] - 2016-01-27

### Added

- Support for SpecFlow v2 ([#276](https://github.com/picklesdoc/pickles/issues/276)) (by [@dirkrombauts](https://github.com/dirkrombauts)).

## [2.2.1] - 2016-01-25

### Changed

- The .nuspec files now contain a releaseNotes elements that points to this file on GitHub.

### Fixed

- xUnit1 and xUnit2: Failure when tests are included that don't have traits ([#268](https://github.com/picklesdoc/pickles/issues/268)) (by [@dirkrombauts](https://github.com/dirkrombauts)).

## [2.2.0] - 2016-01-15

### Added

- Support for xUnit 2 ([#230](https://github.com/picklesdoc/pickles/issues/230)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- Support for nUnit 3 ([#230](https://github.com/picklesdoc/pickles/issues/224)) (by [@dirkrombauts](https://github.com/dirkrombauts)).

### Changed

- The Gherkin parser was updated to version 3.2.0 ([#256](https://github.com/picklesdoc/pickles/pull/256)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- Other dependencies were updated (by [@dirkrombauts](https://github.com/dirkrombauts)).

### Fixed

- The language parameter now correctly sets the default language to use when parsing feature files ([#260](https://github.com/picklesdoc/pickles/issues/260)) (by [@dirkrombauts](https://github.com/dirkrombauts)).
- The MarkDown code in the examples was corrected ([#254](https://github.com/picklesdoc/pickles/issues/254)) (by [@dirkrombauts](https://github.com/dirkrombauts))


## [2.1.0] - 2015-12-30

### Added

- The language of the `.feature` files can now be set in the MSBuild runner as well. See MSBuild configuration should map Language too ([#236](https://github.com/picklesdoc/pickles/issues/236)).

### Fixed

- Remove potential load failure from the Command Line, GUI and MSBuild runners ([#243](https://github.com/picklesdoc/pickles/issues/243))

### Changed

- Some external components were updated.

## [2.0.4] - 2015-12-27

### Fixed

- The PowerShell runner does not load ([#239](https://github.com/picklesdoc/pickles/issues/239))


## [2.0.3] - 2015-15-23

### Fixed

- The presence of a background causes a crash when integrating test results ([#238](https://github.com/picklesdoc/pickles/issues/238))


## [2.0.2] - 2015-11-21

### Fixed

- Test results in example rows of scenario outlines that contain `$` signs were not correctly identified ([#227](https://github.com/picklesdoc/pickles/issues/227))
- Table layout in the DHTML version was sometimes unnecessarily narrow ([#225])https://github.com/picklesdoc/pickles/pull/225))


## [2.0.1] - 2015-11-20

### Fixed

- A crash was fixed that occurred when linking nUnit test results  ([#223](https://github.com/picklesdoc/pickles/issues/223))


## [2.0.0] - 2015-11-03

### Removed

- Pickles no longer supports running on .NET version prior to 4.5: Pickles now uses version 3 of the Gherkin parser, and as such requires .NET 4.5. The new parser is a lot smaller and should be faster too.


## [1.2.3] - 2015-10-06

### Fixed

- In the DHtml version, the alignment of the keywords (Given, When, Then) was fixed for non-English languages ([#219](https://github.com/picklesdoc/pickles/issues/219))


## [1.2.2] - 2015-09-24

### Fixed

- The PowerShell version now correctly traces relative paths from the current directory ([#216](https://github.com/picklesdoc/pickles/issues/216))


## [1.2.1] - 2015-08-14

### Fixed

- In the Html version, the alignment of the keywords (Given, When, Then) was fixed for non-English languages ([#209](https://github.com/picklesdoc/pickles/issues/209))
- A NullReferenceException in the MSTest provider was fixed when the test results also contain ordinary unit test results in addition to the SpecFlow results ([#212](https://github.com/picklesdoc/pickles/issues/212))


## [1.2.0] - 2015-07-28

### Added

- In the DHtml version, there are now links to hide/show the navigation, and to collapse/expand all nodes in the navigation ([#204](https://github.com/picklesdoc/pickles/issues/204))

### Fixed

- The command line version no longer silently fails when an error occurs. You will need to adapt the configuration in order to get more information from the error ([#202]((https://github.com/picklesdoc/pickles/issues/202))
- The issue of failing silently in case the full filename is too long has been fixed ([#199](https://github.com/picklesdoc/pickles/issues/199))


## [1.1.0] - 2015-05-12

### Added

- In both the DHtml and Html versions: Align scenario keywords to the right side so that step texts start from the same position ([#195](https://github.com/picklesdoc/pickles/issues/195))

### Fixed

- Abbreviations displayed incorrectly with spaces between letters ([#192](https://github.com/picklesdoc/pickles/issues/192))


## [1.0.1] - 2015-03-31

### Fixed

- In the DHtml version, feature wide tags with capitals can not be filtered on ([#194](https://github.com/picklesdoc/pickles/issues/194))
- Tables in background for Word & Excel  ([#191](https://github.com/picklesdoc/pickles/issues/191))


## [1.0.0] - 2014-12-15

### Removed

- Support for the nAnt runner has been discontinued. nAnt users can migrate by using nAnt's capabilities to call the command line runner of Pickles. ([#179](https://github.com/picklesdoc/pickles/pull/179))

### Fixed

- The checkmark/cross icon in the GUI client did not always show the cross icon when appropriate, and its state could be changed by the user. ([#177](https://github.com/picklesdoc/pickles/pull/177))


## [0.20.0] - 2014-12-10

### Added

- The command line and powershell runners now return an error code of 0 when everything went fine, and 1 if there was an error.
- The HTML and DHTML output versions no longer horizontally spread tables.
- The GUI version has an icon

### Changed

- The msbuild runner will now break the build when an error occurs. This reflects our belief that the Living Documentation is a first class output of the software development process.


## [0.19.0] - 2014-11-25

### Added

- Each runner has its own download package
- Several improvements to the DHTML version
  - A progress indicator when loading features for the first time
  - Add a toggle to show/hide folder contents (top level folders are collapsed by default)
- The Nuget package for the MSBuild runner adds an msbuild target file - see [MSBuild Task](http://www.picklesdoc.com/#!Pages/MSBuildTask.md) for documentation ([#157](https://github.com/picklesdoc/pickles/pull/157))
- The text "pickled on" was replaced with "generated on".

### Changed

- For contributors: we are now using a new assert library: [NFluent](http://n-fluent.net/).

### Removed

- DITA support has been discontinued. ([#153](https://github.com/picklesdoc/pickles/pull/153))


## [0.18.2] - 2014-09-12

### Fixed

- Fixed issue where the Html version didn't render the pass/fail icons ([#139](https://github.com/picklesdoc/pickles/issues/139))
- Fixed issue where the Html version generated an unexpected entry for a file in obj/Debug ([#140](https://github.com/picklesdoc/pickles/issues/140))
- Removed dependencies from the Nuget packages ([#142](https://github.com/picklesdoc/pickles/issues/142))
- The nuget packages Pickles.CommandLine and Pickles.MSBuild contain the files twice ([#144](https://github.com/picklesdoc/pickles/issues/144))


## [0.18.1] - 2014-09-01

### Fixed

- Fixed issue where multiline arguments of gherkin steps would not be rendered in the DHtml verion ([#134](https://github.com/picklesdoc/pickles/issues/134))


## [0.18.0] - 2014-07-07

### Changed

- The folder structure of the dependencies of the static HTML version was changed. If you override the .css files, you will need to update your workflow to account for that.

### Fixed

- Fixed issue where multiple results files for a single feature causes exceptions when processing scenario outline examples ([#123](https://github.com/picklesdoc/pickles/pull/123))
- Fixed issue where pickles would crash when a specified test results file does not exist ([#126](https://github.com/picklesdoc/pickles/issues/126))


## [0.17.4] - 2014-06-04

### Fixed

- Description blocks are rendered as code elements in DHtml version ([#118](https://github.com/picklesdoc/pickles/issues/118))


## [0.17.3] - 2014-05-28

### Fixed

- PowerShell version works again


## [0.17.2] - 2014-05-28

Some things went wrong during the publication of versions 0.17.0 and 0.17.1. Therefore 0.17.2 is the first published 0.17.x version.

### Added

- Added test status in the DHTML browsing section (left nav) (#110)

### Fixed

- Crash because of nunit result (#108)
- Avoid crash on start of GUI (#112)


## [0.17.1] - 2014-05-28 [YANKED]


## [0.17.0] - 2014-05-28 [YANKED]


## [0.16.0] - 2014-05-15

### Added

- Enable results for individual example results in a scenario outline (in the Html version) ([#105](https://github.com/picklesdoc/pickles/pull/105))

### Fixed

- Features don't render correctly when there are less than 4 spaces ([#106](https://github.com/picklesdoc/pickles/pull/106))


## [0.15.0] - 2014-04-11

### Added

- Allow multiple result files ([#83](https://github.com/picklesdoc/pickles/issues/83)): it is now possible to use multiple test result files. This will come in handy if you need to partition your test runs. Simple use a semicolon-separated list of file paths instead of the single file path as an argument


## [0.14.0] - 2014-03-27

### Added

- The (static) Html version now supports images with .png, .gif, .jpg and .bmp extensions. This partially addresses issue ([#12](https://github.com/picklesdoc/pickles/issues/12))
- Several updates of external dependencies, including the long out-of-date IKVM. Yay!
- All production code and test code now use the excellent [System.IO.Abstraction](https://github.com/tathamoddie/System.IO.Abstractions) library and its TestingHelpers classes.
- We are using build services provided by [MyGet](http://www.myget.org/)


## [0.13.1] - 2013-12-20

### Fixed

- DHTML uses the Feature's description for the Background ([#74](https://github.com/picklesdoc/pickles/issues/74))
- Nuget manifest should mention PicklesDoc instead of only Jeffrey Cameron ([#84](https://github.com/picklesdoc/pickles/issues/84))
- Crash when a feature is not present in the test result (with NUnit) ([#85](https://github.com/picklesdoc/pickles/issues/85))


## [0.13.0] - 2013-11-29

### Added

- The UI version now includes the option to generate each selected output format in its own directory ([#78](https://github.com/picklesdoc/pickles/pull/78))

### Fixed

- Left nav bug fix in DHTML format ([#73](https://github.com/picklesdoc/pickles/pull/73))
- DHTML uses the Feature's description for the Background ([#74](https://github.com/picklesdoc/pickles/issues/74))


## [0.12.1] - 2013-10-28

### Fixed

- Fixes a .NET runtime version issue caused by combining all Pickles runners in one directory. We are back to each runner in a separate directory, and will likely remain so until all assemblies use the same .NET framework version.


## [0.12.0] - 2013-10-25

### Added

- DHTML output format should render Markdown correctly (one step closer toward feature parity for all export formats) ([#60](https://github.com/picklesdoc/pickles/issues/60))

### Changed

- The .zip file contains all 5 Pickles runners in one directory, reducing the size of the download by roughly three quarters.

### Bug(s) Fixed

- When selecting multiple output formats in the UI, pickles generates the first output format multiple times ([#66](https://github.com/picklesdoc/pickles/issues/66))


## [0.11.0] - 2013-09-26

### Added

- Better Error Reporting: both the command line client and the UI show a log ([#36](https://github.com/picklesdoc/pickles/issues/36))

### Fixed

- Pickles crashes when generating excel with really long descriptions ([#62](https://github.com/picklesdoc/pickles/issues/62))
- Calling pickles.exe without any arguments in a directory without features causes a crash ([#63](https://github.com/picklesdoc/pickles/issues/63))


## [0.10.0] - 2013-08-29

### Added

- Add support for SpecRun test results ([#21](https://github.com/picklesdoc/pickles/issues/21)). Refer to the wiki page for instructions on how to configure SpecRun correctly: [Integrating Test Results From SpecRun](http://www.picklesdoc.com/#!Pages/IntegratingTestResultsFromSpecRun.md).

### Fixed

- Please implement marking ignored test cases with yellow sign rather than red for all Test Result providers (not just MSTest) ([#47](https://github.com/picklesdoc/pickles/issues/47))
- Remove duplication of example feature files ([#50](https://github.com/picklesdoc/pickles/issues/50))
- Adding background to features in word ([#58](https://github.com/picklesdoc/pickles/pull/58))
- Handle cucumber feature with no scenarios ([#59](https://github.com/picklesdoc/pickles/pull/59))


## [0.9.0] - 2013-07-23


## [0.8.0] - 2012-12-17


## [0.7.0] - 2012-08-07


## [0.6.0] - [SKIPPED]


## [0.5.0] - 2012-03-13


## [0.4.0] - 2012-01-20


## [0.3.0] - 2011-12-13


## [0.2.0] - 2011-11-14


## [0.1.0] - 2011-10-22
