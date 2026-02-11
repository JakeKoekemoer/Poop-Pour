import type { RouteRecordRaw } from 'vue-router'
import { RouteHelper } from '../helpers/RouteHelper'
import { ADMIN_ROUTES } from '../constants'
import AdminLayout from '@/components/layouts/AdminLayout.vue'

export const adminRoutes: RouteRecordRaw[] = [
  {
    path: '/admin',
    component: AdminLayout,
    children: [
      {
        path: 'dashboard',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DASHBOARD),
        component: () => import('@/views/admin/Dashboard/DashboardPage.vue'),
      },
      {
        path: 'users',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.USERS),
        component: () => import('@/views/admin/Users/UsersPage.vue'),
      },
      {
        path: 'settings',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.SETTINGS),
        component: () => import('@/views/admin/Settings/SettingsPage.vue'),
      },
    ],
  },
]
