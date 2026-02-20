<script setup lang="ts">
import { Search } from 'lucide-vue-next'
import { Input } from '@/components/ui/input'

defineProps<{
  placeholder?: string
}>()

const model = defineModel<string>({ default: '' })
</script>

<template>
  <div class="relative w-64">
    <Search class="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
    <Input v-model="model" :placeholder="placeholder ?? 'Search...'" class="pl-8" />
  </div>
</template>
