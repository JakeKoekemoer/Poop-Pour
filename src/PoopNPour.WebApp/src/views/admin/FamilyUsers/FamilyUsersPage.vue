<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { UserCheck, ArrowLeft, UserPlus } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { useFamilyContextStore } from '@/stores'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import FamilyMembersTable from '@/components/views/admin/FamilyUsers/FamilyMembersTable.vue'

const router = useRouter()
const familyContextStore = useFamilyContextStore()

const familyId = computed(() => familyContextStore.familyId)

function goToFamilies() {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILIES) })
}

function goToAddMember() {
  router.push({
    name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.ADD_FAMILY_MEMBER),
    params: { familyId: familyContextStore.familyId! },
  })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="sm" @click="goToFamilies">
        <ArrowLeft class="w-4 h-4 mr-1" />
        Families
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
      No family selected. Please navigate from the Families list.
    </div>
  </div>
</template>
