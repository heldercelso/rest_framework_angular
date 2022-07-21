# command to build:
# - docker build . -t currency_convert_front_end
# or use the docker-compose.yml with the build parameter

FROM node:16

RUN mkdir /usr/src/app
 
WORKDIR /usr/src/app
COPY package.json package-lock.json ./

RUN npm install
RUN npm install @angular/cli --location=global

COPY . .

RUN ng build