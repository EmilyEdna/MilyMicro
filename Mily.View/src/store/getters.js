export default {
    login: state => {
        // 计算登录状态，返回一个boolean值
        return state.token !== ''
    },
    loadmenus: state => {
        //判断menus是否已经加载了
        return state.loadmenus
    },
    MenuData: state => {
        //返回菜单数据
        return state.MenuData
    },
    RouterData: state => {
        //返回路由数据
        return state.RouterData
    }
}