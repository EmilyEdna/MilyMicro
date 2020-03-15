import store from '../store/store';
import router from '../router/router';
import dynamic from '../router/dynamic'
import { Router } from './ApiFactory';

/**
 * 初始化路由
 * */
const InitRouter = () => {
    if (store.getters.IsLogin) {
        let Params = { "Key": store.state.USER.RolePermissionId };
        Router(Params).then(res => {
            InitRouterCollection(res.ResultData);
            store.commit('ChangeUserRoleRouter', res.ResultData);
        });
    }
}

/**
 * 添加动态路由
 * @param {any} data
 */
const InitRouterCollection = (data) => {
    let Path = [];
    data.forEach(item => {
        Path.push({
            "path": "/" + item.MenuPath,
            "name": item.Title,
            "component": item.RouterPath,
            "meta": { "title": item.Title },
            children: []
        });
        item.ChildFeatures.forEach((items, index) => {
            Path[index].children.push({
                "path": "/" + items.MenuPath,
                "name": items.Title,
                "component": items.RouterPath,
                "meta": { "title": items.Title },
            })
        });
    });
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

export default InitRouter