<script setup lang="ts">
import { computed } from 'vue'
import { Sparkles } from 'lucide-vue-next'
import { useTheme } from '@/composables/useTheme'
import { CountdownTimer } from '@/components/generic/CountdownTimer'
import { EnvConfig } from '@/lib/env'

const { currentThemeConfig } = useTheme()

const launchDate = EnvConfig.launchDate

const launchDateSubtitle = computed(() => {
  return launchDate.toLocaleDateString('en-US', {
    month: 'long',
    day: 'numeric',
    year: 'numeric',
  })
})
</script>

<template>
  <div class="min-h-screen flex items-center justify-center p-4">
    <div class="max-w-4xl mx-auto w-full space-y-8 md:space-y-12">
      <div class="text-center space-y-6">
        <div class="flex justify-center">
          <div class="relative">
            <Sparkles class="w-16 h-16 md:w-20 md:h-20 text-primary animate-pulse" />
            <div
              class="absolute inset-0 w-16 h-16 md:w-20 md:h-20 bg-primary/20 rounded-full blur-xl animate-pulse"
            />
          </div>
        </div>
        <h1 class="text-4xl md:text-5xl lg:text-6xl font-bold text-foreground px-4">Coming Soon</h1>
        <p class="text-lg md:text-xl lg:text-2xl text-muted-foreground max-w-2xl mx-auto px-4">
          Poop & Pour - Baby tracking made simple for tired parents 🌙
        </p>
      </div>

      <CountdownTimer
        :target-date="launchDate"
        title="Launching Soon"
        :subtitle="launchDateSubtitle"
      />
    </div>
  </div>
</template>
