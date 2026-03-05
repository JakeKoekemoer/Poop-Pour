<script setup lang="ts">
import { computed, ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Home, ArrowLeft, Users, Loader2 } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Skeleton } from '@/components/ui/skeleton'
import { useFamilyContextStore } from '@/stores'
import { familyService } from '@/services/FamilyService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import type { FamilyDto } from '@/api/api-client'

const router = useRouter()
const familyContextStore = useFamilyContextStore()

const familyId = computed(() => familyContextStore.familyId)
const family = ref<FamilyDto | null>(null)
const isLoading = ref(true)

async function loadFamily() {
  const id = familyContextStore.familyId
  if (!id) return

  isLoading.value = true
  const response = await familyService.getFamilyById(id)
  if (response.success && response.data) {
    family.value = response.data
  }
  isLoading.value = false
}

function goToDashboard() {
  familyContextStore.setPreventSingleFamilyAutoRedirect(true)
  router.push({ name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DASHBOARD) })
}

function goToManageUsers() {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FAMILY_MEMBERS),
    params: { familyId: familyContextStore.familyId! },
  })
}

const familyDisplayName = computed(() => {
  if (!family.value) return ''
  const { familyName, familyLastName } = family.value
  if (familyLastName && familyLastName !== familyName) {
    return `${familyName} ${familyLastName}`
  }
  return familyName || ''
})

onMounted(loadFamily)
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="sm" @click="goToDashboard">
        <ArrowLeft class="w-4 h-4 mr-1" />
        Dashboard
      </Button>
    </div>

    <div class="flex items-center gap-3">
      <Home class="w-8 h-8 text-primary" />
      <div>
        <Skeleton v-if="isLoading" class="h-9 w-48" />
        <h1 v-else class="text-3xl font-bold">{{ familyDisplayName || 'Family' }}</h1>
        <p class="text-muted-foreground">Manage your family</p>
      </div>
    </div>

    <div v-if="familyId" class="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4">
      <button
        class="flex flex-col items-center justify-center gap-3 rounded-xl border bg-card p-6 text-card-foreground shadow-sm transition-colors hover:bg-accent hover:text-accent-foreground cursor-pointer"
        @click="goToManageUsers"
      >
        <Users class="w-10 h-10" />
        <span class="text-sm font-medium text-center">Manage users in the family</span>
      </button>
    </div>

    <div v-else class="text-muted-foreground">
      No family selected. Please navigate from the dashboard.
    </div>
  </div>
</template>
