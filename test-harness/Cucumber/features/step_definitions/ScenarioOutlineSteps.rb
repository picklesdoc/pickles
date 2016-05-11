Then(/^the scenario will 'pass_(\d+)'$/) do |arg1|
  # Nothing to be done here
end

Then(/^the scenario will 'inconclusive_(\d+)'$/) do |arg1|
  pending # We want pending here
end

Then(/^the scenario will 'fail_(\d+)'$/) do |arg1|
  expect("true").to eql("false")
end

When(/^I have backslashes in the value, for example a 'c:\\Temp\\'$/) do
  # Nothing to be done here
end

When(/^I have parenthesis in the value, for example an '(.*)'$/) do |arg1|
  #Nothing to be done here
end

When(/^I have special characters for regexes in the value, for example a '(.*)'$/) do |arg1|
  #Nothing to be done here
end