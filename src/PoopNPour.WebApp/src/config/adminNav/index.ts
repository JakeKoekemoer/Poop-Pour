import { Users, Home, Baby, Droplets, Utensils, Pill } from 'lucide-vue-next'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import type { AdminNavItem } from './types'

export type { AdminNavItem } from './types'

export const adminNavItems: AdminNavItem[] = [
  {
    label: 'Users',
    routeName: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.USERS),
    icon: Users,
    showOnDashboard: true,
  },
  {
    label: 'Families',
    routeName: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILIES),
    icon: Home,
    showOnDashboard: true,
  },
  {
    label: 'Dependents',
    routeName: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DEPENDENTS),
    icon: Baby,
    showOnDashboard: true,
  },
  {
    label: 'Diaper Logs',
    routeName: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DIAPER_LOGS),
    icon: Droplets,
    showOnDashboard: true,
  },
  {
    label: 'Feed Logs',
    routeName: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FEED_LOGS),
    icon: Utensils,
    showOnDashboard: true,
  },
  {
    label: 'Medicine Logs',
    routeName: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.MEDICINE_LOGS),
    icon: Pill,
    showOnDashboard: true,
  },
]
