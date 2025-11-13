<template>
  <div>
    <InputText
      v-if="isEditing"
      :model-value="modelValue"
      @update:model-value="$emit('update:modelValue', $event)"
    />
    <span v-else>{{ modelValue }}</span>
  </div>
</template>

<script setup>
import InputText from 'primevue/inputtext'

defineProps({
  modelValue: [String, Number],
  isEditing: Boolean
})

defineEmits(['update:modelValue'])
</script>
