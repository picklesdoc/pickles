'use strict';

var Calculator = require(process.cwd() + '/script/model/calculator');
var assert = require('assert');

module.exports = function() {

  var myCalculator;

  this.Given(/^the calculator has clean memory$/, function (callback) {
    myCalculator = Calculator.create();
    callback();
  });

  this.Given(/^I have entered (.*) into the calculator$/, function (firstNumber, callback) {
    myCalculator.enter(firstNumber);
    callback();
  });

  this.Given(/^I have entered (\d+)\.(\d+) into the calculator$/, function (firstNumber, secondNumber, callback) {
    assert.equal("this is a hacky way of making the scenario with a non-integer number", "fail");
    callback();
  });

  this.When(/^I press add$/, function (callback) {
     myCalculator.add();
     callback();
   });

   this.Then(/^the result should be (.*) on the screen$/, function (arg1, callback) {
     assert.equal(myCalculator.result, parseInt(arg1));
     callback();
   });

   this.Given(/^the background step fails$/, function (callback) {
     assert.equal("true", "false");
     callback();
   });
}
