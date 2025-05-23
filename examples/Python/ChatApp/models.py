# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.
#
from pydantic import BaseModel, Field
from typing import List, Optional


class Message(BaseModel):
    """
    Represents a chat message with a role and content.
    Maps to configuration values with keys 'role' and 'content'.
    """
    role: Optional[str] = None
    content: Optional[str] = None


class ModelConfiguration(BaseModel):
    """
    Represents the configuration for an AI model including messages and parameters.
    Maps to configuration values with keys 'model', 'messages', 'max_tokens', 'temperature', and 'top_p'.
    """
    model: Optional[str] = None
    messages: List[Message] = Field(default_factory=list)
    max_tokens: int = 1024
    temperature: float = 0.7
    top_p: float = 0.95