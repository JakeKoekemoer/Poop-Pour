<script setup lang="ts">
import { computed } from 'vue'
import fullLogo from '@/assets/images/P&PLogo.png'
import bottleLogo from '@/assets/images/P&PLogoBottle.png'
import minimalLogo from '@/assets/images/P&PLogoMin.png'

type LogoStyle = 'full' | 'bottle' | 'minimal'

const props = withDefaults(defineProps<{
  logoType?: LogoStyle
  class?: string
  alt?: string
}>(), {
  logoType: 'full',
  alt: 'Poop & Pour Logo',
})

const logoImage = computed(() => {
  switch (props.logoType) {
    case 'bottle':
      return bottleLogo
    case 'minimal':
      return minimalLogo
    case 'full':
    default:
      return fullLogo
  }
})
</script>

<template>
  <img
    :src="logoImage"
    :alt="alt"
    :class="['Logo', $attrs.class]"
  />
</template>
