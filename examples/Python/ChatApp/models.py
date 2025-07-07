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
from typing import List, Optional, Dict, Any


class AzureOpenAIConfiguration:
    """
    Represents the configuration for Azure OpenAI service.
    Maps to configuration values with keys 'api_key', 'endpoint', and 'deployment_name'.
    """

    def __init__(
        self,
        api_key: str,
        endpoint: str,
        deployment_name: Optional[str] = None,
        api_version: Optional[str] = None,
    ):
        """Initialize Azure OpenAI configuration with API key and endpoint."""
        self.api_key = api_key
        self.endpoint = endpoint
        self.deployment_name = deployment_name
        self.api_version = api_version

class ChatCompletionConfiguration:
    """
    Represents the configuration for an AI model including messages and parameters.
    Maps to configuration values with keys 'model', 'messages', 'max_tokens', 'temperature', and 'top_p'.
    """

    def __init__(
        self,
        model: Optional[str] = None,
        messages: Optional[List[Dict[str, str]]] = None,
        max_tokens: int = 1024,
        temperature: float = 0.7,
        top_p: float = 0.95,
    ):
        """Initialize model configuration with parameters for OpenAI API calls."""
        self.model = model
        self.messages = messages or []
        self.max_tokens = int(max_tokens) if max_tokens is not None else 1024
        self.temperature = float(temperature) if temperature is not None else 0.7
        self.top_p = float(top_p) if top_p is not None else 0.95

    @classmethod
    def from_dict(cls, data: Dict[str, Any]) -> "ChatCompletionConfiguration":
        """Create a ChatCompletionConfiguration instance from a dictionary."""
        messages = data.get("messages", [])

        return cls(
            model=data.get("model"),
            messages=messages,
            max_tokens=data.get("max_tokens", 1024),
            temperature=data.get("temperature", 0.7),
            top_p=data.get("top_p", 0.95),
        )
