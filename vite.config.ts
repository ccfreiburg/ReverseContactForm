import { fileURLToPath, URL } from 'url';
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
        '~': fileURLToPath(new URL('./', import.meta.url)),
        'assets': fileURLToPath(new URL('./src/assets', import.meta.url))
      }
    },
    server: {
      strictPort: true,
      hmr: {
          port: 9000
      },
  },
})
