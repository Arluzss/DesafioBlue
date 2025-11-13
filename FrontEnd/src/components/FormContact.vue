<template>
  <div v-if="newContact">
    <DataTable :value="[newContact]" responsiveLayout="scroll" class="p-datatable-sm mb-3">
      <Column header="Nome">
        <template #body="{ data }">
          <InputText v-model="data.name" />
        </template>
      </Column>
      <Column header="Email">
        <template #body="{ data }">
          <InputText v-model="data.email" />
        </template>
      </Column>
      <Column header="Telefone">
        <template #body="{ data }">
          <InputText v-model="data.phone" />
        </template>
      </Column>
      <Column header="Ações">
        <template #body>
          <Button
            icon="pi pi-check"
            class="p-button-text p-button-success p-button-sm"
            @click="$emit('save')"
          />
          <Button
            icon="pi pi-times"
            class="p-button-text p-button-danger p-button-sm"
            @click="$emit('cancel')"
          />
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<script setup>
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'

const props = defineProps({
  newContact: Object
})

</script>
