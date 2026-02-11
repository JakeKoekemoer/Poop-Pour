<script setup lang="ts">
import { computed } from 'vue'
import { useRouter, RouterLink } from 'vue-router'
import { Menu } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Logo, LOGO_STYLES } from '@/components/ui_parts/Logo'

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
  <nav class="border-b border-border/40 bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60">
    <div class="container mx-auto px-6 md:px-8 lg:px-12">
      <div class="flex items-center justify-between h-14 md:h-16">
        <!-- Logo -->
        <RouterLink to="/" class="flex items-center gap-3">
          <Logo :logoType="LOGO_STYLES.MINIMAL" class="w-8 h-8 md:w-10 md:h-10" />
          <span class="text-xl md:text-2xl font-bold text-foreground">Poop & Pour</span>
        </RouterLink>

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
</template>
