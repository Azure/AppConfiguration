# -------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# --------------------------------------------------------------------------
"""
Model classes for Azure OpenAI Chat Application.
"""
from dataclasses import dataclass
from typing import List, Optional, Dict


@dataclass
class AzureOpenAIConfiguration:
    """
    Represents the configuration for Azure OpenAI service.
    """

    api_key: str
    endpoint: str
    deployment_name: str
    api_version: Optional[str] = None


@dataclass
class ChatCompletionConfiguration:
    """
    Represents the configuration for an AI model including messages and parameters.
    """

    max_tokens: int
    temperature: float
    top_p: float
    model: Optional[str] = None
    messages: Optional[List[Dict[str, str]]] = None
