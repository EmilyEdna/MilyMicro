import servcie from './http.service.js'

/**
 * 登录
 * @param {any} param
 */
export const Login = (param) => {
    const target = Object.assign({}, param);
    return servcie({
        url: 'ProxyServcie',
        method: 'post',
        data: target,
        headers: { "Cross": "EdnaEmily", "Method": "Login", "Server": "Main"}
    })
}

/**
 * 全局菜单
 * @param {any} param
 */
export const Menu = (param) => {
    const target = Object.assign({}, param);
    return servcie({
        url: "ProxyServcie",
        method: 'post',
        data: target,
        headers: { "Method": "SearchMenuItem", "Server": "Main" }
    });
}