# Openlane

This solution contains multiple projects responsible for sending offers to a queue, reading/processing them and eventually store them in a database.

## Features

- Simple API endpoint to post message to a RabbitMQ queue
- Listens to the queue for incoming offers
- Process each offer
- Store and update offers in an SQL Server database
- Scalable design for use with Docker

## Installation

1. Clone this repository
2. Build the Docker images using the provided `docker-compose.yml` file.  
```docker-compose up --build```
3. To get started with load-testing, open a terminal in the API project and run the following command:  
```cat load_test.js | docker run --rm -i grafana/k6 run -```
4. This will send a lot of requests to the API
   - The API will only put messages on the queue
   - The project we stress is `OfferWorker` 
   - To scale this, run `docker-compose up -d --scale event-processor-extra=3 --no-recreate`