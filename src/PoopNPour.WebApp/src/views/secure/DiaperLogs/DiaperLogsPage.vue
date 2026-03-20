<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { Droplets, ArrowLeft, Plus, Pencil } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { DataTable, type PaginationState } from '@/components/generic/DataTable'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { diperLogService } from '@/services/DiperLogService'
import { dependentService } from '@/services/DependentService'
import { useFamilyContextStore } from '@/stores'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import {
  getLabelForValue,
  FECAL_DISCHARGE_COLOUR_OPTIONS,
  URINAL_DISCHARGE_COLOUR_OPTIONS,
} from '@/utils/logEnums'
import type { DiperLogDto, DependentDto } from '@/api/api-client'
import { columns } from '@/components/views/secure/DiaperLogs/DiaperLogsColumns'

const route = useRoute()
const router = useRouter()
const familyContextStore = useFamilyContextStore()

const familyId = computed(() => (route.params['familyId'] as string) || familyContextStore.familyId)

const diaperLogs = ref<DiperLogDto[]>([])
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

function goToAddDiaperLog() {
  if (familyId.value) {
    router.push({
      name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.ADD_DIAPER_LOG),
      params: { familyId: familyId.value },
    })
  }
}

function goToEditDiaperLog(id: string) {
  if (familyId.value) {
    router.push({
      name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.EDIT_DIAPER_LOG),
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

async function fetchDiaperLogs() {
  if (!familyId.value) return
  loading.value = true
  const dependentIdFilter =
    selectedDependentId.value && selectedDependentId.value !== ALL_DEPENDENTS
      ? selectedDependentId.value
      : undefined
  const response = await diperLogService.getDiperLogs(
    pagination.value.page,
    pageSize.value,
    dependentIdFilter,
  )
  if (response.success && response.data) {
    diaperLogs.value = response.data.items
    pagination.value = { ...pagination.value, total: response.data.totalCount }
  }
  loading.value = false
}

function onPaginationUpdate(state: PaginationState) {
  pagination.value = state
  fetchDiaperLogs()
}

function onPageSizeUpdate(size: number) {
  pageSize.value = size
  pagination.value = { ...pagination.value, page: 1 }
  fetchDiaperLogs()
}

watch(selectedDependentId, () => {
  pagination.value = { ...pagination.value, page: 1 }
  fetchDiaperLogs()
})

onMounted(async () => {
  await loadDependents()
  fetchDiaperLogs()
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
        <Droplets class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Diaper Logs</h1>
      </div>
      <Button v-if="familyId" @click="goToAddDiaperLog">
        <Plus class="w-4 h-4 mr-2" />
        Add Diaper Log
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
        :data="diaperLogs"
        title="Diaper Logs"
        description="Log diaper changes for dependents"
        :loading="loading"
        :pagination="pagination"
        empty-message="No diaper logs found."
        @update:pagination="onPaginationUpdate"
        @update:page-size="onPageSizeUpdate"
      >
        <template #cell-dependentName="{ row }">
          {{ (row as DiperLogDto).dependentId ? (dependentNameMap[(row as DiperLogDto).dependentId!] ?? '—') : '—' }}
        </template>
        <template #cell-diperDate="{ row }">
          {{ (row as DiperLogDto).diperDate ? new Date((row as DiperLogDto).diperDate!).toLocaleString() : '—' }}
        </template>
        <template #cell-fecalDischargeColour="{ row }">
          {{ getLabelForValue((row as DiperLogDto).fecalDischargeColour as number, FECAL_DISCHARGE_COLOUR_OPTIONS) }}
        </template>
        <template #cell-urinaryDischargeColour="{ row }">
          {{ getLabelForValue((row as DiperLogDto).urinaryDischargeColour as number, URINAL_DISCHARGE_COLOUR_OPTIONS) }}
        </template>
        <template #cell-createdOn="{ row }">
          {{ (row as DiperLogDto).createdOn ? new Date((row as DiperLogDto).createdOn!).toLocaleDateString() : '—' }}
        </template>
        <template #cell-actions="{ row }">
          <div class="flex items-center justify-end gap-1">
            <Button
              size="sm"
              variant="ghost"
              @click="goToEditDiaperLog((row as DiperLogDto).diperLogId!)"
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
