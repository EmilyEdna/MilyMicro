import servcie from './http.service.js'

/**
 * 登录
 * @param {any} param
 */
export const Login = (param) => {
    const target = Object.assign({ RequestPath: "System_Login_Main", Hit: 100 }, param);
    return servcie({
        url: 'JsonAsync',
        method: 'post',
        data: target,
        headers: { "Cross": "EdnaEmily" }
    })
}

/**
 * 全局菜单
 * @param {any} param
 */
export const Menu =  (param) => {
    const target = Object.assign({ RequestPath: "System_SearchMenuItem_Main", Hit: 100 }, param);
    return servcie({
        url: "JsonAsync",
        method: 'post',
        data: target
    });
}