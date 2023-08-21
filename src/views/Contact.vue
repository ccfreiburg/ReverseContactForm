<script setup lang="ts">
import { ref, computed } from "vue";
import { fetchWrapper, validateEmail } from "../services"
import Header from "../components/Header.vue";
import CleanContainer from "../components/CleanContainer.vue";
import ButtonPrimary from "../components/ButtonPrimary.vue";
import { useI18n } from "vue-i18n";

const { t, locale } = useI18n()

const email = ref("");

const thanks = ref(false)
const options = ref([] as Array<any>)
const purpose = ref(0)
const validated = computed(()=>{
    return validateEmail(email.value)
})
function refresh() {
    fetchWrapper.get("/api/Template/purposes").then(
      (x)=> {
        var id = 0;
        options.value = [] as Array<any>
        x.forEach(element => {
          options.value.push({
            id,
            value: element
        })
        });
      }
    )
    if (options.value.length>0)
      purpose.value = options.value[0]
}
refresh()
function submit() {
  fetchWrapper.postraw("/api/Validation", 
  {
    email: email.value,
    language: (locale.value && locale.value!="de"?locale.value:""),
    purpose: options.value[purpose.value].value
  }).then(()=> {
    thanks.value = true
  })
}
</script>

<template>
  <div>
    <Header :title="(thanks?t('thankyou'):t('contactform.title'))" 
    :intro="(thanks?t('contactform.check_your_email'):t('contactform.intro'))" />
    <CleanContainer>
    <div v-if="!thanks">
      <div class="flex flex-col">
        <div>
            <label for="purpose" class="block mb-2 text-sm font-medium text-skin-muted">
            {{t('contactform.who_to_contact')}} 
              </label>
            <select id="purpose" class="block w-full p-2.5" v-model="purpose">
                <option v-for="option in options" :key="option.id" :value="option.id">{{ option.value }}</option>
            </select>
        </div>
        <div class="mt-6">
          <label
            for="purpose"
            class="block mb-2 text-sm font-medium text-skin-muted"
            >
            {{t('contactform.your_email')}} 
            </label
          >
          <input
            type="text"
            v-model="email"
            id="purpose"
            class="block w-full p-2.5"
            required
          />
        </div>
        <div class="flex mt-8 place-content-end">
            <ButtonPrimary :disabled="!validated" @click="submit">
              {{t('OK')}} 
              </ButtonPrimary>
        </div>
      </div>
    </div>

    </CleanContainer>
  </div>
</template>
