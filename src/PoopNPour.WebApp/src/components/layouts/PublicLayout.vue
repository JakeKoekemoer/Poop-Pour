<script setup lang="ts">
import { PublicNavigation } from '@/components/generic/PublicNavigation'
import { PublicFooter } from '@/components/generic/PublicFooter'
</script>

<template>
  <div class="min-h-screen flex flex-col">
    <PublicNavigation />
    <main class="flex-1 pt-[72px]">
      <RouterView />
    </main>
    <PublicFooter />
  </div>
</template>
