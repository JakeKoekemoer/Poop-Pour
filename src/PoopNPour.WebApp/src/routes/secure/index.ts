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
      {
        path: 'families/:familyId/members/add',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.ADD_FAMILY_MEMBER),
        component: () => import('@/views/secure/Families/AddUserToFamilyPage.vue'),
      },
      {
        path: 'families/:familyId/members',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FAMILY_MEMBERS),
        component: () => import('@/views/secure/Families/FamilyMembersPage.vue'),
      },
      {
        path: 'families/:familyId/dependents/add',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.ADD_FAMILY_DEPENDENT),
        component: () => import('@/views/secure/Families/AddFamilyDependentPage.vue'),
      },
      {
        path: 'families/:familyId/dependents',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FAMILY_DEPENDENTS),
        component: () => import('@/views/secure/Families/FamilyDependentsPage.vue'),
      },
      {
        path: 'families/:familyId/feed-logs/add',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.ADD_FEED_LOG),
        component: () => import('@/views/secure/FeedLogs/AddFeedLogPage.vue'),
      },
      {
        path: 'families/:familyId/feed-logs/:id/edit',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.EDIT_FEED_LOG),
        component: () => import('@/views/secure/FeedLogs/EditFeedLogPage.vue'),
      },
      {
        path: 'families/:familyId/feed-logs',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FEED_LOGS),
        component: () => import('@/views/secure/FeedLogs/FeedLogsPage.vue'),
      },
      {
        path: 'families/:familyId/diaper-logs/add',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.ADD_DIAPER_LOG),
        component: () => import('@/views/secure/DiaperLogs/AddDiaperLogPage.vue'),
      },
      {
        path: 'families/:familyId/diaper-logs/:id/edit',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.EDIT_DIAPER_LOG),
        component: () => import('@/views/secure/DiaperLogs/EditDiaperLogPage.vue'),
      },
      {
        path: 'families/:familyId/diaper-logs',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DIAPER_LOGS),
        component: () => import('@/views/secure/DiaperLogs/DiaperLogsPage.vue'),
      },
      {
        path: 'families/:familyId/medicine-logs/add',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.ADD_MEDICINE_LOG),
        component: () => import('@/views/secure/MedicineLogs/AddMedicineLogPage.vue'),
      },
      {
        path: 'families/:familyId/medicine-logs/:id/edit',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.EDIT_MEDICINE_LOG),
        component: () => import('@/views/secure/MedicineLogs/EditMedicineLogPage.vue'),
      },
      {
        path: 'families/:familyId/medicine-logs',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.MEDICINE_LOGS),
        component: () => import('@/views/secure/MedicineLogs/MedicineLogsPage.vue'),
      },
      {
        path: 'families/:familyId',
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FAMILY_DASHBOARD),
        component: () => import('@/views/secure/Families/FamilyDashboardPage.vue'),
      },
    ],
  },
]
