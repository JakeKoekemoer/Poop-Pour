<script setup lang="ts">
import type { DarkMode } from '@/stores/theme/types'
import type { Component } from 'vue'
import { Label } from '@/components/ui/label'
import { RadioGroupItem } from '@/components/ui/radio-group'

interface Props {
  mode: DarkMode
  label: string
  icon: Component
  isSelected: boolean
  value: string
}

defineProps<Props>()
</script>

<template>
  <Label
    :for="value"
    class="flex flex-col items-center gap-2 p-3 rounded-lg border-2 transition-all cursor-pointer hover:bg-accent/50 hover:border-primary/50 focus-within:ring-2 focus-within:ring-ring focus-within:ring-offset-2"
    :class="[
      isSelected
        ? 'border-primary bg-primary/5'
        : 'border-border bg-card',
    ]"
  >
    <!-- Hidden Radio Input -->
    <RadioGroupItem
      :id="value"
      :value="value"
      class="sr-only"
    />

    <!-- Icon -->
    <component
      :is="icon"
      class="w-5 h-5"
      :class="isSelected ? 'text-primary' : 'text-muted-foreground'"
    />
    
    <!-- Label Text -->
    <span
      class="text-xs font-medium"
      :class="isSelected ? 'text-primary' : 'text-foreground'"
    >
      {{ label }}
    </span>
  </Label>
</template>
