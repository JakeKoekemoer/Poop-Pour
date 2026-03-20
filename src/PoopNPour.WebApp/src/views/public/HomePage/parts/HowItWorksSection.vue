<script setup lang="ts">
const steps = [
  {
    number: '01',
    emoji: '🍼',
    title: 'Log a Feed',
    description: 'Quick tap to record feeding time and amount. Takes less than 3 seconds.',
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
    description: 'Clear timeline and summaries help you understand your baby\'s routine.',
  },
]
</script>

<template>
  <section id="how-it-works" class="py-12 md:py-16 lg:py-20 bg-muted/20">
    <div class="container mx-auto px-6 md:px-8 lg:px-12">
      <div class="text-center mb-12 md:mb-16">
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
          <div class="text-6xl md:text-7xl font-bold text-muted-foreground/10 leading-none">
            {{ step.number }}
          </div>

          <!-- Emoji Icon -->
          <div class="text-6xl md:text-7xl">
            {{ step.emoji }}
          </div>

          <!-- Title -->
          <h3 class="text-2xl md:text-3xl font-bold">
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
