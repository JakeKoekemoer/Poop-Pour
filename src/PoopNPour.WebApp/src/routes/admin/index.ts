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
        path: 'users/add',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.ADD_USER),
        component: () => import('@/views/admin/Users/AddUserPage.vue'),
      },
      {
        path: 'users/:id/edit',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.EDIT_USER),
        component: () => import('@/views/admin/Users/EditUserPage.vue'),
      },
      {
        path: 'users/:id/delete',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DELETE_USER),
        component: () => import('@/views/admin/Users/DeleteUserPage.vue'),
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
        path: 'families/add',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.ADD_FAMILY),
        component: () => import('@/views/admin/Families/AddFamilyPage.vue'),
      },
      {
        path: 'families/:id/edit',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.EDIT_FAMILY),
        component: () => import('@/views/admin/Families/EditFamilyPage.vue'),
      },
      {
        path: 'families/:familyId/users',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILY_USERS),
        component: () => import('@/views/admin/FamilyUsers/FamilyUsersPage.vue'),
      },
      {
        path: 'families/:familyId/users/add',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.ADD_FAMILY_MEMBER),
        component: () => import('@/views/admin/FamilyUsers/AddUserToFamilyPage.vue'),
      },
      {
        path: 'dependents',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DEPENDENTS),
        component: () => import('@/views/admin/Dependents/DependentsPage.vue'),
      },
      {
        path: 'dependents/add',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.ADD_DEPENDENT),
        component: () => import('@/views/admin/Dependents/AddDependentPage.vue'),
      },
      {
        path: 'dependents/:id/edit',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.EDIT_DEPENDENT),
        component: () => import('@/views/admin/Dependents/EditDependentPage.vue'),
      },
      {
        path: 'dependents/:id/logs',
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DEPENDENT_LOGS),
        component: () => import('@/views/admin/Dependents/DependentLogsPage.vue'),
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
