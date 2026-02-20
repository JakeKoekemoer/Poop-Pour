<script setup lang="ts">
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import { authService } from '@/services/AuthService/AuthService'
import { Button } from '@/components/ui/button'
import { LogOut } from 'lucide-vue-next'
</script>

<template>
  <div class="min-h-screen">
    <header class="border-b">
      <nav class="container mx-auto px-4 py-4">
        <div class="flex items-center justify-between">
          <h1 class="text-xl font-bold">Poop N' Pour</h1>
          <div class="flex items-center gap-4">
            <RouterLink
              :to="{ name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DASHBOARD) }"
              class="hover:text-primary transition-colors"
            >
              Dashboard
            </RouterLink>
            <RouterLink
              :to="{ name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.PROFILE) }"
              class="hover:text-primary transition-colors"
            >
              Profile
            </RouterLink>
            <Button variant="ghost" size="sm" @click="authService.logout()">
              <LogOut class="w-4 h-4 mr-2" />
              Logout
            </Button>
          </div>
        </div>
      </nav>
    </header>
    <main class="container mx-auto px-4 py-8">
      <RouterView />
    </main>
  </div>
</template>
