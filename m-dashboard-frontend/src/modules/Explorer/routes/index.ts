import { RouteConfig } from 'vue-router';

const explorerRoutes: RouteConfig = {
    path: '/', name: 'Explorer', redirect: '/map-explorer',
    component: () => import(/* webpackChunkName: "auth" */ '@/modules/Explorer/Explorer.vue'),
    children: [
        {
            path: 'total-production', name: 'TotalProduction',
            component: () => import(/* webpackChunkName: "totalProduction" */ '@/modules/Explorer/views/TotalProduction/TotalProduction.vue')
        },
        {
            path: 'production-performance', name: 'AssetExplorer',
            component: () => import(/* webpackChunkName: "AssetExplorer" */ '@/modules/Explorer/views/ProductionStatistics/ProductionStatistics.vue')
        },
        {
            path: 'average-production', name: 'AverageProduction',
            component: () => import(/* webpackChunkName: "AverageProduction" */ '@/modules/Explorer/views/AverageProduction/AverageProduction.vue')
        },
        {
            path: 'map-explorer', name: 'MapExplorer',
            component: () => import(/* webpackChunkName: "MapExplorer" */ '@/modules/Explorer/views/MapExplorer/MapExplorer.vue')
        }
    ]
}

export default explorerRoutes