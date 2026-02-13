<script setup lang="ts">
import { User, Mail, Phone } from 'lucide-vue-next'
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
        <User class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Profile</h1>
      </div>
      <Badge variant="secondary">Secure Area</Badge>
    </div>

    <Separator />

    <div class="grid gap-6 md:grid-cols-2">
      <Card>
        <CardHeader>
          <CardTitle>Personal Information</CardTitle>
          <CardDescription>
            Manage your account details and preferences
          </CardDescription>
        </CardHeader>
        <CardContent class="space-y-4">
          <div class="space-y-2">
            <Label for="name">
              <div class="flex items-center gap-2">
                <User class="w-4 h-4" />
                Full Name
              </div>
            </Label>
            <Input id="name" placeholder="John Doe" disabled />
          </div>
          
          <div class="space-y-2">
            <Label for="email">
              <div class="flex items-center gap-2">
                <Mail class="w-4 h-4" />
                Email
              </div>
            </Label>
            <Input id="email" type="email" placeholder="john@example.com" disabled />
          </div>
          
          <div class="space-y-2">
            <Label for="phone">
              <div class="flex items-center gap-2">
                <Phone class="w-4 h-4" />
                Phone
              </div>
            </Label>
            <Input id="phone" type="tel" placeholder="+1 (555) 000-0000" disabled />
          </div>
          
          <Separator />
          
          <Button class="w-full" disabled>
            Save Changes
          </Button>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Account Settings</CardTitle>
          <CardDescription>
            Configure your account preferences
          </CardDescription>
        </CardHeader>
        <CardContent class="space-y-4">
          <div class="flex items-center justify-between">
            <div class="space-y-0.5">
              <Label>Email Notifications</Label>
              <p class="text-sm text-muted-foreground">
                Receive updates via email
              </p>
            </div>
            <Badge variant="outline">Coming Soon</Badge>
          </div>
          
          <Separator />
          
          <div class="flex items-center justify-between">
            <div class="space-y-0.5">
              <Label>Push Notifications</Label>
              <p class="text-sm text-muted-foreground">
                Get mobile notifications
              </p>
            </div>
            <Badge variant="outline">Coming Soon</Badge>
          </div>
          
          <Separator />
          
          <div class="flex items-center justify-between">
            <div class="space-y-0.5">
              <Label>Two-Factor Authentication</Label>
              <p class="text-sm text-muted-foreground">
                Add extra security
              </p>
            </div>
            <Badge variant="outline">Coming Soon</Badge>
          </div>
        </CardContent>
      </Card>
    </div>
  </div>
</template>
