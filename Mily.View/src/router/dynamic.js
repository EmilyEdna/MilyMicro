/**
 * 动态路由
 * */
let DynamicRouter = {
    routes: [
        {
            path: "/",
            name: "Home",
            meta: { tilte: "gRPC分布式系统" },
            component: () => import("../components/Basic/Home.vue"),
            children: [{
                path: '/Index',
                component: () => import('../components/Basic/Index.vue'),
                meta: { title: '系统首页' }
            }]
        }
    ]
}
export default DynamicRouter;