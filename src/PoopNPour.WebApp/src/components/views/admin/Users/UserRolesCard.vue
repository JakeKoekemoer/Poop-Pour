<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Alert, AlertDescription } from '@/components/ui/alert'
import { Switch } from '@/components/ui/switch'
import { Label } from '@/components/ui/label'
import { useAppToast } from '@/composables/useAppToast'
import { userService } from '@/services/UserService'
import { formatApiError } from '@/utils/formatApiError'

const props = defineProps<{
  userId: string
  initialRoles: string[]
}>()

const toast = useAppToast()

const allRoles = ref<string[]>([])
const isLoadingRoles = ref(false)
const rolesError = ref<string | null>(null)

const userRoles = reactive(new Set<string>(props.initialRoles))
const toggling = reactive(new Map<string, boolean>())

onMounted(async () => {
  isLoadingRoles.value = true
  rolesError.value = null

  const response = await userService.getRoles()
  if (response.success && response.data) {
    allRoles.value = response.data
  } else {
    rolesError.value = response.error
      ? formatApiError(response.error)
      : 'Failed to load roles. Please try again.'
  }

  isLoadingRoles.value = false
})

async function handleToggle(role: string, checked: boolean) {
  toggling.set(role, true)

  if (checked) {
    const response = await userService.addUserRole(props.userId, role)
    if (response.success && response.data) {
      response.data.forEach((r) => userRoles.add(r))
      toast.success('Role added', `"${role}" has been assigned.`)
    } else {
      toast.error('Failed to add role', response.error ? formatApiError(response.error) : 'Please try again.')
    }
  } else {
    const response = await userService.removeUserRole(props.userId, role)
    if (response.success && response.data) {
      userRoles.clear()
      response.data.forEach((r) => userRoles.add(r))
      toast.success('Role removed', `"${role}" has been removed.`)
    } else {
      toast.error('Failed to remove role', response.error ? formatApiError(response.error) : 'Please try again.')
    }
  }

  toggling.set(role, false)
}
</script>

<template>
  <Card>
    <CardHeader>
      <CardTitle>User Roles</CardTitle>
      <CardDescription>Toggle roles assigned to this user account</CardDescription>
    </CardHeader>
    <CardContent class="space-y-4">
      <div v-if="isLoadingRoles" class="space-y-3">
        <div v-for="n in 3" :key="n" class="flex items-center justify-between">
          <div class="h-4 w-24 bg-muted animate-pulse rounded" />
          <div class="h-5 w-8 bg-muted animate-pulse rounded-full" />
        </div>
      </div>

      <Alert v-else-if="rolesError" variant="destructive">
        <AlertDescription>{{ rolesError }}</AlertDescription>
      </Alert>

      <p v-else-if="allRoles.length === 0" class="text-sm text-muted-foreground">
        No roles are configured. Please contact an administrator.
      </p>

      <div v-else class="space-y-3">
        <div
          v-for="role in allRoles"
          :key="role"
          class="flex items-center justify-between"
        >
          <Label :for="`role-${role}`" class="text-sm font-medium cursor-pointer">
            {{ role }}
          </Label>
          <Switch
            :id="`role-${role}`"
            :model-value="userRoles.has(role)"
            :disabled="toggling.get(role) === true"
            @update:model-value="handleToggle(role, $event)"
          />
        </div>
      </div>
    </CardContent>
  </Card>
</template>
