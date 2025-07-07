# -------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# --------------------------------------------------------------------------
"""
Model classes for Azure OpenAI Chat Application.
This module provides data classes for representing chat messages and model configurations
used in the Azure OpenAI-powered chat application.
"""
from dataclasses import dataclass
from typing import List, Optional, Dict


@dataclass
class AzureOpenAIConfiguration:
    """
    Represents the configuration for Azure OpenAI service.
    Maps to configuration values with keys 'api_key', 'endpoint', and 'deployment_name'.
    """

    api_key: str
    endpoint: str
    deployment_name: str
    api_version: Optional[str] = None


@dataclass
class ChatCompletionConfiguration:
    """
    Represents the configuration for an AI model including messages and parameters.
    Maps to configuration values with keys 'model', 'messages', 'max_tokens', 'temperature', and 'top_p'.
    """

    model: Optional[str] = None
    messages: Optional[List[Dict[str, str]]] = None
    max_tokens: int = 1024
    temperature: float = 0.7
    top_p: float = 0.95
