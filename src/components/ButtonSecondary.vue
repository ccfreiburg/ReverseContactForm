<script setup lang="ts"></script>
<template>
<button class="inline-flex items-center justify-center p-0.5 mb-2 mr-2 
overflow-hidden text-sm font-medium text-skin-base rounded-md group
bg-gradient-to-r from-skin-from via-skin-via 
    to-skin-to
   hover:text-skin-inverted dark:text-skin-inverted
   focus:outline-none 
    focus:ring-skin-focus 
    dark:focus:ring-skin-dark
  focus:ring-4" >
    <span class="px-5 py-2 transition-all duration-75 ease-in bg-white rounded-sm dark:bg-gray-900 group-hover:bg-opacity-0">
        <slot></slot>
    </span>
  </button>
</template>

