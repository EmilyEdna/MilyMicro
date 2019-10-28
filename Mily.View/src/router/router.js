import Vue from 'vue';
import Router from 'vue-router';
import login from '../components/Basic/Login.vue';
import home from '../components/Basic/Home.vue'

Vue.use(Router);


export default new Router({
    routes: [
        {
            path: '/',
            redirect: '/Index'
        },
        {
            path: '/',
            component: home,
            meta: { tilte: "gRPC分布式系统" },
            children: [{
                path: '/Index',
                component: () => import('../components/Basic/Index.vue'),
                meta: { title: '系统首页' }
            },
            {
                path: '/SysMenu',
                component: () => import('../views/System/SysMenu.vue'),
                meta: { title: '后台菜单' }
            }]
        },
        {
            path: '/login',
            component: login
        }
    ]
});
