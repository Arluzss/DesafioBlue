<template>
  <Card class="m-4 shadow-2">
    <template #title>Lista de Contatos</template>
    <template #subtitle>Gerencie seus contatos aqui</template>

    <template #content>
      <div class="mb-3">
        <Button label="Novo Contato" icon="pi pi-plus" class="p-button-sm p-button-success" @click="add" />
      </div>

      <FormContact :newContact="newContact" @save="saveContact" @cancel="cancelSaveContact" />

      <DataTable :value="contacts" responsiveLayout="scroll" class="p-datatable-sm">
        <Column header="Nome">
          <template #body="{ data }">
            <EditableCell v-model="data.name" :isEditing="editing === data.id" />
          </template>
        </Column>

        <Column header="Email">
          <template #body="{ data }">
            <EditableCell v-model="data.email" :isEditing="editing === data.id" />
          </template>
        </Column>

        <Column header="Telefone">
          <template #body="{ data }">
            <EditableCell v-model="data.phone" :isEditing="editing === data.id" />
          </template>
        </Column>

        <Column header="Ações" style="width: fit-content; text-align: center">
          <template #body="{ data }">
            <div class="flex gap-2">
              <Button icon="pi pi-eye" class="p-button-text p-button-sm" @click=""/>
              <Button v-if="editing !== data.id" icon="pi pi-pencil" class="p-button-text p-button-sm"
                @click="edit(data.id)" />
              <Button v-if="editing === data.id" icon="pi pi-check" class="p-button-text p-button-success p-button-sm"
                @click="changeContact(data)" />
              <Button icon="pi pi-trash" class="p-button-text p-button-danger p-button-sm" @click="remove(data.id)" />
            </div>
          </template>
        </Column>
      </DataTable>
    </template>
  </Card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import Card from 'primevue/card'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import InputText from 'primevue/inputtext'
import EditableCell from './EditableCell.vue'
import ContactService from '../services/ContactService'
import FormContact from './FormContact.vue'

var contacts = ref([])

const editing = ref(null)
const newContact = ref(null)

async function loadContacts() {
  const data = await ContactService.listContacts();
  contacts.value = data;
}

onMounted(loadContacts);

const edit = (id) => {
  editing.value = id
}

async function changeContact(data) {
  await ContactService.updateContact(data.id, data);
  await loadContacts();
  editing.value = null
}

async function remove(id) {
  await ContactService.deleteContact(id);
  loadContacts();
}

const add = () => {
  newContact.value = {
    id: Date.now(),
    name: 'Nome',
    email: 'Email',
    phone: 'Telefone'
  }
}

async function saveContact() {
  await ContactService.createContact(newContact.value)
  newContact.value = null
  loadContacts()
}

const cancelSaveContact = () => {
  newContact.value = null
}

</script>

<style scoped>
.p-datatable td {
  vertical-align: middle;
}
</style>
