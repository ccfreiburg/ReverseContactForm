<script setup lang="ts">
import { onBeforeMount, ref } from "vue";
import { useI18n } from 'vue-i18n';
import { useRouter } from "vue-router";
import { useAuthStore } from '../stores/auth.store';
import { fetchWrapper } from "../services"
import Header from '../components/Header.vue';
import ButtonPrimary from '../components/ButtonPrimary.vue';
import ButtonSecondary from '../components/ButtonSecondary.vue';

const { t } = useI18n();

const isopen = ref(false);
const openitem = ref({} as any);
const list = ref([] as Array<any>);
const router = useRouter();

function logout() {
  const authStore = useAuthStore();
  authStore.logout();
  router.push("/")
}
function refresh() {
  fetchWrapper.get("/api/template")
    .then((obj) => {
      list.value = obj;
    });
}
onBeforeMount(() => {
  refresh()
});
function open(id: number) {
  const item = list.value.find((item: any) => item.id == id) as any;
  openitem.value = { ...item as object }
  isopen.value = true;
}
function cancel() {
  isopen.value = false
}
function add() {
  openitem.value={
    "active": true,
    "purpose": "",
    "to": "",
    "from": "",
    "subject": "",
    "html": ""
  }
  isopen.value = true
}
function remove() {  
  fetchWrapper.delete("/api/template?id="+openitem.value.id).then(()=>{
      cancel()
      refresh()
    })
}
function copy() {  
  delete openitem.value.id
  openitem.value.purpose += " (copy)"
}
function submit() {
  if (openitem.value.id) {
    fetchWrapper.put("/api/template",openitem.value).then(()=>{
      refresh()
      cancel()
    })
  } else {
    fetchWrapper.post("/api/template",openitem.value).then(()=>{
      refresh()
      cancel()
    })
  }
}
</script>

<template>
  <div class="w-screen h-full bg-white">
    <Header :noimage="true"/>
    <div class="flex flex-col items-center mx-6 overflow-x-auto">
    <div class="flex items-center w-full h-12 place-content-end ">
        <button @click="logout" class="px-2 text-skin-muted hover:text-skin-accent">{{t('logout')}}</button>
    </div>
    <table class="w-full text-sm text-left text-skin-muted">
        <thead class="text-xs uppercase text-skin-base bg-skin-muted">
            <tr>
                <th scope="col" class="px-6 py-3">
                  {{t('templates.active')}}
                </th>
                <th scope="col" class="px-2 py-3">
                  {{t('templates.slug')}}  
                </th>
                <th scope="col" class="px-6 py-3">
                  {{t('templates.purpose')}}  
                </th>
                <th scope="col" class="px-6 py-3">
                  {{t('templates.to')}}
                </th>
                <th scope="col" class="px-6 py-3">
                  {{t('templates.subject')}}
                </th>
            </tr>
        </thead>
        <tbody>
          <tr class="border-b bg-skin-light hover:text-skin-accent"
            v-for="item in list"
            @click="open(item.id)"
          >
            <td class="px-6 py-4">
              <div v-if="item.active">+</div>
              <div v-else>-</div>
            </td>
            <td class="px-6 py-4">{{ item.slug }}</td>
            <td class="px-6 py-4">{{ item.purpose }}</td>
            <td class="px-6 py-4">{{ item.to }}</td>
            <td class="px-6 py-4">{{ item.subject }}</td>
          </tr>
        </tbody>
      </table>
    <div v-if="isopen" class="w-2/3 border-b bg-skin-light">
        <from> 
          <div class="grid gap-6 mt-10 columns-3">
          <div class="flex items-center mt-6">
            <input checked id="checked-checkbox" type="checkbox" v-model="openitem.acitve" class="w-8 h-8 text-skin-accent bg-skin-light border-skin-light focus:ring-skin-focus focus:ring-2">
            <label for="checked-checkbox" class="ml-2 text-sm font-medium text-skin-muted ">{{t('templates.active')}}</label>
          </div>
          <div>
            <label for="purpose" class="block mb-2 text-sm font-medium text-skin-muted">{{t('templates.purpose_full')}}</label>
            <input type="text" v-model="openitem.purpose" id="purpose" class="block w-full p-2.5" placeholder="Purpose" required>
          </div>          
          <div>
            <label for="slug" class="block mb-2 text-sm font-medium text-skin-muted">{{t('templates.slug')}}</label>
            <input type="text" v-model="openitem.slug" id="slug" class="block w-full p-2.5" placeholder="Slug" required>
          </div>
          <div class="col-span-3">
            <label for="to" class="block mb-2 text-sm font-medium text-skin-muted">{{t('templates.to_full')}}</label>
            <input type="text" v-model="openitem.to" id="to" class="block w-full p-2.5" placeholder="To" required>
          </div>
          <div class="col-span-3">
            <label for="from" class="block mb-2 text-sm font-medium text-skin-muted">{{t('templates.from_full')}}</label>
            <input type="text" v-model="openitem.from" id="from" class="block w-full p-2.5" placeholder="From" required>
          </div>
          <div class="col-span-3">
            <label for="subject" class="block mb-2 text-sm font-medium text-skin-muted">{{t('templates.subject')}}</label>
            <input type="text" v-model="openitem.subject" id="subject" class="block w-full p-2.5" placeholder="Subject" required>
          </div>
          <div class="col-span-3">
            <label for="html" class="block mb-2 text-sm font-medium text-skin-muted">{{t('templates.text')}}</label>
            <textarea type="text" v-model="openitem.html" id="html" class="h-40  block w-full p-2.5" placeholder="Message text" required />
          </div>
        <div class="flex justify-end col-span-3 mt-4">
          <ButtonSecondary cancel @click="cancel">{{t('cancel')}}</ButtonSecondary>
          <ButtonSecondary submit @click="copy">{{t('copy')}}</ButtonSecondary>
          <ButtonSecondary submit @click="remove">{{t('delete')}}</ButtonSecondary>
          <ButtonPrimary submit @click="submit">{{t('OK')}}</ButtonPrimary>
        </div>
        </div>
      </from>
      </div>
    <div v-else class="flex justify-end w-2/3 mt-10">
      <ButtonPrimary @click="add">{{t('templates.add')}}</ButtonPrimary>
    </div>
    </div>
  </div>
</template>
