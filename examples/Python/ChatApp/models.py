# -------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# --------------------------------------------------------------------------
"""
Model classes for Azure AI Foundry Chat Application.
"""
from dataclasses import dataclass
from typing import List, Optional, Dict


@dataclass
class AzureAIFoundryConfiguration:
    """
    Represents the configuration for Azure AI Foundry service.
    """

    endpoint: str


@dataclass
class ChatCompletionConfiguration:
    """
    Represents the configuration for an AI model including messages and parameters.
    """

    model: Optional[str] = None
    max_completion_tokens: Optional[int] = None
    reasoning_effort: Optional[str] = None
    verbosity: Optional[str] = None
    stream: Optional[bool] = None
    messages: Optional[List[Dict[str, str]]] = None
