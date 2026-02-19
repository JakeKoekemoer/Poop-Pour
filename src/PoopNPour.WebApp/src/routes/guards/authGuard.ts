import type { NavigationGuardNext, RouteLocationNormalized } from "vue-router";
import { RouteHelper } from "../helpers/RouteHelper";
import { PUBLIC_ROUTES, SECURE_ROUTES } from "../constants";
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
  const isAdmin = userStore.hasRole("Admin");

  if (to.meta.guestOnly && isAuthenticated) {
    const message = to.meta.guestOnlyMessage;
    if (message) {
      notificationStore.enqueue({ severity: 'info', ...message });
    }
    next({ name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.PROFILE) });
    return;
  }

  if (routeName?.startsWith("secure.") || routeName?.startsWith("admin.")) {
    if (!isAuthenticated) {
      next({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) });
      return;
    }

    if (routeName.startsWith("admin.") && !isAdmin) {
      next({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.UNAUTHORIZED) });
      return;
    }
  }

  next();
};
