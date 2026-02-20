<script setup lang="ts">
import { UserPlus } from "lucide-vue-next";
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Separator } from "@/components/ui/separator";
import { Alert, AlertDescription } from "@/components/ui/alert";
import {
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { PasswordInput } from "@/components/generic/PasswordInput";

import { registerSchema, type RegisterFormValues } from "./registerSchema";

const props = defineProps<{
  isSubmitting: boolean;
  errorMessage: string | null;
}>();

const emit = defineEmits<{
  submit: [values: RegisterFormValues];
  goToLogin: [];
}>();

const form = useForm({
  validationSchema: toTypedSchema(registerSchema),
});

const onSubmit = form.handleSubmit((values) => {
  emit("submit", values);
});
</script>

<template>
  <Card class="w-full max-w-md">
    <CardHeader class="space-y-1">
      <div class="flex justify-center mb-4">
        <UserPlus class="w-12 h-12 text-primary" />
      </div>
      <CardTitle class="text-3xl font-bold text-center"> Create Account </CardTitle>
      <CardDescription class="text-center">
        Enter your information to create your account
      </CardDescription>
    </CardHeader>
    <CardContent>
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
            <FormLabel>Password</FormLabel>
            <FormControl>
              <PasswordInput v-bind="componentField" :disabled="props.isSubmitting" />
            </FormControl>
            <FormDescription> Must be at least 8 characters </FormDescription>
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

        <Button type="submit" class="w-full" :disabled="props.isSubmitting">
          {{ props.isSubmitting ? "Creating Account..." : "Create Account" }}
        </Button>
      </form>
    </CardContent>
    <CardFooter class="flex flex-col gap-4">
      <div class="text-sm text-center text-muted-foreground">
        Already have an account?
        <Button variant="link" class="px-1" @click="emit('goToLogin')" :disabled="props.isSubmitting">
          Sign in
        </Button>
      </div>
    </CardFooter>
  </Card>
</template>
