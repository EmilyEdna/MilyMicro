import servcie from './http.service.js'
import lzstring from 'lz-string'


/**
 * 登录
 * @param {any} param
 */
export const Login = (param) => {
    const target = Object.assign({}, param);
    const Author = lzstring.compressToBase64(JSON.stringify({ "Cross": "EdnaEmily", "Method": "Login", "Server": "Main", "DataBase": 1 }));
    return servcie({
        url: 'ProxyServcie',
        method: 'post',
        data: target,
        headers: { Author: Author}
    })
}

/**
 * 全局菜单
 * @param {any} param
 */
export const Menu = (param) => {
    const target = Object.assign({}, param);
    const Author = lzstring.compressToBase64(JSON.stringify({ "Method": "SearchMenuItem", "Server": "Main", "DataBase": 1 }));
    return servcie({
        url: "ProxyServcie",
        method: 'post',
        data: target,
        headers: { Author: Author }
    });
}