import { defineStore } from 'pinia'
import { setFamilyId as setClientFamilyId, clearFamilyId as clearClientFamilyId } from '@/services/api'
import type { FamilyContextState } from './types'

export type { FamilyContextState } from './types'

export const useFamilyContextStore = defineStore('familyContext', {
  state: (): FamilyContextState => ({
    familyId: null,
  }),

  getters: {
    hasFamilyContext: (state): boolean => state.familyId !== null,
  },

  actions: {
    setFamilyContext(familyId: string): void {
      this.familyId = familyId
      setClientFamilyId(familyId)
    },

    clearFamilyContext(): void {
      this.familyId = null
      clearClientFamilyId()
    },
  },
})
