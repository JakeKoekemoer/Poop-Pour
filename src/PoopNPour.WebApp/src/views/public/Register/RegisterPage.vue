<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";

import { authService } from "@/services/AuthService/AuthService";
import { RouteHelper } from "@/routes/helpers/RouteHelper";
import { PUBLIC_ROUTES, SECURE_ROUTES } from "@/routes/constants";

import RegisterForm from "@/components/views/public/Register/RegisterForm.vue";
import type { RegisterFormValues } from "@/components/views/public/Register/registerSchema";

const router = useRouter();
const isSubmitting = ref(false);
const errorMessage = ref<string | null>(null);

async function handleRegister(values: RegisterFormValues) {
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
}

function goToLogin() {
  router.push({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) });
}
</script>

<template>
  <div class="min-h-[calc(100vh-8rem)] flex items-center justify-center p-4">
    <RegisterForm
      :is-submitting="isSubmitting"
      :error-message="errorMessage"
      @submit="handleRegister"
      @go-to-login="goToLogin"
    />
  </div>
</template>
