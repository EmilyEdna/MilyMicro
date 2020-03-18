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

Request.defaults.request.use(Config => {
    let RequestURL = `${Config.method}[${config.url}]`;
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

Request.defaults.response.use(Response => {
    let Head = JSON.parse(lzstring.decompressFromBase64(Response.config.headers.Author));
    if (Head.Cross) {
        cookie.set("Global", lzstring.compressToBase64(Response.data.Data.ResultData.Data.KeyId));
        cookie.set("Authorization", Response.data.Data.ResultData.AuthorToken);
    }
    AllowRequest(AllowRequest.config.RequestURL);
    return new Promise((resolve, reject) => {
        resolve(Response.data.Data);
    });
}, Err => {
        if (Err && Err.response) {
            switch (Err.response.status) {
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
        return Promise.resolve(Err.response)
})

export default Request;