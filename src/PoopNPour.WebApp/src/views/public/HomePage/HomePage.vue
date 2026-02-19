<script setup lang="ts">
import { EnvConfig } from '@/lib/env'
import ComingSoonPage from '@/components/views/public/HomePage/ComingSoonPage.vue'
import MainHomePage from '@/components/views/public/HomePage/MainHomePage.vue'

const showComingSoon = EnvConfig.showComingSoon
</script>

<template>
  <ComingSoonPage v-if="showComingSoon" />
  <MainHomePage v-else />
</template>
