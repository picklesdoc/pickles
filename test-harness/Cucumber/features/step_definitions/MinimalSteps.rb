Then(/^passing step$/) do
  # Nothing to be done here
end

Then(/^inconclusive step$/) do
  pending # We want pending here
end

Then(/^failing step$/) do
  expect("true").to eql("false")
end
