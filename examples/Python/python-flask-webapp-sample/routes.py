import random

from featuremanagement.appinsights import track_event
from flask import render_template, request, flash, redirect, url_for
from flask_login import current_user, login_user, logout_user
from app import app, azure_app_config, feature_manager, login_manager, db
from model import Quote, Users


@app.route("/", methods=["GET", "POST"])
def index():
    global azure_app_config
    # Refresh the configuration from App Configuration service.
    azure_app_config.refresh()
    context = {}
    user = ""
    if current_user.is_authenticated:
        user = current_user.username
        context["user"] = user
    else:
        context["user"] = "Guest"
    if request.method == "POST":
        track_event("LikedQuote", user)
        return redirect(url_for("index"))

    quotes = [
        Quote("You cannot change what you are, only what you do.", "Philip Pullman"),
    ]

    show_greeting = feature_manager.get_variant("QuoteOfTheDay", user=user).configuration

    context["model"] = {}
    context["model"]["show_greeting"] = show_greeting
    context["model"]["quote"] = {}
    context["model"]["quote"] = random.choice(quotes)
    context["isAuthenticated"] = current_user.is_authenticated

    return render_template("index.html", **context)


@login_manager.user_loader
def loader_user(user_id):
    return Users.query.get(user_id)


@app.route("/register", methods=["GET", "POST"])
def register():
    if request.method == "POST":
        user = Users(username=request.form.get("username"), password=request.form.get("password"))
        try:
            db.session.add(user)
            db.session.commit()
        except Exception as e:
            flash("Username already exists")
            return redirect(url_for("register"))
        login_user(user)

        return redirect(url_for("index"))
    return render_template("sign_up.html")


@app.route("/login", methods=["GET", "POST"])
def login():
    if request.method == "POST":
        user = Users.query.filter_by(username=request.form.get("username")).first()
        if user.password == request.form.get("password"):
            login_user(user)
            return redirect(url_for("index"))
    return render_template("login.html")


@app.route("/logout")
def logout():
    logout_user()
    return redirect(url_for("index"))
