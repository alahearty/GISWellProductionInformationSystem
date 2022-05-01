import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import explorerRoutes from '@/modules/Explorer/routes';
import { users } from '@/views/Auth/dummyUsers';
import { User } from '@/types/User';

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  explorerRoutes,
  { path: '/login', name: 'Login', component: () => import('../views/Auth/Login.vue') }
]

const isLocalStorageUserValid = (): boolean => {
  const orbitStorage = localStorage.getItem('orbit')
  if (orbitStorage) {
    const user: User = JSON.parse(orbitStorage);

    const isUserValid = users.findIndex(u => {
      return u.username === user.username && u.password === user.password;
    }) !== -1

    if (isUserValid) {
      return true
    }

    return false
  }

  return false
}

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

router.beforeEach((to, from, next) => {
  if (to.name !== 'Login' && !isLocalStorageUserValid()) {
    next({ name: 'Login' })
    return
  }

  if (to.name === 'Login' && isLocalStorageUserValid()) {
    next({ path: '/map-explorer' })
  }

  next()
})

export default router
