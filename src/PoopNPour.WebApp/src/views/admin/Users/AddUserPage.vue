<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { UserPlus, ChevronLeft } from 'lucide-vue-next'

import { Separator } from '@/components/ui/separator'
import { Button } from '@/components/ui/button'
import { userService } from '@/services/UserService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'

import AddUserForm from '@/components/views/admin/Users/AddUserForm.vue'
import type { RegisterFormValues } from '@/components/views/public/Register/registerSchema'

const router = useRouter()
const toast = useAppToast()
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)

async function handleSubmit(values: RegisterFormValues) {
  isSubmitting.value = true
  errorMessage.value = null

  try {
    const response = await userService.createUser(
      values.email,
      values.userName,
      values.password,
      values.firstName,
      values.lastName,
    )

    if (response.success) {
      toast.success('User created!', `Account for ${values.userName} has been created.`)
      await router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.USERS) })
    } else {
      const message = response.error?.message || 'Failed to create user. Please try again.'
      errorMessage.value = message
      toast.error('User creation failed', message)
    }
  } catch (error) {
    const message = 'An unexpected error occurred. Please try again.'
    errorMessage.value = message
    toast.error('Something went wrong', message)
    console.error('Add user error:', error)
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
      <UserPlus class="w-8 h-8 text-primary" />
      <div>
        <h1 class="text-3xl font-bold">Add User</h1>
        <p class="text-muted-foreground">Create a new user account</p>
      </div>
    </div>

    <Separator />

    <div class="max-w-lg">
      <AddUserForm
        :is-submitting="isSubmitting"
        :error-message="errorMessage"
        @submit="handleSubmit"
        @cancel="handleCancel"
      />
    </div>
  </div>
</template>
