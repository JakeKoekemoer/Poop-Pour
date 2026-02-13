<script setup lang="ts">
import { Facebook, Twitter, Instagram, Mail } from 'lucide-vue-next'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { PUBLIC_ROUTES } from '@/routes/constants'

const currentYear = new Date().getFullYear()

const productLinks = [
  { name: 'Features', route: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) },
  { name: 'Pricing', route: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) },
  { name: 'Download', route: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) },
]

const companyLinks = [
  { name: 'About', route: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) },
  { name: 'Blog', route: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) },
  { name: 'Contact', route: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) },
]

const legalLinks = [
  { name: 'Privacy', route: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) },
  { name: 'Terms', route: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) },
  { name: 'Cookie Policy', route: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.HOME) },
]

const socialLinks = [
  { name: 'Facebook', icon: Facebook, href: '#' },
  { name: 'Twitter', icon: Twitter, href: '#' },
  { name: 'Instagram', icon: Instagram, href: '#' },
  { name: 'Email', icon: Mail, href: 'mailto:hello@poopandpour.com' },
]
</script>

<template>
  <footer class="bg-accent text-accent-foreground py-16">
    <div class="max-w-7xl mx-auto px-4 sm:px-6">
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8 lg:gap-12 mb-12">
        <div class="sm:col-span-2 lg:col-span-1">
          <div class="flex items-center gap-2 mb-4">
            <span class="text-3xl">🍼</span>
            <span class="text-2xl font-bold">Poop & Pour</span>
          </div>
          <p class="text-accent-foreground/80 leading-relaxed mb-4">
            The simplest way to track your baby's daily routine. Built for exhausted parents who need answers fast.
          </p>
          <div class="flex gap-4">
            <a
              v-for="social in socialLinks"
              :key="social.name"
              :href="social.href"
              :aria-label="social.name"
              class="text-accent-foreground/70 hover:text-accent-foreground transition-colors hover:scale-110 transform"
              target="_blank"
              rel="noopener noreferrer"
            >
              <component :is="social.icon" class="w-5 h-5" />
            </a>
          </div>
        </div>

        <div>
          <h3 class="font-bold text-lg mb-4">Product</h3>
          <ul class="space-y-3">
            <li v-for="link in productLinks" :key="link.name">
              <RouterLink
                :to="{ name: link.route }"
                class="text-accent-foreground/80 hover:text-accent-foreground transition-colors"
              >
                {{ link.name }}
              </RouterLink>
            </li>
          </ul>
        </div>

        <div>
          <h3 class="font-bold text-lg mb-4">Company</h3>
          <ul class="space-y-3">
            <li v-for="link in companyLinks" :key="link.name">
              <RouterLink
                :to="{ name: link.route }"
                class="text-accent-foreground/80 hover:text-accent-foreground transition-colors"
              >
                {{ link.name }}
              </RouterLink>
            </li>
          </ul>
        </div>

        <div>
          <h3 class="font-bold text-lg mb-4">Legal</h3>
          <ul class="space-y-3">
            <li v-for="link in legalLinks" :key="link.name">
              <RouterLink
                :to="{ name: link.route }"
                class="text-accent-foreground/80 hover:text-accent-foreground transition-colors"
              >
                {{ link.name }}
              </RouterLink>
            </li>
          </ul>
        </div>
      </div>

      <div class="pt-8 border-t border-accent-foreground/20 text-center text-accent-foreground/70">
        <p>&copy; {{ currentYear }} Poop & Pour. All rights reserved.</p>
      </div>
    </div>
  </footer>
</template>
