<script setup lang="ts">
import { computed } from 'vue'
import fullLogo from '@/assets/images/P&PLogo.png'
import bottleLogo from '@/assets/images/P&PLogoBottle.png'
import minimalLogo from '@/assets/images/P&PLogoMin.png'
import type { LogoStyle, LogoPosition } from './types'
import { LOGO_STYLES, LOGO_POSITIONS } from './types'

const props = withDefaults(defineProps<{
  logoType?: LogoStyle
  position?: LogoPosition
  class?: string
  alt?: string
}>(), {
  logoType: LOGO_STYLES.FULL,
  position: LOGO_POSITIONS.LEFT,
  alt: 'Poop & Pour Logo',
})

const logoImage = computed(() => {
  switch (props.logoType) {
    case LOGO_STYLES.BOTTLE:
      return bottleLogo
    case LOGO_STYLES.MINIMAL:
      return minimalLogo
    case LOGO_STYLES.FULL:
    default:
      return fullLogo
  }
})

const isCombined = computed(() => props.logoType === LOGO_STYLES.COMBINED)
const isBottleLeft = computed(() => props.position === LOGO_POSITIONS.LEFT)
const getAltText = (suffix: string) => `${props.alt} - ${suffix}`
</script>

<template>
  <div v-if="isCombined" :class="['flex items-center gap-2', $attrs.class]">
    <img
      v-if="isBottleLeft"
      :src="bottleLogo"
      :alt="getAltText('Bottle')"
      class="Logo"
    />
    <img
      :src="minimalLogo"
      :alt="getAltText('Minimal')"
      class="Logo"
    />
    <img
      v-if="!isBottleLeft"
      :src="bottleLogo"
      :alt="getAltText('Bottle')"
      class="Logo"
    />
  </div>
  <img
    v-else
    :src="logoImage"
    :alt="alt"
    :class="['Logo', $attrs.class]"
  />
</template>
