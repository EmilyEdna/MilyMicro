import servcie from './http.service.js'

export const Login = (param) => {
    return servcie({
        url: 'JsonAsync',
        method: 'post',
        data: param
    })
}
