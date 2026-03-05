<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { UserCheck, ChevronLeft } from 'lucide-vue-next'

import { Separator } from '@/components/ui/separator'
import { Button } from '@/components/ui/button'
import { familyUserService } from '@/services/FamilyUserService'
import { useFamilyContextStore } from '@/stores'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'

import AddUserToFamilyForm, { type AddUserToFamilyFormValues } from '@/components/views/admin/FamilyUsers/AddUserToFamilyForm.vue'

const router = useRouter()
const toast = useAppToast()
const familyContextStore = useFamilyContextStore()

const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)

async function handleSubmit(values: AddUserToFamilyFormValues) {
  const familyId = familyContextStore.familyId
  if (!familyId) return

  isSubmitting.value = true
  errorMessage.value = null

  try {
    const response = await familyUserService.addUserToFamily(familyId, values.email)

    if (response.success) {
      toast.success('Member added!', `${values.email} has been added to the family.`)
      await router.push({
        name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILY_USERS),
        params: { familyId },
      })
    } else {
      const message = response.error?.message || 'Failed to add member. Please try again.'
      errorMessage.value = message
      toast.error('Add member failed', message)
    }
  } catch (error) {
    const message = 'An unexpected error occurred. Please try again.'
    errorMessage.value = message
    toast.error('Something went wrong', message)
    console.error('Add family member error:', error)
  } finally {
    isSubmitting.value = false
  }
}

function handleCancel() {
  const familyId = familyContextStore.familyId
  router.push({
    name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILY_USERS),
    params: familyId ? { familyId } : {},
  })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="icon" @click="handleCancel">
        <ChevronLeft class="w-5 h-5" />
      </Button>
      <UserCheck class="w-8 h-8 text-primary" />
      <div>
        <h1 class="text-3xl font-bold">Add Member</h1>
        <p class="text-muted-foreground">Add a user to this family by their email address</p>
      </div>
    </div>

    <Separator />

    <div class="max-w-lg">
      <AddUserToFamilyForm
        :is-submitting="isSubmitting"
        :error-message="errorMessage"
        @submit="handleSubmit"
        @cancel="handleCancel"
      />
    </div>
  </div>
</template>
