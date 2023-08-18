<template>
    <div>
        <div class="flex items-center w-full h-20 shadow-md place-content-center">
            <img class="h-12" :src="logo" />
        </div>
    <section class="flex flex-col place-items-center">
      <div class="w-11/12 overflow-hidden -inset-y-8 sm:h-56 md:h-64 lg:h-72">
          <img class="object-cover w-full" :src="hero" alt="" />
      </div>
      <CleanContainer>
        <div class="flex flex-col items-center justify-center md:mt-4">
            <div class="leading-tight sm:px-2 menu-underline">
      <h1 class="font-bold tracking-widest uppercase sm:text-xl md:text-2xl text-md">
        {{ $props.title }}
      </h1>
    </div>
          <div class="w-3/5 mt-6">
            <p class="md:leading-loose font-bold text-xs md:text-sm tracking-wider text-center text-[#888888]">
              {{ $props.intro }}</p>
          </div>
        </div>
    </CleanContainer>
    </section>
</div>
  </template>
  
<script lang="ts">
import CleanContainer from '../components/CleanContainer.vue';

export default {
    props: {
      title: String,
      intro: String,
    },
    components: {CleanContainer},
    name: "Header",
    setup() {
      return {
        logo: new URL('../assets/images/logo.png', import.meta.url).href,
        hero: new URL('../assets/images/Calvary Chapel 22 Winter UhlArt Fotogrfie 2022-61-Header.jpg', import.meta.url).href

      }
    }
}
</script>