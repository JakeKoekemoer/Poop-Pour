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
      {
        path: 'families',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILIES),
        component: () => import('@/views/admin/Families/FamiliesPage.vue'),
      },
      {
        path: 'family-users',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILY_USERS),
        component: () => import('@/views/admin/FamilyUsers/FamilyUsersPage.vue'),
      },
      {
        path: 'dependents',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DEPENDENTS),
        component: () => import('@/views/admin/Dependents/DependentsPage.vue'),
      },
      {
        path: 'diaper-logs',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DIAPER_LOGS),
        component: () => import('@/views/admin/DiaperLogs/DiaperLogsPage.vue'),
      },
      {
        path: 'feed-logs',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FEED_LOGS),
        component: () => import('@/views/admin/FeedLogs/FeedLogsPage.vue'),
      },
      {
        path: 'medicine-logs',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.MEDICINE_LOGS),
        component: () => import('@/views/admin/MedicineLogs/MedicineLogsPage.vue'),
      },
    ],
  },
]
