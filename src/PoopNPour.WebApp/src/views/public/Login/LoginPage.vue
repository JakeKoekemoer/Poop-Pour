<script setup lang="ts">
import { LogIn } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '@/components/ui/card'
import { Label } from '@/components/ui/label'
import { Input } from '@/components/ui/input'
import { Separator } from '@/components/ui/separator'
</script>

<template>
  <div class="min-h-[calc(100vh-8rem)] flex items-center justify-center p-4">
    <Card class="w-full max-w-md">
      <CardHeader class="space-y-1">
        <div class="flex justify-center mb-4">
          <LogIn class="w-12 h-12 text-primary" />
        </div>
        <CardTitle class="text-3xl font-bold text-center">Login</CardTitle>
        <CardDescription class="text-center">
          Sign in to your account to continue
        </CardDescription>
      </CardHeader>
      <CardContent class="space-y-4">
        <div class="space-y-2">
          <Label for="email">Email</Label>
          <Input
            id="email"
            type="email"
            placeholder="name@example.com"
            disabled
          />
        </div>
        <div class="space-y-2">
          <Label for="password">Password</Label>
          <Input
            id="password"
            type="password"
            placeholder="••••••••"
            disabled
          />
        </div>
        
        <Separator />
        
        <div class="text-center">
          <p class="text-sm text-muted-foreground">
            Authentication functionality coming soon...
          </p>
        </div>
      </CardContent>
      <CardFooter class="flex flex-col gap-4">
        <Button class="w-full" disabled>
          Sign In
        </Button>
        <div class="text-sm text-center text-muted-foreground">
          Don't have an account?
          <Button variant="link" class="px-1" disabled>
            Sign up
          </Button>
        </div>
      </CardFooter>
    </Card>
  </div>
</template>
