<script setup lang="ts">
import { Baby, Droplet } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
</script>

<template>
  <section class="py-16 md:py-24 lg:py-32">
    <div class="container mx-auto px-4 max-w-4xl">
      <div class="text-center space-y-10">
        <!-- Decorative Icons at top -->
        <div class="flex items-center justify-center gap-4">
          <Baby class="w-10 h-10 md:w-12 md:h-12 text-primary" />
          <Droplet class="w-10 h-10 md:w-12 md:h-12 text-primary" />
        </div>

        <!-- Heading -->
        <h2 class="text-4xl md:text-5xl lg:text-6xl font-bold">
          Ready to simplify parenting?
        </h2>

        <!-- Subheading -->
        <p class="text-xl md:text-2xl text-muted-foreground">
          Join thousands of parents who've made tracking effortless
        </p>

        <!-- CTA Button -->
        <div class="flex justify-center pt-4">
          <Button size="lg" class="text-lg px-10 py-6">
            Start Tracking Free
          </Button>
        </div>
      </div>
    </div>
  </section>
</template>
