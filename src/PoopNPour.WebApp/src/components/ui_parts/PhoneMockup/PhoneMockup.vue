<script setup lang="ts">
withDefaults(defineProps<{
  screenBg?: string
  maxHeight?: string
  aspectRatio?: string
}>(), {
  screenBg: 'bg-primary',
  maxHeight: 'none',
  aspectRatio: '9/19',
})
</script>

<template>
  <div class="relative w-full">
    <!-- Phone Frame (CSS-based) -->
    <div 
      :class="['relative mx-auto w-full', $attrs.class]"
      :style="{ 
        aspectRatio: aspectRatio,
        maxHeight: maxHeight !== 'none' ? maxHeight : undefined
      }"
    >
      <!-- Phone Shadow -->
      <div class="absolute top-2 left-2 w-full h-full rounded-[2.5rem] bg-foreground/30 opacity-40"></div>
      
      <!-- Phone Frame -->
      <div class="relative w-full h-full rounded-[2.5rem] bg-foreground p-2 shadow-2xl">
        <!-- Notch -->
        <div class="absolute top-0 left-1/2 -translate-x-1/2 w-[60px] h-5 bg-foreground rounded-b-xl z-10"></div>
        
        <!-- Screen Area -->
        <div :class="['w-full h-full rounded-[2rem] overflow-hidden', screenBg]">
          <slot />
        </div>
        
        <!-- Home Indicator -->
        <div class="absolute bottom-2 left-1/2 -translate-x-1/2 w-10 h-1 bg-muted-foreground/60 rounded-full z-10"></div>
      </div>
    </div>
  </div>
</template>
