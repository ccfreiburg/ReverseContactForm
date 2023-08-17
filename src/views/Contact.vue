<script setup lang="ts">
import { ref, computed } from "vue";
import { fetchWrapper, validateEmail } from "../services"
import Header from "../components/Header.vue";
import CleanContainer from "../components/CleanContainer.vue";
import ButtonPrimary from "../components/ButtonPrimary.vue";
const email = ref("");
const intro = ref(
  `Our information on the internet is free from any privacy related functionality. 
  It is not using cookies nor external embedded content nor tracking. 
  But for the contact form we expect you to provide your email address first in order to
  protect us and our staff. 
  After you have entered your address you will receive an email with the link to the
  web form, where you can send your message to us. We are looking forward to hear from you!
  `
);
const thanks = ref(false)
const options = ref([])
const purpose = ref(0)
const validated = computed(()=>{
    return validateEmail(email.value)
})
function refresh() {
    fetchWrapper.get("/api/Template/purposes").then((x)=> options.value=x)
}
refresh()
function submit() {
  fetchWrapper.postraw("/api/Validation", 
  {
    email: email.value,
    purpose: options.value[purpose.value]
  }).then(()=> {
    thanks.value = true
  })
}
</script>

<template>
  <div>
    <Header :title="'Send Email'" :intro="intro" />
    <CleanContainer>
    <div v-if="!thanks">
      <div class="flex flex-col">
        <div>
            <label for="purpose" class="block mb-2 text-sm font-medium text-skin-muted">Who do you want to contact</label>
            <select id="purpose" class=" block w-full p-2.5" v-model="purpose">
                <option v-for="(option, index) in options" :value="index">{{ option }}</option>
            </select>
        </div>
        <div class="mt-6">
          <label
            for="purpose"
            class="block mb-2 text-sm font-medium text-skin-muted"
            >Your Email Address</label
          >
          <input
            type="text"
            v-model="email"
            id="purpose"
            class="block w-full p-2.5"
            placeholder="Email address"
            required
          />
        </div>
        <div class="flex mt-8 place-content-end">
            <ButtonPrimary :disabled="!validated" @click="submit">Ok</ButtonPrimary>
        </div>
      </div>
    </div>
    <div v-else class="flex flex-col items-center justify-center md:mt-4">
        <div class="leading-tight sm:px-2 menu-underline">
            <h1 class="font-bold tracking-widest uppercase sm:text-xl md:text-2xl text-md">
        T       hank you!
            </h1>
        </div>
        <div class="w-3/5 mt-6">
            <p class="md:leading-loose font-bold text-xs md:text-sm tracking-wider text-center text-[#888888]">
                Please check your Inbox and maybe your Spam folder.
            </p>
         </div>
    </div>
    </CleanContainer>
  </div>
</template>
