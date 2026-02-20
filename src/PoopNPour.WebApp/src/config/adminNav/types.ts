import type { Component } from 'vue'

export interface AdminNavItem {
  label: string
  routeName: string
  icon: Component
  showOnDashboard: boolean
}
