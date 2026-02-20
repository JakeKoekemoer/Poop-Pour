<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { DataTable, type PaginationState } from '@/components/generic/DataTable'
import { userService } from '@/services/UserService/UserService'
import type { UserProfile } from '@/stores'
import { columns } from './Columns'

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
  />
</template>
