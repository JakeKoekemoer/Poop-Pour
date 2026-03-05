<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { UserCheck, ArrowLeft, UserPlus } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { useFamilyContextStore } from '@/stores'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import FamilyMembersTable from '@/components/family/FamilyMembersTable.vue'

const router = useRouter()
const familyContextStore = useFamilyContextStore()

const familyId = computed(() => familyContextStore.familyId)

function goToFamilyDashboard() {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FAMILY_DASHBOARD),
    params: { familyId: familyContextStore.familyId! },
  })
}

function goToAddMember() {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.ADD_FAMILY_MEMBER),
    params: { familyId: familyContextStore.familyId! },
  })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="sm" @click="goToFamilyDashboard">
        <ArrowLeft class="w-4 h-4 mr-1" />
        Back to Family
      </Button>
    </div>

    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <UserCheck class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Users in Family</h1>
      </div>
      <Button v-if="familyId" @click="goToAddMember">
        <UserPlus class="w-4 h-4 mr-2" />
        Add Member
      </Button>
    </div>

    <FamilyMembersTable v-if="familyId" :family-id="familyId" />

    <div v-else class="text-muted-foreground">
      No family selected. Please navigate from the family dashboard.
    </div>
  </div>
</template>
