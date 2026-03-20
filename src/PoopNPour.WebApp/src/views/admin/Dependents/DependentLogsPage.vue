<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { Baby, ArrowLeft, Droplets, Utensils, Pill } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { DataTable, type PaginationState } from '@/components/generic/DataTable'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { diperLogService } from '@/services/DiperLogService'
import { feedLogService } from '@/services/FeedLogService'
import { medicineLogService } from '@/services/MedicineLogService'
import { dependentService } from '@/services/DependentService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import {
  getLabelForValue,
  FECAL_DISCHARGE_COLOUR_OPTIONS,
  URINAL_DISCHARGE_COLOUR_OPTIONS,
  FEED_LOG_TYPE_OPTIONS,
} from '@/utils/logEnums'
import type { DataTableColumn } from '@/components/generic/DataTable'
import type { DiperLogDto, FeedLogDto, MedicineLogDto, DependentDto } from '@/api/api-client'
import { columns as diaperColumns } from '@/components/views/secure/DiaperLogs/DiaperLogsColumns'
import { columns as feedColumns } from '@/components/views/secure/FeedLogs/FeedLogsColumns'
import { columns as medicineColumns } from '@/components/views/secure/MedicineLogs/MedicineLogsColumns'

const route = useRoute()
const router = useRouter()

const dependentId = computed(() => route.params['id'] as string)
const dependent = ref<DependentDto | null>(null)

const diaperLogs = ref<DiperLogDto[]>([])
const feedLogs = ref<FeedLogDto[]>([])
const medicineLogs = ref<MedicineLogDto[]>([])

const diaperPagination = ref<PaginationState>({ page: 1, total: 0 })
const feedPagination = ref<PaginationState>({ page: 1, total: 0 })
const medicinePagination = ref<PaginationState>({ page: 1, total: 0 })

const pageSize = ref(10)
const diaperLoading = ref(false)
const feedLoading = ref(false)
const medicineLoading = ref(false)

const columnsWithoutDependentAndActions = (cols: DataTableColumn[]) =>
  cols.filter((c) => c.key !== 'dependentName' && c.key !== 'actions')

const diaperColumnsFiltered = computed(() => columnsWithoutDependentAndActions(diaperColumns))
const feedColumnsFiltered = computed(() => columnsWithoutDependentAndActions(feedColumns))
const medicineColumnsFiltered = computed(() => columnsWithoutDependentAndActions(medicineColumns))

function getDependentDisplayName(d: DependentDto | null) {
  if (!d) return '—'
  const name = d.dependentName ?? ''
  const surname = d.dependentSurname ?? ''
  return `${name} ${surname}`.trim() || (d.dependentId ?? '—')
}

function goToDependents() {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DEPENDENTS) })
}

async function loadDependent() {
  if (!dependentId.value) return
  const response = await dependentService.getDependentById(dependentId.value)
  if (response.success && response.data) {
    dependent.value = response.data
  }
}

async function fetchDiaperLogs() {
  if (!dependentId.value) return
  diaperLoading.value = true
  const response = await diperLogService.getDiperLogs(
    diaperPagination.value.page,
    pageSize.value,
    dependentId.value,
  )
  if (response.success && response.data) {
    diaperLogs.value = response.data.items
    diaperPagination.value = { ...diaperPagination.value, total: response.data.totalCount }
  }
  diaperLoading.value = false
}

async function fetchFeedLogs() {
  if (!dependentId.value) return
  feedLoading.value = true
  const response = await feedLogService.getFeedLogs(
    feedPagination.value.page,
    pageSize.value,
    dependentId.value,
  )
  if (response.success && response.data) {
    feedLogs.value = response.data.items
    feedPagination.value = { ...feedPagination.value, total: response.data.totalCount }
  }
  feedLoading.value = false
}

async function fetchMedicineLogs() {
  if (!dependentId.value) return
  medicineLoading.value = true
  const response = await medicineLogService.getMedicineLogs(
    medicinePagination.value.page,
    pageSize.value,
    dependentId.value,
  )
  if (response.success && response.data) {
    medicineLogs.value = response.data.items
    medicinePagination.value = { ...medicinePagination.value, total: response.data.totalCount }
  }
  medicineLoading.value = false
}

