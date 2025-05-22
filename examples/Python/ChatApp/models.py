# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.
#

class Message:
    """
    Represents a chat message with a role and content.
    Maps to configuration values with keys 'role' and 'content'.
    """
    def __init__(self, role=None, content=None):
        self.role = role
        self.content = content
    
    @classmethod
    def from_dict(cls, data):
        """Create a Message instance from a dictionary."""
        return cls(
            role=data.get('role'),
            content=data.get('content')
        )


class ModelConfiguration:
    """
    Represents the configuration for an AI model including messages and parameters.
    Maps to configuration values with keys 'model', 'messages', 'max_tokens', 'temperature', and 'top_p'.
    """
    def __init__(self, model=None, messages=None, max_tokens=1024, temperature=0.7, top_p=0.95):
        self.model = model
        self.messages = messages or []
        self.max_tokens = max_tokens
        self.temperature = temperature
        self.top_p = top_p
    
    @classmethod
    def from_dict(cls, data):
        """Create a ModelConfiguration instance from a dictionary."""
        messages = [Message.from_dict(msg) for msg in data.get('messages', [])]
        
        return cls(
            model=data.get('model'),
            messages=messages,
            max_tokens=data.get('max_tokens', 1024),
            temperature=data.get('temperature', 0.7),
            top_p=data.get('top_p', 0.95)
        )