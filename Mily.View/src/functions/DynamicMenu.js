import store from '../store/store';
import router from '../router/router';
import dynamic from '../router/dynamic'
import { Router } from './ApiFactory';
import { Local } from '../extension/local'

/**
 * 初始化路由
 * */
const InitRouter = async () => {
    let res = await Router({ "Key": store.state.USER.RolePermissionId });
    InitRouterCollection(res.ResultData);
    store.commit('ChangeUserRoleRouter', res.ResultData);
    return true;
}

/**
 * 获取缓存的路由初始化
 * */
const InitCacheRouter = () => {
    InitRouterCollection(Local.RoleRouter);
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

export default { InitRouter, InitCacheRouter }