from django.shortcuts import render
from django.conf import settings

def index(request):
    context = {
        "message": settings.MESSAGE,
        "key": settings.SECRET_KEY,
        "font_size": settings.FONT_SIZE
        }
    return render(request, 'hello_azure/index.html', context)
