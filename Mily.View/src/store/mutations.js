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
     * 设置用户的权限菜单
     * @param {any} State
     * @param {any} Object
     */
    ChangeUserRoleMenu(State, Object) {
        State.RoleMenu = Object;
        localStorage.RoleMenu = JSON.stringify(Object);
        sessionStorage.IsLoadMenu = Object ? true : false;
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