import lzstring from 'lz-string'
import servcie from './Request'


/**
 * 登录
 * @param {any} param
 */
export const Login = (param) => {
    const target = Object.assign({}, param);
    const Author = lzstring.compressToBase64(JSON.stringify({ "Cross": "Mily", "DataBase": 1,"Method":2 }));
    return servcie({
        url: 'Main/System/Login/ProxyMain',
        method: 'post',
        data: target,
        headers: { Author: Author }
    })
}

/**
 * 获取菜单
 * @param {any} param
 */
export const RoleMenu = (param) => {
    const target = Object.assign({}, param);
    const Author = lzstring.compressToBase64(JSON.stringify({ "DataBase": 1, "Method": 2 }));
    return servcie({
        url: "Main/System/GetMenuItem/ProxyMain",
        method: 'post',
        data: target,
        headers: { Author: Author }
    });
}

/**
 * 获取路由
 * @param {any} param
 */
export const RoleRouter = (param) => {
    const target = Object.assign({}, param);
    const Author = lzstring.compressToBase64(JSON.stringify({ "DataBase": 1, "Method": 2 }));
    return servcie({
        url: "Main/System/GetMenuRouter/ProxyMain",
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
    const Author = lzstring.compressToBase64(JSON.stringify({ "DataBase": 1, "Method": 2 }));
    return servcie({
        url: "Main/System/SearchMenuItemPage/ProxyMain",
        method: 'post',
        data: target,
        headers: { Author: Author }
    });
}

/**
 * 逻辑删除菜单
 * @param {any} param
 */
export const DeleteMenu = (param) => {
    const target = Object.assign({}, param);
    const Author = lzstring.compressToBase64(JSON.stringify({ "DataBase": 1 }));
    return servcie({
        url: "Main/System/DeleteMenuItem/ProxyMain",
        method: 'post',
        data: target,
        headers: { Author: Author }
    });
}