import Vue from 'vue';
import Router from 'vue-router';
import login from '../components/Basic/Login.vue';
import home from '../components/Basic/Home.vue'

Vue.use(Router);


export default new Router({
    routes: [
        {
            path: '/',
            component: home
        },
        {
            path: '/login',
            component: login
        }
    ]
});
