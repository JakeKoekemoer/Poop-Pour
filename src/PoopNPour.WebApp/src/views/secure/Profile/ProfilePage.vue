<script setup lang="ts">
import { User } from 'lucide-vue-next'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'

import PersonalInfoForm from '@/components/views/secure/Profile/PersonalInfoForm.vue'
import AccountSettingsCard from '@/components/views/secure/Profile/AccountSettingsCard.vue'
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
      <PersonalInfoForm />
      <AccountSettingsCard />
    </div>
  </div>
</template>
