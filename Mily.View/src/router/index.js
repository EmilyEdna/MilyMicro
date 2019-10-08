import Vue from 'vue';
import Router from 'vue-router';
import login from '../components/Basic/Login.vue';

Vue.use(Router);

export default new Router({
    routes: [{
        path: '/',
        component: login
    }]
});