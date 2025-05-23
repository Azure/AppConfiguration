# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.
#
from typing import List, Optional, Dict, Any


class Message:
    """
    Represents a chat message with a role and content.
    Maps to configuration values with keys 'role' and 'content'.
    """

    def __init__(self, role: Optional[str] = None, content: Optional[str] = None):
        self.role = role
        self.content = content

    @classmethod
    def from_dict(cls, data: Dict[str, Any]) -> "Message":
        """Create a Message instance from a dictionary."""
        return cls(role=data.get("role"), content=data.get("content"))


class ModelConfiguration:
    """
    Represents the configuration for an AI model including messages and parameters.
    Maps to configuration values with keys 'model', 'messages', 'max_tokens', 'temperature', and 'top_p'.
    """

    def __init__(
        self,
        model: Optional[str] = None,
        messages: Optional[List[Message]] = None,
        max_tokens: int = 1024,
        temperature: float = 0.7,
        top_p: float = 0.95,
    ):
        self.model = model
        self.messages = messages or []
        self.max_tokens = int(max_tokens) if max_tokens is not None else 1024
        self.temperature = float(temperature) if temperature is not None else 0.7
        self.top_p = float(top_p) if top_p is not None else 0.95

    @classmethod
    def from_dict(cls, data: Dict[str, Any]) -> "ModelConfiguration":
        """Create a ModelConfiguration instance from a dictionary."""
        messages = [Message.from_dict(msg) for msg in data.get("messages", [])]

        return cls(
            model=data.get("model"),
            messages=messages,
            max_tokens=data.get("max_tokens", 1024),
            temperature=data.get("temperature", 0.7),
            top_p=data.get("top_p", 0.95),
        )
