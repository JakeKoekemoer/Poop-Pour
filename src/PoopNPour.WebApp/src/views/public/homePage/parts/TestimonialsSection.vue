<script setup lang="ts">
import { Star } from 'lucide-vue-next'

const testimonials = [
  {
    quote: 'Finally, an app that doesn\'t make me think at 3AM. Just tap and done. Exactly what sleep-deprived parents need.',
    name: 'Sarah M.',
    initial: 'S',
    color: 'bg-primary/20 text-primary',
  },
  {
    quote: 'We tried 4 other apps before this. They were all too complicated. Poop & Pour is perfect - simple, fast, and actually useful.',
    name: 'James K.',
    initial: 'J',
    color: 'bg-secondary/20 text-secondary-foreground',
  },
  {
    quote: 'The timeline view helped us figure out our baby\'s feeding pattern. Our pediatrician was impressed with how organized our data was!',
    name: 'Emily R.',
    initial: 'E',
    color: 'bg-accent/20 text-accent-foreground',
  },
]
</script>

<template>
  <section class="py-12 md:py-16 lg:py-20">
    <div class="container mx-auto px-6 md:px-8 lg:px-12">
      <div class="text-center mb-12 md:mb-16">
        <h2 class="text-4xl md:text-5xl lg:text-6xl font-bold mb-4">
          Why Parents Love It
        </h2>
      </div>

      <div class="grid md:grid-cols-3 gap-10 lg:gap-12">
        <div
          v-for="testimonial in testimonials"
          :key="testimonial.name"
          class="flex flex-col space-y-5"
        >
          <!-- Star Rating -->
          <div class="flex gap-1">
            <Star
              v-for="i in 5"
              :key="i"
              class="w-6 h-6 fill-yellow-400 text-yellow-400"
            />
          </div>

          <!-- Quote -->
          <p class="text-muted-foreground text-lg md:text-xl leading-relaxed flex-grow">
            "{{ testimonial.quote }}"
          </p>

          <!-- Avatar and Name -->
          <div class="flex items-center gap-4 pt-4">
            <!-- Avatar Placeholder -->
            <div
              :class="[
                'w-16 h-16 rounded-full flex items-center justify-center font-semibold text-xl shadow-sm',
                testimonial.color,
              ]"
            >
              {{ testimonial.initial }}
            </div>
            <div class="font-semibold text-lg">{{ testimonial.name }}</div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
