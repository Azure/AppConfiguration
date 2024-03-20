# Baseline Test Secenarios for Feature Management Libraries

The following test scenarios use FeatureFlag.v2.0.0.schema.sample.json as the baseline file for testing the Feature Management libraries.

| Test # | Feature Flag Name | Inputs                            | Expected Result | Description |
|--------|-------------------|-----------------------------------|-----------------|-------------|
| 1      | Alpha             | N/A                               | True            | An Enabled Feature Flag with no Filtered            |
| 2      | Beta              | N/A                               | False           | A Disabled Feature Flag with no Filters             |
| 3      | Gamma             | N/A                               | False           | Time Window filter where both Start and End have yet to happen            |
| 4      | Delta             | N/A                               | False           | Time Window filter where neither Start nor End have happened            |
| 5      | Epsilon           | N/A                               | True            | Time Window filter with a Start that has passed            |
| 6      | Zeta              | N/A                               | True            | Time Window filter where the End hasn't passed             |
| 7      | Eta               | user="Adam"                       | False           | Targeting Filter, Adam is not part of the default rollout            |
| 8      | Eta               | user="Ellie"                      | True            | Targeting Filter, Ellie is part of the default rollout            |
| 9      | Eta               | user="Alice"                      | True            | Targeting Filter, Alice is a targeted user            |
| 10     | Eta               | user="Adam" groups=["Stage1"]     | True            | Targeting Filter, Adam now part of the 100% rolled out group Stage1 is now targeted            |
| 11     | Eta               | groups=["Stage2"]                 | True            | Targeting Filter, no user Stage 2 is 50% rolloued out.            |
| 12     | Eta               | user="Adam" groups=["Stage2"]     | True            | Targeting Filter, Adam who is not part of the default rollout is part of the first 50% of Stage 2             |
| 13     | Eta               | user="Chad" groups=["Stage2"]     | False           | Targeting Filter, Chad is neither part of the default rollout or part of the first 50% of Stage 2            |
| 14     | Eta               | groups=["Stage3"]                 | False           | Targeting Filter, the Stage 3 is group is on the exclusion list            |
| 15     | Eta               | user="Alice" groups=["Stage3"]    | False           | Targeting Filter, Alice who is targeted is excluded because she is part of the Stage 3 group           |
| 16     | Eta               | user="Ellie" groups=["Stage3"]    | False           | Targeting Filter, Ellie who was enabled by the default rollout is now excluded as part of the Stage 3 group            |
| 17     | Eta               | user="Dave" groups=["Stage1"]     | False           | Targeting Filter, Dave is on the exclusion list, is still excluded even though he is part of the 100% rolled out Stage 1            |
| 18     | Theta             | user="Adam"                       | True            | Targeting Filter, 74% default rollout Adam is part of it.            |
| 19     | Theta             | user="Adam" groups=["Stage1"]     | True            | Targeting Filter, group is not part of default rollout calculation, no change            |
| 20     | Theta             | user="Adam" groups=["Stage2"]     | True            | Targeting Filter, group is not part of default rollout calculation, no change            |
| 21     | Theta             | user="Adam" groups=["Stage3"]     | True            | Targeting Filter, group is not part of default rollout calculation, no change            |
| 22     | Theta             | user="Brittney" groups=["Stage1"] | False           | Targeting Filter, 74% default rollout Brittney is not part of it            |
| 23     | Theta             | user="Brittney" groups=["Stage2"] | False           | Targeting Filter, group is not part of default rollout calculation, no change            |
| 24     | Theta             | user="Brittney" groups=["Stage3"] | False           | Targeting Filter, group is not part of default rollout calculation, no change            |
| 25     | Theta             | user="Brittney" groups=["Stage4"] | False           | Targeting Filter, group is not part of default rollout calculation, no change            |
| 26     | Iota              | N/A                               | True            | Feature Flag with two feature filters, first returns true, so it's enabled.            |
| 27     | Kappa             | N/A                               | True            | Feature Flag with two feature filters, second returns true, so it's enabled.            |
| 28     | Lambda            | N/A                               | True            | Feature Flag with two feature filters and requirement type specified as Any. Second filter returns true            |
| 29     | Mu                | N/A                               | False           | Feature Flag with two feature filters and requirement type specified as Any. Neither filter returns true            |
| 30     | Nu                | user="Adam"                       | False           | Feature Flag with two feature filters and requirement type specified as All. Only the first filter returns true          |
| 31     | Xi                | user="Adam"                       | True            | Feature Flag with two feature filters and requirement type specified as All. Both filters return true            |

## Extra Test Scenario

Update the json to have the default rollout percentage of Theta to be 75% instead of 74% and run the test scenarios again. The expected result for tests 22-25 should now be True, this validates we are using the same fomula for our rollout calculation.
