import axios from 'axios';
import cookie from 'js-cookie';
import lzstring from 'lz-string'
import Enumerable from 'linq';

//请求队列
let RequestPendingArray = [];

/**
 * 组织重复请求
 * @param {string} RequestURL
 * @param {any} Cancel
 */
const StopRepeatRequest = (RequestURL, Config) => {
    let Tigger = Enumerable.from(RequestPendingArray).indexOf(item => item.name == RequestURL);
    if (Tigger > -1) //重复了
    {
        RequestPendingArray[Tigger].cancel()//取消上个重复请求
        // 删掉在pendingRequest中的请求标识
        RequestPendingArray.splice(Tigger, 1);
    }
    Config.cancelToken = axios.CancelToken.source().token;
    Config.RequestURL = RequestURL;
    RequestPendingArray.push({
        name: RequestURL,
        cancel: axios.CancelToken.source().cancel,
        routeChangeCancel: Config.routeChangeCancel
    })
    return Config;
}

/**
 * 允许请求
 * @param {any} RequestURL
 */
const AllowRequest = (RequestURL) => {
    let Tigger = Enumerable.from(RequestPendingArray).indexOf(item => item.name == RequestURL);
    if (Tigger > -1) RequestPendingArray.splice(Tigger, 1);
}

const Request = axios.create({
    baseURL: 'http://127.0.0.1:8520/Proxy/',
    timeout: 8000,
    headers: { "Content-Type": "application/json" },
    withCredentials: false,
    routeChangeCancel: true
});

Request.interceptors.request.use(Config => {
    let RequestURL = `${Config.method}[${Config.url}]`;
    let Head = JSON.parse(lzstring.decompressFromBase64(Config.headers.Author));
    if (!Head.Cross) {
        //登录后将数据保存到Cookie
        Config.data.Global = cookie.get("Global");
        Config.headers["Authorization"] = cookie.get("Authorization");
    }
    Config.data = JSON.stringify(Config.data);
    StopRepeatRequest(RequestURL, Config);
    return new Promise((resolve, reject) => {
        resolve(Config);
    });
}, Err => { return Promise.reject(Err) });

Request.interceptors.response.use(Response => {
    let Head = JSON.parse(lzstring.decompressFromBase64(Response.config.headers.Author));
    if (Head.Cross) {
        cookie.set("Global", lzstring.compressToBase64(Response.data.Data.ResultData.Data.KeyId));
        cookie.set("Authorization", Response.data.Data.ResultData.AuthorToken);
    }
    AllowRequest(Response.config.RequestURL);
    return Response.data.Data;
}, Err => {
        if (Err && Err.response) {
            console.log(Err.response.status);
        } else {
            console.log('连接到服务器失败')
        }
        return Promise.resolve(Err.response)
})

export default Request;