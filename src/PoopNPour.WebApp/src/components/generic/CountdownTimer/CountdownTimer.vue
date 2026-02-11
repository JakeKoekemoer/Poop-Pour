<script setup lang="ts">
import { ref, onMounted, onUnmounted, computed } from 'vue'
import { Calendar } from 'lucide-vue-next'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'
import { CountdownUnit } from './CountdownUnit'
import type { CountdownTimerProps, TimeRemaining } from './types'

const props = withDefaults(defineProps<CountdownTimerProps>(), {
  title: 'Launching Soon',
  showIcon: true
})

const timeRemaining = ref<TimeRemaining>({
  days: 0,
  hours: 0,
  minutes: 0,
  seconds: 0
})

const targetTimestamp = computed(() => {
  return typeof props.targetDate === 'string' 
    ? new Date(props.targetDate).getTime() 
    : props.targetDate.getTime()
})

const isCountdownComplete = computed(() => {
  return timeRemaining.value.days === 0 
    && timeRemaining.value.hours === 0 
    && timeRemaining.value.minutes === 0 
    && timeRemaining.value.seconds === 0
})

let intervalId: number | null = null

const calculateTimeRemaining = () => {
  const now = new Date().getTime()
  const distance = targetTimestamp.value - now

  if (distance < 0) {
    timeRemaining.value = { days: 0, hours: 0, minutes: 0, seconds: 0 }
    if (intervalId) {
      clearInterval(intervalId)
    }
    return
  }

  timeRemaining.value = {
    days: Math.floor(distance / (1000 * 60 * 60 * 24)),
    hours: Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)),
    minutes: Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60)),
    seconds: Math.floor((distance % (1000 * 60)) / 1000)
  }
}

onMounted(() => {
  calculateTimeRemaining()
  intervalId = window.setInterval(calculateTimeRemaining, 1000)
})

onUnmounted(() => {
  if (intervalId) {
    clearInterval(intervalId)
  }
})
</script>

<template>
  <Card class="shadow-lg">
    <CardHeader class="text-center space-y-4 pb-4">
      <div v-if="showIcon" class="flex justify-center">
        <div class="w-12 h-12 rounded-lg bg-primary/10 flex items-center justify-center">
          <Calendar class="w-6 h-6 text-primary" />
        </div>
      </div>
      
      <div class="space-y-2">
        <CardTitle class="text-2xl">
          {{ title }}
        </CardTitle>
        
        <CardDescription v-if="subtitle" class="text-base">
          {{ subtitle }}
        </CardDescription>
      </div>
    </CardHeader>

    <Separator />

    <CardContent class="pt-6">
      <!-- Countdown Display -->
      <div class="grid grid-cols-4 gap-4">
        <CountdownUnit :value="timeRemaining.days" label="Days" />
        <CountdownUnit :value="timeRemaining.hours" label="Hours" />
        <CountdownUnit :value="timeRemaining.minutes" label="Minutes" />
        <CountdownUnit :value="timeRemaining.seconds" label="Seconds" />
      </div>

      <!-- Countdown Complete Message -->
      <div v-if="isCountdownComplete" class="mt-6 text-center">
        <Separator class="mb-4" />
        <slot name="complete">
          <Badge variant="default" class="text-base px-4 py-2">
            🎉 We're live!
          </Badge>
        </slot>
      </div>
    </CardContent>
  </Card>
</template>
