<script setup lang="ts">
import { ref } from 'vue'
import { DataTable, type PaginationState } from '@/components/generic/DataTable'
import { columns } from './Columns'

const users = ref([])
const pagination = ref<PaginationState>({ page: 1, total: 0 })
</script>

<template>
  <DataTable
    :columns="columns"
    :data="users"
    title="All Users"
    description="Manage user accounts and permissions"
    :searchable="true"
    search-placeholder="Search users..."
    empty-message="No users found."
    v-model:pagination="pagination"
  />
</template>
