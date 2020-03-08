import store from '../store/store';
import router from '../router/router';
import dynamic from '../router/dynamic'
import { Menu } from './ApiFactory';

let Path = [];

/**
 * 初始化菜单
 * */
const InitMenu = () => {
    if (store.getters.IsLogin) {
        let Params = { "Key": store.state.USER.RolePermissionId };
        Menu(Params).then(res => {
            InitRouter(res.ResultData);
            store.commit('ChangeUserRoleMenu', res.ResultData);
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
        item.children.forEach(items => {
            let values = items.component;
            items.component = resolve => require([`@/views/${values}`], resolve);
        });
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
                    "name": item.Path,
                    "component": item.RouterPath,
                    "meta": { "title": item.Title },
                    children:[]
                });
                item.FuncRouters.forEach((items,index) => {
                    Path[index].children.push({
                        "path": "/" + items.MenuPath,
                        "name":items.MenuPath,
                        "component": items.FuncRouterPath,
                        "meta": { "title": items.FuncName },
                    })
                });
            }
    });
}
export default InitMenu