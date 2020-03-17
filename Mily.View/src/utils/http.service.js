import axios from 'axios';
import cookie from 'js-cookie';
import lzstring from 'lz-string'

let PendingRqesut = [];
// 默认把请求视为切换路由就会把pending状态的请求取消，false为不取消

//https://juejin.im/post/5d441a98e51d4561db5e39ee
/**
 * 创建请求
 */
const service = () => {
    let Instance = axios.create({
        baseURL: 'http://127.0.0.1:8520/Proxy/',
        timeout: 3000,
        headers: { "Content-Type": "application/json" },
        withCredentials: false,
    });
    // 默认把请求视为切换路由就会把pending状态的请求取消，false为不取消
    Instance.defaults.routeChangeCancel = true;
    // 请求拦截
    instance.interceptors.request.use(ReqeustFilter, error => Promise.reject(error));
    // 响应拦截
    instance.interceptors.response.use(response => {
        ResponseFilter(response);
        return response.data.Data;
    }, error => {
            let ErrorFormat = {};
            let Response = error.response;
            // 请求已发出，但服务器响应的状态码不在 2xx 范围内
            if (Response) {
                handleResponseIntercept(Response.config);
                // 设置返回的错误对象格式（按照自己项目实际需求）
                errorFormat = {
                    status: response.status,
                    data: response.data.Data
                };
            }
            // 如果是主动取消了请求，做个标识
            if (axios.isCancel(error)) {
                ErrorFormat.SelfCancel = true;
            }
            // 其实还有一个情况
            // 在设置引发错误的请求时，error.message才是错误信息
            // 但我觉得这个一般是脚本错误，我们项目提示也不应该提示脚本错误给用户看，一般都是我们自定义一些默认错误提示，如“创建成功！”
            // 所以这里不针对此情况做处理。

            return Promise.reject(errorFormat);
    });
    return Instance;
}
/**
 * 请求拦截器
 * @param {any} config
 */
const ReqeustFilter = option => {
    // 区别请求的唯一标识，这里用方法名+请求路径
    // 如果一个项目里有多个不同baseURL的请求
    // 可以改成`${option.method} ${option.baseURL}${option.url}`
    let RequestMark = `${option.method}[${option.url}]`;
    // 找当前请求的标识是否存在PendingRqesut中，即是否重复请求了
    let MarkIndex = PendingRqesut.findIndex(item => {
        return item.name === RequestMark;
    });
    //存在，说明重复了
    if (MarkIndex > -1) {
        // 取消上个重复的请求
        PendingRqesut[MarkIndex].cancel();
        // 删掉在pendingRequest中的请求标识
        PendingRqesut.splice(MarkIndex, 1);
    }
    // （重新）新建针对这次请求的axios的cancelToken标识
    const CancelToken = axios.CancelToken;
    const Source = CancelToken.source();
    config.cancelToken = Source.token;
    // 设置自定义配置RequestMark项，主要用于响应拦截中
    option.Mark = RequestMark;
    // 记录本次请求的标识
    PendingRqesut.push({
        name: RequestMark,
        cancel: Source.cancel,
        routeChangeCancel: option.routeChangeCancel // 可能会有优先级高于默认设置的routeChangeCancel项值
    });
    let Head = JSON.parse(lzstring.decompressFromBase64(option.headers.Author));
    if (!Head.Cross) {
        //如果登录了就把cookie设置到头里
        option.data.Global = cookie.get("Global");
        option.headers["Authorization"] = cookie.get("Authorization");
    }
    option.data = JSON.stringify(option.data);
    return option;
}
/**
 * 响应拦截器
 * @param {any} option
 */
const ResponseFilter = setting => {
    // 根据请求拦截里设置的RequestMark配置来寻找对应PendingRqesut里对应的请求标识
    let MarkIndex = PendingRqesut.findIndex(item => {
        return item.name === setting.config.Mark;
    });
    // 找到了就删除该标识
    if (MarkIndex > -1)
        pendingRequest.splice(MarkIndex, 1);
    let Head = JSON.parse(lzstring.decompressFromBase64(setting.option.headers.Author));
    if (Head.Cross) {
        cookie.set("Global", lzstring.compressToBase64(setting.data.Data.ResultData.Data.KeyId));
        cookie.set("Authorization", setting.data.Data.ResultData.AuthorToken);
    }
}


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