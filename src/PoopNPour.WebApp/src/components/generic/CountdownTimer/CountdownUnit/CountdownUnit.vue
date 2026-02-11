<script setup lang="ts">
import { Badge } from '@/components/ui/badge'
import { Label } from '@/components/ui/label'
import type { CountdownUnitProps } from './types'

defineProps<CountdownUnitProps>()
</script>

<template>
  <div class="flex flex-col items-center space-y-3">
    <Badge 
      variant="outline" 
      class="w-full h-20 flex items-center justify-center text-4xl md:text-5xl font-bold bg-primary/5 border-primary/20 hover:bg-primary/10"
    >
      {{ value }}
    </Badge>
    <Label class="text-sm text-muted-foreground uppercase tracking-wide font-medium">
      {{ label }}
    </Label>
  </div>
</template>
