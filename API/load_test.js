import http from 'k6/http';
import {check, sleep} from 'k6';

export let options = {
    stages: [
        {duration: '30s', target: 50}, // Ramp-up to 50 virtual users over 30s
        {duration: '1m', target: 50}, // Stay at 50 users for 1 minute
        {duration: '30s', target: 0},   // Ramp-down to 0 virtual users over 30s
    ]
};

export default function () {
    let body = {
        Id: getRandomId(),
        Price: getRandomInt(5_000),
        State: getRandomInt(3)
    }

    let res = http.post('http://host.docker.internal:5000/api/sendMessage', JSON.stringify(body), {
        headers: {'Content-Type': 'application/json'}
    });

    check(res, {'is status 200': (r) => r.status === 200});
    // Wait for 2 seconds between each request
    sleep(2); 
}

function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}

//Makes random test id [A-0 -> Z-499]
//this way, it occasionally generates the same id, which will update instead of create
function getRandomId() {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    const charactersLength = characters.length;
    result += characters.charAt(Math.floor(Math.random() * charactersLength));
    result += `-${getRandomInt(500)}`;
    return result;
}