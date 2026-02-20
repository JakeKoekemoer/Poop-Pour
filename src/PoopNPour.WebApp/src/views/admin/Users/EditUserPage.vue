<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { UserPen, ChevronLeft } from 'lucide-vue-next'

import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Button } from '@/components/ui/button'
import { userService } from '@/services/UserService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'

import EditUserForm, { type EditUserFormValues } from '@/components/views/admin/Users/EditUserForm.vue'
import UserClaimsCard from '@/components/views/admin/Users/UserClaimsCard.vue'
import UserRolesCard from '@/components/views/admin/Users/UserRolesCard.vue'

const router = useRouter()
const route = useRoute()
const toast = useAppToast()

const userId = route.params.id as string

const isLoading = ref(true)
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)
const initialValues = ref<EditUserFormValues | undefined>(undefined)
const userName = ref<string>('')
const initialRoles = ref<string[]>([])

onMounted(async () => {
  const response = await userService.getUserById(userId)
  if (response.success && response.data) {
    const user = response.data
    userName.value = user.userName
    initialRoles.value = user.roles
    initialValues.value = {
      email: user.email,
      firstName: user.firstName,
      lastName: user.lastName,
    }
  } else {
    toast.error('User not found', 'Could not load the user. Redirecting back.')
    await router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.USERS) })
  }
  isLoading.value = false
})

async function handleSubmit(values: EditUserFormValues) {
  isSubmitting.value = true
  errorMessage.value = null

  try {
    const response = await userService.updateUser(
      userId,
      values.firstName,
      values.lastName,
      values.email,
    )

    if (response.success) {
      toast.success('User updated!', `Changes for ${userName.value} have been saved.`)
      await router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.USERS) })
    } else {
      const message = response.error?.message || 'Failed to update user. Please try again.'
      errorMessage.value = message
      toast.error('Update failed', message)
    }
  } catch (error) {
    const message = 'An unexpected error occurred. Please try again.'
    errorMessage.value = message
    toast.error('Something went wrong', message)
    console.error('Edit user error:', error)
  } finally {
    isSubmitting.value = false
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
      <UserPen class="w-8 h-8 text-primary" />
      <div>
        <h1 class="text-3xl font-bold">Edit User</h1>
        <p class="text-muted-foreground">
          {{ userName ? `Editing ${userName}` : 'Loading user...' }}
        </p>
      </div>
    </div>

    <Tabs default-value="profile">
      <TabsList>
        <TabsTrigger value="profile">Profile</TabsTrigger>
        <TabsTrigger value="roles">Roles</TabsTrigger>
        <TabsTrigger value="claims">Claims</TabsTrigger>
      </TabsList>

      <TabsContent value="profile" class="max-w-lg mt-6">
        <EditUserForm
          v-if="!isLoading"
          :is-submitting="isSubmitting"
          :error-message="errorMessage"
          :initial-values="initialValues"
          @submit="handleSubmit"
          @cancel="handleCancel"
        />
        <div v-else class="space-y-4">
          <div class="h-10 bg-muted animate-pulse rounded-md" />
          <div class="h-10 bg-muted animate-pulse rounded-md" />
          <div class="h-10 bg-muted animate-pulse rounded-md" />
        </div>
      </TabsContent>

      <TabsContent value="roles" class="max-w-2xl mt-6">
        <UserRolesCard v-if="!isLoading" :user-id="userId" :initial-roles="initialRoles" />
      </TabsContent>

      <TabsContent value="claims" class="max-w-2xl mt-6">
        <UserClaimsCard v-if="!isLoading" :user-id="userId" />
      </TabsContent>
    </Tabs>
  </div>
</template>
