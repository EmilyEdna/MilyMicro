import Vue from 'vue';
import App from './App.vue';
import router from './router/router';
import ElementUI from 'element-ui';
import store from './store/index'
import cookie from 'js-cookie';
import dynamic from './utils/DynamicMenu';
import 'element-ui/lib/theme-chalk/index.css'; // 默认主题
import './assets/css/icon.css';


Vue.config.productionTip = false;

Vue.use(ElementUI, {
    size: 'small'
});

router.beforeEach((to, from, next) => {
    document.title = `${to.meta.title}|后台管理系统`;
    const global = cookie.get("Global");
    //Cookie不存在未登录
    if (!global && to.path !== '/login') {
        next("/login");
    } else {
        if (!store.getters.loadmenus) 
            dynamic();
        next();
    }
})

new Vue({
    router,
    store,
    render: h => h(App)
}).$mount('#app');
