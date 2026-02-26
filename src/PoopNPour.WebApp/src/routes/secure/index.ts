import type { RouteRecordRaw } from 'vue-router'
import { RouteHelper } from '../helpers/RouteHelper'
import { SECURE_ROUTES } from '../constants'
import SecureLayout from '@/components/layouts/SecureLayout.vue'

export const secureRoutes: RouteRecordRaw[] = [
  {
    path: '/secure',
    component: SecureLayout,
    children: [
      {
        path: 'dashboard',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DASHBOARD),
        component: () => import('@/views/secure/Dashboard/DashboardPage.vue'),
      },
      {
        path: 'profile',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.PROFILE),
        component: () => import('@/views/secure/Profile/ProfilePage.vue'),
      },
      {
        path: 'families/create',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.CREATE_FAMILY),
        component: () => import('@/views/secure/Families/CreateFamilyPage.vue'),
      },
    ],
  },
]
