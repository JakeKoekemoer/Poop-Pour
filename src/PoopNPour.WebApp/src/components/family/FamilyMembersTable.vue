<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Trash2 } from 'lucide-vue-next'
import { DataTable, type PaginationState } from '@/components/generic/DataTable'
import { Button } from '@/components/ui/button'
import { familyUserService } from '@/services/FamilyUserService'
import { useAppToast } from '@/composables/useAppToast'
import type { FamilyMemberDto } from '@/api/api-client'
import { columns, getRoleLabel } from './Columns'

const props = defineProps<{
  familyId: string
}>()

const toast = useAppToast()

const members = ref<FamilyMemberDto[]>([])
const pagination = ref<PaginationState>({ page: 1, total: 0 })
const pageSize = ref(10)
const loading = ref(false)
const removingUserId = ref<string | null>(null)

async function fetchMembers() {
  loading.value = true
  const response = await familyUserService.getFamilyMembers(
    props.familyId,
    pagination.value.page,
    pageSize.value,
  )
  if (response.success && response.data) {
    members.value = response.data.items
    pagination.value = { ...pagination.value, total: response.data.totalCount }
  }
  loading.value = false
}

async function removeMember(member: FamilyMemberDto) {
  if (!member.userId) return
  removingUserId.value = member.userId

  const response = await familyUserService.removeUserFromFamily(props.familyId, member.userId)

  if (response.success) {
    const name = [member.firstName, member.lastName].filter(Boolean).join(' ') || member.email || 'Member'
    toast.success('Member removed', `${name} has been removed from the family.`)
    await fetchMembers()
  } else {
    toast.error('Remove failed', response.error?.message ?? 'Failed to remove member. Please try again.')
  }

  removingUserId.value = null
}

function onPaginationUpdate(state: PaginationState) {
  pagination.value = state
  fetchMembers()
}

function onPageSizeUpdate(size: number) {
  pageSize.value = size
  pagination.value = { ...pagination.value, page: 1 }
  fetchMembers()
}

onMounted(fetchMembers)
</script>

<template>
  <DataTable
    :columns="columns"
    :data="members"
    title="Family Members"
    description="Users who belong to this family"
    empty-message="No members found."
    :loading="loading"
    :pagination="pagination"
    @update:pagination="onPaginationUpdate"
    @update:page-size="onPageSizeUpdate"
  >
    <template #cell-fullName="{ row }">
      {{ [(row as FamilyMemberDto).firstName, (row as FamilyMemberDto).lastName].filter(Boolean).join(' ') || '—' }}
    </template>
    <template #cell-role="{ row }">
      {{ getRoleLabel((row as FamilyMemberDto).role) }}
    </template>
    <template #cell-joinedOn="{ row }">
      {{ (row as FamilyMemberDto).joinedOn ? new Date((row as FamilyMemberDto).joinedOn!).toLocaleDateString() : '—' }}
    </template>
    <template #cell-actions="{ row }">
      <div class="flex items-center justify-end gap-1">
        <Button
          size="sm"
          variant="ghost"
          :disabled="removingUserId === (row as FamilyMemberDto).userId"
          @click="removeMember(row as FamilyMemberDto)"
        >
          <Trash2 class="h-4 w-4 text-destructive" />
        </Button>
      </div>
    </template>
  </DataTable>
</template>
