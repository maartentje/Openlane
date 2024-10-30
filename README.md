# Openlane

This solution contains multiple projects responsible for sending offers to a queue, reading/processing them and eventually store them in a database.

## Features

- Simple API endpoint to post message to a RabbitMQ queue
- Listens to the queue for incoming offers
- Process each offer
- Store and update offers in an SQL Server database
- Scalable design for use with Docker

## Install & Run

1. Clone this repository
2. Build the Docker images using the provided `docker-compose.yml` file.  
```docker-compose up --build```
3. When everything is running, you can send a POST-request to `http://localhost:5000/api/sendMessage` with an example body:  
```{"Id":"Test-Id", "Price":"99.99", "State":"0"}```
4. To check the result you can either look at the logs or connect to the database and see the offer data
   - Server name: `localhost`
   - Login: `sa`
   - Password: `Password1*`
5. To get started with load-testing, open a terminal in the API project and run the following command:  
```cat load_test.js | docker run --rm -i grafana/k6 run -```
6. This will send a lot of requests to the API
   - The API will only put messages on the queue
   - The project we stress is `OfferWorker` - with one replica this will handle the messages very slow (~5s/message)  
   - To scale this, change the `replicas` value in `docker-compose.yml`
   - Restart the container (2.) and the load-test (6.) to see a noticeable difference