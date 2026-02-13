<script setup lang="ts">
import { Card, CardContent } from '@/components/ui/card'
import { Zap, Lock, Moon } from 'lucide-vue-next'

const smallFeatures = [
  {
    icon: Zap,
    title: 'Lightning Fast',
    description: 'Log feeding, diaper, or sleep in under 3 seconds. No forms, no friction.',
  },
  {
    icon: Lock,
    title: 'Private & Secure',
    description: 'Your data is encrypted and never shared. What happens in the app stays in the app.',
  },
  {
    icon: Moon,
    title: 'Dark Mode Ready',
    description: 'Easy on the eyes during late-night feeds. Switch themes with one tap.',
  },
]
</script>

<template>
  <section class="py-16 md:py-24 bg-gradient-to-b from-card to-background/50">
    <div class="max-w-7xl mx-auto px-4 sm:px-6">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <Card class="md:row-span-2 bg-gradient-to-br from-accent to-primary relative overflow-hidden">
          <CardContent class="p-8 lg:p-12 text-accent-foreground relative z-10 h-full flex flex-col justify-center">
            <div class="max-w-xl">
              <h3 class="text-3xl lg:text-4xl font-bold mb-4">Built for 3AM</h3>
              <p class="text-lg lg:text-xl text-accent-foreground/90 mb-8 leading-relaxed">
                Large buttons, clear text, and zero thinking required. Because parenting is hard enough.
              </p>
            </div>
            
            <div class="absolute bottom-0 right-0 w-64 h-64 opacity-20">
              <div class="text-9xl">📱</div>
            </div>
          </CardContent>
        </Card>

        <Card
          v-for="feature in smallFeatures"
          :key="feature.title"
          class="bg-card hover:shadow-xl transition-all"
        >
          <CardContent class="p-8">
            <div class="w-16 h-16 bg-gradient-to-br from-secondary to-primary rounded-2xl flex items-center justify-center mb-6 shadow-md">
              <component :is="feature.icon" class="w-8 h-8 text-primary-foreground" />
            </div>
            <h3 class="text-xl sm:text-2xl font-bold text-foreground mb-3">
              {{ feature.title }}
            </h3>
            <p class="text-muted-foreground leading-relaxed">
              {{ feature.description }}
            </p>
          </CardContent>
        </Card>
      </div>
    </div>
  </section>
</template>
