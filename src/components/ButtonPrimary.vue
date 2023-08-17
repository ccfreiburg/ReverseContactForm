<script setup lang="ts"></script>
<template>
    <div>
  <button ref="theButton" :disabled="disabled"
    class="disabled:cursor-not-allowed py-2.5 px-0.5 text-center rounded-md group bg-gradient-to-r from-skin-from via-skin-via to-skin-to focus:ring-4 text-skin-inverted focus:outline-none focus:ring-skin-focus dark:focus:ring-skin-dark'"
  >
    <span v-if="!disabled" class="px-5 py-2.5 transition-all ease-in duration-75 rounded-md group-hover:bg-opacity-50 group-hover:bg-white hover:text-skin-base dark:hover:text-skin-inverted"> 
        <slot></slot>
    </span>
    <span v-else class="px-5 py-2.5 bg-white bg-opacity-60" >
        <slot></slot>
    </span>
  </button>
</div>
</template>
<script setup>
const props = defineProps({
  disabled: Boolean
})
</script>