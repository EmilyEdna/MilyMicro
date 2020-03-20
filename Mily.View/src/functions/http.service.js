import axios from 'axios';
import cookie from 'js-cookie';
import lzstring from 'lz-string'

const service = axios.create({
    baseURL: 'http://127.0.0.1:8520/Proxy/',
    timeout: 3000,
    headers: { "Content-Type": "application/json" },
    withCredentials: false,
});

/*
 *处理请求
 */
service.interceptors.request.use(config => {
    let Head = JSON.parse(lzstring.decompressFromBase64(config.headers.Author));
    if (!Head.Cross) {
        //if not login push the cookie in this data
        config.data.Global = cookie.get("Global");
        config.headers["Authorization"] = cookie.get("Authorization");
    }
    config.data = JSON.stringify(config.data);
    return new Promise(resolve => {
        setTimeout(() => {
            resolve(config);
        }, 500);
    })
}, err => {
    return Promise.reject(err);
})
/*
 *处理返回结果
 */
service.interceptors.response.use(response => {
    let Head = JSON.parse(lzstring.decompressFromBase64(response.config.headers.Author));
    if (Head.Cross) {
        cookie.set("Global", lzstring.compressToBase64(response.data.Data.ResultData.Data.KeyId));
        cookie.set("Authorization", response.data.Data.ResultData.AuthorToken);
    }

    return response.data.Data;
}, err => {
    if (err && err.response) {
        switch (err.response.status) {
            case 400:
                console.log('错误请求')
                break;
            case 401:
                console.log('未授权，请重新登录')
                break;
            case 403:
                console.log('拒绝访问')
                break;
            case 404:
                console.log('请求错误,未找到该资源')
                break;
            case 405:
                console.log('请求方法未允许')
                break;
            case 408:
                console.log('请求超时')
                break;
            case 500:
                console.log('服务器端出错')
                break;
            case 501:
                console.log('网络未实现')
                break;
            case 502:
                console.log('网络错误')
                break;
            case 503:
                console.log('服务不可用')
                break;
            case 504:
                console.log('网络超时')
                break;
            case 505:
                console.log('http版本不支持该请求')
                break;
            default:
                console.log(`连接错误${err.response.status}`)
        }
    } else {
        console.log('连接到服务器失败')
    }
    return Promise.resolve(err.response)
});

export default service;