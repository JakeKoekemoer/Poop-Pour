<script setup lang="ts">
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import { authService } from '@/services/AuthService/AuthService'
import { Button } from '@/components/ui/button'
import { LogOut } from 'lucide-vue-next'
import { adminNavItems } from '@/config/adminNav'
</script>

<template>
  <div class="min-h-screen flex flex-col">
    <header class="border-b bg-muted shrink-0">
      <nav class="px-4 py-4">
        <div class="flex items-center justify-between">
          <h1 class="text-xl font-bold">Poop N' Pour - Admin</h1>
          <div class="flex items-center gap-4">
            <RouterLink
              :to="{ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DASHBOARD) }"
              class="hover:text-primary transition-colors"
            >
              Dashboard
            </RouterLink>
            <Button variant="ghost" size="sm" @click="authService.logout()">
              <LogOut class="w-4 h-4 mr-2" />
              Logout
            </Button>
          </div>
        </div>
      </nav>
    </header>

    <div class="flex flex-1 overflow-hidden">
      <aside class="w-56 shrink-0 border-r bg-muted/40 flex flex-col gap-1 p-3 overflow-y-auto">
        <RouterLink
          v-for="item in adminNavItems"
          :key="item.routeName"
          :to="{ name: item.routeName }"
          class="flex items-center gap-3 rounded-md px-3 py-2 text-sm font-medium transition-colors hover:bg-accent hover:text-accent-foreground"
          active-class="bg-accent text-accent-foreground"
        >
          <component :is="item.icon" class="w-4 h-4 shrink-0" />
          {{ item.label }}
        </RouterLink>
      </aside>

      <main class="flex-1 overflow-y-auto p-8">
        <RouterView />
      </main>
    </div>
  </div>
</template>
