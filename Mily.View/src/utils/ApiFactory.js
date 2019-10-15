import servcie from './http.service.js'

export const Login = (param) => {
    const target = Object.assign({ RequestPath: "System_Login_Main", Hit: 100 }, param);
    return servcie({
        url: 'JsonAsync',
        method: 'post',
        data: target,
        headers: { "Cross":"LISwNgnkA==="}
    })
}
