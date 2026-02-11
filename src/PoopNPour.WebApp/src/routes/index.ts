import { createRouter, createWebHistory } from 'vue-router'
import { publicRoutes } from './public'
import { secureRoutes } from './secure'
import { adminRoutes } from './admin'
import { authGuard } from './guards/authGuard'
import { RouteHelper } from './helpers/RouteHelper'
import { PUBLIC_ROUTES } from './constants'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    ...publicRoutes,
    ...secureRoutes,
    ...adminRoutes,
    {
      path: '/:pathMatch(.*)*',
      redirect: { name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.NOT_FOUND) },
    },
  ],
})

router.beforeEach(authGuard)

export default router
