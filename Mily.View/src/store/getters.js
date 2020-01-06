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
    IsLoadMenu: State => {
        return State.RoleMenu !== null;
    }
}

export default getters;