/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for
 * license information.
 */
package com.example;

import java.io.IOException;
import java.util.HashMap;

import com.example.feature.filters.PercentageFilter;

public class HelloWorld {
    
    private static final String CONFIG_STORE_CONNECTION_STRING = System.getenv().get("CONFIG_STORE_CONNECTION_STRING");
    private static final String PERCENTAGE_FILTER = "percentageFilter";
    private static final String EMPTY_LABEL = "\0";

    public static void main(String[] args) throws IOException {
        HashMap<String, FeatureFilter> featureFilters = new HashMap<String, FeatureFilter>();
        featureFilters.put(PERCENTAGE_FILTER, new PercentageFilter());
        
        String label = EMPTY_LABEL;
        
        FeatureManager featureManager = new FeatureManager(CONFIG_STORE_CONNECTION_STRING, label, featureFilters);
        
        if (featureManager.isEnabledAsync("Beta").block()) {
            System.out.println("Hello World");
        } else {
            System.out.println("Bye World");
        }
    }

}
