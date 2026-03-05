<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Pencil, Trash2 } from 'lucide-vue-next'
import { DataTable, type PaginationState } from '@/components/generic/DataTable'
import { Button } from '@/components/ui/button'
import { ConfirmDeleteDialog } from '@/components/generic/ConfirmDeleteDialog'
import { dependentService } from '@/services/DependentService'
import { familyService } from '@/services/FamilyService'
import { useAppToast } from '@/composables/useAppToast'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import type { DependentDto, FamilyDto } from '@/api/api-client'
import { columns } from './Columns'

const router = useRouter()
const toast = useAppToast()

const familyMap = ref<Map<string, string>>(new Map())

function getFamilyDisplayName(family: FamilyDto) {
  const lastName = family.familyLastName && family.familyLastName !== family.familyName
    ? ` ${family.familyLastName}`
    : ''
  return `${family.familyName ?? ''}${lastName}`
}

function getDependentDisplayName(dependent: DependentDto) {
  const name = dependent.dependentName ?? ''
  const surname = dependent.dependentSurname ?? ''
  return `${name} ${surname}`.trim() || '—'
}

function goToEditDependent(id: string) {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.EDIT_DEPENDENT), params: { id } })
}

const dependents = ref<DependentDto[]>([])
const pagination = ref<PaginationState>({ page: 1, total: 0 })
const pageSize = ref(10)
const searchTerm = ref('')
const loading = ref(false)
const dependentToDelete = ref<DependentDto | null>(null)
const deleteDialogOpen = ref(false)
const isDeletingDependent = ref(false)

function openDeleteDependentDialog(dependent: DependentDto) {
  dependentToDelete.value = dependent
  deleteDialogOpen.value = true
}

async function confirmDeleteDependent() {
  if (!dependentToDelete.value?.dependentId) return

  isDeletingDependent.value = true
  try {
    const response = await dependentService.deleteDependent(dependentToDelete.value.dependentId)

    if (response.success) {
      deleteDialogOpen.value = false
      dependentToDelete.value = null
      await fetchDependents()
      toast.success('Dependent deleted', 'The dependent has been removed.')
    } else {
      const message = response.error?.message ?? 'Failed to delete dependent. Please try again.'
      toast.error('Delete failed', message)
    }
  } catch (error) {
    toast.error('Something went wrong', 'An unexpected error occurred. Please try again.')
    console.error('Delete dependent error:', error)
  } finally {
    isDeletingDependent.value = false
  }
}

async function fetchFamiliesForDisplay() {
  const response = await familyService.getFamilies(1, 200)
  if (response.success && response.data) {
    const map = new Map<string, string>()
    for (const f of response.data.items) {
      if (f.familyId) map.set(f.familyId, getFamilyDisplayName(f))
    }
    familyMap.value = map
  }
}

async function fetchDependents() {
  loading.value = true
  const response = await dependentService.getDependents(
    pagination.value.page,
    pageSize.value,
    undefined,
    searchTerm.value || undefined,
  )
  if (response.success && response.data) {
    dependents.value = response.data.items
    pagination.value = { ...pagination.value, total: response.data.totalCount }
  }
  loading.value = false
}

function onPaginationUpdate(state: PaginationState) {
  pagination.value = state
  fetchDependents()
}

function onPageSizeUpdate(size: number) {
  pageSize.value = size
  pagination.value = { ...pagination.value, page: 1 }
  fetchDependents()
}

function onSearch(query: string) {
  searchTerm.value = query
  pagination.value = { ...pagination.value, page: 1 }
  fetchDependents()
}

onMounted(async () => {
  await fetchFamiliesForDisplay()
  await fetchDependents()
})
</script>

<template>
  <DataTable
    :columns="columns"
    :data="dependents"
    title="All Dependents"
    description="Manage dependents across families"
    :searchable="true"
    :server-search="true"
    search-placeholder="Search dependents..."
    empty-message="No dependents found."
    :loading="loading"
    :pagination="pagination"
    @update:pagination="onPaginationUpdate"
    @update:page-size="onPageSizeUpdate"
    @search="onSearch"
  >
    <template #cell-family="{ row }">
      {{ familyMap.get((row as DependentDto).familyId ?? '') ?? '—' }}
    </template>
    <template #cell-age="{ row }">
      {{ (row as DependentDto).age ?? '—' }}
    </template>
    <template #cell-createdOn="{ row }">
      {{ (row as DependentDto).createdOn ? new Date((row as DependentDto).createdOn!).toLocaleDateString() : '—' }}
    </template>
    <template #cell-actions="{ row }">
      <div class="flex items-center justify-end gap-1">
        <Button size="sm" variant="ghost" @click="goToEditDependent((row as DependentDto).dependentId!)">
          <Pencil class="h-4 w-4" />
        </Button>
        <Button
          size="sm"
          variant="ghost"
          class="text-destructive hover:text-destructive"
          @click="openDeleteDependentDialog(row as DependentDto)"
        >
          <Trash2 class="h-4 w-4" />
        </Button>
      </div>
    </template>
  </DataTable>

  <ConfirmDeleteDialog
    v-model:open="deleteDialogOpen"
    title="Delete dependent"
    :description="dependentToDelete ? `Are you sure you want to delete ${getDependentDisplayName(dependentToDelete)}? This action cannot be undone.` : ''"
    :loading="isDeletingDependent"
    @confirm="confirmDeleteDependent"
  />
</template>
