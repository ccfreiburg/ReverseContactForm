<script setup lang="ts">
import { onBeforeMount, ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from '../stores/auth.store';
import { fetchWrapper } from "../services"
import ButtonPrimary from '../components/ButtonPrimary.vue';
import ButtonSecondary from '../components/ButtonSecondary.vue';

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
  <div class="w-screen h-full bg-white border-b">
    <div class="flex flex-col items-center mx-6 overflow-x-auto">
    <div class="flex items-center w-full h-12 place-content-end ">
        <button @click="logout" class="px-2 text-skin-muted hover:text-skin-accent">Logout</button>
    </div>
    <table class="w-full text-sm text-left text-skin-muted">
        <thead class="text-xs uppercase text-skin-base bg-skin-muted">
            <tr>
                <th scope="col" class="px-6 py-3">
                  Active
                </th>
                <th scope="col" class="px-6 py-3">
                  Purpose
                </th>
                <th scope="col" class="px-6 py-3">
                  To
                </th>
                <th scope="col" class="px-6 py-3">
                  Subject
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
            <td class="px-6 py-4">{{ item.purpose }}</td>
            <td class="px-6 py-4">{{ item.to }}</td>
            <td class="px-6 py-4">{{ item.subject }}</td>
          </tr>
        </tbody>
      </table>
    <div v-if="isopen" class="w-2/3 border-b bg-skin-light">
        <from> 
          <div class="grid gap-6 mt-10 columns-2">
          <div class="flex items-center mt-6">
            <input checked id="checked-checkbox" type="checkbox" v-model="openitem.acitve" class="w-8 h-8 text-skin-accent bg-skin-light border-skin-light focus:ring-skin-focus focus:ring-2">
            <label for="checked-checkbox" class="ml-2 text-sm font-medium text-skin-muted ">Active</label>
          </div>
          <div>
            <label for="purpose" class="block mb-2 text-sm font-medium text-skin-muted">Purpose of Email</label>
            <input type="text" v-model="openitem.purpose" id="purpose" class="block w-full p-2.5" placeholder="Purpose" required>
          </div>
          <div class="col-span-2">
            <label for="to" class="block mb-2 text-sm font-medium text-skin-muted">Send Email to</label>
            <input type="text" v-model="openitem.to" id="to" class="block w-full p-2.5" placeholder="To" required>
          </div>
          <div class="col-span-2">
            <label for="from" class="block mb-2 text-sm font-medium text-skin-muted">Email from</label>
            <input type="text" v-model="openitem.from" id="from" class="block w-full p-2.5" placeholder="From" required>
          </div>
          <div class="col-span-2">
            <label for="subject" class="block mb-2 text-sm font-medium text-skin-muted">Subject</label>
            <input type="text" v-model="openitem.subject" id="subject" class="block w-full p-2.5" placeholder="Subject" required>
          </div>
          <div class="col-span-2">
            <label for="html" class="block mb-2 text-sm font-medium text-skin-muted">Text</label>
            <textarea type="text" v-model="openitem.html" id="html" class="h-40  block w-full p-2.5" placeholder="Message text" required />
          </div>
        <div class="flex justify-end col-span-2 mt-4">
          <ButtonSecondary cancel @click="cancel">Cancel</ButtonSecondary>
          <ButtonSecondary submit @click="remove">Delete</ButtonSecondary>
          <ButtonPrimary submit @click="submit">Ok</ButtonPrimary>
        </div>
        </div>
      </from>
      </div>
    <div v-else class="flex justify-end w-2/3 mt-10">
      <ButtonPrimary @click="add">Add</ButtonPrimary>
    </div>
    </div>
  </div>
</template>
