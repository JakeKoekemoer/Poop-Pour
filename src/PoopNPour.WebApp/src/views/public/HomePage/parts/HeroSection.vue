<script setup lang="ts">
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { PUBLIC_ROUTES } from '@/routes/constants'

const scrollToSection = () => {
  const element = document.getElementById('how-it-works')
  if (element) {
    element.scrollIntoView({ behavior: 'smooth', block: 'start' })
  }
}
</script>

<template>
  <section class="relative min-h-screen flex items-center overflow-hidden">
    <!-- Gradient Background -->
    <div class="absolute inset-0 bg-gradient-to-br from-background via-card to-background/50"></div>
    <div class="absolute inset-0 bg-gradient-to-b from-black/5 via-transparent to-transparent"></div>
    
    <!-- Decorative Elements -->
    <div class="absolute top-20 right-20 w-32 h-32 bg-primary/20 rounded-full blur-3xl"></div>
    <div class="absolute bottom-40 left-20 w-40 h-40 bg-secondary/20 rounded-full blur-3xl"></div>
    <div class="absolute top-1/3 right-1/4 w-3 h-3 bg-primary rounded-full"></div>
    <div class="absolute top-1/2 right-1/3 w-2 h-2 bg-accent rounded-full"></div>
    <div class="absolute bottom-1/3 left-1/4 w-2 h-2 bg-secondary rounded-full"></div>

    <!-- Content -->
    <div class="relative max-w-7xl mx-auto px-4 sm:px-6 py-20 lg:py-32 w-full">
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-12 lg:gap-16 items-center">
        <!-- Left Column - Text Content -->
        <div class="space-y-6 lg:space-y-8">
          <Badge variant="secondary" class="inline-block px-4 py-2">
            <span class="text-sm font-medium">✨ Trusted by 10,000+ parents</span>
          </Badge>
          
          <h1 class="text-4xl sm:text-5xl lg:text-6xl xl:text-7xl font-bold leading-tight text-foreground">
            Track feeds, <span class="text-primary">poops</span> & sleep
          </h1>
          
          <p class="text-lg sm:text-xl text-muted-foreground leading-relaxed max-w-lg">
            The simplest way to track your baby's daily routine. Built for exhausted parents who need answers fast.
          </p>
          
          <div class="flex flex-col sm:flex-row gap-4 pt-4">
            <Button size="lg" as-child class="shadow-lg hover:shadow-xl transition-shadow">
              <RouterLink :to="{ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) }">
                Start Free
              </RouterLink>
            </Button>
            <Button 
              size="lg" 
              variant="outline" 
              as-child 
              class="hover:bg-primary/10 transition-colors"
            >
              <a href="#how-it-works" @click.prevent="scrollToSection">
                See How It Works →
              </a>
            </Button>
          </div>
        </div>

        <!-- Right Column - Hero Image/Mockup -->
        <div class="relative lg:order-last">
          <div class="relative aspect-[3/4] max-w-md mx-auto">
            <!-- Placeholder for phone mockup - gradient background -->
            <div class="absolute inset-0 bg-gradient-to-br from-primary/20 to-secondary/20 rounded-3xl shadow-2xl flex items-center justify-center">
              <div class="text-center space-y-4 p-8">
                <span class="text-6xl">📱</span>
                <p class="text-muted-foreground">App Preview</p>
              </div>
            </div>
            
            <!-- Decorative Elements around mockup -->
            <div class="absolute -top-8 -left-8 w-16 h-16 bg-card rounded-2xl shadow-lg flex items-center justify-center text-3xl">
              🍼
            </div>
            <div class="absolute -bottom-8 -right-8 w-16 h-16 bg-card rounded-2xl shadow-lg flex items-center justify-center text-3xl">
              💩
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
