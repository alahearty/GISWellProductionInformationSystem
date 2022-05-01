import Vue from 'vue'
import Vuex from 'vuex'
import { indexStore } from "@/modules/Index/store/index";
import { explorerStore } from "@/modules/Explorer/store/index";
import { ApplicationState } from './applicationState';
import { authStore } from '@/views/Auth/store';

Vue.use(Vuex)

export default new Vuex.Store<ApplicationState>({
  mutations: {
  },
  actions: {
  },
  modules: {
    index: indexStore,
    explorer: explorerStore,
    auth: authStore
  }
})
