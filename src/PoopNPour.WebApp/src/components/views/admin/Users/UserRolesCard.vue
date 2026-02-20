<script setup lang="ts">
import { ref } from 'vue'
import { X, Plus } from 'lucide-vue-next'

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { useAppToast } from '@/composables/useAppToast'
import { userService } from '@/services/UserService'
import { formatApiError } from '@/utils/formatApiError'

const props = defineProps<{
  userId: string
  initialRoles: string[]
}>()

const toast = useAppToast()

const roles = ref<string[]>([...props.initialRoles])
const removingRole = ref<string | null>(null)
const isAdding = ref(false)
const newRole = ref('')

async function handleRemove(role: string) {
  removingRole.value = role

  const response = await userService.removeUserRole(props.userId, role)
  if (response.success && response.data) {
    roles.value = response.data
    toast.success('Role removed', `"${role}" has been removed.`)
  } else {
    toast.error('Failed to remove role', response.error ? formatApiError(response.error) : 'Please try again.')
  }

  removingRole.value = null
}

async function handleAdd() {
  const role = newRole.value.trim()
  if (!role) return

  isAdding.value = true

  const response = await userService.addUserRole(props.userId, role)
  if (response.success && response.data) {
    roles.value = response.data
    newRole.value = ''
    toast.success('Role added', `"${role}" has been added.`)
  } else {
    toast.error('Failed to add role', response.error ? formatApiError(response.error) : 'Please try again.')
  }

  isAdding.value = false
}
</script>

<template>
  <Card>
    <CardHeader>
      <CardTitle>User Roles</CardTitle>
      <CardDescription>Manage roles assigned to this user account</CardDescription>
    </CardHeader>
    <CardContent class="space-y-4">
      <div v-if="roles.length > 0" class="flex flex-wrap gap-2">
        <div
          v-for="role in roles"
          :key="role"
          class="flex items-center gap-1"
        >
          <Badge>{{ role }}</Badge>
          <Button
            size="icon"
            variant="ghost"
            class="h-5 w-5 text-muted-foreground hover:text-destructive"
            :disabled="removingRole === role"
            @click="handleRemove(role)"
          >
            <X class="h-3 w-3" />
          </Button>
        </div>
      </div>
      <p v-else class="text-sm text-muted-foreground">No roles assigned to this user.</p>

      <div class="flex gap-2">
        <Input
          v-model="newRole"
          placeholder="e.g. Admin"
          :disabled="isAdding"
          class="max-w-xs"
          @keyup.enter="handleAdd"
        />
        <Button
          variant="outline"
          size="sm"
          :disabled="isAdding || !newRole.trim()"
          @click="handleAdd"
        >
          <Plus class="h-4 w-4 mr-1" />
          Add Role
        </Button>
      </div>
    </CardContent>
  </Card>
</template>
