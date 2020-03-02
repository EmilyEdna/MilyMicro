import servcie from './http.service.js'
import lzstring from 'lz-string'


/**
 * 登录
 * @param {any} param
 */
export const Login = (param) => {
    const target = Object.assign({}, param);
    const Author = lzstring.compressToBase64(JSON.stringify({ "Cross": "Mily", "DataBase": 1 }));
    return servcie({
        url: 'Main/System/Login/ProxyMain',
        method: 'post',
        data: target,
        headers: { Author: Author }
    })
}

/**
 * 全局菜单
 * @param {any} param
 */
export const Menu = (param) => {
    const target = Object.assign({}, param);
    const Author = lzstring.compressToBase64(JSON.stringify({ "DataBase": 1 }));
    return servcie({
        url: "Main/System/GetMenuItem/ProxyMain",
        method: 'post',
        data: target,
        headers: { Author: Author }
    });
}

/**
 * 菜单分页查询
 * @param {any} param
 */
export const SearchMenuPage = (param) => {
    const target = Object.assign({}, param);
    const Author = lzstring.compressToBase64(JSON.stringify({ "DataBase": 1 }));
    return servcie({
        url: "Main/System/SearchMenuItemPage/ProxyMain",
        method: 'post',
        data: target,
        headers: { Author: Author }
    });
}