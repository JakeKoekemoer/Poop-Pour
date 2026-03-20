<script setup lang="ts">
import { IconCard, IconCardTitle, IconCardBody } from '@/components/ui_parts/IconCard'
import { Heading } from '@/components/ui_parts/Heading'
import { Subheading } from '@/components/ui_parts/Subheading'
import { PhoneMockup } from '@/components/ui_parts/PhoneMockup'
</script>

<template>
  <section class="py-6 md:py-8">
    <div class="container mx-auto px-6 md:px-8 lg:px-12">
      <!-- Top Banner with Split Background -->
      <div class="rounded-2xl overflow-hidden mb-5 md:mb-6">
        <div class="grid lg:grid-cols-3 lg:items-stretch">
          <!-- Left Side: Primary Background with Text -->
          <div class="lg:col-span-2 bg-primary p-6 md:p-8 lg:p-10 flex flex-col justify-center">
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
      <div class="grid md:grid-cols-2 gap-4 px-0">
        <IconCard>
          <template #icon>
            <div class="text-5xl">
              ⚡
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
            <div class="text-5xl">
              🔒
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
