<script setup lang="ts">
import { Button } from '@/components/ui/button'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { PUBLIC_ROUTES } from '@/routes/constants'
</script>

<template>
  <section class="py-16 md:py-24 bg-gradient-to-r from-primary via-accent to-primary relative overflow-hidden">
    <!-- Decorative Elements -->
    <div class="absolute top-0 left-0 w-full h-full opacity-10">
      <div class="absolute top-10 left-10 text-9xl">💩</div>
      <div class="absolute bottom-10 right-10 text-9xl">🍼</div>
      <div class="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 text-9xl opacity-50">💩</div>
    </div>

    <!-- Content -->
    <div class="relative max-w-4xl mx-auto px-4 sm:px-6 text-center">
      <h2 class="text-4xl sm:text-5xl lg:text-6xl font-bold text-primary-foreground mb-6">
        Ready to simplify parenting?
      </h2>
      <p class="text-lg sm:text-xl text-primary-foreground/90 mb-10 max-w-2xl mx-auto">
        Join thousands of parents who trust Poop & Pour to track their baby's daily routine.
      </p>
      <Button 
        size="lg" 
        variant="secondary" 
        as-child 
        class="text-lg px-8 py-6 h-auto shadow-xl hover:shadow-2xl hover:scale-105 transition-all duration-300"
      >
        <RouterLink :to="{ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) }">
          Start Free Today
        </RouterLink>
      </Button>
    </div>
  </section>
</template>
