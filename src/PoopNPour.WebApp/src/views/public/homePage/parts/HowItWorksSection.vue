<script setup lang="ts">
import { Baby, Droplet, BarChart3 } from 'lucide-vue-next'

const steps = [
  {
    number: '01',
    icon: Baby,
    title: 'Log a Feed',
    description: 'Quick tap to record feeding time and amount. Takes less than 3 seconds.',
  },
  {
    number: '02',
    icon: Droplet,
    title: 'Log a Diaper',
    description: 'Simple buttons for wet, dirty, or both. No complicated forms.',
  },
  {
    number: '03',
    icon: BarChart3,
    title: 'See Patterns',
    description: 'Clear timeline and summaries help you understand your baby\'s routine.',
  },
]
</script>

<template>
  <section id="how-it-works" class="py-16 md:py-24 lg:py-32 bg-muted/20">
    <div class="container mx-auto px-4 max-w-7xl">
      <div class="text-center mb-16 md:mb-20">
        <h2 class="text-4xl md:text-5xl lg:text-6xl font-bold mb-4">
          How It Works
        </h2>
        <p class="text-xl md:text-2xl text-muted-foreground">
          Three simple steps to peace of mind
        </p>
      </div>

      <div class="grid md:grid-cols-3 gap-12 md:gap-8 lg:gap-12">
        <div
          v-for="step in steps"
          :key="step.number"
          class="flex flex-col items-center text-center space-y-6"
        >
          <!-- Step Number -->
          <div class="text-7xl md:text-8xl font-bold text-muted-foreground/15 mb-4 leading-none">
            {{ step.number }}
          </div>

          <!-- Icon -->
          <div class="p-5 rounded-3xl bg-primary/10 shadow-sm mb-4">
            <component :is="step.icon" class="w-14 h-14 text-primary" />
          </div>

          <!-- Title -->
          <h3 class="text-2xl md:text-3xl font-semibold mb-2">
            {{ step.title }}
          </h3>

          <!-- Description -->
          <p class="text-muted-foreground text-lg max-w-xs leading-relaxed">
            {{ step.description }}
          </p>
        </div>
      </div>
    </div>
  </section>
</template>
