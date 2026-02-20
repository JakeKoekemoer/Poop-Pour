<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { Trash2, ChevronLeft, TriangleAlert } from 'lucide-vue-next'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Separator } from '@/components/ui/separator'
import { Badge } from '@/components/ui/badge'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Skeleton } from '@/components/ui/skeleton'
import { userService } from '@/services/UserService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'
import type { UserProfile } from '@/stores'

const router = useRouter()
const route = useRoute()
const toast = useAppToast()

const userId = route.params.id as string

const CONFIRM_PHRASE = 'I understand'

const isLoading = ref(true)
const isDeleting = ref(false)
const user = ref<UserProfile | null>(null)
const confirmationInput = ref('')

const isConfirmed = computed(() => confirmationInput.value === CONFIRM_PHRASE)

onMounted(async () => {
  const response = await userService.getUserById(userId)
  if (response.success && response.data) {
    user.value = response.data
  } else {
    toast.error('User not found', 'Could not load the user. Redirecting back.')
    await router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.USERS) })
  }
  isLoading.value = false
})

async function handleDelete() {
  isDeleting.value = true

  try {
    const response = await userService.deleteUser(userId)
    if (response.success) {
      toast.success('User deleted', `${user.value?.userName ?? 'User'} has been permanently deleted.`)
      await router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.USERS) })
    } else {
      toast.error('Delete failed', response.error?.message ?? 'Failed to delete user. Please try again.')
    }
  } catch {
    toast.error('Something went wrong', 'An unexpected error occurred. Please try again.')
  } finally {
    isDeleting.value = false
  }
}

function handleCancel() {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.USERS) })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="icon" @click="handleCancel">
        <ChevronLeft class="w-5 h-5" />
      </Button>
      <Trash2 class="w-8 h-8 text-destructive" />
      <div>
        <h1 class="text-3xl font-bold">Delete User</h1>
        <p class="text-muted-foreground">
          {{ user ? `Deleting ${user.userName}` : 'Loading user...' }}
        </p>
      </div>
    </div>

    <Separator />

    <div class="max-w-lg space-y-6">
      <template v-if="isLoading">
        <div class="space-y-3">
          <Skeleton class="h-5 w-48" />
          <Skeleton class="h-5 w-64" />
          <Skeleton class="h-5 w-40" />
        </div>
      </template>

      <template v-else-if="user">
        <div class="flex items-start gap-3 rounded-md border border-destructive/50 bg-destructive/10 p-4 text-destructive">
          <TriangleAlert class="h-5 w-5 mt-0.5 shrink-0" />
          <p class="text-sm">
            This action is <strong>permanent and cannot be undone</strong>. Please review the user
            details below before confirming.
          </p>
        </div>

        <Card>
          <CardHeader>
            <CardTitle>User Details</CardTitle>
            <CardDescription>Confirm this is the account you want to delete</CardDescription>
          </CardHeader>
          <CardContent class="space-y-3">
            <div class="grid grid-cols-3 gap-y-3 text-sm">
              <span class="text-muted-foreground font-medium">Username</span>
              <span class="col-span-2 font-mono">{{ user.userName }}</span>

              <span class="text-muted-foreground font-medium">Email</span>
              <span class="col-span-2">{{ user.email }}</span>

              <span class="text-muted-foreground font-medium">First Name</span>
              <span class="col-span-2">{{ user.firstName || '—' }}</span>

              <span class="text-muted-foreground font-medium">Last Name</span>
              <span class="col-span-2">{{ user.lastName || '—' }}</span>

              <span class="text-muted-foreground font-medium">Roles</span>
              <div class="col-span-2 flex flex-wrap gap-1">
                <Badge v-for="role in user.roles" :key="role" variant="secondary">{{ role }}</Badge>
                <span v-if="!user.roles.length" class="text-muted-foreground">None</span>
              </div>
            </div>
          </CardContent>
        </Card>

        <div class="space-y-2">
          <Label for="confirm-input">
            Type <strong>{{ CONFIRM_PHRASE }}</strong> to confirm
          </Label>
          <Input
            id="confirm-input"
            v-model="confirmationInput"
            :placeholder="CONFIRM_PHRASE"
            :disabled="isDeleting"
            @keyup.enter="isConfirmed && handleDelete()"
          />
        </div>

        <div class="flex gap-3">
          <Button variant="destructive" :disabled="isDeleting || !isConfirmed" @click="handleDelete">
            <Trash2 class="mr-2 h-4 w-4" />
            {{ isDeleting ? 'Deleting...' : 'Delete User' }}
          </Button>
          <Button variant="outline" :disabled="isDeleting" @click="handleCancel">
            Cancel
          </Button>
        </div>
      </template>
    </div>
  </div>
</template>