function onDiaperPaginationUpdate(state: PaginationState) {
  diaperPagination.value = state
  fetchDiaperLogs()
}

function onFeedPaginationUpdate(state: PaginationState) {
  feedPagination.value = state
  fetchFeedLogs()
}

function onMedicinePaginationUpdate(state: PaginationState) {
  medicinePagination.value = state
  fetchMedicineLogs()
}

function onPageSizeUpdate(size: number) {
  pageSize.value = size
  diaperPagination.value = { ...diaperPagination.value, page: 1 }
  feedPagination.value = { ...feedPagination.value, page: 1 }
  medicinePagination.value = { ...medicinePagination.value, page: 1 }
  fetchDiaperLogs()
  fetchFeedLogs()
  fetchMedicineLogs()
}

watch(dependentId, () => {
  loadDependent()
  fetchDiaperLogs()
  fetchFeedLogs()
  fetchMedicineLogs()
})

onMounted(async () => {
  await loadDependent()
  fetchDiaperLogs()
  fetchFeedLogs()
  fetchMedicineLogs()
})
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="sm" @click="goToDependents">
        <ArrowLeft class="w-4 h-4 mr-1" />
        Dependents
      </Button>
    </div>

    <div class="flex items-center gap-3">
      <Baby class="w-8 h-8 text-primary" />
      <h1 class="text-3xl font-bold">{{ getDependentDisplayName(dependent) }}</h1>
    </div>

    <Tabs default-value="diaper" class="w-full">
      <TabsList class="grid w-full grid-cols-3">
        <TabsTrigger value="diaper" class="flex items-center gap-2">
          <Droplets class="h-4 w-4" />
          Diaper Logs
        </TabsTrigger>
        <TabsTrigger value="feed" class="flex items-center gap-2">
          <Utensils class="h-4 w-4" />
          Feed Logs
        </TabsTrigger>
        <TabsTrigger value="medicine" class="flex items-center gap-2">
          <Pill class="h-4 w-4" />
          Medicine Logs
        </TabsTrigger>
      </TabsList>

      <TabsContent value="diaper">
        <DataTable
          :columns="diaperColumnsFiltered"
          :data="diaperLogs"
          title="Diaper Logs"
          description="Diaper changes for this dependent"
          :loading="diaperLoading"
          :pagination="diaperPagination"
          empty-message="No diaper logs found."
          @update:pagination="onDiaperPaginationUpdate"
          @update:page-size="onPageSizeUpdate"
        >
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
        </DataTable>
      </TabsContent>

      <TabsContent value="feed">
        <DataTable
          :columns="feedColumnsFiltered"
          :data="feedLogs"
          title="Feed Logs"
          description="Feeding events for this dependent"
          :loading="feedLoading"
          :pagination="feedPagination"
          empty-message="No feed logs found."
          @update:pagination="onFeedPaginationUpdate"
          @update:page-size="onPageSizeUpdate"
        >
          <template #cell-feedType="{ row }">
            {{ getLabelForValue((row as FeedLogDto).feedType as number, FEED_LOG_TYPE_OPTIONS) }}
          </template>
          <template #cell-timeFed="{ row }">
            {{ (row as FeedLogDto).timeFed ? new Date((row as FeedLogDto).timeFed!).toLocaleString() : '—' }}
          </template>
          <template #cell-mililitersFed="{ row }">
            {{ (row as FeedLogDto).mililitersFed ?? '—' }}
          </template>
          <template #cell-createdOn="{ row }">
            {{ (row as FeedLogDto).createdOn ? new Date((row as FeedLogDto).createdOn!).toLocaleDateString() : '—' }}
          </template>
        </DataTable>
      </TabsContent>

      <TabsContent value="medicine">
        <DataTable
          :columns="medicineColumnsFiltered"
          :data="medicineLogs"
          title="Medicine Logs"
          description="Medicine administration for this dependent"
          :loading="medicineLoading"
          :pagination="medicinePagination"
          empty-message="No medicine logs found."
          @update:pagination="onMedicinePaginationUpdate"
          @update:page-size="onPageSizeUpdate"
        >
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
        </DataTable>
      </TabsContent>
    </Tabs>
  </div>
</template>
