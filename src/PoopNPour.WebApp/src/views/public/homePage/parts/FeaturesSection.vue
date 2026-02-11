<script setup lang="ts">
import { Zap, Lock, Baby, Droplet } from 'lucide-vue-next'
import { IconCard, IconCardTitle, IconCardBody } from '@/components/ui_parts/IconCard'
import { Heading } from '@/components/ui_parts/Heading'
import { Subheading } from '@/components/ui_parts/Subheading'
import { PhoneMockup } from '@/components/ui_parts/PhoneMockup'
</script>

<template>
  <section class="py-6 md:py-8">
    <div class="w-full">
      <!-- Top Banner with Split Background -->
      <div class="rounded-2xl overflow-hidden mb-5 md:mb-6">
        <div class="grid lg:grid-cols-3 lg:items-stretch">
          <!-- Left Side: Primary Background with Text -->
          <div class="lg:col-span-2 bg-primary p-4 md:p-5 lg:p-6 flex flex-col justify-center">
            <Heading class="text-primary-foreground mb-2">
              Built for 3AM
            </Heading>
            <Subheading class="text-primary-foreground leading-relaxed">
              Large buttons, clear text, and zero thinking required. Because parenting is hard enough.
            </Subheading>
          </div>
          
          <!-- Right Side: Secondary Background with Phone Mockup -->
          <div class="lg:col-span-1 bg-secondary flex items-center justify-center p-3 md:p-4">
            <PhoneMockup class="max-w-[150px] max-h-[200px]" :aspect-ratio="'1/4'">
              <br/>
              PUT IMAGE CONTENT HERE
            </PhoneMockup>
          </div>
        </div>
      </div>

      <!-- Feature Cards -->
      <div class="grid md:grid-cols-2 gap-4 max-w-5xl mx-auto px-4 md:px-6">
        <IconCard>
          <template #icon>
            <div class="p-4 rounded-xl bg-primary">
              <Zap class="w-8 h-8 text-secondary" />
            </div>
          </template>
          <template #content>
            <IconCardTitle>
              Lightning Fast
            </IconCardTitle>
            <IconCardBody>
              Log entries in under 3 seconds. No forms, no fuss, no frustration.
            </IconCardBody>
          </template>
        </IconCard>
        <IconCard>
          <template #icon>
            <div class="p-4 rounded-xl bg-secondary">
              <Lock class="w-8 h-8 text-primary" />
            </div>
          </template>
          <template #content>
            <IconCardTitle>
              Private & Secure
            </IconCardTitle>
            <IconCardBody>
              Your data stays yours. No ads, no selling, no nonsense.
            </IconCardBody>
          </template>
        </IconCard>
      </div>
    </div>
  </section>
</template>
