# Baseline Test Secenarios for Feature Management Libraries

## No Filters Test Scenarios

The following test scenarios use NoFilters.sample.json as the baseline file for testing the Feature Management libraries.

| Test # | Feature Flag Name   | Expected Result | Description                              |
|--------|---------------------|-----------------|------------------------------------------|
| 1      | EnabledFeatureFlag  | True            | An Enabled Feature Flag with no Filtered |
| 2      | DisabledFeatureFlag | False           | A Disabled Feature Flag with no Filters  |

## Time Window Filter Test Scenarios

The following test scenarios use TimeWindowFilter.sample.json as the baseline file for testing the Feature Management libraries.

| Test # | Feature Flag Name | Expected Result | Description                                                       |
|--------|-------------------|-----------------|-------------------------------------------------------------------|
| 1      | PastTimeWindow    | False           | Time Window filter where both Start and End have already passed   |
| 2      | FutureTimeWindow  | False           | Time Window filter where neither Start nor End have happened      |
| 3      | PresentTimeWindow | True            | Time Window filter where Start has happend but End hasn't happend |
| 4      | StartedTimeWindow | True            | Time Window filter with a Start that has passed                   |
| 5      | WillEndTimeWindow | True            | Time Window filter where the End hasn't passed                    |

## Targeting Filter Test Scenarios

The following test scenarios use TargetingFilter.sample.json as the baseline file for testing the Feature Management libraries.

| Test # | Feature Flag Name       | Inputs                            | Expected Result | Description                                                                                                              |
|--------|-------------------------|-----------------------------------|-----------------|--------------------------------------------------------------------------------------------------------------------------|
| 1      | ComplexTargeting        | user="Aiden"                      | False           | Targeting Filter, Aiden is not part of the default rollout                                                               |
| 2      | ComplexTargeting        | user="Blossom"                    | True            | Targeting Filter, Blossom is part of the default rollout                                                                 |
| 3      | ComplexTargeting        | user="Alice"                      | True            | Targeting Filter, Alice is a targeted user                                                                               |
| 4      | ComplexTargeting        | user="Aiden" groups=["Stage1"]    | True            | Targeting Filter, Aiden now part of the 100% rolled out group Stage1 is now targeted                                     |
| 5      | ComplexTargeting        | groups=["Stage2"]                 | False           | Targeting Filter, no user Stage 2 is 50% rolloued out.                                                                   |
| 6      | ComplexTargeting        | user="Aiden" groups=["Stage2"]    | True            | Targeting Filter, Aiden who is not part of the default rollout is part of the first 50% of Stage 2                       |
| 7      | ComplexTargeting        | user="Chad" groups=["Stage2"]     | False           | Targeting Filter, Chad is neither part of the default rollout or part of the first 50% of Stage 2                        |
| 8      | ComplexTargeting        | groups=["Stage3"]                 | False           | Targeting Filter, the Stage 3 is group is on the exclusion list                                                          |
| 9      | ComplexTargeting        | user="Alice" groups=["Stage3"]    | False           | Targeting Filter, Alice who is targeted is excluded because she is part of the Stage 3 group                             |
| 10     | ComplexTargeting        | user="Blossom" groups=["Stage3"]  | False           | Targeting Filter, Blossom who was enabled by the default rollout is now excluded as part of the Stage 3 group            |
| 11     | ComplexTargeting        | user="Dave" groups=["Stage1"]     | False           | Targeting Filter, Dave is on the exclusion list, is still excluded even though he is part of the 100% rolled out Stage 1 |
| 12     | RolloutPercentageUpdate | user="Aiden"                      | True            | Targeting Filter, 62% default rollout Aiden is part of it.                                                               |
| 13     | RolloutPercentageUpdate | user="Aiden" groups=["Stage1"]    | True            | Targeting Filter, group is not part of default rollout calculation, no change                                            |
| 14     | RolloutPercentageUpdate | user="Aiden" groups=["Stage2"]    | True            | Targeting Filter, group is not part of default rollout calculation, no change                                            |
| 15     | RolloutPercentageUpdate | user="Aiden" groups=["Stage3"]    | True            | Targeting Filter, group is not part of default rollout calculation, no change                                            |
| 16     | RolloutPercentageUpdate | user="Brittney"                   | False           | Targeting Filter, 62% default rollout Brittney is not part of it                                                         |
| 17     | RolloutPercentageUpdate | user="Brittney" groups=["Stage1"] | False           | Targeting Filter, group is not part of default rollout calculation, no change                                            |
| 18     | RolloutPercentageUpdate | user="Brittney" groups=["Stage2"] | False           | Targeting Filter, group is not part of default rollout calculation, no change                                            |
| 19     | RolloutPercentageUpdate | user="Brittney" groups=["Stage3"] | False           | Targeting Filter, group is not part of default rollout calculation, no change                                            |

