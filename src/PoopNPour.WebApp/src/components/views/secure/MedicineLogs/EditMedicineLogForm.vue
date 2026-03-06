<script setup lang="ts">
import { watch } from 'vue'
import { useForm } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import * as z from 'zod'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Textarea } from '@/components/ui/textarea'
import { Alert, AlertDescription } from '@/components/ui/alert'
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'

export interface EditMedicineLogFormValues {
  medicineName: string
  dosage: string
  timeAdministered: string
  notes: string
}

const editMedicineLogSchema = z.object({
  medicineName: z.string().min(1, 'Medicine name is required'),
  dosage: z.string().optional(),
  timeAdministered: z.string().min(1, 'Time administered is required'),
  notes: z.string().optional(),
})

const props = defineProps<{
  initialValues?: EditMedicineLogFormValues
  isSubmitting: boolean
  errorMessage: string | null
}>()

const emit = defineEmits<{
  submit: [values: EditMedicineLogFormValues]
  cancel: []
}>()

const form = useForm({
  validationSchema: toTypedSchema(editMedicineLogSchema),
})

watch(
  () => props.initialValues,
  (vals) => {
    if (vals) {
      form.setValues({
        medicineName: vals.medicineName,
        dosage: vals.dosage ?? '',
        timeAdministered: vals.timeAdministered,
        notes: vals.notes ?? '',
      })
    }
  },
  { immediate: true },
)

const onSubmit = form.handleSubmit((values) => {
  emit('submit', values as EditMedicineLogFormValues)
})
</script>

<template>
  <form @submit="onSubmit" class="space-y-4">
    <FormField v-slot="{ componentField }" name="medicineName">
      <FormItem>
        <FormLabel>Medicine Name</FormLabel>
        <FormControl>
          <Input
            type="text"
            placeholder="e.g. Paracetamol"
            v-bind="componentField"
            :disabled="props.isSubmitting"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="dosage">
      <FormItem>
        <FormLabel>Dosage (optional)</FormLabel>
        <FormControl>
          <Input
            type="text"
            placeholder="e.g. 5ml"
            v-bind="componentField"
            :disabled="props.isSubmitting"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="timeAdministered">
      <FormItem>
        <FormLabel>Time Administered</FormLabel>
        <FormControl>
          <Input
            type="datetime-local"
            v-bind="componentField"
            :disabled="props.isSubmitting"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="notes">
      <FormItem>
        <FormLabel>Notes (optional)</FormLabel>
        <FormControl>
          <Textarea
            v-bind="componentField"
            placeholder="Additional notes..."
            :disabled="props.isSubmitting"
            rows="3"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <Alert v-if="props.errorMessage" variant="destructive">
      <AlertDescription>
        {{ props.errorMessage }}
      </AlertDescription>
    </Alert>

    <div class="flex gap-3 pt-2">
      <Button type="submit" :disabled="props.isSubmitting">
        {{ props.isSubmitting ? 'Saving...' : 'Save Changes' }}
      </Button>
      <Button type="button" variant="outline" :disabled="props.isSubmitting" @click="emit('cancel')">
        Cancel
      </Button>
    </div>
  </form>
</template>
