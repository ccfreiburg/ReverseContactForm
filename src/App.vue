<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router';
import { useAuthStore } from './stores';
const authStore = useAuthStore();
</script>

<template>
    <div class="app-container">
        <RouterView />
    </div>
</template>
