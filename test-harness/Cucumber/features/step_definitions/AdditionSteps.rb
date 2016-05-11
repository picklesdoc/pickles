Given(/^the calculator has clean memory$/) do
  @result = 0
  @numbersList = []
end

Given(/^I have entered (\d+) into the calculator$/) do |arg1|
  @numbersList << arg1.to_i
end

Given(/^I have entered (\d+)\.(\d+) into the calculator$/) do |arg1, arg2|
  expect("this is a hacky way of making the scenario with a non-integer number").to eql("fail")
end

When(/^I press add$/) do
  @numbersList.each do |number|
      @result = @result + number
  end
end

Then(/^the result should be (\d+) on the screen$/) do |arg1|
  expect(@result).to eql(arg1.to_i)
end

Then(/^the result should be (\d+)\.(\d+) on the screen$/) do |arg1, arg2|
  expect("this is a hacky way of making the scenario with a non-integer number").to eql("fail")
end

Given(/^the background step fails$/) do
  expect("true").to eql("false")
end
