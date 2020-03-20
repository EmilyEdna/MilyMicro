import Vue from 'vue';
import App from './App.vue';
import router from './router/router';
import ElementUI from 'element-ui';
import store from './store/store'
import cookie from 'js-cookie';
import { Session } from './extension/session'
import { Local } from './extension/local'
import RouterCheck from './validate/RouterValidate'
import 'element-ui/lib/theme-chalk/index.css'; // 默认主题
import './assets/css/icon.css';
/*
 全局公用插件
 */
import Enumerable from 'linq';
/*
 注册全局插件
 */
Vue.prototype.Linq = Enumerable;

Vue.config.productionTip = false;

Vue.use(ElementUI, {
    size: 'small'
});

if (Session.IsLogin && !store.getters.IsLogin) store.commit("ChangeUserLocalStorage", Local.USER);
if (Session.IsLoadRouter && !store.getters.IsLoadRouter) store.commit("ChangeUserRoleRouter", Local.RoleRouter);

router.beforeEach((to, from, next) => {
    document.title = `${to.meta.title}|后台管理系统`;
    const global = cookie.get("Global");
    //Cookie不存在未登录
    if (!global && to.path !== '/login') next("/login");
    else RouterCheck(next);
})

 new Vue({
    router,
    store,
    render: h => h(App)
}).$mount('#app');
