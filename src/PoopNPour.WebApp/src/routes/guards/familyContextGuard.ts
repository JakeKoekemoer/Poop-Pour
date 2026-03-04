import type { NavigationGuardNext, RouteLocationNormalized } from 'vue-router'
import { useFamilyContextStore } from '@/stores'

export const familyContextGuard = (
  to: RouteLocationNormalized,
  _from: RouteLocationNormalized,
  next: NavigationGuardNext,
): void => {
  const familyContextStore = useFamilyContextStore()
  const incomingFamilyId = to.params.familyId

  if (incomingFamilyId && typeof incomingFamilyId === 'string') {
    familyContextStore.setFamilyContext(incomingFamilyId)
  } else {
    familyContextStore.clearFamilyContext()
  }

  next()
}
