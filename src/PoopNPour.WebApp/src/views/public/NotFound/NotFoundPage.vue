<script setup lang="ts">
import { FileQuestion, Home, ArrowLeft } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { PUBLIC_ROUTES } from '@/routes/constants'
</script>

<template>
  <div class="min-h-screen flex items-center justify-center p-4">
    <div class="w-full max-w-md space-y-8 text-center">
      <div class="space-y-6">
        <div class="flex justify-center">
          <FileQuestion class="w-24 h-24 text-muted-foreground" />
        </div>
        <div class="space-y-2">
          <h1 class="text-8xl md:text-9xl font-bold">404</h1>
          <p class="text-xl text-muted-foreground">Page Not Found</p>
        </div>
        <p class="text-sm text-muted-foreground max-w-sm mx-auto">
          The page you're looking for doesn't exist or has been moved.
        </p>
      </div>

      <div class="flex flex-col sm:flex-row gap-4 justify-center">
        <Button size="lg" as-child>
          <RouterLink :to="{ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) }">
            <Home class="mr-2 h-4 w-4" />
            Go Home
          </RouterLink>
        </Button>
        <Button size="lg" variant="outline" @click="$router.back()">
          <ArrowLeft class="mr-2 h-4 w-4" />
          Go Back
        </Button>
      </div>
    </div>
  </div>
</template>
