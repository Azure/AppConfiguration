/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for
 * license information.
 */
package com.example;
import com.example.entity.FeatureFilterEvaluationContext;

/**
 * This class defines a custom exception type for when an expected Filter is not found
 * when checking if a Feature is enabled. A FilterNotFoundException is only thrown when
 * failfast is enabled, which is true by default.
 *
 */
public class FilterNotFoundException extends RuntimeException {

    private static final long serialVersionUID = 1L;

    private final FeatureFilterEvaluationContext filter;
    
    private final String message;

    /**
     * Creates a new instance of the FilterNotFoundException
     * 
     * @param message the error message.
     * @param filter The filter context used to find the not found filter.
     */
    public FilterNotFoundException(String message, FeatureFilterEvaluationContext filter) {
        super(message);
        this.message = message;
        this.filter = filter;
    }

    @Override
    public String getMessage() {
        if (filter == null) {
            return getCause().getMessage() + ".";
        }
        return this.message + ": " + filter.getName();

    }

}
