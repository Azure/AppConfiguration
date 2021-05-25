/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for
 * license information.
 */
package com.example;

import java.io.IOException;
import java.util.HashMap;
import java.util.LinkedHashMap;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.azure.core.http.rest.PagedIterable;
import com.azure.data.appconfiguration.ConfigurationClient;
import com.azure.data.appconfiguration.ConfigurationClientBuilder;
import com.azure.data.appconfiguration.models.ConfigurationSetting;
import com.azure.data.appconfiguration.models.SettingSelector;
import com.example.entity.Feature;
import com.example.entity.FeatureFilterEvaluationContext;
import com.example.entity.FeatureManagementItem;
import com.example.entity.FeatureSet;
import com.fasterxml.jackson.databind.ObjectMapper;

import reactor.core.publisher.Mono;

public class FeatureManager {

    private static final Logger LOGGER = LoggerFactory.getLogger(FeatureManager.class);

    private static final String FEATURE_FLAG_PREFIX = ".appconfig.featureflag/";

    private static final String FEATURE_FLAG_CONTENT_TYPE = "application/vnd.microsoft.appconfig.ff+json;charset=utf-8";

    private static ObjectMapper mapper = new ObjectMapper();

    private HashMap<String, FeatureFilter> featureFilters;

    private HashMap<String, Feature> featureManagement;

    private HashMap<String, Boolean> onOff;

    public FeatureManager(String connectionString, String label, HashMap<String, FeatureFilter> featureFilters) throws IOException {
        this.featureFilters = featureFilters;
        ConfigurationClientBuilder builder = new ConfigurationClientBuilder();
        ConfigurationClient client = builder.connectionString(connectionString).buildClient();
        SettingSelector settingSelector = new SettingSelector().setKeyFilter(".appconfig*").setLabelFilter(label);
        PagedIterable<ConfigurationSetting> features = client.listConfigurationSettings(settingSelector);

        FeatureSet featureSet = createFeatureSet(features);
        HashMap<String, Object> featureFlags = featureSet.getFeatureManagement();

        featureManagement = new HashMap<String, Feature>();
        onOff = new HashMap<String, Boolean>();

        for (String key : featureFlags.keySet()) {
            addToFeatures(featureFlags, key, "");
        }
    }

    private FeatureSet createFeatureSet(PagedIterable<ConfigurationSetting> settings)
            throws IOException {
        FeatureSet featureSet = new FeatureSet();
        // Reading In Features
        for (ConfigurationSetting setting : settings) {
            Object feature = createFeature(setting);
            if (feature != null) {
                featureSet.addFeature(setting.getKey().trim().substring(FEATURE_FLAG_PREFIX.length()), feature);
            }
        }
        return featureSet;
    }

    private Object createFeature(ConfigurationSetting item) throws IOException {
        Feature feature = null;
        if (item.getContentType() != null && item.getContentType().equals(FEATURE_FLAG_CONTENT_TYPE)) {
            try {
                String key = item.getKey().trim().substring(FEATURE_FLAG_PREFIX.length());
                FeatureManagementItem featureItem = mapper.readValue(item.getValue(), FeatureManagementItem.class);
                feature = new Feature(key, featureItem);

                // Setting Enabled For to null, but enabled = true will result in the
                // feature being on. This is the case of a feature is on/off and set to
                // on. This is to tell the difference between conditional/off which looks
                // exactly the same... It should never be the case of Conditional On, and
                // no filters coming from Azure, but it is a valid way from the config
                // file, which should result in false being returned.
                if (feature.getEnabledFor().size() == 0 && featureItem.getEnabled()) {
                    return true;
                } else if (!featureItem.getEnabled()) {
                    return false;
                }
                return feature;

            } catch (IOException e) {
                throw new IOException("Unabled to parse Feature Management values from Azure.", e);
            }

        } else {
            String message = String.format("Found Feature Flag %s with invalid Content Type of %s", item.getKey(),
                    item.getContentType());
            throw new IOException(message);
        }
    }

    @SuppressWarnings("unchecked")
    private void addToFeatures(HashMap<String, Object> features, String key, String combined) {
        Object featureKey = features.get(key);
        if (!combined.isEmpty() && !combined.endsWith(".")) {
            combined += ".";
        }
        if (featureKey instanceof Boolean) {
            onOff.put(combined + key, (Boolean) featureKey);
        } else {
            Feature feature = null;
            try {
                feature = mapper.convertValue(featureKey, Feature.class);
            } catch (IllegalArgumentException e) {
                LOGGER.error("Found invalid feature {} with value {}.", combined + key, featureKey.toString());
            }

            // When coming from a file "feature.flag" is not a possible flag name
            if (feature != null && feature.getEnabledFor() == null && feature.getKey() == null) {
                if (LinkedHashMap.class.isAssignableFrom(featureKey.getClass())) {
                    features = (LinkedHashMap<String, Object>) featureKey;
                    for (String fKey : features.keySet()) {
                        addToFeatures(features, fKey, combined + key);
                    }
                }
            } else {
                if (feature != null) {
                    feature.setKey(key);
                    featureManagement.put(key, feature);
                }
            }
        }
    }

    /**
     * Checks to see if the feature is enabled. If enabled it check each filter, once a
     * single filter returns true it returns true. If no filter returns true, it returns
     * false. If there are no filters, it returns true. If feature isn't found it returns
     * false.
     * 
     * @param feature Feature being checked.
     * @return state of the feature
     * @throws FilterNotFoundException
     */
    public Mono<Boolean> isEnabledAsync(String feature) throws FilterNotFoundException {
        return Mono.just(checkFeatures(feature));
    }

    private boolean checkFeatures(String feature) throws FilterNotFoundException {
        boolean enabled = false;
        if (featureManagement == null || onOff == null) {
            return false;
        }

        Feature featureItem = featureManagement.get(feature);
        Boolean boolFeature = onOff.get(feature);

        if (boolFeature != null) {
            return boolFeature;
        } else if (featureItem == null) {
            return false;
        }

        for (FeatureFilterEvaluationContext filter : featureItem.getEnabledFor().values()) {
            if (filter != null && filter.getName() != null) {
                if (!featureFilters.containsKey(filter.getName())) {
                    String message = "Was unable to find Filter " + filter.getName()
                            + ".";
                    throw new FilterNotFoundException(message, filter);
                }
                FeatureFilter featureFilter = featureFilters.get(filter.getName());
                enabled = Mono.just(featureFilter.evaluate(filter)).block();
            }
            if (enabled) {
                return enabled;
            }
        }
        return enabled;
    }

}
