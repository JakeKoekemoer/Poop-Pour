<script setup lang="ts">
import { Settings, Database, Shield, Bell, Palette } from 'lucide-vue-next'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { Label } from '@/components/ui/label'
import { Input } from '@/components/ui/input'
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <Settings class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">System Settings</h1>
      </div>
      <Badge variant="destructive">Admin Area</Badge>
    </div>

    <Separator />

    <div class="grid gap-6 md:grid-cols-2">
      <Card>
        <CardHeader>
          <div class="flex items-center gap-2">
            <Database class="w-5 h-5 text-primary" />
            <CardTitle>Database Settings</CardTitle>
          </div>
          <CardDescription>
            Configure database connections and backups
          </CardDescription>
        </CardHeader>
        <CardContent class="space-y-4">
          <div class="space-y-2">
            <Label>Database Status</Label>
            <div class="flex items-center gap-2">
              <Badge variant="secondary">Connected</Badge>
            </div>
          </div>
          <Separator />
          <div class="space-y-2">
            <Label>Last Backup</Label>
            <p class="text-sm text-muted-foreground">Never</p>
          </div>
          <Button class="w-full" variant="outline" disabled>
            Configure Database
          </Button>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <div class="flex items-center gap-2">
            <Shield class="w-5 h-5 text-primary" />
            <CardTitle>Security Settings</CardTitle>
          </div>
          <CardDescription>
            Manage authentication and authorization
          </CardDescription>
        </CardHeader>
        <CardContent class="space-y-4">
          <div class="flex items-center justify-between">
            <div class="space-y-0.5">
              <Label>Two-Factor Auth</Label>
              <p class="text-sm text-muted-foreground">
                Require 2FA for all users
              </p>
            </div>
            <Badge variant="outline">Coming Soon</Badge>
          </div>
          <Separator />
          <div class="flex items-center justify-between">
            <div class="space-y-0.5">
              <Label>Session Timeout</Label>
              <p class="text-sm text-muted-foreground">
                Auto-logout after inactivity
              </p>
            </div>
            <Badge variant="outline">Coming Soon</Badge>
          </div>
          <Button class="w-full" variant="outline" disabled>
            Configure Security
          </Button>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <div class="flex items-center gap-2">
            <Bell class="w-5 h-5 text-primary" />
            <CardTitle>Notification Settings</CardTitle>
          </div>
          <CardDescription>
            Configure system notifications
          </CardDescription>
        </CardHeader>
        <CardContent class="space-y-4">
          <div class="space-y-2">
            <Label for="email">Admin Email</Label>
            <Input
              id="email"
              type="email"
              placeholder="admin@example.com"
              disabled
            />
          </div>
          <Separator />
          <div class="flex items-center justify-between">
            <div class="space-y-0.5">
              <Label>Email Alerts</Label>
              <p class="text-sm text-muted-foreground">
                System error notifications
              </p>
            </div>
            <Badge variant="outline">Coming Soon</Badge>
          </div>
          <Button class="w-full" variant="outline" disabled>
            Configure Notifications
          </Button>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <div class="flex items-center gap-2">
            <Palette class="w-5 h-5 text-primary" />
            <CardTitle>Appearance Settings</CardTitle>
          </div>
          <CardDescription>
            Customize system appearance
          </CardDescription>
        </CardHeader>
        <CardContent class="space-y-4">
          <div class="space-y-2">
            <Label>Theme</Label>
            <p class="text-sm text-muted-foreground">
              Default theme for all users
            </p>
          </div>
          <Separator />
          <div class="space-y-2">
            <Label>Branding</Label>
            <p class="text-sm text-muted-foreground">
              Custom logo and colors
            </p>
          </div>
          <Button class="w-full" variant="outline" disabled>
            Configure Appearance
          </Button>
        </CardContent>
      </Card>
    </div>
  </div>
</template>
