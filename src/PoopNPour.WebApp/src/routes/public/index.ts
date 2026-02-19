import type { RouteRecordRaw } from "vue-router";
import { RouteHelper } from "../helpers/RouteHelper";
import { PUBLIC_ROUTES } from "../constants";
import PublicLayout from "@/components/layouts/PublicLayout.vue";

export const publicRoutes: RouteRecordRaw[] = [
  {
    path: "/",
    component: PublicLayout,
    children: [
      {
        path: "",
        name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME),
        component: () => import("@/views/public/HomePage/HomePage.vue"),
      },
      {
        path: "login",
        name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN),
        component: () => import("@/views/public/Login/LoginPage.vue"),
        meta: {
          guestOnly: true,
          guestOnlyMessage: {
            summary: 'Already signed in',
            detail: 'You are already signed in. Here\'s your profile.',
          },
        },
      },
      {
        path: "register",
        name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.REGISTER),
        component: () => import("@/views/public/Register/RegisterPage.vue"),
        meta: {
          guestOnly: true,
          guestOnlyMessage: {
            summary: 'Already registered',
            detail: 'You already have an account. Here\'s your profile.',
          },
        },
      },
      {
        path: "unauthorized",
        name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.UNAUTHORIZED),
        component: () => import("@/views/public/Unauthorized/UnauthorizedPage.vue"),
      },
      {
        path: "404",
        name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.NOT_FOUND),
        component: () => import("@/views/public/NotFound/NotFoundPage.vue"),
      },
    ],
  },
];
