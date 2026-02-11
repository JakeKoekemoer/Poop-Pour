import { createRouter, createWebHistory } from 'vue-router'
import { publicRoutes } from './publicRoutes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: publicRoutes,
})

export default router
