#!/usr/bin/env bash
python /code/manage.py makemigrations
python /code/manage.py migrate
exec "$@"