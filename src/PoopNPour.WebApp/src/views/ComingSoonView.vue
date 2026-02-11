<script setup lang="ts">
import { Sparkles, Palette, Heart, Zap } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { useTheme } from '@/composables/useTheme'

const { currentThemeConfig } = useTheme()
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

      <!-- Theme Showcase -->
      <div class="space-y-4">
        <div class="text-center">
          <p class="text-sm font-medium text-muted-foreground mb-4">
            Currently viewing: {{ currentThemeConfig().emoji }}
            <span class="font-semibold text-foreground">{{ currentThemeConfig().displayName }}</span>
            theme
          </p>
        </div>

        <!-- Feature Cards -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div
            class="bg-card border-2 border-border rounded-xl p-6 space-y-3 hover:border-primary transition-colors"
          >
            <div
              class="w-12 h-12 rounded-lg bg-primary/10 flex items-center justify-center text-primary"
            >
              <Heart class="w-6 h-6" />
            </div>
            <h3 class="font-semibold text-card-foreground">Simple Tracking</h3>
            <p class="text-sm text-muted-foreground">
              Easy-to-use interface designed for sleep-deprived parents
            </p>
          </div>

          <div
            class="bg-card border-2 border-border rounded-xl p-6 space-y-3 hover:border-primary transition-colors"
          >
            <div
              class="w-12 h-12 rounded-lg bg-secondary/20 flex items-center justify-center text-accent"
            >
              <Palette class="w-6 h-6" />
            </div>
            <h3 class="font-semibold text-card-foreground">Beautiful Themes</h3>
            <p class="text-sm text-muted-foreground">
              Multiple calming color schemes perfect for any time of day
            </p>
          </div>

          <div
            class="bg-card border-2 border-border rounded-xl p-6 space-y-3 hover:border-primary transition-colors"
          >
            <div
              class="w-12 h-12 rounded-lg bg-accent/10 flex items-center justify-center text-accent"
            >
              <Zap class="w-6 h-6" />
            </div>
            <h3 class="font-semibold text-card-foreground">Quick Actions</h3>
            <p class="text-sm text-muted-foreground">
              Fast logging when every second counts during night feeds
            </p>
          </div>
        </div>
      </div>

      <!-- Button Examples -->
      <div class="flex flex-wrap justify-center gap-3">
        <Button variant="default" size="lg">
          <Sparkles class="w-4 h-4" />
          Primary Action
        </Button>
        <Button variant="secondary" size="lg"> Secondary Action </Button>
        <Button variant="outline" size="lg"> Outline Action </Button>
        <Button variant="ghost" size="lg"> Ghost Action </Button>
      </div>

      <!-- Color Badges -->
      <div class="flex flex-wrap justify-center gap-2">
        <span class="px-3 py-1.5 text-xs font-medium rounded-full bg-primary text-primary-foreground">
          Primary
        </span>
        <span
          class="px-3 py-1.5 text-xs font-medium rounded-full bg-secondary text-secondary-foreground"
        >
          Secondary
        </span>
        <span class="px-3 py-1.5 text-xs font-medium rounded-full bg-accent text-accent-foreground">
          Accent
        </span>
        <span class="px-3 py-1.5 text-xs font-medium rounded-full bg-muted text-muted-foreground">
          Muted
        </span>
        <span
          class="px-3 py-1.5 text-xs font-medium rounded-full bg-success text-success-foreground"
        >
          Success
        </span>
        <span
          class="px-3 py-1.5 text-xs font-medium rounded-full bg-destructive text-destructive-foreground"
        >
          Destructive
        </span>
      </div>

      <!-- Footer -->
      <div class="text-center pt-8">
        <p class="text-sm text-muted-foreground">
          Try switching themes using the button in the bottom-right corner!
        </p>
      </div>
    </div>
  </div>
</template>
