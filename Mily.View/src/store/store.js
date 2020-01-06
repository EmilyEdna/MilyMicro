import Vue from 'vue'
import Vuex from 'vuex'
import mutations from './mutations'
import getters from './getters'
import state from './state'

Vue.use(Vuex)

export default new Vuex.Store({
    state,
    mutations,
    getters
});