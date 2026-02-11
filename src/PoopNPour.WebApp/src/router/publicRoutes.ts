import type { RouteRecordRaw } from 'vue-router'

export const publicRoutes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'coming-soon',
    component: () => import('@/views/ComingSoonView.vue'),
  },
]
