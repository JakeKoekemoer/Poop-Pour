<script setup lang="ts">
import { Button } from '@/components/ui/button'
import { Plus, Minus, RotateCcw, Sparkles } from 'lucide-vue-next'
import { useExampleStore } from '@/stores/example'

const store = useExampleStore()
</script>

<template>
  <div class="min-h-screen bg-background p-8">
    <div class="max-w-4xl mx-auto space-y-8">
      <!-- Header -->
      <header class="text-center space-y-4">
        <div class="flex items-center justify-center gap-2">
          <Sparkles class="w-8 h-8 text-primary" />
          <h1 class="text-4xl font-bold">PoopNPour WebApp</h1>
        </div>
        <p class="text-muted-foreground">
          Built with Vue 3, shadcn-vue, Lucide icons, and Pinia
        </p>
      </header>

      <!-- Main Content -->
      <main class="space-y-8">
        <!-- Card Section -->
        <div class="rounded-lg border bg-card p-6 shadow-sm">
          <h2 class="text-2xl font-semibold mb-4">Welcome to Your App</h2>
          <p class="text-muted-foreground mb-6">
            This is a demonstration of shadcn-vue components, Lucide icons, and Pinia state management.
          </p>

          <!-- Store Demo -->
          <div class="space-y-4">
            <div class="flex items-center gap-4">
              <span class="text-lg font-medium">Counter:</span>
              <span class="text-2xl font-bold">{{ store.count }}</span>
            </div>

            <div class="flex items-center gap-4">
              <span class="text-lg font-medium">Double Count:</span>
              <span class="text-2xl font-bold text-primary">{{ store.doubleCount }}</span>
            </div>

            <div class="flex items-center gap-4">
              <span class="text-lg font-medium">Name:</span>
              <span class="text-lg">{{ store.name }}</span>
            </div>

            <!-- Action Buttons -->
            <div class="flex flex-wrap gap-2 pt-4">
              <Button @click="store.increment()" variant="default">
                <Plus class="w-4 h-4 mr-2" />
                Increment
              </Button>
              <Button @click="store.decrement()" variant="secondary">
                <Minus class="w-4 h-4 mr-2" />
                Decrement
              </Button>
              <Button @click="store.reset()" variant="outline">
                <RotateCcw class="w-4 h-4 mr-2" />
                Reset
              </Button>
            </div>
          </div>
        </div>

        <!-- Info Section -->
        <div class="rounded-lg border bg-card p-6 shadow-sm">
          <h3 class="text-xl font-semibold mb-4">Setup Complete!</h3>
          <ul class="space-y-2 text-muted-foreground">
            <li>✅ shadcn-vue components configured</li>
            <li>✅ Lucide icons ready to use</li>
            <li>✅ Pinia store set up and working</li>
            <li>✅ Tailwind CSS configured</li>
            <li>✅ TypeScript paths configured</li>
          </ul>
        </div>
      </main>
    </div>
  </div>
</template>
