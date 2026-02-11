import type { NavigationGuardNext, RouteLocationNormalized } from 'vue-router'
import { RouteHelper } from '../helpers/RouteHelper'
import { PUBLIC_ROUTES } from '../constants'

export const authGuard = (
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) => {
  const routeName = to.name as string

  // TODO: Replace with actual auth check when auth system is implemented
  // For now, these will always be false, causing redirects to login/unauthorized pages
  const isAuthenticated = false
  const isAdmin = false

  // Check if the route requires authentication
  if (routeName?.startsWith('secure.') || routeName?.startsWith('admin.')) {
    if (!isAuthenticated) {
      // Redirect unauthenticated users to login page
      next({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) })
      return
    }

    // Check if the route requires admin privileges
    if (routeName.startsWith('admin.') && !isAdmin) {
      // Redirect non-admin users to unauthorized page
      next({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.UNAUTHORIZED) })
      return
    }
  }

  // Allow navigation
  next()
}
