import type { NavigationGuardNext, RouteLocationNormalized } from 'vue-router'
import { RouteHelper } from '../helpers/RouteHelper'
import { PUBLIC_ROUTES } from '../constants'
import { useUserStore } from '@/stores'

export const authGuard = (
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) => {
  const routeName = to.name as string
  const userStore = useUserStore()

  const isAuthenticated = userStore.isAuthenticated

  const isAdmin = userStore.hasRole('Admin')

  if (routeName?.startsWith('secure.') || routeName?.startsWith('admin.')) {
    if (!isAuthenticated) {
      next({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) })
      return
    }

    if (routeName.startsWith('admin.') && !isAdmin) {
      next({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.UNAUTHORIZED) })
      return
    }
  }

  next()
}
