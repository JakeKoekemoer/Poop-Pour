<script setup lang="ts">
import { Sparkles } from 'lucide-vue-next'
</script>

<template>
  <div class="min-h-[calc(100vh-8rem)] flex items-center justify-center">
    <div class="text-center space-y-6 max-w-md mx-auto px-4">
      <div class="flex justify-center">
        <Sparkles class="w-16 h-16 text-primary animate-pulse" />
      </div>
      <h1 class="text-5xl font-bold">Coming Soon</h1>
      <p class="text-xl text-muted-foreground">
        We're working on something amazing. Stay tuned!
      </p>
    </div>
  </div>
</template>
