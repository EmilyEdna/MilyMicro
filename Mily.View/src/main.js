import Vue from 'vue';
import App from './App.vue';
import router from './router/router';
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css'; // 默认主题
import './assets/css/icon.css';
import cookie from 'js-cookie';

Vue.config.productionTip = false;

Vue.use(ElementUI, {
    size: 'small'
});

router.beforeEach((to, from, next) => {
    document.title = `${to.meta.title}|后台管理系统`;
    const global = cookie.get("Global");
    if (!global && to.path !== '/login') {
        next("/login");
    } else
        next();
})

new Vue({
    router,
    render: h => h(App)
}).$mount('#app');
