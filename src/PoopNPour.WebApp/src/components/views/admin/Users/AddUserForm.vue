<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useForm } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import { Shuffle } from 'lucide-vue-next'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Separator } from '@/components/ui/separator'
import { Alert, AlertDescription } from '@/components/ui/alert'
import {
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'
import { PasswordInput } from '@/components/generic/PasswordInput'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'

import { addUserSchema, type AddUserFormValues } from './addUserSchema'
import { userService } from '@/services/UserService'

const props = defineProps<{
  isSubmitting: boolean
  errorMessage: string | null
}>()

const emit = defineEmits<{
  submit: [values: AddUserFormValues]
  cancel: []
}>()

const form = useForm({
  validationSchema: toTypedSchema(addUserSchema),
})

const roles = ref<string[]>([])
const isLoadingRoles = ref(false)
const rolesError = ref<string | null>(null)

const onSubmit = form.handleSubmit((values) => {
  emit('submit', values)
})

const passwordInputRef = ref<InstanceType<typeof PasswordInput> | null>(null)

function generatePassword(): string {
  const upper = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
  const lower = 'abcdefghijklmnopqrstuvwxyz'
  const digits = '0123456789'
  const special = '!@#$%^&*()-_=+[]{}|;:,.<>?'
  const all = upper + lower + digits + special

  const randomByte = () => crypto.getRandomValues(new Uint8Array(1))[0] as number
  const pick = (charset: string) => charset[randomByte() % charset.length] as string

  // Guarantee at least one character from each required category.
  const required = [pick(upper), pick(lower), pick(digits), pick(special)]

  const remaining = Array.from({ length: 12 }, () => pick(all))

  return [...required, ...remaining]
    .sort(() => (randomByte() % 2 === 0 ? -1 : 1))
    .join('')
}

function handleGenerate() {
  const password = generatePassword()
  form.setFieldValue('password', password)
  passwordInputRef.value?.reveal()
}

onMounted(async () => {
  isLoadingRoles.value = true
  rolesError.value = null

  const response = await userService.getRoles()
  if (response.success && response.data && response.data.length > 0) {
    roles.value = response.data
  } else if (!response.success) {
    rolesError.value = response.error?.message || 'Failed to load roles. Please try again.'
  } else {
    rolesError.value = 'No roles are configured. Please contact an administrator.'
  }

  isLoadingRoles.value = false
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

    <FormField v-slot="{ componentField }" name="userName">
      <FormItem>
        <FormLabel>Username</FormLabel>
        <FormControl>
          <Input
            type="text"
            placeholder="johndoe"
            v-bind="componentField"
            :disabled="props.isSubmitting"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="password">
      <FormItem>
        <div class="flex items-center justify-between">
          <FormLabel>Password</FormLabel>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            class="h-auto gap-1.5 px-2 py-0.5 text-xs text-muted-foreground"
            :disabled="props.isSubmitting"
            @click="handleGenerate"
          >
            <Shuffle class="h-3 w-3" />
            Generate
          </Button>
        </div>
        <FormControl>
          <PasswordInput ref="passwordInputRef" v-bind="componentField" :disabled="props.isSubmitting" />
        </FormControl>
        <FormDescription>Must be at least 8 characters</FormDescription>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="role">
      <FormItem>
        <FormLabel>Role</FormLabel>
        <FormControl>
          <Select
            :model-value="componentField.modelValue"
            :disabled="props.isSubmitting || isLoadingRoles || !roles.length"
            @update:model-value="componentField['onUpdate:modelValue']"
          >
            <SelectTrigger>
              <SelectValue placeholder="Select a role" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem
                v-for="role in roles"
                :key="role"
                :value="role"
              >
                {{ role }}
              </SelectItem>
            </SelectContent>
          </Select>
        </FormControl>
        <FormDescription v-if="isLoadingRoles">Loading roles...</FormDescription>
        <FormDescription v-else-if="rolesError" class="text-destructive">
          {{ rolesError }}
        </FormDescription>
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
        {{ props.isSubmitting ? 'Creating User...' : 'Create User' }}
      </Button>
      <Button type="button" variant="outline" :disabled="props.isSubmitting" @click="emit('cancel')">
        Cancel
      </Button>
    </div>
  </form>
</template>
