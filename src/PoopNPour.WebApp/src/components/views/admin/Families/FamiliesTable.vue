<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Pencil, Users } from 'lucide-vue-next'
import { DataTable, type PaginationState } from '@/components/generic/DataTable'
import { Button } from '@/components/ui/button'
import { familyService } from '@/services/FamilyService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import type { FamilyDto } from '@/api/api-client'
import { columns } from './Columns'

const router = useRouter()

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
      </div>
    </template>
  </DataTable>
</template>
