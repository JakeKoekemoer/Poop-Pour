<script setup lang="ts">
import { ref, onMounted } from 'vue'
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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { dependentService } from '@/services/DependentService'
import type { DependentDto } from '@/api/api-client'

export interface AddMedicineLogFormValues {
  dependentId: string
  medicineName: string
  dosage: string
  timeAdministered: string
  notes: string
}

const addMedicineLogSchema = z.object({
  dependentId: z.string().min(1, 'Dependent is required'),
  medicineName: z.string().min(1, 'Medicine name is required'),
  dosage: z.string().optional(),
  timeAdministered: z.string().min(1, 'Time administered is required'),
  notes: z.string().optional(),
})

const props = defineProps<{
  familyId: string
  isSubmitting: boolean
  errorMessage: string | null
  preSelectedDependentId?: string
}>()

const emit = defineEmits<{
  submit: [values: AddMedicineLogFormValues]
  cancel: []
}>()

const form = useForm({
  validationSchema: toTypedSchema(addMedicineLogSchema),
})

const dependents = ref<DependentDto[]>([])
const isLoadingDependents = ref(false)

function getDependentDisplayName(dependent: DependentDto) {
  const name = dependent.dependentName ?? ''
  const surname = dependent.dependentSurname ?? ''
  return `${name} ${surname}`.trim() || (dependent.dependentId ?? '—')
}

function getDateTimeLocalNow(): string {
  const d = new Date()
  d.setMinutes(d.getMinutes() - d.getTimezoneOffset())
  return d.toISOString().slice(0, 16)
}

const onSubmit = form.handleSubmit((values) => {
  emit('submit', values as AddMedicineLogFormValues)
})

onMounted(async () => {
  form.setFieldValue('timeAdministered', getDateTimeLocalNow())
  if (props.preSelectedDependentId) {
    form.setFieldValue('dependentId', props.preSelectedDependentId)
  } else if (props.familyId) {
    isLoadingDependents.value = true
    const response = await dependentService.getDependents(1, 200, props.familyId)
    if (response.success && response.data) {
      dependents.value = response.data.items
    }
    isLoadingDependents.value = false
  }
})
</script>

<template>
  <form @submit="onSubmit" class="space-y-4">
    <FormField v-if="!props.preSelectedDependentId" v-slot="{ componentField }" name="dependentId">
      <FormItem>
        <FormLabel>Dependent</FormLabel>
        <FormControl>
          <Select
            :model-value="componentField.modelValue"
            :disabled="props.isSubmitting || isLoadingDependents || !dependents.length"
            @update:model-value="componentField['onUpdate:modelValue']"
          >
            <SelectTrigger class="w-full">
              <SelectValue placeholder="Select a dependent" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem
                v-for="d in dependents"
                :key="d.dependentId!"
                :value="d.dependentId!"
              >
                {{ getDependentDisplayName(d) }}
              </SelectItem>
            </SelectContent>
          </Select>
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

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
        {{ props.isSubmitting ? 'Creating...' : 'Add Medicine Log' }}
      </Button>
      <Button type="button" variant="outline" :disabled="props.isSubmitting" @click="emit('cancel')">
        Cancel
      </Button>
    </div>
  </form>
</template>
