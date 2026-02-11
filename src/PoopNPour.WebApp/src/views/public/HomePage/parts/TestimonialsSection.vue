<script setup lang="ts">
import { TestimonialCard } from '@/components/generic/TestimonialCard'

const testimonials = [
  {
    rating: 5,
    text: 'This app is a lifesaver! Super simple to use, even at 3 AM when my brain is mush. Just tap and done. Exactly what sleep-deprived parents need.',
    name: 'Sarah M.',
    initial: 'SM',
  },
  {
    rating: 5,
    text: 'Finally, a baby tracker that doesn\'t feel like work. Clean, fast, and actually useful.',
    name: 'James K.',
    initial: 'JK',
  },
  {
    rating: 5,
    text: 'I tried 5 other apps before this one. This is THE ONE. Wish I found it sooner - would have saved so much stress when tracking feeds was!',
    name: 'Emily R.',
    initial: 'ER',
  },
]
</script>

<template>
  <section class="py-16 md:py-24 bg-card">
    <div class="max-w-7xl mx-auto px-4 sm:px-6">
      <!-- Section Header -->
      <h2 class="text-3xl sm:text-4xl lg:text-5xl font-bold text-accent mb-12 lg:mb-16">
        Why Parents Love It
      </h2>

      <!-- Testimonials Grid -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 lg:gap-8">
        <TestimonialCard
          v-for="(testimonial, index) in testimonials"
          :key="index"
          :rating="testimonial.rating"
          :text="testimonial.text"
          :name="testimonial.name"
          :initial="testimonial.initial"
        />
      </div>
    </div>
  </section>
</template>