### Validation of Rollout Percentage

The above test scenarios need to be rerun with TargetingFilter.modified.sample.json as the baseline file for testing the Feature Management libraries. This validates that the same formula is being used to caluclate the rollout percentage. The modified files has a roolout percentage 1 higher than the original which takes Brittnety from being disabled to enabled. So, the results of tests 16 through 19 should be True.

| Test # | Feature Flag Name       | Inputs                            | Expected Result | Description                                                                                                              |
|--------|-------------------------|-----------------------------------|-----------------|--------------------------------------------------------------------------------------------------------------------------|
| 13     | RolloutPercentageUpdate | user="Aiden" groups=["Stage1"]    | True            | Targeting Filter, group is not part of default rollout calculation, no change                                            |
| 14     | RolloutPercentageUpdate | user="Aiden" groups=["Stage2"]    | True            | Targeting Filter, group is not part of default rollout calculation, no change                                            |
| 15     | RolloutPercentageUpdate | user="Aiden" groups=["Stage3"]    | True            | Targeting Filter, group is not part of default rollout calculation, no change                                            |
| 16     | RolloutPercentageUpdate | user="Brittney"                   | True            | Targeting Filter, 62% default rollout Brittney is not part of it                                                         |
| 17     | RolloutPercentageUpdate | user="Brittney" groups=["Stage1"] | True            | Targeting Filter, group is not part of default rollout calculation, no change                                            |
| 18     | RolloutPercentageUpdate | user="Brittney" groups=["Stage2"] | True            | Targeting Filter, group is not part of default rollout calculation, no change                                            |
| 19     | RolloutPercentageUpdate | user="Brittney" groups=["Stage3"] | True            | Targeting Filter, group is not part of default rollout calculation, no change                                            |

## Requirement Type Test Scenarios

The following test scenarios use RequirementType.sample.json as the baseline file for testing the Feature Management libraries.

| Test # | Feature Flag Name                    | Inputs      | Expected Result | Description                                                                                                     |
|--------|--------------------------------------|-------------|-----------------|-----------------------------------------------------------------------------------------------------------------|
| 1      | FirstOrFeatureFlag                   | N/A         | True            | Feature Flag with two feature filters, first returns true, so it's enabled.                                     |
| 2      | SecondOrFeatureFlag                  | N/A         | True            | Feature Flag with two feature filters, second returns true, so it's enabled.                                    |
| 3      | FristRequirementTypeAnyFeatureFlag   | N/A         | True            | Feature Flag with two feature filters and requirement type specified as Any. Second filter returns true         |
| 4      | SecondtRequirementTypeAnyFeatureFlag | N/A         | False           | Feature Flag with two feature filters and requirement type specified as Any. Neither filter returns true        |
| 5      | TrueRequirementTypeAllFeatureFlag    | user="Adam" | False           | Feature Flag with two feature filters and requirement type specified as All. Only the first filter returns true |
| 6      | FalseRequirementTypeAllFeatureFlag   | user="Adam" | True            | Feature Flag with two feature filters and requirement type specified as All. Both filters return true           |
