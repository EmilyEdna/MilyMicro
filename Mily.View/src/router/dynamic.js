/**
 * 动态路由
 * */
let DynamicRouter = {
    routes: [
        {
            path: '/',
            redirect: '/Index'
        },
        {
            path: "/",
            name: "Home",
            meta: { tilte: "gRPC分布式系统" },
            component: () => import("../components/Basic/Home.vue"),
            children: [{
                path: '/Index',
                name:"Index",
                component: () => import('../components/Basic/Index.vue'),
                meta: { title: '系统首页' }
            }]
        },
        {
            path: '/login',
            name:"login",
            meta: { tilte: "后台登录" },
            component: import('../components/Basic/Login.vue')
        }
    ]
}
export default DynamicRouter;