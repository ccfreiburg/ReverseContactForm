<script setup lang="ts">
import { ref, computed } from "vue";
import { useRoute } from "vue-router";
import { fetchWrapper } from "../services"

import Header from "../components/Header.vue";
import CleanContainer from "../components/CleanContainer.vue";
import ButtonPrimary from "../components/ButtonPrimary.vue";
const route = useRoute()

const intro = ref(`Our information on the internet is free from any privacy related functionality. 
  It is not using cookies nor external embedded content nor tracking. 
  But for the contact form we expect you to provide your email address first in order to
  protect us and our staff. 
  After you have entered your address you will receive an email with the link to the
  web form, where you can send your message to us. We are looking forward to hear from you!
  `
);
const thanks = ref(false)
const error = ref("")
const message = ref("")
const subject = ref("")
const valid = ref("")
const validated = computed(()=>{
    return message.value.length > 3 
})
async function refresh() {
    error.value = ""
    try {
        await fetchWrapper.get("/api/Validation?slug="+route.params.slug)
            .then((o)=> {
                valid.value= o.purpose
            })
    } catch (e) {
        error.value = "This Link is not valid"
    }
}
refresh()
function submit() {
 const data = {
    slug: route.params.slug,
    subject: subject.value,
    message: message.value
 }
 fetchWrapper.postraw("/api/message",data).then(()=>thanks.value=true)
}
</script>

<template>
    <div>
        <Header :title="'Send Email'" :intro="intro" />
        <CleanContainer>
            <div v-if="error.length>0" class="flex flex-col w-full">
                {{ error }}
            </div>
            <div v-else-if="!thanks" class="flex flex-col w-full">
                <div>
                    Your Email to {{ valid }}
                </div>
                <div class="col-span-2">
                    <label for="subject" class="block mb-2 text-sm font-medium text-skin-muted">Subject</label>
                    <input type="text" v-model="subject" id="subject" class="block w-full p-2.5" placeholder="Subject" required />
                </div>
                <div class="col-span-2">
                    <label for="html" class="block mb-2 text-sm font-medium text-skin-muted">Your Message</label>
                    <textarea type="text" v-model="message" id="html" class="h-40  block w-full p-2.5" placeholder="Message text" required />
                </div>
                <div class="flex mt-8 place-content-end">
                    <ButtonPrimary :disabled="!validated" @click="submit">Ok</ButtonPrimary>
                </div>
            </div>
            <div v-else class="flex flex-col items-center justify-center md:mt-4">
                <div class="leading-tight sm:px-2 menu-underline">
                    <h1 class="font-bold tracking-widest uppercase sm:text-xl md:text-2xl text-md">
                        Thank you!
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