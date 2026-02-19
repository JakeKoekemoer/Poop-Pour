<script setup lang="ts">
import { Card, CardContent } from '@/components/ui/card'

const steps = [
  {
    number: '01',
    emoji: '🍼',
    title: 'Log a Feed',
    description: 'Quick one-tap logging for breast, bottle, or solid feeding. Track time and amount. Takes less than 3 seconds.',
  },
  {
    number: '02',
    emoji: '💩',
    title: 'Log a Diaper',
    description: 'Simple buttons for wet, dirty, or both. No complicated forms.',
  },
  {
    number: '03',
    emoji: '📊',
    title: 'See Patterns',
    description: 'Beautiful charts and insights help you understand your baby\'s routine.',
  },
]
</script>

<template>
  <section id="how-it-works" class="py-16 md:py-24 bg-gradient-to-b from-background/50 to-card">
    <div class="max-w-7xl mx-auto px-4 sm:px-6">
      <div class="text-center mb-12 lg:mb-16">
        <h2 class="text-3xl sm:text-4xl lg:text-5xl font-bold text-accent mb-4">
          How It Works
        </h2>
        <p class="text-lg sm:text-xl text-muted-foreground max-w-2xl mx-auto">
          Three simple steps to start tracking
        </p>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 lg:gap-8">
        <Card
          v-for="step in steps"
          :key="step.number"
          class="relative bg-gradient-to-br from-background to-card hover:shadow-xl hover:-translate-y-2 transition-all duration-300 cursor-pointer border-border/50"
        >
          <div class="absolute top-6 right-6 w-12 h-12 bg-muted rounded-full flex items-center justify-center">
            <span class="text-lg font-bold text-muted-foreground">{{ step.number }}</span>
          </div>

          <CardContent class="p-8 pt-6">
            <div class="w-20 h-20 bg-gradient-to-br from-secondary to-primary rounded-2xl flex items-center justify-center text-4xl mb-6 shadow-md">
              {{ step.emoji }}
            </div>

            <h3 class="text-xl sm:text-2xl font-bold text-foreground mb-3">
              {{ step.title }}
            </h3>
            <p class="text-muted-foreground leading-relaxed">
              {{ step.description }}
            </p>
          </CardContent>
        </Card>
      </div>
    </div>
  </section>
</template>
