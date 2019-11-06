export default {
    changeToken(state, token) {
        //登录或者注册时，存储token的方法
        state.token = token
        try {
            localStorage.token = token
        } catch (e) { }
    },
    clearToken(state) {
        //退出登录时清除token的方法
        localStorage.token = ''
        state.token = ''
    },
    changeMenuState(state, params) {
        if (params.routerdata)
            localStorage.Router = JSON.stringify(params.routerdata);
        if (params.menudata)
            localStorage.Menu = JSON.stringify(params.menudata);
        //切换menus加载状态
        state.loadmenus = params.loadmenus
    },
}