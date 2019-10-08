import axios from 'axios';

const service = axios.create({
    baseURL: 'http://127.0.0.1:9091/Condition/',
    timeout: 5000
});

service.interceptors.request.use(config => {
    config.data = JSON.stringify(config.data);
    return config;
}, error => {
    return Promise.reject(error);
})
service.interceptors.response.use(response => {
    return response.data.Data;
}, err => {
    if (err && err.response) {
        switch (err.response.status) {
            case 400:
                console.log('��������')
                break;
            case 401:
                console.log('δ��Ȩ�������µ�¼')
                break;
            case 403:
                console.log('�ܾ�����')
                break;
            case 404:
                console.log('�������,δ�ҵ�����Դ')
                break;
            case 405:
                console.log('���󷽷�δ����')
                break;
            case 408:
                console.log('����ʱ')
                break;
            case 500:
                console.log('�������˳���')
                break;
            case 501:
                console.log('����δʵ��')
                break;
            case 502:
                console.log('�������')
                break;
            case 503:
                console.log('���񲻿���')
                break;
            case 504:
                console.log('���糬ʱ')
                break;
            case 505:
                console.log('http�汾��֧�ָ�����')
                break;
            default:
                console.log(`���Ӵ���${err.response.status}`)
        }
    } else {
        console.log('���ӵ�������ʧ��')
    }
    return Promise.resolve(err.response)
});

export default service;