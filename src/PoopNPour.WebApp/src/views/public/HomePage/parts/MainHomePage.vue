<script setup lang="ts">
import { Baby, Activity, Moon, Bell } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { PUBLIC_ROUTES } from '@/routes/constants'
</script>

<template>
  <div class="min-h-screen py-8 md:py-16 px-4">
    <div class="max-w-7xl mx-auto space-y-12 md:space-y-16">
      <!-- Hero Section -->
      <div class="text-center space-y-6">
        <div class="flex justify-center">
          <Baby class="w-16 h-16 md:w-24 md:h-24 text-primary" />
        </div>
        <div class="space-y-3">
          <Badge variant="secondary" class="mb-2">Baby Tracking App</Badge>
          <h1 class="text-4xl md:text-5xl lg:text-7xl font-bold text-foreground">
            Poop & Pour
          </h1>
        </div>
        <p class="text-lg md:text-xl lg:text-2xl text-muted-foreground max-w-3xl mx-auto px-4">
          Baby tracking made simple for tired parents 🌙
        </p>
        <div class="flex gap-4 justify-center pt-4">
          <Button size="lg" as-child>
            <RouterLink :to="{ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) }">
              Get Started
            </RouterLink>
          </Button>
        </div>
      </div>

      <Separator class="max-w-xs mx-auto" />

      <!-- Features Section -->
      <div class="grid sm:grid-cols-2 lg:grid-cols-3 gap-6 md:gap-8">
        <Card>
          <CardHeader>
            <div class="flex justify-center mb-4">
              <Activity class="w-10 h-10 md:w-12 md:h-12 text-primary" />
            </div>
            <CardTitle class="text-center text-lg md:text-xl">Track Activities</CardTitle>
            <CardDescription class="text-center">
              Monitor feeding, diaper changes, and sleep patterns all in one place.
            </CardDescription>
          </CardHeader>
        </Card>

        <Card>
          <CardHeader>
            <div class="flex justify-center mb-4">
              <Moon class="w-10 h-10 md:w-12 md:h-12 text-primary" />
            </div>
            <CardTitle class="text-center text-lg md:text-xl">Sleep Easy</CardTitle>
            <CardDescription class="text-center">
              Simple interface designed for sleep-deprived parents at 3 AM.
            </CardDescription>
          </CardHeader>
        </Card>

        <Card class="sm:col-span-2 lg:col-span-1">
          <CardHeader>
            <div class="flex justify-center mb-4">
              <Bell class="w-10 h-10 md:w-12 md:h-12 text-primary" />
            </div>
            <CardTitle class="text-center text-lg md:text-xl">Smart Reminders</CardTitle>
            <CardDescription class="text-center">
              Get gentle reminders for feeding times and important tasks.
            </CardDescription>
          </CardHeader>
        </Card>
      </div>
    </div>
  </div>
</template>
