# command to build:
# - docker build . -t django-rest-api
# or use the docker-compose.yml with the build parameter
FROM python:3.7.6-alpine

# defining the working directory which will receive the web_project files
WORKDIR /code

# creating virtual environment and adding to path
ENV VIRTUAL_ENV=/venv
RUN python -m venv $VIRTUAL_ENV
ENV PATH="$VIRTUAL_ENV/bin:$PATH"

# upgrading pip version
RUN pip install --upgrade pip

# some features needed to install requirements.txt on the alpine linux:
RUN apk update && apk add --no-cache postgresql postgresql-dev gcc python3-dev musl-dev bash graphviz graphviz-dev ttf-freefont
# to use postgre psql tool (to access db content directly) include postgresql on the list

# avoid pyc files
ENV PYTHONDONTWRITEBYTECODE 1

# python output is sent straight to terminal (container log) without being buffered
# so the output of the application (django logs) can be seen in real time.
ENV PYTHONUNBUFFERED 1

# copy the whole django project into /code and installing the requirements
COPY requirements.txt .
RUN pip install -r requirements.txt
COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh

#ENTRYPOINT ["docker-entrypoint.sh"]
#CMD ["docker-entrypoint.sh"]
#RUN mkdir templates
#RUN echo "yes" | python /code/manage.py collectstatic