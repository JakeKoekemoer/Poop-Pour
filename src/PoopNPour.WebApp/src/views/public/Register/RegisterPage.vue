<script setup lang="ts">
import { ref } from "vue";
import { UserPlus } from "lucide-vue-next";
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";
import * as z from "zod";
import { useRouter } from "vue-router";

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

import { authService } from "@/services/AuthService/AuthService";
import { RouteHelper } from "@/routes/helpers/RouteHelper";
import { PUBLIC_ROUTES, SECURE_ROUTES } from "@/routes/constants";

// Form validation schema
const formSchema = toTypedSchema(
  z.object({
    email: z.string().email("Invalid email address"),
    userName: z
      .string()
      .min(3, "Username must be at least 3 characters")
      .max(50, "Username must be less than 50 characters"),
    password: z.string().min(8, "Password must be at least 8 characters"),
    firstName: z.string().optional(),
    lastName: z.string().optional(),
  }),
);

const form = useForm({
  validationSchema: formSchema,
});

const router = useRouter();
const isSubmitting = ref(false);
const errorMessage = ref<string | null>(null);

const onSubmit = form.handleSubmit(async (values) => {
  isSubmitting.value = true;
  errorMessage.value = null;

  try {
    const response = await authService.register(
      values.email,
      values.userName,
      values.password,
      values.firstName,
      values.lastName,
    );

    if (response.success) {
      // Redirect to dashboard on success
      await router.push({ name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DASHBOARD) });
    } else {
      errorMessage.value = response.error.message || "Registration failed. Please try again.";
    }
  } catch (error) {
    errorMessage.value = "An unexpected error occurred. Please try again.";
    console.error("Registration error:", error);
  } finally {
    isSubmitting.value = false;
  }
});

const goToLogin = () => {
  router.push({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) });
};
</script>

<template>
  <div class="min-h-[calc(100vh-8rem)] flex items-center justify-center p-4">
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
          <!-- Email Field -->
          <FormField v-slot="{ componentField }" name="email">
            <FormItem>
              <FormLabel>Email</FormLabel>
              <FormControl>
                <Input
                  type="email"
                  placeholder="name@example.com"
                  v-bind="componentField"
                  :disabled="isSubmitting"
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          </FormField>

          <!-- Username Field -->
          <FormField v-slot="{ componentField }" name="userName">
            <FormItem>
              <FormLabel>Username</FormLabel>
              <FormControl>
                <Input
                  type="text"
                  placeholder="johndoe"
                  v-bind="componentField"
                  :disabled="isSubmitting"
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          </FormField>

          <!-- Password Field -->
          <FormField v-slot="{ componentField }" name="password">
            <FormItem>
              <FormLabel>Password</FormLabel>
              <FormControl>
                <Input
                  type="password"
                  placeholder="••••••••"
                  v-bind="componentField"
                  :disabled="isSubmitting"
                />
              </FormControl>
              <FormDescription> Must be at least 8 characters </FormDescription>
              <FormMessage />
            </FormItem>
          </FormField>

          <Separator />

          <!-- First Name Field (Optional) -->
          <FormField v-slot="{ componentField }" name="firstName">
            <FormItem>
              <FormLabel>First Name (Optional)</FormLabel>
              <FormControl>
                <Input
                  type="text"
                  placeholder="John"
                  v-bind="componentField"
                  :disabled="isSubmitting"
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          </FormField>

          <!-- Last Name Field (Optional) -->
          <FormField v-slot="{ componentField }" name="lastName">
            <FormItem>
              <FormLabel>Last Name (Optional)</FormLabel>
              <FormControl>
                <Input
                  type="text"
                  placeholder="Doe"
                  v-bind="componentField"
                  :disabled="isSubmitting"
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          </FormField>

          <!-- Error Alert -->
          <Alert v-if="errorMessage" variant="destructive">
            <AlertDescription>
              {{ errorMessage }}
            </AlertDescription>
          </Alert>

          <!-- Submit Button -->
          <Button type="submit" class="w-full" :disabled="isSubmitting">
            {{ isSubmitting ? "Creating Account..." : "Create Account" }}
          </Button>
        </form>
      </CardContent>
      <CardFooter class="flex flex-col gap-4">
        <div class="text-sm text-center text-muted-foreground">
          Already have an account?
          <Button variant="link" class="px-1" @click="goToLogin" :disabled="isSubmitting">
            Sign in
          </Button>
        </div>
      </CardFooter>
    </Card>
  </div>
</template>
