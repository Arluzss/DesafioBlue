<template>
  <Card class="m-4 shadow-2">
    <template #content>
      <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem;">
        <div>
          <h2>Lista de Contatos</h2>
        </div>
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

        <Column header="Ações" style="width: fit-content;">
          <template #body="{ data }">
            <div class="flex justify-center gap-2 w-full">
              <Button icon="pi pi-eye" class="p-button-text p-button-sm" @click="" />
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
import EditableCell from '../components/EditableCell.vue'
import ContactService from '../services/ContactService'
import FormContact from '../components/FormContact.vue'
import { useToast } from "primevue";

const toast = useToast();

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
  try {
    await ContactService.updateContact(data.id, data);
    await loadContacts();
    editing.value = null

    toast.add({
      severity: 'success',
      summary: 'Sucesso',
      detail: 'Contato atualizado com sucesso!',
      life: 3000
    });
  } catch (error) {
    const errors = error.response.data.errors;

    Object.keys(errors).forEach(field => {
      toast.add({
        severity: 'error',
        summary: `Erro no campo ${field}`,
        detail: errors[field].join(', '),
        life: 4000
      });
    });
  }
}

async function remove(id) {
  try {
    await deleteContact(id);
    loadContacts();
  } catch (error) {
    toast.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao deletar contato!', life: 3000 });
  }
}

const add = () => {
  newContact.value = {
    name: '',
    email: '',
    phone: ''
  }
}

async function saveContact() {
  try {
    await ContactService.createContact(newContact.value)
    newContact.value = null
    loadContacts()
    toast.add({
      severity: 'success',
      summary: 'Sucesso',
      detail: 'Contato criado com sucesso!',
      life: 3000
    });
  } catch (error) {
    const errors = error.response.data.errors;

    Object.keys(errors).forEach(field => {
      toast.add({
        severity: 'error',
        summary: `Erro no campo ${field}`,
        detail: errors[field].join(', '),
        life: 4000
      });
    });
  }
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
