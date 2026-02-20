import type { NavigationGuardNext, RouteLocationNormalized } from "vue-router";
import { RouteHelper } from "../helpers/RouteHelper";
import { PUBLIC_ROUTES, SECURE_ROUTES, ADMIN_ROUTES } from "../constants";
import { useUserStore, useNotificationStore } from "@/stores";

export const authGuard = (
  to: RouteLocationNormalized,
  _from: RouteLocationNormalized,
  next: NavigationGuardNext,
) => {
  const routeName = to.name as string;
  const userStore = useUserStore();
  const notificationStore = useNotificationStore();

  const isAuthenticated = userStore.isAuthenticated;
  const isAdmin = userStore.hasRole("Administrator");

  if (to.meta.guestOnly && isAuthenticated) {
    const message = to.meta.guestOnlyMessage;
    if (message) {
      notificationStore.enqueue({ severity: "info", ...message });
    }
    const destination = isAdmin
      ? RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DASHBOARD)
      : RouteHelper.GetSecureRouteName(SECURE_ROUTES.PROFILE);
    next({ name: destination });
    return;
  }

  if (routeName && RouteHelper.isProtectedRoute(routeName)) {
    if (!isAuthenticated) {
      next({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) });
      return;
    }

    if (RouteHelper.isAdminRoute(routeName) && !isAdmin) {
      next({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.UNAUTHORIZED) });
      return;
    }

    if (RouteHelper.isSecureRoute(routeName) && isAdmin) {
      next({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DASHBOARD) });
      return;
    }
  }

  next();
};
