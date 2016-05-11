'use strict';

var assert = require('assert');

module.exports = function() {

  this.Then(/^the scenario will 'pass_(\d+)'$/, function (result, callback) {
    // nothing to be done here
    callback();
  });

  this.Then(/^the scenario will 'inconclusive_(\d+)'$/, function (result, callback) {
    // we want pending here
    callback(null, 'pending');
  });

  this.Then(/^the scenario will 'fail_(\d+)'$/, function (result, callback) {
    assert.equal("true", "false");
    callback();
  });

  this.When(/^I have backslashes in the value, for example a '(.*)'$/, function (filePath, callback) {
    // nothing to be done here
    callback();
  });

  this.When(/^I have parenthesis in the value, for example an '(.*)'$/, function (description, callback) {
  	// nothing to be done here
  	callback();
  });
  
  this.When(/^I have special characters for regexes in the value, for example a '(.*)'$/, function (description, callback) {
    // nothing to be done here
    callback();
  });
};
