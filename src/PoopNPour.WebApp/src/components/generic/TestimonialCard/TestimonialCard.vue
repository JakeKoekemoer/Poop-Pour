<script setup lang="ts">
import { Card, CardContent } from '@/components/ui/card'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { StarRating } from '@/components/generic/StarRating'

interface Props {
  rating: number
  text: string
  name: string
  initial: string
  imageSrc?: string
}

defineProps<Props>()
</script>

<template>
  <Card class="bg-gradient-to-br from-background to-card hover:shadow-xl transition-all">
    <CardContent class="p-8">
      <!-- Rating -->
      <div class="mb-4">
        <StarRating :rating="rating" />
      </div>

      <!-- Testimonial Text -->
      <p class="text-foreground leading-relaxed mb-6">
        "{{ text }}"
      </p>

      <!-- Author -->
      <div class="flex items-center gap-3">
        <Avatar>
          <AvatarImage v-if="imageSrc" :src="imageSrc" :alt="name" />
          <AvatarFallback class="bg-primary text-primary-foreground font-semibold">
            {{ initial }}
          </AvatarFallback>
        </Avatar>
        <span class="font-semibold text-foreground">{{ name }}</span>
      </div>
    </CardContent>
  </Card>
</template>
