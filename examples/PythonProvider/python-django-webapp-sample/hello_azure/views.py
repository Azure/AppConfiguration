from django.shortcuts import render, redirect
from django.http import HttpResponse
from django.views.decorators.csrf import csrf_exempt
from django.conf import settings
from django.core.exceptions import ImproperlyConfigured

def index(request):
    print('Request for index page received')
    name = getattr(settings, "USER_NAME", None)
    
    if not name:
        raise ImproperlyConfigured('Configuration could not be loaded')

    language = getattr(settings, "LANGUAGE_CODE", None)
    secret_key = getattr(settings, "SECRET_KEY", None)
    context = {"name": name, "language": language, "key": secret_key}
    return render(request, 'hello_azure/index.html', context)