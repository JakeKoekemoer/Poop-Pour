<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { Baby, ArrowLeft, UserPlus } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { useFamilyContextStore } from '@/stores'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import FamilyDependentsTable from '@/components/family/FamilyDependentsTable.vue'

const route = useRoute()
const router = useRouter()
const familyContextStore = useFamilyContextStore()

const familyId = computed(() => (route.params['familyId'] as string) || familyContextStore.familyId)

function goToFamilyDashboard() {
  if (familyId.value) {
    familyContextStore.setFamilyContext(familyId.value)
    router.push({
      name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FAMILY_DASHBOARD),
      params: { familyId: familyId.value },
    })
  } else {
    router.push({ name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DASHBOARD) })
  }
}

function goToAddDependent() {
  if (familyId.value) {
    router.push({
      name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.ADD_FAMILY_DEPENDENT),
      params: { familyId: familyId.value },
    })
  }
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="sm" @click="goToFamilyDashboard">
        <ArrowLeft class="w-4 h-4 mr-1" />
        Family
      </Button>
    </div>

    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <Baby class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Dependents in Family</h1>
      </div>
      <Button v-if="familyId" @click="goToAddDependent">
        <UserPlus class="w-4 h-4 mr-2" />
        Add Dependent
      </Button>
    </div>

    <FamilyDependentsTable v-if="familyId" :family-id="familyId" />

    <div v-else class="text-muted-foreground">
      No family selected. Please navigate from the Family dashboard.
    </div>
  </div>
</template>
