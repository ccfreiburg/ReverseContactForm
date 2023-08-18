<script setup lang="ts">
import { ref } from "vue";
import { useAuthStore } from "../stores/auth.store";
import { useI18n } from 'vue-i18n';
import ButtonPrimary from "../components/ButtonPrimary.vue";
import Header from "../components/Header.vue";

const { t } = useI18n();
const authStore = useAuthStore();
//const { user: authUser } = storeToRefs(authStore);

const error = ref("");
const username = ref("");
const password = ref("");

async function submit() {
    error.value = ""
    try {
        await authStore.login(username.value, password.value);
    } catch (e) {
        error.value = t('passerror')
        password.value = ""
    }
}
</script>

<template>
  <div>
    <Header></Header>
  <div class="flex flex-col items-center w-full mt-12">
    <div class="w-1/3 bg-skin-light">

    <div class="grid w-full gap-6 mt-10 columns-2" v-on:keyup.enter="submit">
      <div class="col-span-2">
        <label
          for="purpose"
          class="block mb-2 text-sm font-medium"
          >{{ t('username') }}</label>
        <input
          type="text"
          v-model="username"
          id="purpose"
          class="block w-full p-2.5"
          required
        />
      </div>
      <div class="col-span-2">
        <label for="to" class="block mb-2 text-sm font-medium"
        >{{ t('password') }}</label>
        <input
          type="password"
          v-model="password"
          id="to"
          class="block w-full p-2.5"
          required
        />
      </div>
    </div>
    <div class="h-10 pt-2 text-red-600">
        <div v-if="error.length>0">
        {{ error }}
        </div>
    </div>
      <div class="flex justify-end mt-2">
        <ButtonPrimary @click="submit">{{ t('OK') }}</ButtonPrimary>
      </div>
    </div>
  </div>
</div>
</template>
