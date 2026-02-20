<script setup lang="ts">
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import type { AcceptableValue } from 'reka-ui'
import { PAGE_SIZE_OPTIONS } from './types'

const model = defineModel<number>({ required: true })

function onValueChange(value: AcceptableValue) {
  const size = Number(value)
  if (!isNaN(size)) model.value = size
}
</script>

<template>
  <div class="flex items-center gap-2 text-sm text-muted-foreground">
    <span class="shrink-0">Rows per page</span>
    <Select :model-value="String(model)" @update:model-value="onValueChange">
      <SelectTrigger class="h-8 w-20">
        <SelectValue />
      </SelectTrigger>
      <SelectContent>
        <SelectItem v-for="opt in PAGE_SIZE_OPTIONS" :key="opt" :value="String(opt)">
          {{ opt }}
        </SelectItem>
      </SelectContent>
    </Select>
  </div>
</template>
