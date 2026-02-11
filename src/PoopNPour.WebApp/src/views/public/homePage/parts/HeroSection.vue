<script setup lang="ts">
import { Sparkles, Baby, Droplet, ArrowRight } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
</script>

<template>
  <section class="py-8 md:py-12 lg:py-16">
    <div class="container mx-auto px-6 md:px-8 lg:px-12">
      <div class="grid lg:grid-cols-2 gap-8 lg:gap-12 items-center">
        <!-- Left Column: Content -->
        <div class="space-y-8">
          <!-- Trust Badge -->
          <div class="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-accent border border-accent/20">
            <Sparkles class="w-4 h-4 text-accent-foreground" />
            <span class="text-sm font-medium text-accent-foreground">Trusted by 10,000+ parents</span>
          </div>

          <!-- Main Headline with Emojis -->
          <div class="space-y-2">
            <h1 class="text-5xl md:text-6xl lg:text-7xl font-extrabold tracking-tight leading-[1.1]">
              Track feeds,<br/><span class="text-primary">poops</span> & sleep
            </h1>
            <!-- Decorative Emojis inline with headline -->
            <div class="flex items-center gap-3 text-4xl md:text-5xl">
              🍼💩
            </div>
          </div>

          <!-- Subheadline -->
          <p class="text-xl md:text-2xl text-muted-foreground max-w-2xl leading-relaxed">
            The simplest way to track your baby's daily routine. Built for exhausted parents who need answers fast.
          </p>

          <!-- CTAs -->
          <div class="flex flex-col sm:flex-row items-start sm:items-center gap-4 pt-2">
            <Button size="lg" class="text-base px-8">
              Start Free
            </Button>
            <a
              href="#how-it-works"
              class="inline-flex items-center gap-2 text-base font-medium text-primary hover:underline transition-colors"
            >
              See How It Works
              <ArrowRight class="w-4 h-4" />
            </a>
          </div>
        </div>

        <!-- Right Column: Phone Mockup -->
        <div class="flex justify-center lg:justify-end order-first lg:order-last">
          <div class="relative w-full max-w-[280px] md:max-w-[360px] lg:max-w-[450px]">
            <!-- Phone Frame (CSS-based) -->
            <div class="relative mx-auto w-full aspect-[9/19]">
              <!-- Phone Shadow -->
              <div class="absolute top-2 left-2 w-full h-full rounded-[2.5rem] bg-foreground/20 opacity-30"></div>
              
              <!-- Phone Frame -->
              <div class="relative w-full h-full rounded-[2.5rem] bg-foreground p-2 shadow-2xl">
                <!-- Notch -->
                <div class="absolute top-0 left-1/2 -translate-x-1/2 w-[60px] h-5 bg-foreground rounded-b-xl"></div>
                
                <!-- Screen Area -->
                <div class="w-full h-full rounded-[2rem] bg-gradient-to-b from-[#fefcf8] to-[#faf8f3] overflow-hidden">
                  <!-- App UI Icons -->
                  <div class="h-full flex flex-col items-center justify-center gap-10 p-8">
                    <div class="flex flex-col items-center gap-3">
                      <div class="p-5 rounded-3xl bg-primary/15 shadow-lg">
                        <Baby class="w-14 h-14 text-primary" />
                      </div>
                      <div class="text-sm font-semibold text-center text-foreground">Feed</div>
                    </div>
                    <div class="flex flex-col items-center gap-3">
                      <div class="p-5 rounded-3xl bg-primary/15 shadow-lg">
                        <Droplet class="w-14 h-14 text-primary" />
                      </div>
                      <div class="text-sm font-semibold text-center text-foreground">Diaper</div>
                    </div>
                  </div>
                </div>
                
                <!-- Home Indicator -->
                <div class="absolute bottom-2 left-1/2 -translate-x-1/2 w-10 h-1 bg-muted-foreground/50 rounded-full"></div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
