<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Pencil, Trash2, Users } from 'lucide-vue-next'
import { DataTable, type PaginationState } from '@/components/generic/DataTable'
import { Button } from '@/components/ui/button'
import { ConfirmDeleteDialog } from '@/components/generic/ConfirmDeleteDialog'
import { familyService } from '@/services/FamilyService'
import { useAppToast } from '@/composables/useAppToast'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import type { FamilyDto } from '@/api/api-client'
import { columns } from './Columns'

const router = useRouter()
const toast = useAppToast()

function goToEditFamily(id: string) {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.EDIT_FAMILY), params: { id } })
}

function goToFamilyUsers(familyId: string) {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILY_USERS), params: { familyId } })
}

const families = ref<FamilyDto[]>([])
const pagination = ref<PaginationState>({ page: 1, total: 0 })
const pageSize = ref(10)
const searchTerm = ref('')
const loading = ref(false)
const familyToDelete = ref<FamilyDto | null>(null)
const deleteDialogOpen = ref(false)
const isDeletingFamily = ref(false)

function getFamilyDisplayName(family: FamilyDto) {
  const lastName = family.familyLastName && family.familyLastName !== family.familyName
    ? ` ${family.familyLastName}`
    : ''
  return `${family.familyName ?? ''}${lastName}`
}

function openDeleteFamilyDialog(family: FamilyDto) {
  familyToDelete.value = family
  deleteDialogOpen.value = true
}

async function confirmDeleteFamily() {
  if (!familyToDelete.value?.familyId) return

  isDeletingFamily.value = true
  try {
    const response = await familyService.deleteFamily(familyToDelete.value.familyId)

    if (response.success) {
      deleteDialogOpen.value = false
      familyToDelete.value = null
      await fetchFamilies()
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

async function fetchFamilies() {
  loading.value = true
  const response = await familyService.getFamilies(
    pagination.value.page,
    pageSize.value,
    searchTerm.value || undefined,
  )
  if (response.success && response.data) {
    families.value = response.data.items
    pagination.value = { ...pagination.value, total: response.data.totalCount }
  }
  loading.value = false
}

function onPaginationUpdate(state: PaginationState) {
  pagination.value = state
  fetchFamilies()
}

function onPageSizeUpdate(size: number) {
  pageSize.value = size
  pagination.value = { ...pagination.value, page: 1 }
  fetchFamilies()
}

function onSearch(query: string) {
  searchTerm.value = query
  pagination.value = { ...pagination.value, page: 1 }
  fetchFamilies()
}

onMounted(fetchFamilies)
</script>

<template>
  <DataTable
    :columns="columns"
    :data="families"
    title="All Families"
    description="Manage family groups and their members"
    :searchable="true"
    :server-search="true"
    search-placeholder="Search families..."
    empty-message="No families found."
    :loading="loading"
    :pagination="pagination"
    @update:pagination="onPaginationUpdate"
    @update:page-size="onPageSizeUpdate"
    @search="onSearch"
  >
    <template #cell-createdOn="{ row }">
      {{ new Date((row as FamilyDto).createdOn!).toLocaleDateString() }}
    </template>
    <template #cell-actions="{ row }">
      <div class="flex items-center justify-end gap-1">
        <Button size="sm" variant="ghost" @click="goToFamilyUsers((row as FamilyDto).familyId!)">
          <Users class="h-4 w-4" />
        </Button>
        <Button size="sm" variant="ghost" @click="goToEditFamily((row as FamilyDto).familyId!)">
          <Pencil class="h-4 w-4" />
        </Button>
        <Button
          size="sm"
          variant="ghost"
          class="text-destructive hover:text-destructive"
          @click="openDeleteFamilyDialog(row as FamilyDto)"
        >
          <Trash2 class="h-4 w-4" />
        </Button>
      </div>
    </template>
  </DataTable>

  <ConfirmDeleteDialog
    v-model:open="deleteDialogOpen"
    title="Delete family"
    :description="familyToDelete ? `Are you sure you want to delete ${getFamilyDisplayName(familyToDelete)}? This action cannot be undone.` : ''"
    :loading="isDeletingFamily"
    @confirm="confirmDeleteFamily"
  />
</template>
