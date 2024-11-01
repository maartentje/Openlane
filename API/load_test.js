import http from 'k6/http';
import {check, sleep} from 'k6';

export let options = {
    stages: [
        {duration: '1s', target: 500}, // Ramp-up to 500 virtual users in 1s
        {duration: '1m', target: 500}, // Stay at 500 users for 1 minute
    ]
};

export default function () {
    let body = {
        Price: getRandomInt(5_000),
        CarTitle: "Golf Car"
    }

    let res = http.post('http://host.docker.internal:5000/api/offer', JSON.stringify(body), {
        headers: {'Content-Type': 'application/json'}
    });

    check(res, {'is status 200': (r) => r.status === 200});
    // Wait for 2 seconds between each request
    sleep(2); 
}

function getRandomInt(max) {
    //min price is 100
    return ((Math.floor(Math.random() * max)) + 100);
}