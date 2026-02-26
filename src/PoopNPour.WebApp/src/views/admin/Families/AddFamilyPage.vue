<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { Home, ChevronLeft } from 'lucide-vue-next'

import { Separator } from '@/components/ui/separator'
import { Button } from '@/components/ui/button'
import { familyService } from '@/services/FamilyService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'

import AddFamilyForm, { type AddFamilyFormValues } from '@/components/views/admin/Families/AddFamilyForm.vue'

const router = useRouter()
const toast = useAppToast()
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)

async function handleSubmit(values: AddFamilyFormValues) {
  isSubmitting.value = true
  errorMessage.value = null

  try {
    const response = await familyService.createFamily({
      familyName: values.familyName,
      familyLastName: values.familyLastName,
    })

    if (response.success) {
      toast.success('Family created!', `The ${values.familyName} family has been created.`)
      await router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILIES) })
    } else {
      const message = response.error?.message || 'Failed to create family. Please try again.'
      errorMessage.value = message
      toast.error('Family creation failed', message)
    }
  } catch (error) {
    const message = 'An unexpected error occurred. Please try again.'
    errorMessage.value = message
    toast.error('Something went wrong', message)
    console.error('Add family error:', error)
  } finally {
    isSubmitting.value = false
  }
}

function handleCancel() {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILIES) })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="icon" @click="handleCancel">
        <ChevronLeft class="w-5 h-5" />
      </Button>
      <Home class="w-8 h-8 text-primary" />
      <div>
        <h1 class="text-3xl font-bold">Add Family</h1>
        <p class="text-muted-foreground">Create a new family group</p>
      </div>
    </div>

    <Separator />

    <div class="max-w-lg">
      <AddFamilyForm
        :is-submitting="isSubmitting"
        :error-message="errorMessage"
        @submit="handleSubmit"
        @cancel="handleCancel"
      />
    </div>
  </div>
</template>
