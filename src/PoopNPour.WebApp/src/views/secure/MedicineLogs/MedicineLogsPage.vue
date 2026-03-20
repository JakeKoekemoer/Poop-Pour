<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { Pill, ArrowLeft, Plus, Pencil } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { DataTable, type PaginationState } from '@/components/generic/DataTable'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { medicineLogService } from '@/services/MedicineLogService'
import { dependentService } from '@/services/DependentService'
import { useFamilyContextStore } from '@/stores'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import type { MedicineLogDto, DependentDto } from '@/api/api-client'
import { columns } from '@/components/views/secure/MedicineLogs/MedicineLogsColumns'

const route = useRoute()
const router = useRouter()
const familyContextStore = useFamilyContextStore()

const familyId = computed(() => (route.params['familyId'] as string) || familyContextStore.familyId)

const medicineLogs = ref<MedicineLogDto[]>([])
const dependents = ref<DependentDto[]>([])
const dependentNameMap = ref<Record<string, string>>({})
const pagination = ref<PaginationState>({ page: 1, total: 0 })
const pageSize = ref(10)
const ALL_DEPENDENTS = '__all__'
const selectedDependentId = ref<string>(ALL_DEPENDENTS)
const loading = ref(false)

function getDependentDisplayName(dependent: DependentDto) {
  const name = dependent.dependentName ?? ''
  const surname = dependent.dependentSurname ?? ''
  return `${name} ${surname}`.trim() || (dependent.dependentId ?? '—')
}

function goToFamilyDashboard() {
  if (familyId.value) {
    familyContextStore.setFamilyContext(familyId.value)
    router.push({
      name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FAMILY_DASHBOARD),
      params: { familyId: familyId.value },
    })
  } else {
    router.push({ name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DASHBOARD) })
  }
}

function goToAddMedicineLog() {
  if (familyId.value) {
    router.push({
      name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.ADD_MEDICINE_LOG),
      params: { familyId: familyId.value },
    })
  }
}

function goToEditMedicineLog(id: string) {
  if (familyId.value) {
    router.push({
      name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.EDIT_MEDICINE_LOG),
      params: { familyId: familyId.value, id },
    })
  }
}

async function loadDependents() {
  if (!familyId.value) return
  const response = await dependentService.getDependents(1, 200, familyId.value)
  if (response.success && response.data) {
    dependents.value = response.data.items
    const map: Record<string, string> = {}
    for (const d of response.data.items) {
      if (d.dependentId) map[d.dependentId] = getDependentDisplayName(d)
    }
    dependentNameMap.value = map
  }
}

async function fetchMedicineLogs() {
  if (!familyId.value) return
  loading.value = true
  const dependentIdFilter =
    selectedDependentId.value && selectedDependentId.value !== ALL_DEPENDENTS
      ? selectedDependentId.value
      : undefined
  const response = await medicineLogService.getMedicineLogs(
    pagination.value.page,
    pageSize.value,
    dependentIdFilter,
  )
  if (response.success && response.data) {
    medicineLogs.value = response.data.items
    pagination.value = { ...pagination.value, total: response.data.totalCount }
  }
  loading.value = false
}

function onPaginationUpdate(state: PaginationState) {
  pagination.value = state
  fetchMedicineLogs()
}

function onPageSizeUpdate(size: number) {
  pageSize.value = size
  pagination.value = { ...pagination.value, page: 1 }
  fetchMedicineLogs()
}

watch(selectedDependentId, () => {
  pagination.value = { ...pagination.value, page: 1 }
  fetchMedicineLogs()
})

onMounted(async () => {
  await loadDependents()
  fetchMedicineLogs()
})
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="sm" @click="goToFamilyDashboard">
        <ArrowLeft class="w-4 h-4 mr-1" />
        Family
      </Button>
    </div>

    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <Pill class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Medicine Logs</h1>
      </div>
      <Button v-if="familyId" @click="goToAddMedicineLog">
        <Plus class="w-4 h-4 mr-2" />
        Add Medicine Log
      </Button>
    </div>

    <div v-if="familyId" class="space-y-4">
      <div class="flex items-center gap-4">
        <Select v-model="selectedDependentId">
          <SelectTrigger class="w-[240px]">
            <SelectValue placeholder="All dependents" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem :value="ALL_DEPENDENTS">All dependents</SelectItem>
            <SelectItem
              v-for="d in dependents"
              :key="d.dependentId!"
              :value="d.dependentId!"
            >
              {{ getDependentDisplayName(d) }}
            </SelectItem>
          </SelectContent>
        </Select>
      </div>

      <DataTable
        :columns="columns"
        :data="medicineLogs"
        title="Medicine Logs"
        description="Log medicine administration for dependents"
        :loading="loading"
        :pagination="pagination"
        empty-message="No medicine logs found."
        @update:pagination="onPaginationUpdate"
        @update:page-size="onPageSizeUpdate"
      >
        <template #cell-dependentName="{ row }">
          {{ (row as MedicineLogDto).dependentId ? (dependentNameMap[(row as MedicineLogDto).dependentId!] ?? '—') : '—' }}
        </template>
        <template #cell-medicineName="{ row }">
          {{ (row as MedicineLogDto).medicineName ?? '—' }}
        </template>
        <template #cell-dosage="{ row }">
          {{ (row as MedicineLogDto).dosage ?? '—' }}
        </template>
        <template #cell-timeAdministered="{ row }">
          {{ (row as MedicineLogDto).timeAdministered ? new Date((row as MedicineLogDto).timeAdministered!).toLocaleString() : '—' }}
        </template>
        <template #cell-createdOn="{ row }">
          {{ (row as MedicineLogDto).createdOn ? new Date((row as MedicineLogDto).createdOn!).toLocaleDateString() : '—' }}
        </template>
        <template #cell-actions="{ row }">
          <div class="flex items-center justify-end gap-1">
            <Button
              size="sm"
              variant="ghost"
              @click="goToEditMedicineLog((row as MedicineLogDto).medicineLogId!)"
            >
              <Pencil class="h-4 w-4" />
            </Button>
          </div>
        </template>
      </DataTable>
    </div>

    <div v-else class="text-muted-foreground">
      No family selected. Please navigate from the Family dashboard.
    </div>
  </div>
</template>
