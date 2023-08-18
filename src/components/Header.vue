<template>
    <div>
      <div class="flex items-center w-full h-20 shadow-md place-content-center">

      <div class="w-10/12
                          md:w-9/12
                          xl:w-8/12
                          2xl:w-[1536px] 
                          flex justify-between">
        <div class="relative flex flex-row items-center w-1/6 flex-nowrap">
          <button id="language_id" @click.stop="showDropdown=true" class="inline-flex items-center text-sm font-medium text-center rounded-lg"
            type="button"> 
            <svg class="w-3 h-3 sm:w-4 sm:h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"
              xmlns="http://www.w3.org/2000/svg">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M21 12a9 9 0 01-9 9m9-9a9 9 0 00-9-9m9 9H3m9 9a9 9 0 01-9-9m9 9c1.657 0 3-4.03 3-9s-1.343-9-3-9m0 18c-1.657 0-3-4.03-3-9s1.343-9 3-9m-9 9a9 9 0 019-9">
              </path>
            </svg>
            <div class="invisible w-0 text-xs sm:w-14 sm:visible">{{ t(locale) }}</div>
            <svg class="invisible w-4 h-4 sm:visible" aria-hidden="true" fill="none" stroke="currentColor"
              viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path>
            </svg>
          </button>
          <div v-show="showDropdown" class="absolute bg-white border-2 rounded shadow top-10 left-6 w-44">
            <ul class="py-1 text-sm text-gray-700">
              <li v-for="navLocale in availableLocales as string[]" :key="navLocale" class="p-1 hover:cursor-pointer" @click="showDropdown=false">
                  <RouterLink :to="getRouterLink(navLocale)">{{ t(navLocale) }}</RouterLink>
              </li>
            </ul>
          </div>
        </div>
        <div class="content-center flex-grow text-center align-middle">
          <a href="https://ccfreiburg.de" class="inline-block">
            <img :src="logo" class="m-1 h-7 sm:h-14"/>
          </a>
        </div>
        <div class="relative flex flex-row items-center w-1/6 flex-nowrap">&nbsp;
        </div>
      </div>
      </div>
    <section class="flex flex-col place-items-center">
      <div v-if="!noimage" class="w-11/12 overflow-hidden -inset-y-8 sm:h-56 md:h-64 lg:h-72">
          <img class="object-cover w-full" :src="hero" alt="" />
      </div>
      <CleanContainer v-if="$props.title">
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
import { onBeforeUnmount, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import CleanContainer from '../components/CleanContainer.vue';
import { useRoute } from 'vue-router';

export default {
    props: {
      title: String,
      intro: String,
      noimage: Boolean,
    },
    components: {CleanContainer},
    name: "Header",
    setup() {
      const showDropdown = ref(false)
      const { t, availableLocales, locale } = useI18n();
      const route = useRoute()

      function hide() {
      showDropdown.value = false;
      //ctx.emit('closeAll')
    }
    function closeIfClickedOutside(event: any) {
      if (
        event.target.name === 'language_id' ||
        event.target.nodeName === 'OPTION' ||
        event.target.nodeName === 'LI'
      )
        return;
        hide();
    }
    function getRouterLink( local : string ) :string {
      const r = route.path.substring(3,4)
      if (r=="/")
        return (local=="de"?"":"/"+local)+route.path.substring(3)
      return (local=="de"?"":"/"+local)+route.path
    }

    function setLocaleFromPath() {
      const routstarts = availableLocales.filter((i)=>i!="de").map((i)=>"/"+i+"/")
      if (routstarts.some(substr => route.path.startsWith(substr))) {
        const loc = route.path.substring(1,3)
        locale.value = loc
      } else locale.value = "de"
    }

    watch(()=>route.path, setLocaleFromPath )

    function closeIfScrolled(event: any) {
      closeIfClickedOutside(event)
    }
    onMounted(() => {
      document.addEventListener('click', closeIfClickedOutside);
      document.addEventListener('scroll', closeIfScrolled);
      setLocaleFromPath()
    })

    onBeforeUnmount(() => {
      document.removeEventListener('click', closeIfClickedOutside);
      document.removeEventListener('scroll', closeIfScrolled);
    })
      return {
        logo: new URL('../assets/images/logo.png', import.meta.url).href,
        hero: new URL('../assets/images/Calvary Chapel 22 Winter UhlArt Fotogrfie 2022-61-Header.jpg', import.meta.url).href,
        showDropdown,
        t, 
        availableLocales,
        locale,
        getRouterLink
      }
    }
}
</script>