<script setup lang="ts">
import { watch } from 'vue'
import { useForm } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import * as z from 'zod'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Separator } from '@/components/ui/separator'
import { Alert, AlertDescription } from '@/components/ui/alert'
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'

export interface EditUserFormValues {
  email: string
  firstName?: string
  lastName?: string
}

const editUserSchema = z.object({
  email: z.string().email('Invalid email address'),
  firstName: z.string().optional(),
  lastName: z.string().optional(),
})

const props = defineProps<{
  isSubmitting: boolean
  errorMessage: string | null
  initialValues?: EditUserFormValues
}>()

const emit = defineEmits<{
  submit: [values: EditUserFormValues]
  cancel: []
}>()

const form = useForm({
  validationSchema: toTypedSchema(editUserSchema),
})

watch(
  () => props.initialValues,
  (vals) => {
    if (vals) {
      form.setValues({
        email: vals.email,
        firstName: vals.firstName ?? '',
        lastName: vals.lastName ?? '',
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
    <FormField v-slot="{ componentField }" name="email">
      <FormItem>
        <FormLabel>Email</FormLabel>
        <FormControl>
          <Input
            type="email"
            placeholder="name@example.com"
            v-bind="componentField"
            :disabled="props.isSubmitting"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <Separator />

    <FormField v-slot="{ componentField }" name="firstName">
      <FormItem>
        <FormLabel>First Name (Optional)</FormLabel>
        <FormControl>
          <Input
            type="text"
            placeholder="John"
            v-bind="componentField"
            :disabled="props.isSubmitting"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="lastName">
      <FormItem>
        <FormLabel>Last Name (Optional)</FormLabel>
        <FormControl>
          <Input
            type="text"
            placeholder="Doe"
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
