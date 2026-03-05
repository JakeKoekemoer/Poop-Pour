<script setup lang="ts">
import { computed, ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Home, ArrowLeft, Users, Loader2, Trash2 } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { ConfirmDeleteDialog } from '@/components/generic/ConfirmDeleteDialog'
import { Skeleton } from '@/components/ui/skeleton'
import { useAppToast } from '@/composables/useAppToast'
import { useFamilyContextStore } from '@/stores'
import { familyService } from '@/services/FamilyService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import type { FamilyDto } from '@/api/api-client'

const router = useRouter()
const familyContextStore = useFamilyContextStore()
const toast = useAppToast()

const familyId = computed(() => familyContextStore.familyId)
const family = ref<FamilyDto | null>(null)
const isLoading = ref(true)
const deleteDialogOpen = ref(false)
const isDeletingFamily = ref(false)

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

function openDeleteFamilyDialog() {
  deleteDialogOpen.value = true
}

async function confirmDeleteFamily() {
  const id = familyContextStore.familyId
  if (!id) return

  isDeletingFamily.value = true
  try {
    const response = await familyService.deleteFamily(id)

    if (response.success) {
      deleteDialogOpen.value = false
      family.value = null
      familyContextStore.clearFamilyContext()
      familyContextStore.setPreventSingleFamilyAutoRedirect(true)
      router.push({ name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DASHBOARD) })
      toast.success('Family deleted', 'The family has been removed.')
    } else {
      const message = response.error?.message ?? 'Failed to delete family. Please try again.'
      toast.error('Delete failed', message)
    }
  } catch (error) {
    toast.error('Something went wrong', 'An unexpected error occurred. Please try again.')
    console.error('Delete family error:', error)
  } finally {
    isDeletingFamily.value = false
  }
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

    <div class="flex items-start justify-between gap-4">
      <div class="flex items-center gap-3">
        <Home class="w-8 h-8 text-primary shrink-0" />
        <div>
          <Skeleton v-if="isLoading" class="h-9 w-48" />
          <h1 v-else class="text-3xl font-bold">{{ familyDisplayName || 'Family' }}</h1>
          <p class="text-muted-foreground">Manage your family</p>
        </div>
      </div>
      <Button
        v-if="familyId"
        variant="outline"
        size="sm"
        class="shrink-0 border-destructive text-destructive hover:bg-destructive hover:text-destructive-foreground"
        @click="openDeleteFamilyDialog"
      >
        <Trash2 class="w-4 h-4 mr-1.5" />
        Delete family
      </Button>
    </div>

    <ConfirmDeleteDialog
      v-model:open="deleteDialogOpen"
      title="Delete family"
      :description="family ? `Are you sure you want to delete ${familyDisplayName}? This action cannot be undone.` : ''"
      :loading="isDeletingFamily"
      @confirm="confirmDeleteFamily"
    />

    <div v-if="familyId" class="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4">
      <Button
        variant="ghost"
        class="flex flex-col items-center justify-center gap-3 rounded-xl border bg-card p-6 h-auto text-card-foreground shadow-sm transition-colors hover:bg-accent hover:text-accent-foreground cursor-pointer"
        @click="goToManageUsers"
      >
        <Users class="w-10 h-10" />
        <span class="text-sm font-medium text-center">Manage users in the family</span>
      </Button>
    </div>

    <div v-else class="text-muted-foreground">
      No family selected. Please navigate from the dashboard.
    </div>
  </div>
</template>
