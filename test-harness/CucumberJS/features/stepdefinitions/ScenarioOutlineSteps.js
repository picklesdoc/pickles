'use strict';

var assert = require('assert');
var {defineSupportCode} = require('cucumber');

defineSupportCode(function({Then, When}) {

  Then(/^the scenario will 'pass_(\d+)'$/, function (result, callback) {
    // nothing to be done here
    callback();
  });

  Then(/^the scenario will 'inconclusive_(\d+)'$/, function (result, callback) {
    // we want pending here
    callback(null, 'pending');
  });

  Then(/^the scenario will 'fail_(\d+)'$/, function (result, callback) {
    assert.equal("true", "false");
    callback();
  });

  Then(/^the scenario will 'pass'$/, function (callback) {
    // nothing to be done here
    callback();
  });

  Then(/^the scenario will 'inconclusive'$/, function (callback) {
    // we want pending here
    callback(null, 'pending');
  });

  Then(/^the scenario will 'fail'$/, function (callback) {
    assert.equal("true", "false");
    callback();
  });

  When(/^I have backslashes in the value, for example a '(.*)'$/, function (filePath, callback) {
    // nothing to be done here
    callback();
  });

  When(/^I have parenthesis in the value, for example an '(.*)'$/, function (description, callback) {
    // nothing to be done here
    callback();
  });

  When(/^I have special characters for regexes in the value, for example a '(.*)'$/, function (description, callback) {
    // nothing to be done here
    callback();
  });

  When(/^I have a field with value '(.*)'$/, function name(value, callback) {
    // nothing to be done here
    callback();
  });
});
