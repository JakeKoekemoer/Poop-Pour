<script setup lang="ts">
import { computed } from 'vue'
import { useRouter, RouterLink, RouterView } from 'vue-router'
import { Baby, Menu } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'

const router = useRouter()

const navLinks = computed(() => {
  return router.getRoutes()
    .filter((route) => route.meta?.showInNavbar === true)
    .map((route) => ({
      name: route.meta?.navbarLabel as string || route.name as string,
      to: route.path,
    }))
})
</script>

<template>
  <div class="min-h-screen bg-background">
    <!-- Navigation Bar -->
    <nav class="border-b border-border/40 bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60">
      <div class="container mx-auto px-4 max-w-7xl">
        <div class="flex items-center justify-between h-16 md:h-20">
          <!-- Logo -->
          <div class="flex items-center gap-2">
            <div class="p-2 rounded-lg bg-primary/10">
              <Baby class="w-6 h-6 md:w-7 md:h-7 text-primary" />
            </div>
            <span class="text-xl md:text-2xl font-bold text-foreground">Poop & Pour</span>
          </div>

          <!-- Navigation Links -->
          <div class="hidden md:flex items-center gap-8">
            <RouterLink
              v-for="link in navLinks"
              :key="link.name"
              :to="link.to"
              class="text-base font-medium text-foreground hover:text-primary transition-colors"
            >
              {{ link.name }}
            </RouterLink>
          </div>

          <!-- CTA Button -->
          <Button class="hidden sm:inline-flex">
            Get Started
          </Button>

          <!-- Mobile Menu Button -->
          <Button variant="ghost" size="icon" class="sm:hidden">
            <Menu class="h-6 w-6" />
          </Button>
        </div>
      </div>
    </nav>

    <!-- Main Content -->
    <main>
      <RouterView />
    </main>
  </div>
</template>
