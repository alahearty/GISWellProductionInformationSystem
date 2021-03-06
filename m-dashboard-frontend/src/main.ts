import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import * as VueGoogleMaps from 'vue2-google-maps';
// Import Bootstrap an BootstrapVue CSS files (order is important)
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import '@/scss/style.scss';
import Chart from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';

Chart.plugins.register(ChartDataLabels)

// Make BootstrapVue available throughout your project
Vue.use(BootstrapVue)
// Optionally install the BootstrapVue icon components plugin
Vue.use(IconsPlugin)
Vue.use(VueGoogleMaps, {
  load: {
     key: 'AIzaSyCxnzT9xHD88NpGPR4TPynrb_ccBn8EFC8',
    // key: 'AIzaSyDDK5QC8XNNuMJUgwGaXFQA4ta0vqe80r0',
    libraries: 'places,drawing,visualization'
  },
});

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
