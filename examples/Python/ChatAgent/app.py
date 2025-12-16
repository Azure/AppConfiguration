import asyncio
import os
from agent_framework.declarative import AgentFactory
from azure.identity import DefaultAzureCredential
from azure.appconfiguration.provider import load

async def main():
    endpoint = os.environ["AZURE_APPCONFIGURATION_ENDPOINT"]
    credential = DefaultAzureCredential()

    config = load(endpoint=endpoint, credential=credential)

    yaml_str = config["ChatAgent:Spec"]
    
    agent = AgentFactory(client_kwargs={"credential": credential, "project_endpoint": config["ChatAgent:ProjectEndpoint"]}).create_agent_from_yaml(yaml_str)

    while True:
        print("How can I help? (type 'quit' to exit)")

        user_input = input("User: ")
        
        if user_input.lower() in ['quit', 'exit', 'bye']:
            break
    
        response = await agent.run(user_input)
        print("Agent response: ", response.text)
        input("Press enter to continue...")
            
    print("Exiting... Goodbye...")

if __name__ == "__main__":
    asyncio.run(main())
