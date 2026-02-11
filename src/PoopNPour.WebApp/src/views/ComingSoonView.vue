<script setup lang="ts">
import { Sparkles, Palette, Heart, Zap } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { useTheme } from '@/composables/useTheme'
import { CountdownTimer } from '@/components/generic/CountdownTimer'

const { currentThemeConfig } = useTheme()

// Launch date: March 1, 2026
const launchDate = new Date('2026-03-01T00:00:00')
</script>

<template>
  <div class="min-h-[calc(100vh-8rem)] flex items-center justify-center p-4">
    <div class="max-w-4xl mx-auto space-y-12">
      <!-- Hero Section -->
      <div class="text-center space-y-6">
        <div class="flex justify-center">
          <div class="relative">
            <Sparkles class="w-20 h-20 text-primary animate-pulse" />
            <div
              class="absolute inset-0 w-20 h-20 bg-primary/20 rounded-full blur-xl animate-pulse"
            />
          </div>
        </div>
        <h1 class="text-5xl md:text-6xl font-bold text-foreground">Coming Soon</h1>
        <p class="text-xl md:text-2xl text-muted-foreground max-w-2xl mx-auto">
          Poop & Pour - Baby tracking made simple for tired parents 🌙
        </p>
      </div>

      <!-- Countdown Timer -->
      <CountdownTimer
        :target-date="launchDate"
        title="Launching Soon"
        subtitle="March 1st, 2026"
      />
    </div>
  </div>
</template>
