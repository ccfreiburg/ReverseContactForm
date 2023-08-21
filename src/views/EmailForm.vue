<script setup lang="ts">
import { ref, computed, onBeforeMount } from "vue";
import { useRoute } from "vue-router";
import { useI18n } from "vue-i18n";
import { fetchWrapper } from "../services"

import Header from "../components/Header.vue";
import CleanContainer from "../components/CleanContainer.vue";
import ButtonPrimary from "../components/ButtonPrimary.vue";

const route = useRoute()
const { t } = useI18n();

const thanks = ref(false)
const error = ref("")
const message = ref("")
const subject = ref("")
const valid = ref("")
const title = computed(()=>
        (error.value.length>0?t('error'):(thanks.value?t('thankyou'):t('emailform.title')))
        )
const intro = computed(()=>
        (error.value.length>0?t('emailform.invalid'):(thanks.value?t('emailform.done'):t('emailform.intro')))
        )
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

onBeforeMount(()=>thanks.value=false)
</script>

<template>
    <div>
        <Header :title="title" 
                :intro="intro" />

        <CleanContainer v-if="error.length==0 && !thanks">
            <div class="flex flex-col w-full">
                <div>
                    {{t('emailform.your_email_to') +": "+ valid }}
                </div>
                <div class="col-span-2">
                    <label for="subject" class="block mb-2 text-sm font-medium text-skin-muted">
                        {{t('emailform.subject')}}
                        </label>
                    <input type="text" v-model="subject" id="subject" class="block w-full p-2.5" placeholder="Subject" required />
                </div>
                <div class="col-span-2">
                    <label for="html" class="block mb-2 text-sm font-medium text-skin-muted">
                        {{t('emailform.message')}}
                        </label>
                    <textarea type="text" v-model="message" id="html" class="h-40  block w-full p-2.5" placeholder="Message text" required />
                </div>
                <div class="flex mt-8 place-content-end">
                    <ButtonPrimary :disabled="!validated" @click="submit">{{t('emailform.send')}}</ButtonPrimary>
                </div>
            </div>
        </CleanContainer>
    </div>
</template>