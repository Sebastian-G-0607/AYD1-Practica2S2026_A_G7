<script setup lang="ts">
import { ref } from 'vue'

interface Props {
  id: string
  modelValue: string
  label?: string
  type?: 'text' | 'email' | 'password'
  placeholder?: string
  icon?: string
  error?: string
  disabled?: boolean
}

withDefaults(defineProps<Props>(), {
  type: 'text',
  placeholder: '',
  disabled: false,
})

const emit = defineEmits<{
  (e: 'update:modelValue', value: string): void
}>()

const showPassword = ref(false)

function togglePassword() {
  showPassword.value = !showPassword.value
}

function handleInput(event: Event) {
  const target = event.target as HTMLInputElement
  emit('update:modelValue', target.value)
}
</script>

<template>
  <div class="w-full flex flex-col gap-1.5 text-left">
    <label
      v-if="label"
      :for="id"
      class="text-xs font-semibold tracking-wider text-on-surface/80 uppercase font-label-md"
    >
      {{ label }}
    </label>

    <div class="relative flex items-center">
      <!-- Left Icon -->
      <span
        v-if="icon"
        class="material-symbols-outlined absolute left-3.5 text-on-surface/40 select-none text-xl pointer-events-none transition-colors duration-200"
      >
        {{ icon }}
      </span>

      <input
        :id="id"
        :type="type === 'password' ? (showPassword ? 'text' : 'password') : type"
        :value="modelValue"
        :placeholder="placeholder"
        :disabled="disabled"
        @input="handleInput"
        :class="[
          'w-full bg-surface-dim/80 border text-on-surface placeholder:text-on-surface/30 rounded-lg py-3 text-sm font-body-md transition-all duration-200 outline-none',
          icon ? 'pl-11' : 'pl-4',
          type === 'password' ? 'pr-11' : 'pr-4',
          error
            ? 'border-error/80 focus:border-error focus:ring-1 focus:ring-error'
            : 'border-surface-bright/60 hover:border-surface-bright focus:border-primary/80 focus:ring-1 focus:ring-primary/80',
          disabled ? 'opacity-50 cursor-not-allowed' : '',
        ]"
      />

      <button
        v-if="type === 'password'"
        type="button"
        @click="togglePassword"
        :disabled="disabled"
        class="absolute right-3.5 text-on-surface/40 hover:text-on-surface/80 transition-colors focus:outline-none flex items-center justify-center p-1 rounded"
        :title="showPassword ? 'Ocultar contraseña' : 'Mostrar contraseña'"
      >
        <span class="material-symbols-outlined text-xl select-none">
          {{ showPassword ? 'visibility_off' : 'visibility' }}
        </span>
      </button>
    </div>

    <p v-if="error" class="text-xs text-error font-caption flex items-center gap-1 mt-0.5">
      <span class="material-symbols-outlined text-sm">warning</span>
      {{ error }}
    </p>
  </div>
</template>
