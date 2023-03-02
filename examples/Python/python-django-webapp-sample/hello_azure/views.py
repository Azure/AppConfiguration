from django.shortcuts import render
from django.conf import settings

def index(request):
    context = {
        "name": settings.USER_NAME,
        "key": settings.SECRET_KEY,
        "color": settings.COLOR,
        "font_size": settings.FONT_SIZE
        }
    return render(request, 'hello_azure/index.html', context)
