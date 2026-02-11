<script setup lang="ts">
import { ShieldAlert } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { PUBLIC_ROUTES } from '@/routes/constants'
</script>

<template>
  <div class="min-h-screen flex items-center justify-center p-4">
    <div class="w-full max-w-md space-y-8 text-center">
      <div class="space-y-6">
        <div class="flex justify-center">
          <ShieldAlert class="w-24 h-24 text-destructive" />
        </div>
        <div class="space-y-2">
          <h1 class="text-5xl md:text-6xl font-bold">Unauthorized</h1>
          <p class="text-muted-foreground">
            You don't have permission to access this page
          </p>
        </div>
        <div class="bg-destructive/10 border border-destructive/20 rounded-lg p-6">
          <p class="text-sm text-muted-foreground">
            Please contact an administrator if you believe this is an error.
          </p>
        </div>
      </div>

      <Button size="lg" class="w-full sm:w-auto" as-child>
        <RouterLink :to="{ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) }">
          Return Home
        </RouterLink>
      </Button>
    </div>
  </div>
</template>
