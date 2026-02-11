<script setup lang="ts">
import { RouterView } from 'vue-router'
import { Navbar } from '@/components/ui_parts/Navbar'
</script>

<template>
  <div class="min-h-screen bg-background relative">
    <!-- Gradient Overlay -->
    <div class="absolute inset-0 bg-gradient-to-br from-background via-background/95 to-background/50 pointer-events-none"></div>
    
    <!-- Navigation Bar -->
    <Navbar />

    <!-- Main Content -->
    <main class="relative">
      <RouterView />
    </main>
  </div>
</template>
