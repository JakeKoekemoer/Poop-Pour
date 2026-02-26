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

export interface EditFamilyFormValues {
  familyName: string
  familyLastName: string
}

const editFamilySchema = z.object({
  familyName: z.string().min(1, 'Family name is required'),
  familyLastName: z.string().min(1, 'Last name is required'),
})

const props = defineProps<{
  isSubmitting: boolean
  errorMessage: string | null
  initialValues?: EditFamilyFormValues
}>()

const emit = defineEmits<{
  submit: [values: EditFamilyFormValues]
  cancel: []
}>()

const form = useForm({
  validationSchema: toTypedSchema(editFamilySchema),
})

watch(
  () => props.initialValues,
  (vals) => {
    if (vals) {
      form.setValues({
        familyName: vals.familyName,
        familyLastName: vals.familyLastName,
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
    <FormField v-slot="{ componentField }" name="familyName">
      <FormItem>
        <FormLabel>Family Name</FormLabel>
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

    <FormField v-slot="{ componentField }" name="familyLastName">
      <FormItem>
        <FormLabel>Last Name</FormLabel>
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
