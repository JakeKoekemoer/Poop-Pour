<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { Pill, ChevronLeft } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { medicineLogService } from '@/services/MedicineLogService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'
import AddMedicineLogForm, { type AddMedicineLogFormValues } from '@/components/views/secure/MedicineLogs/AddMedicineLogForm.vue'

const router = useRouter()
const route = useRoute()
const toast = useAppToast()

const familyId = computed(() => route.params['familyId'] as string)
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)

async function handleSubmit(values: AddMedicineLogFormValues) {
  isSubmitting.value = true
  errorMessage.value = null

  try {
    const timeAdministered = values.timeAdministered
      ? new Date(values.timeAdministered)
      : undefined
    const notes = values.notes?.trim() ? [values.notes.trim()] : null

    const response = await medicineLogService.createMedicineLog({
      dependentId: values.dependentId,
      medicineName: values.medicineName,
      dosage: values.dosage || null,
      timeAdministered,
      notes,
    })

    if (response.success) {
      toast.success('Medicine log added', 'The medicine log has been recorded.')
      await router.push({
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.MEDICINE_LOGS),
        params: { familyId: familyId.value },
      })
    } else {
      const message = response.error?.message ?? 'Failed to add medicine log. Please try again.'
      errorMessage.value = message
      toast.error('Add failed', message)
    }
  } catch (error) {
    const message = 'An unexpected error occurred. Please try again.'
    errorMessage.value = message
    toast.error('Something went wrong', message)
    console.error('Add medicine log error:', error)
  } finally {
    isSubmitting.value = false
  }
}

function handleCancel() {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.MEDICINE_LOGS),
    params: { familyId: familyId.value },
  })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="icon" @click="handleCancel">
        <ChevronLeft class="w-5 h-5" />
      </Button>
      <Pill class="w-8 h-8 text-primary" />
      <div>
        <h1 class="text-3xl font-bold">Add Medicine Log</h1>
        <p class="text-muted-foreground">Record medicine administration for a dependent</p>
      </div>
    </div>

    <Separator />

    <div class="max-w-lg">
      <AddMedicineLogForm
        v-if="familyId"
        :family-id="familyId"
        :is-submitting="isSubmitting"
        :error-message="errorMessage"
        @submit="handleSubmit"
        @cancel="handleCancel"
      />
    </div>
  </div>
</template>
