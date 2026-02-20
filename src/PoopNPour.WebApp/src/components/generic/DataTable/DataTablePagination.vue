<script setup lang="ts">
import { computed } from 'vue'
import { ChevronLeft, ChevronRight, ChevronsLeft, ChevronsRight } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import type { PaginationState } from './types'

const props = defineProps<{
  pagination: PaginationState
  /** Passed from DataTable's internal pageSize for from/to/totalPages calculations. */
  pageSize: number
}>()

const emit = defineEmits<{
  'update:pagination': [state: PaginationState]
}>()

const totalPages = computed(() =>
  Math.max(1, Math.ceil(props.pagination.total / props.pageSize)),
)

const from = computed(() => {
  if (props.pagination.total === 0) return 0
  return (props.pagination.page - 1) * props.pageSize + 1
})

const to = computed(() =>
  Math.min(props.pagination.page * props.pageSize, props.pagination.total),
)

function goTo(page: number) {
  emit('update:pagination', { ...props.pagination, page })
}
</script>

<template>
  <div class="flex items-center gap-4 text-sm text-muted-foreground">
    <span class="shrink-0">
      {{ from }}–{{ to }} of {{ pagination.total }}
    </span>

    <div class="flex items-center gap-1">
      <Button
        variant="outline"
        size="icon"
        class="h-8 w-8"
        :disabled="pagination.page <= 1"
        @click="goTo(1)"
      >
        <ChevronsLeft class="h-4 w-4" />
        <span class="sr-only">First page</span>
      </Button>
      <Button
        variant="outline"
        size="icon"
        class="h-8 w-8"
        :disabled="pagination.page <= 1"
        @click="goTo(pagination.page - 1)"
      >
        <ChevronLeft class="h-4 w-4" />
        <span class="sr-only">Previous page</span>
      </Button>
      <span class="px-2 text-foreground">
        Page {{ pagination.page }} of {{ totalPages }}
      </span>
      <Button
        variant="outline"
        size="icon"
        class="h-8 w-8"
        :disabled="pagination.page >= totalPages"
        @click="goTo(pagination.page + 1)"
      >
        <ChevronRight class="h-4 w-4" />
        <span class="sr-only">Next page</span>
      </Button>
      <Button
        variant="outline"
        size="icon"
        class="h-8 w-8"
        :disabled="pagination.page >= totalPages"
        @click="goTo(totalPages)"
      >
        <ChevronsRight class="h-4 w-4" />
        <span class="sr-only">Last page</span>
      </Button>
    </div>
  </div>
</template>
