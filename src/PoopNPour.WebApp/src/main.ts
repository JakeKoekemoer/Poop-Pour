import './assets/css/index.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import PrimeVue from 'primevue/config'
import ToastService from 'primevue/toastservice'
import router from './routes'
import App from './App.vue'
import { useUserStore } from './stores'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)

const userStore = useUserStore()
userStore.initializeAuth()

app.use(router)
app.use(PrimeVue, { unstyled: true })
app.use(ToastService)
app.mount('#app')
