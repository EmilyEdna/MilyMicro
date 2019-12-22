import store from '../store/index';
import { Menu } from './ApiFactory';
import router from '../router/router';
import dynamic from '../router/dynamic'

let Path = [];
let Paths = [];

/**
 * 初始化菜单
 * */
const InitMenu = () => {
    if (store.getters.login) {
        let Params = { "Key": JSON.parse(store.state.token).RolePermissionId };
        Menu(Params).then(res => {
            InitRouter(res.ResultData);
            let TempData = {
                loadmenus: true,
                routerdata: Paths,
                menudata: res.ResultData
            }
            store.commit('changeMenuState', TempData);
        });
    }
}

/**
 * 添加动态路由
 * @param {any} data
 */
const InitRouter = (data) => {
    InitChild(data);
    Path.forEach(item => {
        let value = item.component;
        item.component = resolve => require([`@/views/${value}`], resolve);
        dynamic.routes[1].children.push(item);
    });
    router.options.routers = dynamic.routes;
    router.addRoutes(dynamic.routes);
}

/**
 * 递归遍历子菜单
 * @param {any} data
 */
const InitChild = (data) => {
    data.forEach(item => {
        if (item.Parent)
            InitChild(item.ChildMenus);
        else
            if (item.RouterPath != null) {
                Path.push({
                    "path": "/" + item.Path,
                    "component": item.RouterPath,
                    "meta": { "title": item.Title }
                });
                Paths.push({
                    "path": "/" + item.Path,
                    "component": item.RouterPath,
                    "meta": { "title": item.Title }
                });
            }
    });
}
export default InitMenu