<script setup lang="ts">
import { Zap, Lock, Baby, Droplet } from 'lucide-vue-next'

const features = [
  {
    icon: Zap,
    title: 'Lightning Fast',
    description: 'Log entries in under 3 seconds. No forms, no fuss, no frustration.',
  },
  {
    icon: Lock,
    title: 'Private & Secure',
    description: 'Your data stays yours. No ads, no selling, no nonsense.',
  },
]
</script>

<template>
  <section class="py-16 md:py-24 lg:py-32 bg-muted/20">
    <div class="container mx-auto px-4 max-w-7xl">
      <div class="grid lg:grid-cols-2 gap-16 lg:gap-20 items-center">
        <!-- Left Column: Content -->
        <div class="space-y-10 order-2 lg:order-1">
          <div class="space-y-5">
            <h2 class="text-4xl md:text-5xl lg:text-6xl font-bold">
              Built for 3AM
            </h2>
            <p class="text-xl md:text-2xl text-muted-foreground leading-relaxed">
              Large buttons, clear text, and zero thinking required. Because parenting is hard enough.
            </p>
          </div>

          <!-- Feature Cards -->
          <div class="grid gap-6">
            <div
              v-for="feature in features"
              :key="feature.title"
              class="flex gap-5 p-6 rounded-xl bg-background border shadow-sm hover:shadow-md transition-shadow"
            >
              <div class="p-4 rounded-xl bg-primary/10 shrink-0">
                <component :is="feature.icon" class="w-8 h-8 text-primary" />
              </div>
              <div>
                <h3 class="text-2xl font-semibold mb-2">
                  {{ feature.title }}
                </h3>
                <p class="text-muted-foreground text-lg">
                  {{ feature.description }}
                </p>
              </div>
            </div>
          </div>

          <!-- Decorative Icons -->
          <div class="flex items-center gap-4 pt-6">
            <Baby class="w-8 h-8 md:w-10 md:h-10 text-primary" />
            <Droplet class="w-8 h-8 md:w-10 md:h-10 text-primary" />
          </div>
        </div>

        <!-- Right Column: Phone Mockup (Night Mode) -->
        <div class="flex justify-center lg:justify-end order-1 lg:order-2">
          <div class="relative w-[280px] md:w-[340px] lg:w-[420px]">
            <!-- Phone Frame (CSS-based, Dark Theme) -->
            <div class="relative mx-auto w-full aspect-[9/19] max-w-[280px] md:max-w-[340px] lg:max-w-[420px]">
              <!-- Phone Shadow -->
              <div class="absolute top-2 left-2 w-full h-full rounded-[2.5rem] bg-foreground/30 opacity-40"></div>
              
              <!-- Phone Frame -->
              <div class="relative w-full h-full rounded-[2.5rem] bg-foreground p-2 shadow-2xl">
                <!-- Notch -->
                <div class="absolute top-0 left-1/2 -translate-x-1/2 w-[60px] h-5 bg-foreground rounded-b-xl"></div>
                
                <!-- Screen Area (Dark Mode) -->
                <div class="w-full h-full rounded-[2rem] bg-gradient-to-b from-[#1a1a1a] to-[#0f0f0f] overflow-hidden">
                  <!-- App UI Icons (Night Mode Style) -->
                  <div class="h-full flex flex-col items-center justify-center gap-8 p-8">
                    <div class="flex flex-col items-center gap-3">
                      <div class="p-6 rounded-3xl bg-primary/25 border border-primary/30 shadow-lg">
                        <Baby class="w-12 h-12 text-primary" />
                      </div>
                      <div class="text-sm font-semibold text-center text-muted-foreground">Feed</div>
                    </div>
                    <div class="flex flex-col items-center gap-3">
                      <div class="p-6 rounded-3xl bg-primary/25 border border-primary/30 shadow-lg">
                        <Droplet class="w-12 h-12 text-primary" />
                      </div>
                      <div class="text-sm font-semibold text-center text-muted-foreground">Diaper</div>
                    </div>
                  </div>
                </div>
                
                <!-- Home Indicator -->
                <div class="absolute bottom-2 left-1/2 -translate-x-1/2 w-10 h-1 bg-muted-foreground/60 rounded-full"></div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
