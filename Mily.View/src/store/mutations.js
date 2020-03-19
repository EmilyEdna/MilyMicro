import cookie from 'js-cookie';

const mutations = {
    /**
     * 设置用户本地存储
     * @param {any} State
     * @param {any} Object
     */
    ChangeUserLocalStorage(State, Object) {
        State.USER = Object;
        localStorage.USER = JSON.stringify(Object);
        sessionStorage.IsLogin = Object ? true : false;
    },
    /**
     * 设置用路由权限
     * @param {any} State
     * @param {any} Object
     */
    ChangeUserRoleRouter(State, Object) {
        State.RoleRouter = Object;
        localStorage.RoleRouter = JSON.stringify(Object);
        sessionStorage.IsLoadRouter = Object ? true : false;
    },
    /**
     * 退出登录
     * @param {any} State
     */
    ChangeUserLogOut(State) {
        localStorage.clear();
        sessionStorage.clear();
        cookie.remove("Authorization");
        cookie.remove("Global");
    }
}

export default mutations;