<script setup lang="ts">
import { Button } from '@/components/ui/button'
import { Heading } from '@/components/ui_parts/Heading'
import { Subheading } from '@/components/ui_parts/Subheading'
</script>

<template>
  <section class="py-12 md:py-16 lg:py-20">
    <div class="container mx-auto px-6 md:px-8 lg:px-12 max-w-4xl">
      <div class="text-center space-y-8">
        <div class="flex items-center justify-center text-5xl md:text-6xl">
          🍼💩
        </div>

        <Heading>Ready to simplify parenting?</Heading>
        <Subheading>Join thousands of parents who've made tracking effortless</Subheading>

        <div class="flex justify-center pt-4">
          <Button size="lg" class="text-lg px-10 py-6">Start Tracking Free</Button>
        </div>
      </div>
    </div>
  </section>
</template>
