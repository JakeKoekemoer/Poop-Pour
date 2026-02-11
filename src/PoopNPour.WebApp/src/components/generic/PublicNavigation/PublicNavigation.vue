<script setup lang="ts">
import { ref } from 'vue'
import { Menu, X } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Sheet, SheetContent, SheetTrigger } from '@/components/ui/sheet'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { PUBLIC_ROUTES } from '@/routes/constants'

const isOpen = ref(false)

const navigationLinks = [
  { name: 'Home', route: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) },
  { name: 'About', route: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) }, // TODO: Update when About page exists
]
</script>

<template>
  <nav class="fixed top-0 left-0 right-0 z-50 bg-card shadow-md transition-all duration-300">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 py-4 flex items-center justify-between">
      <!-- Logo -->
      <RouterLink 
        :to="{ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) }" 
        class="flex items-center gap-2 hover:opacity-80 transition-opacity"
        aria-label="Poop & Pour Home"
      >
        <span class="text-2xl sm:text-3xl" aria-hidden="true">🍼</span>
        <span class="text-xl sm:text-2xl font-bold text-foreground">Poop & Pour</span>
      </RouterLink>

      <!-- Desktop Navigation -->
      <div class="hidden md:flex items-center gap-8">
        <RouterLink
          v-for="link in navigationLinks"
          :key="link.name"
          :to="{ name: link.route }"
          class="text-foreground hover:text-primary transition-colors font-medium"
          :aria-label="`Navigate to ${link.name}`"
        >
          {{ link.name }}
        </RouterLink>
        <Button as-child>
          <RouterLink :to="{ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) }">
            Get Started
          </RouterLink>
        </Button>
      </div>

      <!-- Mobile Menu -->
      <Sheet v-model:open="isOpen">
        <SheetTrigger as-child>
          <Button variant="outline" size="icon" class="md:hidden">
            <Menu class="h-5 w-5" />
            <span class="sr-only">Open menu</span>
          </Button>
        </SheetTrigger>
        <SheetContent side="right" class="w-[300px]">
          <div class="flex flex-col gap-6 mt-8">
            <RouterLink
              v-for="link in navigationLinks"
              :key="link.name"
              :to="{ name: link.route }"
              class="text-lg font-medium text-foreground hover:text-primary transition-colors"
              @click="isOpen = false"
            >
              {{ link.name }}
            </RouterLink>
            <Button as-child class="w-full">
              <RouterLink :to="{ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) }">
                Get Started
              </RouterLink>
            </Button>
          </div>
        </SheetContent>
      </Sheet>
    </div>
  </nav>
</template>
