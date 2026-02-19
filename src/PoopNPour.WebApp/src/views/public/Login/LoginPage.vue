<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";

import { RouteHelper } from "@/routes/helpers/RouteHelper";
import { PUBLIC_ROUTES, SECURE_ROUTES } from "@/routes/constants";

import LoginForm from "@/components/views/public/Login/LoginForm.vue";
import type { LoginFormValues } from "@/components/views/public/Login/loginSchema";

const router = useRouter();
const isSubmitting = ref(false);
const errorMessage = ref<string | null>(null);

async function handleLogin(_values: LoginFormValues) {
  isSubmitting.value = true;
  errorMessage.value = null;

  try {
    // TODO: wire up authService.login when available
    await router.push({ name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DASHBOARD) });
  } catch (error) {
    errorMessage.value = "An unexpected error occurred. Please try again.";
    console.error("Login error:", error);
  } finally {
    isSubmitting.value = false;
  }
}

function goToRegister() {
  router.push({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.REGISTER) });
}
</script>

<template>
  <div class="min-h-[calc(100vh-8rem)] flex items-center justify-center p-4">
    <LoginForm
      :is-submitting="isSubmitting"
      :error-message="errorMessage"
      @submit="handleLogin"
      @go-to-register="goToRegister"
    />
  </div>
</template>
