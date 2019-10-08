import axios from 'axios';

const service = axios.create({
    timeout: 5000
});

service.interceptors.request.use(config => {
    config.data = JSON.stringify(config.data);
    return config;
}, error => {
        return Promise.reject(error);
})