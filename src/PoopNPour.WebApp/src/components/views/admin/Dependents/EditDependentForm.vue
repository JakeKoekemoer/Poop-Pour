<script setup lang="ts">
import { watch } from 'vue'
import { useForm } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import * as z from 'zod'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Alert, AlertDescription } from '@/components/ui/alert'
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'

export interface EditDependentFormValues {
  dependentName: string
  dependentSurname: string
  dateOfBirth: string
}

const editDependentSchema = z.object({
  dependentName: z.string().min(1, 'First name is required'),
  dependentSurname: z.string().min(1, 'Surname is required'),
  dateOfBirth: z.string().min(1, 'Date of birth is required'),
})

const props = defineProps<{
  isSubmitting: boolean
  errorMessage: string | null
  initialValues?: EditDependentFormValues
}>()

const emit = defineEmits<{
  submit: [values: EditDependentFormValues]
  cancel: []
}>()

const form = useForm({
  validationSchema: toTypedSchema(editDependentSchema),
})

watch(
  () => props.initialValues,
  (vals) => {
    if (vals) {
      form.setValues({
        dependentName: vals.dependentName,
        dependentSurname: vals.dependentSurname,
        dateOfBirth: vals.dateOfBirth,
      })
    }
  },
  { immediate: true },
)

const onSubmit = form.handleSubmit((values) => {
  emit('submit', values)
})
</script>

<template>
  <form @submit="onSubmit" class="space-y-4">
    <FormField v-slot="{ componentField }" name="dependentName">
      <FormItem>
        <FormLabel>First Name</FormLabel>
        <FormControl>
          <Input
            type="text"
            placeholder="e.g. Emma"
            v-bind="componentField"
            :disabled="props.isSubmitting"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="dependentSurname">
      <FormItem>
        <FormLabel>Surname</FormLabel>
        <FormControl>
          <Input
            type="text"
            placeholder="e.g. Smith"
            v-bind="componentField"
            :disabled="props.isSubmitting"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="dateOfBirth">
      <FormItem>
        <FormLabel>Date of Birth</FormLabel>
        <FormControl>
          <Input
            type="date"
            v-bind="componentField"
            :disabled="props.isSubmitting"
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
