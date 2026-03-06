<script setup lang="ts">
import { Droplets, Utensils, Pill } from 'lucide-vue-next'
import { Card, CardTitle } from '@/components/ui/card'
import { Avatar, AvatarFallback } from '@/components/ui/avatar'
import { Button } from '@/components/ui/button'
import type { DependentDto } from '@/api/api-client'

const props = defineProps<{
  dependent: DependentDto
}>()

const emit = defineEmits<{
  'log-diaper': []
  'log-feed': []
  'log-medicine': []
}>()

function getDependentDisplayName(dependent: DependentDto) {
  const name = dependent.dependentName ?? ''
  const surname = dependent.dependentSurname ?? ''
  return `${name} ${surname}`.trim() || '—'
}

function getInitials(dependent: DependentDto) {
  const name = dependent.dependentName ?? ''
  const surname = dependent.dependentSurname ?? ''
  const first = name.charAt(0).toUpperCase()
  const last = surname.charAt(0).toUpperCase()
  if (first && last) return `${first}${last}`
  if (first) return first
  if (last) return last
  return '?'
}
</script>

<template>
  <Card class="flex flex-col gap-0 py-4 px-4">
    <div class="flex min-w-0 items-center gap-3">
      <Avatar class="size-12 shrink-0">
        <AvatarFallback class="text-base font-semibold">
          {{ getInitials(props.dependent) }}
        </AvatarFallback>
      </Avatar>
      <CardTitle class="truncate text-xl font-semibold">
        {{ getDependentDisplayName(props.dependent) }}
      </CardTitle>
    </div>
    <div class="mt-3 flex items-center justify-center gap-1">
      <Button
        variant="ghost"
        size="icon"
        class="size-10 rounded-full"
        title="Log diaper"
        @click="emit('log-diaper')"
      >
        <Droplets class="size-10" />
      </Button>
      <Button
        variant="ghost"
        size="icon"
        class="size-10 rounded-full"
        title="Log feed"
        @click="emit('log-feed')"
      >
        <Utensils class="size-10" />
      </Button>
      <Button
        variant="ghost"
        size="icon"
        class="size-10 rounded-full"
        title="Log medicine"
        @click="emit('log-medicine')"
      >
        <Pill class="size-10" />
      </Button>
    </div>
  </Card>
</template>
