<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Pencil, Trash2 } from 'lucide-vue-next'
import { DataTable, type PaginationState } from '@/components/generic/DataTable'
import { Button } from '@/components/ui/button'
import { userService } from '@/services/UserService/UserService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import type { UserProfile } from '@/stores'
import { columns } from './Columns'

const router = useRouter()

function goToEditUser(id: string) {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.EDIT_USER), params: { id } })
}

function goToDeleteUser(id: string) {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DELETE_USER), params: { id } })
}

const users = ref<UserProfile[]>([])
const pagination = ref<PaginationState>({ page: 1, total: 0 })
const pageSize = ref(10)
const searchTerm = ref('')
const loading = ref(false)

async function fetchUsers() {
  loading.value = true
  const response = await userService.getUsers(
    pagination.value.page,
    pageSize.value,
    searchTerm.value || undefined,
  )
  if (response.success && response.data) {
    users.value = response.data.items
    pagination.value = { ...pagination.value, total: response.data.totalCount }
  }
  loading.value = false
}

function onPaginationUpdate(state: PaginationState) {
  pagination.value = state
  fetchUsers()
}

function onPageSizeUpdate(size: number) {
  pageSize.value = size
  pagination.value = { ...pagination.value, page: 1 }
  fetchUsers()
}

function onSearch(query: string) {
  searchTerm.value = query
  pagination.value = { ...pagination.value, page: 1 }
  fetchUsers()
}

onMounted(fetchUsers)
</script>

<template>
  <DataTable
    :columns="columns"
    :data="users"
    title="All Users"
    description="Manage user accounts and permissions"
    :searchable="true"
    :server-search="true"
    search-placeholder="Search users..."
    empty-message="No users found."
    :loading="loading"
    :pagination="pagination"
    @update:pagination="onPaginationUpdate"
    @update:page-size="onPageSizeUpdate"
    @search="onSearch"
  >
    <template #cell-actions="{ row }">
      <div class="flex items-center justify-end gap-1">
        <Button size="sm" variant="ghost" @click="goToEditUser((row as UserProfile).id)">
          <Pencil class="h-4 w-4" />
        </Button>
        <Button size="sm" variant="ghost" class="text-muted-foreground hover:text-destructive" @click="goToDeleteUser((row as UserProfile).id)">
          <Trash2 class="h-4 w-4" />
        </Button>
      </div>
    </template>
  </DataTable>
</template>
