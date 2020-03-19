const getters = {
    /*
     * 判断是否登录
     */
    IsLogin: State => {
        return State.USER !== null;
    },
    /*
     * 判断是否已经加载了菜单
     * */
    IsLoadRouter: State => {
        return State.RoleRouter !== null;
    }
}

export default getters;