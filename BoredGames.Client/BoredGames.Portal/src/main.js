import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import Toast from "vue-toastification"
import { plugin, defaultConfig } from '@formkit/vue'
import formkitConfig from './formkit.config.js'
import Argon from '@/plugins/argon-kit'


import "vue-toastification/dist/index.css";

const app = createApp(App)

app.use(Argon);
app.use(router)
app.use(Toast)
app.use(plugin, defaultConfig(formkitConfig))

app.mount('#app')
