<script setup lang="ts">
import { Button } from '@/components/ui/button'
import { Heading } from '@/components/ui_parts/Heading'
import { Subheading } from '@/components/ui_parts/Subheading'
import { Logo, LOGO_STYLES } from '@/components/ui_parts/Logo'
</script>

<template>
  <section class="py-16 md:py-24 lg:py-32">
    <div class="container mx-auto px-4 max-w-4xl">
      <div class="text-center space-y-10">
        <!-- Decorative Logo at top -->
        <div class="flex items-center justify-center">
          <Logo :logoType="LOGO_STYLES.MINIMAL" class="w-10 h-10 md:w-12 md:h-12" />
        </div>

        <!-- Heading -->
        <Heading>
          Ready to simplify parenting?
        </Heading>

        <!-- Subheading -->
        <Subheading>
          Join thousands of parents who've made tracking effortless
        </Subheading>

        <!-- CTA Button -->
        <div class="flex justify-center pt-4">
          <Button size="lg" class="text-lg px-10 py-6">
            Start Tracking Free
          </Button>
        </div>
      </div>
    </div>
  </section>
</template>
