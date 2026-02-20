<script setup lang="ts">
import { LogIn } from "lucide-vue-next";
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
import { Alert, AlertDescription } from "@/components/ui/alert";
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { PasswordInput } from "@/components/generic/PasswordInput";

import { loginSchema, type LoginFormValues } from "./loginSchema";

const props = defineProps<{
  isSubmitting: boolean;
  errorMessage: string | null;
}>();

const emit = defineEmits<{
  submit: [values: LoginFormValues];
  goToRegister: [];
}>();

const form = useForm({
  validationSchema: toTypedSchema(loginSchema),
});

const onSubmit = form.handleSubmit((values) => {
  emit("submit", values);
});
</script>

<template>
  <Card class="w-full max-w-md">
    <CardHeader class="space-y-1">
      <div class="flex justify-center mb-4">
        <LogIn class="w-12 h-12 text-primary" />
      </div>
      <CardTitle class="text-3xl font-bold text-center">Login</CardTitle>
      <CardDescription class="text-center"> Sign in to your account to continue </CardDescription>
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

        <FormField v-slot="{ componentField }" name="password">
          <FormItem>
            <FormLabel>Password</FormLabel>
            <FormControl>
              <PasswordInput v-bind="componentField" :disabled="props.isSubmitting" />
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
          {{ props.isSubmitting ? "Signing In..." : "Sign In" }}
        </Button>
      </form>
    </CardContent>
    <CardFooter class="flex flex-col gap-4">
      <div class="text-sm text-center text-muted-foreground">
        Don't have an account?
        <Button variant="link" class="px-1" @click="emit('goToRegister')" :disabled="props.isSubmitting">
          Sign up
        </Button>
      </div>
    </CardFooter>
  </Card>
</template>
