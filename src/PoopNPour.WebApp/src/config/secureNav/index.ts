import { Home } from 'lucide-vue-next'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import type { SecureNavItem } from './types'

export type { SecureNavItem } from './types'

export const secureNavItems: SecureNavItem[] = [
  {
    label: 'Create Family',
    routeName: RouteHelper.GetSecureRouteName(SECURE_ROUTES.CREATE_FAMILY),
    icon: Home,
  },
]
