/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for
 * license information.
 */
package com.example.feature.filters;

import static com.example.FilterParameters.TIME_WINDOW_FILTER_SETTING_END;
import static com.example.FilterParameters.TIME_WINDOW_FILTER_SETTING_START;

import java.time.ZonedDateTime;
import java.time.format.DateTimeFormatter;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import com.example.FeatureFilter;
import com.example.entity.FeatureFilterEvaluationContext;

/**
 * A feature filter that can be used at activate a feature based on a time window.
 */
public class TimeWindowFilter implements FeatureFilter {

    private static final Logger LOGGER = LoggerFactory.getLogger(TimeWindowFilter.class);

    /**
     * Evaluates whether a feature is enabled based on a configurable time window.
     * @param context The feature evaluation context.
     * @return True if the feature is enabled, false otherwise.
     */
    public boolean evaluate(FeatureFilterEvaluationContext context) {
        String start = (String) context.getParameters().get(TIME_WINDOW_FILTER_SETTING_START);
        String end = (String) context.getParameters().get(TIME_WINDOW_FILTER_SETTING_END);

        ZonedDateTime now = ZonedDateTime.now();

        if (start != null  && !start.equals("") && end != null  && !end.equals("")) {
            LOGGER.warn("The {} feature filter is not valid for feature {}. It must specify either {}, {}, or both.",
                    this.getClass().getSimpleName(), context.getName(), TIME_WINDOW_FILTER_SETTING_START,
                    TIME_WINDOW_FILTER_SETTING_END);
            return false;
        }

        ZonedDateTime startTime = start != null  && !start.equals("")
                ? ZonedDateTime.parse(start, DateTimeFormatter.RFC_1123_DATE_TIME)
                : null;
        ZonedDateTime endTime = end != null  && !end.equals("")
                ? ZonedDateTime.parse(end, DateTimeFormatter.RFC_1123_DATE_TIME)
                : null;

        return (start != null  && !start.equals("") || now.isAfter(startTime))
                && (end != null  && !end.equals("") || now.isBefore(endTime));
    }

}
