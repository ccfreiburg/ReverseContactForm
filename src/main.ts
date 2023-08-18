import './style.css'
import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { createI18n } from 'vue-i18n'

import App from "./App.vue";
import { router } from "./router";
import en from './locales/en.json';
import de from './locales/de.json';

type MessageSchema = typeof en
const config = {
  legacy: false,
  locale: "de",
  fallbackLocale: "de",
  availableLocales: ["de","en"],
  messages: {
    de,
    en
  },
}

const i18n = createI18n<[MessageSchema], 'en' | 'de'>(config);
const app = createApp(App);

app.use(createPinia());
app.use(router);
app.use(i18n);

app.mount("#app");

export default config.availableLocales