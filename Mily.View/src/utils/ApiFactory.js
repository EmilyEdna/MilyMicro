import servcie from './http.service.js'

/**
 * µÇÂ¼
 * @param {any} param
 */
export const Login = (param) => {
    const target = Object.assign({ RequestPath: "System_Login_Main", Hit: 100 }, param);
    return servcie({
        url: 'JsonAsync',
        method: 'post',
        data: target,
        headers: { "Cross":"LISwNgnkA==="}
    })
}

/**
 * È«¾Ö²Ëµ¥
 * @param {any} param
 */
export const Menu = (param) => {
    const target = Object.assign({ RequestPath: "System_SearchMenuItem_Main", Hit: 100 }, param);
    return servcie({
        url:"JsonAsync",
        url: 'JsonAsync',
        method: 'post',
        data: target
    });
}