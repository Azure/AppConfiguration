# Baseline Test Secenarios for Feature Management Libraries

## No Filters Test Scenarios

The following tests are the baseline file for testing the Feature Management libraries. These tests validate feature flags that have no filters applied to them.

[No Filters Tests](NoFilters.tests.json)

[No Filters Test Scenarios](NoFilters.sample.json)

## Time Window Filter Test Scenarios

The following tests are the baseline file for testing the Feature Management libraries. These tests validate feature flags that have time window filters applied to them.

[Time Window Filter Tests](TimeWindowFilter.tests.json)

[Time Window Filter Test Scenarios](TimeWindowFilter.sample.json)

## Targeting Filter Test Scenarios

The following tests are the baseline file for testing the Feature Management libraries. These tests validate feature flags that have targeting filters applied to them.

[Targeting Filter Tests](TargetingFilter.tests.json)

[Targeting Filter Test Scenarios](TargetingFilter.sample.json)

### Validation of Rollout Percentage

The following tests are the baseline file for testing the Feature Management libraries. These tests validate that the same formula is being used to calculate the rollout percentage. The modified files have a rollout percentage 1 higher than the original which takes Brittney from being disabled to enabled. So, the results of tests 12 through 19 should be True.

[Targeting Filter Tests](TargetingFilter.modified.tests.json)

[Targeting Filter Test Scenarios](TargetingFilter.modified.sample.json)

## Requirement Type Test Scenarios

The following tests are the baseline file for testing the Feature Management libraries. These tests validate feature flags that contain multiple feature filters and the requirement type field usage with the Any and All operators.

[Requirement Type Tests](RequirementType.tests.json)

[Requirement Type Test Scenarios](RequirementType.sample.json)
