<script setup lang="ts">
import { EnvConfig } from '@/lib/env'
import ComingSoonPage from './parts/ComingSoonPage.vue'
import MainHomePage from './parts/MainHomePage.vue'

const showComingSoon = EnvConfig.showComingSoon
</script>

<template>
  <ComingSoonPage v-if="showComingSoon" />
  <MainHomePage v-else />
</template>
