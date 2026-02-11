<script setup lang="ts">
import { Check } from 'lucide-vue-next'
import { Label } from '@/components/ui/label'
import { RadioGroupItem } from '@/components/ui/radio-group'
import ColorPreview from '@/components/generic/ColorPreview'
import ThemeInfo from './ThemeInfo.vue'
import type { ThemeConfig } from '@/stores/theme/types'

interface Props {
  theme: ThemeConfig
  isSelected: boolean
  value: string
}

defineProps<Props>()

const emit = defineEmits<{
  select: []
}>()
</script>

<template>
  <Label
    :for="value"
    class="flex items-center gap-3 p-3 rounded-lg border-2 transition-all cursor-pointer hover:border-primary focus-within:ring-2 focus-within:ring-ring focus-within:ring-offset-2"
    :class="[
      isSelected
        ? 'border-primary bg-primary/5'
        : 'border-border bg-card hover:bg-accent/50',
    ]"
  >
    <!-- Hidden Radio Input -->
    <RadioGroupItem
      :id="value"
      :value="value"
      class="sr-only"
    />

    <!-- Color Preview -->
    <div class="flex gap-1 flex-shrink-0">
      <ColorPreview :color="theme.colors.primary" />
      <ColorPreview :color="theme.colors.secondary" />
    </div>

    <!-- Theme Info -->
    <ThemeInfo
      :emoji="theme.emoji"
      :title="theme.displayName"
      :description="theme.description"
    />

    <!-- Check Icon -->
    <Check
      v-if="isSelected"
      class="w-5 h-5 text-primary flex-shrink-0"
      aria-hidden="true"
    />
  </Label>
</template>
