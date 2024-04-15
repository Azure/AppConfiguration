from dataclasses import dataclass
from flask_login import UserMixin
from app import db


@dataclass
class Quote:
    message: str
    author: str


# Create user model
class Users(UserMixin, db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(250), unique=True, nullable=False)
    password = db.Column(db.String(250), nullable=False)
