## Introduction

This project implements two REST APIs developed in Django and ASP.Netcore. Both work independently and have the same functionality (basic API to store Segment, Clients and Transactions).
The purpose was to replicate the backend keeping the same frontend (developed in Angular).
It has a PostgreSQL database that aims to store exchange rates for different segments (as registered) to perform metrics involving these rates.
At first, the only metric implemented is the conversion of foreign currency into BRL.


## Technologies

Back-end (API):
 - API 1: Django Rest Framework `3.13` (Python `3.7`);
 - API 2: ASP.Netcore Framework `6.0` (C# `10.0`).
Front-end:
 - Angular version `13.3.0`;
 - HTML/CSS.

Other languages/technologies used:
 - Docker

### Structure

```shell
.
├── CurrencyConvert                                                       # Front-end Angular
│   ├── dist                                                              # Build result - used in nginx
│   ├── src                                                               # Source code
│   │   ├── app
│   │   │   ├── components                                                # Defining the Front-end templates functionality
│   │   │   │   ├── calculate-fee
│   │   │   │   │   ├── calculate-fee.component.css
│   │   │   │   │   ├── calculate-fee.component.html
│   │   │   │   │   ├── calculate-fee.component.spec.ts
│   │   │   │   │   └── calculate-fee.component.ts
│   │   │   │   ├── client
│   │   │   │   │   ├── client.component.css
│   │   │   │   │   ├── client.component.html
│   │   │   │   │   ├── client.component.spec.ts
│   │   │   │   │   └── client.component.ts
│   │   │   │   ├── fee-details
│   │   │   │   │   ├── fee-details.component.css
│   │   │   │   │   ├── fee-details.component.html
│   │   │   │   │   ├── fee-details.component.spec.ts
│   │   │   │   │   └── fee-details.component.ts
│   │   │   │   ├── fee-list
│   │   │   │   │   ├── fee-list.component.css
│   │   │   │   │   ├── fee-list.component.html
│   │   │   │   │   ├── fee-list.component.spec.ts
│   │   │   │   │   └── fee-list.component.ts
│   │   │   │   └── segment
│   │   │   │       ├── segment.component.css
│   │   │   │       ├── segment.component.html
│   │   │   │       ├── segment.component.spec.ts
│   │   │   │       └── segment.component.ts
│   │   │   ├── models                                                    # Modeling Json format to submit to the API
│   │   │   │   ├── client.model.spec.ts
│   │   │   │   ├── client.model.ts
│   │   │   │   ├── feescharged.model.spec.ts
│   │   │   │   ├── feescharged.model.ts
│   │   │   │   ├── segment.model.spec.ts
│   │   │   │   └── segment.model.ts 
│   │   │   ├── services                                                  # Defining the endpoints (URL) and HTTP methods to be called
│   │   │   │   ├── client.service.spec.ts
│   │   │   │   ├── client.service.ts
│   │   │   │   ├── feescharged.service.spec.ts
│   │   │   │   ├── feescharged.service.ts
│   │   │   │   ├── segment.service.spec.ts
│   │   │   │   └── segment.service.ts
│   │   │   ├── app.component.css
│   │   │   ├── app.component.html
│   │   │   ├── app.component.spec.ts
│   │   │   ├── app.component.ts                                          # Root components
│   │   │   ├── app.module.ts
│   │   │   └── app-routing.module.ts                                     # Defining API endpoints
│   │   ├── environments                                                  # Angular environment variables (prod or dev)
│   │   ├── favicon.ico
│   │   ├── index.html                                                    # Root HTML - used by Nginx
│   │   ├── main.ts
│   │   ├── polyfills.ts                                                  # Internal angular CLI
│   │   ├── styles.css
│   │   └── test.ts
│   ├── angular.json                                                      # Default angular paths
│   ├── Dockerfile                                                        # Dockerfile to build
│   ├── package.json                                                      # Default angular packages
│   ├── package-lock.json
│   ├── README.md
│   ├── tsconfig.app.json
│   ├── tsconfig.json                                                     # Default angular ts configs
│   └── tsconfig.spec.json
│
├── django-rest-api                                                       # Django project (python)
│   ├── fee_app
│   │   ├── migrations                                                    # Django ORM migrations
│   │   ├── admin.py
│   │   ├── apps.py
│   │   ├── models.py                                                     # Modeling database
│   │   ├── serializers.py
│   │   ├── tests.py
│   │   ├── urls.py                                                       # Defining endpoints
│   │   ├── utils.py                                                      # Calling ExchangeRates API
│   │   └── views.py                                                      # Implementing endpoints logic
│   ├── fee_project
│   │   ├── asgi.py
│   │   ├── settings.py                                                   # General project settings
│   │   ├── urls.py
│   │   └── wsgi.py
│   ├── docker-entrypoint.sh                                              # Entrypoint to start database migrations when starting the project
│   ├── Dockerfile                                                        # Dockerfile to build
│   ├── manage.py
│   └── requirements.txt                                                  # Libraries
│
├── netcore-rest-api                                                      # Asp.netcore project (c#)
│   ├── FeeApi
│   │   ├── Controllers                                                   # Defining URL endpoints and implementing logic
│   │   │   ├── ClientController.cs
│   │   │   ├── FeeItemsController.cs
│   │   │   └── SegmentController.cs
│   │   ├── Models                                                        # Modeling database
│   │   │   ├── FeeContext.cs
│   │   │   └── FeeItem.cs
│   │   ├── Properties
│   │   ├── Utils
│   │   │   └── utils.cs                                                  # Calling ExchangeRates API
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.json
│   │   ├── FeeApi.csproj                                                 # Libraries
│   │   └── Program.cs                                                    # General project settings
│   └── Dockerfile                                                        # Dockerfile to build
│
├── nginx_http
│   ├── django_project.conf
│   └── netcore_project.conf
├── postgres_data_django                                                  # Database generated after initialized
├── postgres_data_netcore                                                 # Database generated after initialized
├── details.txt
├── docker-compose-django.yaml                                            # Compose file for django
├── docker-compose-netcore.yaml                                           # Compose file for netcore
├── README.md
└── variables.env                                                         # Docker environment variables
```


## Running the project:

To run the project it is necessary to have `docker` and `docker-compose` installed in your environment.

```shell
# To build the Django application:
$ docker-compose -f docker-compose-django.yaml build

# To build the Asp.Netcore application:
$ docker-compose -f docker-compose-netcore.yaml build

# Initializing project (Django, Angular, PostgreSql and Nginx):
$ docker-compose -f docker-compose-django.yaml up

# Initializing project (Asp.Netcore, Angular, PostgreSql and Nginx):
$ docker-compose -f docker-compose-netcore.yaml up
```

From that point on, all containers will be available.
To access the application, just open the address `http://localhost` in your browser.

### Configuring the ExchangeRates API (consuming external API)

The ExchangeRates API (free version) is used to acquire the currency conversion rate. This version only offers the possibility to convert from EUR (Euro) to BRL (Real).
Its use is not mandatory, if not configured, this API will use the default value of `4.7776` defined in:
 - Django: django-rest-api/fee_app/utils.py
 - Asp.netcore: netcore-rest-api/FeeApi/Utils/utils.cs

Steps to set up ExchangeRates:
1. Create your free account at https://exchangeratesapi.io/
2. Copy your given API_KEY and paste it inside variables.env file

### Handling the project

* Open the browser and go to `http://localhost` (it redirects to `localhost/fees-charged`);
* Register new segments by clicking on `Novo Segmento` (`localhost/new-segment`);
* Register new clients by clicking on `Novo Cliente` (`localhost/new-client`);
* Register new purchases (fees) by clicking on `Comprar` (`localhost/calculate-fee`);
* See transactions on `Home` or `Taxas Cobradas` (`localhost/fees-charged`).
* To edit/delete a transaction, select it from the `Lista de Taxas Cobradas` and click `Editar`.

Steps to first transaction (buy):
1. Register some segment;
2. Register some customer using the segment;
3. Make purchases using the registered customer's cpf (document).

## Development

The structure contained in the `docker-compose-<framework>.yaml` file provides distinct containers for:
* API (Django or Asp.Netcore);
* Database (PostgreSql);
* Front-end (Angular);
* Reverse Proxy (Nginx).

### API Implementad Routes:

* POST `/api/new-client`: Create new client
    ```
    # Json post example:
    {"name": "Helder", "cpf": 123, "segment": "Premium"}
    ```
* POST `/api/fees-charged`: Create new charge
    ```
    # Json post example:
    {"client": 123, "source_currency_amount": "70"}
    ```
* PUT `/api/fees-charged/:cpf/:id`: Update a charge by cpf and id
    ```
    # Json post example:
    {"source_currency_amount": "70"}
    ```
* GET `/api/fees-charged` - Retrieve all fees of all clients
* GET `/api/fees-charged/:cpf:id` - Retrieve a specific charged fee by client and id
* GET `/api/fees-charged?client=[cpf]&segment=[text]` - Filter all fees by client and/or segment
* DELETE `/api/fees-charged/:cpf:id` - Delete a charged fee by cpf and id
* DELETE `/api/fees-charged` - Delete all charged fees

### Database

The model diagram can be seen in the root directory of this project as `database_diagram.png`.

It was generated using the Django model but the Asp.Netcore is similar.
Command to generate database diagram (it needs django-extensions and pygraphviz libs):
```shell
$ docker exec -ti web python manage.py graph_models fee_app -g -o database_diagram.png
```
