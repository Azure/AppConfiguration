from django.shortcuts import render
from django.conf import settings

def index(request):
    # Refresh the configuration from App Configuration service.
    settings.AZURE_APPCONFIGURATION.refresh()
    # Update Django settings with the app configuration key-values
    settings.CONFIG.update(settings.AZURE_APPCONFIGURATION)
    context = {
        "message": settings.CONFIG.get('message'),
        "key": settings.CONFIG.get('secret_key'),
        "color": settings.CONFIG.get('color'),
        "font_size": settings.CONFIG.get('font_size')
        }
    return render(request, 'hello_azure/index.html', context)
